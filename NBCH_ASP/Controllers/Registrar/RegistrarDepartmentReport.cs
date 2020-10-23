using System;
using System.Linq;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using NBCH_ASP.Infrastructure;
using NBCH_ASP.Infrastructure.DataFromConfigurationFile.ISecrets;
using NBCH_ASP.Models.Registrar.RegistrarDepartmentReport;
using NBCH_LIB;
using NBCH_LIB.Interfaces;
using NBCH_LIB.Models;
using NBCH_LIB.Models.Registrar;
using NBCH_LIB.SOAP.SOAP1C.GetAccountsList;
using static NBCH_LIB.Organization;
using static NBCH_LIB.SOAP.SOAP1C.SOAP1C;

namespace NBCH_ASP.Controllers.Registrar {
	[Authorize(Roles = @"admin")]
	public class RegistrarDepartmentReportController : Controller {
		/// <summary>
		/// Логгер.
		/// </summary>
		private ILogger<RegistrarDepartmentReportController> _Logger;
		
		/// <summary>
		/// Веб служба 1С.
		/// </summary>
		private readonly IService1СSoap _Service1C;

		/// <summary>
		/// Сервис для работы с архивом.
		/// </summary>
		private readonly IServiceRegistrar _ServiceRegistrar;

		/// <summary>
		/// Данные для подключения к сервису 1С.
		/// </summary>
		private readonly ISecret1C _Secret1C;

		/// <summary>
		/// Конструктор.
		/// </summary>
		/// <param name="serviceRegistrar">Сервис для работы с архивом</param>
		/// <param name="service1C">Сервис 1С</param>
		/// <param name="secret1C">Данные для подключения</param>
		/// <param name="logger">Логгер</param>
		public RegistrarDepartmentReportController(IServiceRegistrar serviceRegistrar, IService1СSoap service1C,
			ISecret1C secret1C, ILogger<RegistrarDepartmentReportController> logger) {
			
			_ServiceRegistrar		= serviceRegistrar;
			_Service1C		= service1C;
			_Secret1C		= secret1C;
			_Logger			= logger;
		}

		/// <summary>
		/// Отчет руководителя.
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public async Task<IActionResult> Index() {
			SellPoint[] sellPoint	= new SellPoint[0];
			try {
				sellPoint = await _ServiceRegistrar.GetSellPointsAsync(
					default,
					default,
					AvailableOrganizations,
					CancellationToken.None);
			}
			catch (Exception exception) {
				_Logger.LogError(
					exception,
					"Не удалось получить список точек продаж. Пользователь: {login}, ошибка: {exceptionMessage}",
					HelperASP.Login(User), exception.Message);
			}

			RegistrarDepartmentReportModel model	= new RegistrarDepartmentReportModel() {
				RegionsWebServiceListName	= Secret1C.GetRegions(_Secret1C, Request.Cookies[Startup.WebService1CRegion] ?? _Secret1C.Servers.Keys.First()),
				SellPoints	= new SelectList(sellPoint.OrderBy(i => i.SellPointName),  "SellPoint1CCode", "SellPointName"),
				DateFrom	= DateTime.Now,
				DateTo		= DateTime.Now
			};

			return View(model);
		}

		/// <summary>
		/// Договора с данными по загруженным файлам
		/// </summary>
		/// <param name="sellPoint">Точка продаж</param>
		/// <param name="region">Регион</param>
		/// <param name="dateFrom">Начало периода</param>
		/// <param name="dateTo">Конец периода</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns>Договора</returns>
		[HttpPost]
		public async Task<IActionResult> Index(string sellPoint,  string region, DateTime dateFrom, DateTime dateTo,
			CancellationToken cancellationToken) {

			SellPoint[] sellPoints		= new SellPoint[0];
			AccountsForCheck[] accounts	= new AccountsForCheck[0];
			try {
				sellPoints	= await _ServiceRegistrar.GetSellPointsAsync(default, default, AvailableOrganizations, cancellationToken);
				accounts	= await GetAccountsAsync(sellPoint, region, dateFrom, dateTo, cancellationToken);
			}
			catch (Exception exception) {
				_Logger.LogError(
					exception,
					"Ошибка запроса данных. Пользователь: {login}, sellPoint: {sellPoint}, region: {region}," +
					" dateFrom: {dateFrom}, dateTo: {dateTo}, availableOrganizations {availableOrganizations}, ошибка: {exceptionMessage}",
					HelperASP.Login(User), sellPoint, region, AvailableOrganizations, exception.Message);
			}
			
			RegistrarDepartmentReportModel model = new RegistrarDepartmentReportModel() {
				RegionsWebServiceListName	= Secret1C.GetRegions(_Secret1C, Request.Cookies[Startup.WebService1CRegion] ?? _Secret1C.Servers.Keys.First()),
				SellPoints					= new SelectList(sellPoints.OrderBy(i => i.SellPointName), "SellPoint1CCode", "SellPointName"),
				AccountsForCheck			= accounts
			};

			return View(model);
		}

		/// <summary>
		/// Договора
		/// </summary>
		/// <param name="sellPoint">Точка продаж</param>
		/// <param name="region">Регион</param>
		/// <param name="dateFrom">Начало периода</param>
		/// <param name="dateTo">Конец периода</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns></returns>
		private async Task<AccountsForCheck[]> GetAccountsAsync(string sellPoint, string region, DateTime dateFrom,
			DateTime dateTo, CancellationToken cancellationToken) {
			string[] status = {
				AccountStatus.Open.GetDescription(),
				AccountStatus.Close.GetDescription()
			};

			AccountLegendNResult accounts	= await _Service1C.GetAccountsLegendsAsync(_Secret1C.Servers[region],
				_Secret1C.Login, _Secret1C.Password, dateFrom, dateTo, sellPoint, 1000, cancellationToken, status);
			var accountOpened	= accounts.AccountLegends.
				Where(i => !string.IsNullOrEmpty(i.date_status_acting)).
				Select(i => i);

			return await _ServiceRegistrar.
				GetAccountsInfoForCheckDocumentsAsync(accountOpened.Select(i => i.doc_number).
					ToArray(),
					cancellationToken);
		}

		/// <summary>
		/// Доступные организации
		/// </summary>
		private string[] AvailableOrganizations {
			get {
				Organizations[] organizations	= OrganizationsByLogin(User.Identity.Name);
				string[] result					= organizations.Select(i => i.GetDescription()).ToArray();

				return result;
			}
		}
	}
}
