using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NBCH_ASP.Infrastructure;
using NBCH_ASP.Infrastructure.DataFromConfigurationFile.ISecrets;
using NBCH_ASP.Infrastructure.NBCH;
using NBCH_LIB.Interfaces;
using NBCH_LIB.Models;
using static NBCH_ASP.Infrastructure.WebAPI.ClientListApi;

namespace NBCH_ASP.Controllers.WebAPI {
	#if !(DEBUG)
	[Authorize(Roles = @"role,roleadmin")]
	#endif
	[Route("api/[controller]")]
	[ApiController]
	public class ClientListApi : ControllerBase {
		/// <summary>
		/// Сервис логирования
		/// </summary>
		private readonly ILogger<ClientListApi> _Logger;

		/// <summary>
		/// Сервис по работе с 1С
		/// </summary>
		private readonly IService1СSoap _Service1C;

		/// <summary>
		/// Данные для подключения.
		/// </summary>
		private readonly ISecret1C _Secret1C;

		/// <summary>
		/// Конструктор.
		/// </summary>
		/// <param name="service1C">Сервис для работы с 1С</param>
		/// <param name="secret1C">Данные для подключения</param>
		/// <param name="logger">Сервис логирования</param>
		public ClientListApi(IService1СSoap service1C, ISecret1C secret1C, ILogger<ClientListApi> logger) {
			_Service1C	= service1C;
			_Secret1C	= secret1C;
			_Logger		= logger;
		}

		/// <summary>
		/// Список контрагентов по счету.
		/// </summary>
		/// <param name="account">Номер договора</param>
		/// <param name="region">Регион пользователя</param>
		/// <returns>Список контрагентов по счету</returns>
		[HttpGet]
		public async Task<IActionResult> Get(string account, string region) {
			ObjectResult checkResult	=  GetCheckParams(StatusCode, account, region);
			if (checkResult != default) return checkResult;
			
			(Client[] Clients, string[] Errors) clients;
			try {
				clients	= await RegistrarDocuments.GetClientsFrom1CAccountAsync(_Service1C, _Secret1C, region, account);
			}
			catch (Exception exception) {
				_Logger.LogError(
					exception,
					"Ошибка получения списка контрагентов по договору {account1CCode}. Пользователь: {login}." +
					"	 Ошибка: {exceptionMessage}, StackTrace: {StackTrace}.",
					account, HelperASP.Login(User), exception.Message, exception.StackTrace);
				return StatusCode(StatusCodes.Status500InternalServerError, exception.Message);
			}

			if (clients.Errors.Length > 0) {
				_Logger.LogError(
					"Ошибка получения списка контрагентов по договору {account1CCode}. Пользователь: {login}," +
					" ошибка: {errorMessage}.",
					account, HelperASP.Login(User), string.Join(". ", clients.Errors));
				return StatusCode(StatusCodes.Status500InternalServerError, string.Join(". ", clients.Errors));
			}
			
			return Ok(clients.Clients);
		}
	}
}
