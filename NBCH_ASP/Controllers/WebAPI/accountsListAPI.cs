using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NBCH_ASP.Infrastructure;
using NBCH_ASP.Infrastructure.DataFromConfigurationFile.ISecrets;
using NBCH_LIB.Interfaces;
using NBCH_LIB.SOAP.SOAP1C.GetAccountsList;
using Microsoft.AspNetCore.Authorization;
using NBCH_ASP.Models.WebAPI.AccountsListApi;

using NBCH_LIB;
using NBCH_LIB.Models.Inspecting;
using static NBCH_ASP.Infrastructure.WebAPI.AccountsListApi;

namespace NBCH_ASP.Controllers.WebAPI {
	#if !(DEBUG)
	[Authorize(Roles = @"role,roleadmin")]
	#endif
	[Route("api/[controller]")]
	[ApiController]
	public class AccountsListApi : ControllerBase {
		/// <summary>
		/// Сервис логирования
		/// </summary>
		private readonly ILogger<AccountsListApi> _Logger;

		/// <summary>
		/// Максимальный срок в днях с даты заведения договора, когда его следует отображать в списке.
		/// </summary>
		private const int _ShowDays = 7;

		/// <summary>
		/// Максимальное кол-во строк в выборке.
		/// </summary>
		private const int _MaxRowCount = 500;

		/// <summary>
		/// Сервис для работы с 1С
		/// </summary>
		private readonly IService1СSoap _Service1C;

		/// <summary>
		/// Данные для подключения к сервису.
		/// </summary>
		private readonly ISecret1C _Secret1C;

		/// <summary>
		/// Сервис для логирования работы проверяющих сотрудников.
		/// </summary>
		private readonly IServiceInspecting _ServiceInspecting;


		/// <summary>
		/// Конструктор.
		/// </summary>
		/// <param name="service1">Сервис 1С</param>
		/// <param name="secret1C">Данные для подключения к сервису 1С</param>
		/// <param name="logger">Сервис логирования</param>
		/// <param name="serviceInspecting">Сервис для логирования работы проверяющих сотрудников</param>
		public AccountsListApi(IService1СSoap service1, ISecret1C secret1C, ILogger<AccountsListApi> logger,
			IServiceInspecting serviceInspecting) {
			
			_Service1C			= service1;
			_Secret1C			= secret1C;
			_Logger				= logger;
			_ServiceInspecting	= serviceInspecting;
		}

		/// <summary>
		/// Список договоров.
		/// </summary>
		/// <param name="accountStatus">Статусы договоров</param>
		/// <param name="region">Регион пользователя</param>
		/// <returns>Список договоров с выбранным статусом</returns>
		// GET: api/<accountsListAPI>
		[HttpGet]
		// AccountLegendNResult
		public async Task<IActionResult> Get([FromQuery] int[] accountStatus, string region) {
			ObjectResult checkResult	= GetCheckParams(StatusCode, accountStatus, region, _Secret1C);
			if (checkResult != default) return checkResult;

			string[] accountStatusString	= GetAccountStatusList(accountStatus);

			AccountLegendNResultApi accounts;
			try {
				AccountLegendNResult accounts1C =
					await _Service1C.GetAccountsLegendsAsync(_Secret1C.Servers[region],
						_Secret1C.Login, _Secret1C.Password, DateTime.Now.AddDays(-_ShowDays), DateTime.Now,
						default, _MaxRowCount, CancellationToken.None, accountStatusString);
				accounts	= new AccountLegendNResultApi(accounts1C) {
					Role = PermissionLevel.Get(HelperASP.Login(User))
				};

				if (accounts.Errors.Length > 0)
					return StatusCode(StatusCodes.Status500InternalServerError, string.Join(", ", accounts.Errors));
				
				InspectorAccount[] inspectors	= await _ServiceInspecting.GedInspectorsByAccountListAsync(
					accounts.AccountLegendApi.Select(i => i.doc_number).ToArray(), CancellationToken.None);

				foreach (AccountLegendApi accountLegendApi in accounts.AccountLegendApi) {
					accountLegendApi.inspector = inspectors
						.FirstOrDefault(i => i.Account1CCode == accountLegendApi.doc_number)
						?.Inspector;
				}
			}
			catch (Exception exception) {
				_Logger.LogError(
					exception,
					"Ошибка получения списка договоров. Статусы: {accountStatus}, регион: {region}, пользователь: {login}. Ошибка: {exceptionMessage}, StackTrace: {StackTrace}.",
					string.Join(',', accountStatus), region, HelperASP.Login(User), exception.Message, exception.StackTrace);
				return StatusCode(StatusCodes.Status500InternalServerError, exception.Message);
			}

			FilterByOrganization(accounts, User);

			return Ok(accounts);
		}
	}
}

