using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NBCH_ASP.Infrastructure;
using NBCH_ASP.Infrastructure.DataFromConfigurationFile.ISecrets;
using NBCH_LIB;
using NBCH_LIB.Interfaces;
using NBCH_LIB.SOAP.SOAP1C.GetAccountsList;
using static NBCH_LIB.SOAP.SOAP1C.SOAP1C;

namespace NBCH_ASP.Components {
	public class AccountTableViewComponent : ViewComponent {
		private readonly ILogger<AccountTableViewComponent> _Logger;
		/// <summary>
		/// Кол-во строк в выборке.
		/// </summary>
		private const int _RecordOnList = 100;

		/// <summary>
		/// Максимальная давность договора в днях.
		/// </summary>
		private const int _DepthInDays = 7;

		/// <summary>
		/// Сервис 1С
		/// </summary>
		private readonly IService1СSoap _Service1C;

		/// <summary>
		/// Данные для подключения к сервису 1С.
		/// </summary>
		private readonly ISecret1C _Secret1C;

		/// <summary>
		/// Конструктор.
		/// </summary>
		/// <param name="service1C">Сервис 1С</param>
		/// <param name="secret1C">Данные для подключения к 1С</param>
		/// <param name="logger">Логгер</param>
		public AccountTableViewComponent(IService1СSoap service1C, ISecret1C secret1C, ILogger<AccountTableViewComponent> logger) {
			_Service1C	= service1C;
			_Secret1C	= secret1C;
			_Logger		= logger;
		}

		/// <summary>
		/// Список договоров.
		/// </summary>
		/// <param name="region">Регион</param>
		/// <param name="orgs">Организации</param>
		/// <param name="accountStatus">Статусы договоров</param>
		/// <returns></returns>
		public async Task<IViewComponentResult> InvokeAsync(string region, Dictionary<Organization.Organizations, bool> orgs, params AccountStatus[] accountStatus) {
			try {
				region ??= _Secret1C.Servers.Keys.First();

				ViewData["orgs"] = orgs;
				string[] accountStatusString	= accountStatus?.Select(i => i.GetDescription()).ToArray() ?? new string[0];

				AccountLegendNResult accounts	= new AccountLegendNResult();
				try {
					accounts	= await _Service1C.GetAccountsLegendsAsync(_Secret1C.Servers[region],
						_Secret1C.Login, _Secret1C.Password, DateTime.Now.AddDays(-_DepthInDays), DateTime.Now, default,
						_RecordOnList, CancellationToken.None, accountStatusString);
				}
				catch (Exception exception) {
					_Logger.LogError(
						exception,
						"Не удалось получить список договоров. Servers: {servers}, region: {region}," +
						" accountStatusString: {accountStatusString}, ошибка: {exceptionMessage}",
						_Secret1C.Servers[region], region, string.Join(", ", accountStatusString), exception.Message);
				}
				
				if (accounts?.Errors?.Length > 0)
					ViewData["Error"] = string.Join("; ", accounts.Errors ?? new [] {"null"});

				List<AccountLegend> lst		= new List<AccountLegend>();

				foreach (KeyValuePair<Organization.Organizations, bool> org in orgs) {
					if (!org.Value) continue;
					
					IEnumerable<AccountLegend> lstByOrganization = accounts?.AccountLegends.Where(i =>
						i.organization_name.Equals(org.Key.GetDescription())) ?? new AccountLegend[0];
					lst.AddRange(lstByOrganization);
				}

				accounts.AccountLegends = lst.ToArray();

				return View(accounts);
			}
			catch (Exception exception) {
				ViewData["Error"]	= exception.Message;

				return View((Object)null);
			}
		}
	}
}
