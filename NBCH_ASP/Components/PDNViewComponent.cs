using Microsoft.AspNetCore.Mvc;
using System;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NBCH_ASP.Models.NBCH.PDNComponent;
using NBCH_LIB.Interfaces;
using NBCH_LIB.Models.PDN;
using NBCH_LIB.SOAP.SOAPNBCH;

namespace NBCH_ASP.Components {
	/// <summary>
	/// Компонент для отображения ПДН.
	/// </summary>
	public class PDNViewComponent : ViewComponent {
		/// <summary>
		/// Логгер.
		/// </summary>
		private readonly ILogger<PDNViewComponent> _Logger;
		
		/// <summary>
		///  Сервис НБКИ.
		/// </summary>
		private readonly IServicePDN _ServiceServicePDN;

		/// <summary>
		/// Конструктор.
		/// </summary>
		/// <param name="serviceServicePDN">Сервис НБКИ</param>
		/// <param name="logger">Логгер</param>
		public PDNViewComponent(IServicePDN serviceServicePDN, ILogger<PDNViewComponent> logger) {
			_ServiceServicePDN	= serviceServicePDN;
			_Logger				= logger;
		}

		/// <summary>
		/// Отобразить ПДН.
		/// </summary>
		/// <param name="account1CCode">Номер договора 1С</param>
		/// <param name="accountDate">Дата договора</param>
		/// <param name="client1CCode">Код клиента 1С</param>
		/// <param name="canEdit">Можно редактировать</param>
		/// <returns>Представление содержащее ПДН</returns>
		public async Task<IViewComponentResult> InvokeAsync(string account1CCode, DateTime accountDate, string client1CCode,
			bool canEdit) {

			CancellationToken cancellationToken = CancellationToken.None;
			
			PDNDataView pdnDataView = new PDNDataView {CanEdit = canEdit};

			if (!string.IsNullOrEmpty(account1CCode) && !string.IsNullOrEmpty(client1CCode))
				try {
					pdnDataView.PDNInfoList = await _ServiceServicePDN.GetSavedPDNAsync(account1CCode, cancellationToken);
					if (pdnDataView.PDNInfoList == default) {
						PDNInfoList pdnInfoList	= 
							await _ServiceServicePDN.CalculatePDNAsync(account1CCode, accountDate, client1CCode, cancellationToken);
						
						if (pdnInfoList.ReportDate >= accountDate.AddDays(-SOAPNBCH.NBCHAnketaExpiredDay)) {
							await _ServiceServicePDN.SavePDNAsync(pdnInfoList, cancellationToken);
							pdnDataView.PDNInfoList	= pdnInfoList;
							pdnDataView.Message = @"Данные обновлены";
						}
						else {
							pdnDataView.Message = @"Для расчета ПДН, требуется обновить анкету НБКИ";
						}
					};
				}
				catch (PDNAnketaNotFoundException) {
					pdnDataView.Message = "Необходимо запросить анкету НБКИ";
				}
				catch (EndpointNotFoundException) {
					pdnDataView.Message = "Не удалось подключиться к службе NBCH (расчет ПДН)";
				}
				catch (Exception exception) {
					string message = exception.InnerException?.Message ?? exception.Message;
					_Logger.LogError(
						exception,
						"Не удалось сформировать ПДН. account1CCode: {account1CCode}, client1CCode: {client1CCode} ошибка: {exceptionMessage}",
						account1CCode, client1CCode, message);
					pdnDataView.Message = message;
				}

			return View(pdnDataView);
		}
	}
}
