using System;
using System.Linq;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NBCH_ASP.Models.NBCH.ArchiveCH;
using NBCH_LIB.Interfaces;
using NBCH_LIB.SOAP.SOAPNBCH;

namespace NBCH_ASP.Controllers.NBCH
{
	[Authorize(Roles = @"role,roleadmin")]
	public class ArchiveCHController : Controller {
		/// <summary>
		/// Логгер.
		/// </summary>
		private readonly ILogger<ArchiveCHController> _Logger;
		
		/// <summary>
		/// Сервис НБКИ.
		/// </summary>
		private readonly IServiceNBCH _ServiceNBCH;

		/// <summary>
		/// Конструктор.
		/// </summary>
		/// <param name="serviceNBCH">Сервис НБКИ</param>
		/// <param name="logger">Логгер</param>
		public ArchiveCHController(IServiceNBCH serviceNBCH, ILogger<ArchiveCHController> logger) {
			_ServiceNBCH	= serviceNBCH;
			_Logger			= logger;
		}

		/// <summary>
		/// Отобразить кредитную историю по ID анкеты.
		/// </summary>
		/// <param name="creditHistoryID">ID кредитной анкеты</param>
		/// <returns></returns>
		[HttpGet]
		[ActionName("GetCH")]
		public async Task<IActionResult> GetCHFromArchive(string creditHistoryID) {
			creditHistoryID	= String.IsNullOrEmpty(creditHistoryID) ? "0" : creditHistoryID;
			int.TryParse(creditHistoryID, out int creditHistoryIDint);
			ViewData["SelectedCreditHistoryID"]	= creditHistoryID;

			ArchiveCHModel archiveCHModel			= new ArchiveCHModel();
			if (creditHistoryIDint != default) {
				archiveCHModel.Report				= await _ServiceNBCH.GetSavedReportAsync(creditHistoryIDint, CancellationToken.None);

				if (string.IsNullOrEmpty(archiveCHModel.Report.ErrorMessage)) {
					archiveCHModel.ClientPersonalInfo.AddressReq	= archiveCHModel.Report.AddressReply.Select(i => (AddressReq)i).ToArray();
					// todo: передается не массив, а один документ
					archiveCHModel.ClientPersonalInfo.PersonReq		= (PersonReq)archiveCHModel.Report.PersonReply[0];
					// todo: передается не массив, а один документ
					archiveCHModel.ClientPersonalInfo.IdReq			= (IdReq)archiveCHModel.Report.Documents[0];
					ViewData["ReportDate"] = archiveCHModel?.Report?.calc?.ReportDate ?? default;
				}
			}
			else
				archiveCHModel.Report	= new Report() {ErrorMessage = $"Анкета НБКИ с номером '{creditHistoryID}' не найдена"};

			return View("Index", archiveCHModel);
		}

		/// <summary>
		/// Отобразить кредитную историю по коду клиента 1С.
		/// </summary>
		/// <param name="client1CCode">Код клиента</param>
		/// <returns></returns>
		[HttpGet]
		[ActionName("GetCHByClient")]
		public async Task<IActionResult> GetCHFromArchiveByClient(string client1CCode) {
			CancellationToken cancellationToken = CancellationToken.None;
			
			ArchiveCHModel archiveCHModel = new ArchiveCHModel();
			if (!String.IsNullOrEmpty(client1CCode)) {
				int creditHistoryID = default;
				try { creditHistoryID = await _ServiceNBCH.GetClientCreditHistoryIDAsync(client1CCode, cancellationToken); }
				catch (Exception exception) {
					archiveCHModel.Report.ErrorMessage	= "Не удалось получить ID анкеты клиента.";
					_Logger.LogError(
						exception,
						"Не удалось получить ID анкеты клиента {client1CCode}. Ошибка: {exceptionMessage}",
						client1CCode, exception.Message);
				}

				if (creditHistoryID != default) {
					try {
						archiveCHModel.Report =
							await _ServiceNBCH.GetSavedReportAsync(creditHistoryID, cancellationToken);
					}
					catch (Exception exception) {
						archiveCHModel.Report.ErrorMessage	= "Не удалось получить КИ клиента.";
						_Logger.LogError(
							exception,
							"Не удалось получить КИ клиента. Клиент: {client1CCode}, creditHistoryID, {creditHistoryID}, ошибка: {exceptionMessage}",
							client1CCode, creditHistoryID, exception.Message);
					}
				}

				if (archiveCHModel.Report.ErrorMessage == default) {
					archiveCHModel.ClientPersonalInfo.AddressReq	= archiveCHModel.Report.AddressReply.Select(i => (AddressReq)i).ToArray();
					// todo: передается не массив, а один документ
					archiveCHModel.ClientPersonalInfo.PersonReq		= (PersonReq)archiveCHModel.Report.PersonReply[0];
					// todo: передается не массив, а один документ
					archiveCHModel.ClientPersonalInfo.IdReq			= (IdReq)archiveCHModel.Report.Documents[0];
					ViewData["ReportDate"] = archiveCHModel?.Report?.calc?.ReportDate ?? default;
				}
			}

			return View("Index", archiveCHModel);
		}

		/// <summary>
		/// Компонент отображает даты запросов КИ по коду клиента из 1С.
		/// </summary>
		/// <param name="client1CCode">Код клиента из 1С</param>
		/// <returns></returns>
		public IActionResult GetCreditHistoryList(string client1CCode) {
			return ViewComponent("CreditHistoryList", client1CCode);
		}

	}
}