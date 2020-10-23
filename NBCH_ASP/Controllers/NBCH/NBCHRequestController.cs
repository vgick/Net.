using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NBCH_ASP.Infrastructure;
using NBCH_ASP.Infrastructure.DataFromConfigurationFile.ISecrets;
using NBCH_ASP.Infrastructure.NBCH;
using NBCH_ASP.Models.NBCH;
using NBCH_ASP.Models.NBCH.NBCHRequest;
using NBCH_LIB;
using NBCH_LIB.Interfaces;
using NBCH_LIB.SOAP.SOAP1C;
using NBCH_LIB.SOAP.SOAPNBCH;

namespace NBCH_ASP.Controllers.NBCH
{
	[Authorize(Roles = @"role,roleadmin")]
	public class NBCHRequestController : Controller {
		/// <summary>
		/// Логгер
		/// </summary>
		private readonly ILogger<NBCHRequestController> _Logger;
		
		/// <summary>
		/// Веб служба 1С.
		/// </summary>
		private readonly IService1СSoap _Service1C;

		/// <summary>
		/// Веб служба НБКИ.
		/// </summary>
		private readonly IServiceNBCHsoap _ServiceNBCHsoap;

		/// <summary>
		/// Настройки приложения.
		/// </summary>
		private readonly IConfiguration _Configuration;

		/// <summary>
		/// Данные для подключения к сервису 1С.
		/// </summary>
		private readonly ISecret1C _Secret1Cs;

		/// <summary>
		/// Конструктор.
		/// </summary>
		/// <param name="service1C">Сервис для работы с данными 1С</param>
		/// <param name="serviceNBCH">Сервис для работы с данными НБКИ</param>
		/// <param name="configuration">Конфигурация</param>
		/// <param name="secret1C">Данные для подключения</param>
		/// <param name="serviceNBCHsoap">Веб служба НБКИ</param>
		/// <param name="logger">Логгер</param>
		public NBCHRequestController(IService1СSoap service1C, IServiceNBCH serviceNBCH, IConfiguration configuration,
			ISecret1C secret1C, IServiceNBCHsoap serviceNBCHsoap, ILogger<NBCHRequestController> logger) {
			
			_Service1C			= service1C;
			_Configuration		= configuration;
			_Secret1Cs			= secret1C;
			_ServiceNBCHsoap	= serviceNBCHsoap;
			_Logger				= logger;
		}

		/// <summary>
		/// Отобразить пустую форму для запроса данных в 1С, НБКИ.
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public IActionResult Index() {
			IndexModel model = new IndexModel {
				RegionsWebServiceListName = Secret1C.GetRegions(_Secret1Cs,
					Request.Cookies[Startup.WebService1CRegion] ?? _Secret1Cs.Servers.Keys.First())
			};

			ViewData["RequestCreditHistoryButtonDisabled"]	= "style=\"display: none\"";
			Organization.Organizations[] orgs	= Organization.OrganizationsByLogin(User.Identity.Name);
			Dictionary<Organization.Organizations, bool> orgsSelected	=
				orgs.ToDictionary(organization => organization, organization => true);

			ViewData["orgs"]	= orgsSelected;

			return View(model);
		}

		/// <summary>
		/// Отобразить данные клиента.
		/// </summary>
		/// <param name="model">Данные формы</param>
		/// <param name="submit">Тип запроса (получить данные из 1С, получить данные из НБКИ)</param>
		/// <param name="clientPersonalInfo"></param>
		/// <param name="regionWebServiceListName">Регион, сервер которого используются для получения данных из 1С</param>
		/// <param name="orgs">Выбранные организации</param>
		/// <returns></returns>
		[HttpPost]
		public async Task<IActionResult> Index(IndexModel model, string submit, ClientPersonalInfo clientPersonalInfo,
			string regionWebServiceListName, string[] orgs) {
			
			IndexModel.SubmitType submitType	=
					Enum.IsDefined(typeof(IndexModel.SubmitType), submit)
						? (IndexModel.SubmitType)Enum.Parse(typeof(IndexModel.SubmitType), submit)
						: IndexModel.SubmitType.Null;

			if (!int.TryParse(Request.Cookies["ClientTimeZone"], out int clientTimeZone)) {
				submitType			= IndexModel.SubmitType.Null;
				model.ErrorMessage	= "Ошибка определения часового пояса на клиентской машине";
			}

			model.ClientPersonalInfo	= clientPersonalInfo ?? new ClientPersonalInfo();
			ViewData["RequestCreditHistoryButtonDisabled"] = "style=\"display: none\"";
			ViewData["orgs"]		= Organization.OrganizationsByLogin(User.Identity.Name, orgs);

			switch (submitType) {
				case IndexModel.SubmitType.Null:
					break;
				case IndexModel.SubmitType.GetFrom1C:
					if ((model.Account1CCode ?? default) == default) break;
					ModelState.Clear();

					try {
						model = await NBCHRequest.GetDataFrom1CAsync(_Service1C, model.Account1CCode, _Secret1Cs, regionWebServiceListName, _Configuration);
						ViewData["RequestCreditHistoryButtonDisabled"] = "";
					}
					catch (Exception exception) {
						_Logger.LogError(
							exception,
							"Ошибка получения данных из 1С. Пользователь: {login}, данные запроса: {model}, регион {region}, ошибка: {exceptionMessage}.",
							HelperASP.Login(User), model, regionWebServiceListName, exception.Message);

						model.ErrorMessage = exception.InnerException != default
							? model.ErrorMessage += Environment.NewLine + exception.InnerException.Message
							: exception.Message;
					}
					break;
				case IndexModel.SubmitType.GetCH:
					Report report	= new Report();
					model.ClientTimeZone	= clientTimeZone;

					try {
						report	= await NBCHRequest.GetCreditHistoryAsync(_ServiceNBCHsoap, model,
							SecretNBCH.GetSecretNBCH(_Configuration, model.InquiryReq.ConsentReq.reportUser), _Logger);
					}
					catch (EndpointNotFoundException) {
						report.ErrorMessage = "Не удалось подключиться к службе NBCH (запрос данных из НБКИ).";
						_Logger.LogError(report.ErrorMessage);
					}
					catch (Exception exception) {
						report.ErrorMessage = exception.ToString();
						_Logger.LogError(
							exception,
							"Ошибка получения данных из НБКИ. Пользователь: {login}, данные запроса: {model}, ошибка: {exceptionMessage}.",
							HelperASP.Login(User), model, exception.Message);
					}
				
					model.AccountReply		= report.AccountReply;
					model.Calc				= report.calc;
					model.ErrorMessage		= report.ErrorMessage;
					break;
				default:
					break;
			}

			ViewData["ReportDate"]	= model?.Calc?.ReportDate ?? default;

			model.RegionsWebServiceListName = Secret1C.GetRegions(_Secret1Cs, regionWebServiceListName);
			Response.Cookies.Append(Startup.WebService1CRegion, regionWebServiceListName);

			return View(model);
		}

		/// <summary>
		/// Представление с договорами на проверке.
		/// </summary>
		/// <param name="region">Регион (для выбора приоритетного сервера)</param>
		/// <param name="orgs">Список выбранных организаций</param>
		/// <returns>Список договоров</returns>
		public IActionResult GetAccountTable(string region, string[] orgs) {
			Dictionary<Organization.Organizations, bool> organizations = Organization.OrganizationsByLogin(User.Identity.Name, orgs);
			SOAP1C.AccountStatus[] accst	= { SOAP1C.AccountStatus.Verification, SOAP1C.AccountStatus.CheckSB };

			return ViewComponent("AccountTable", new { region, orgs = organizations, accountStatus = accst });
		}
	}
}