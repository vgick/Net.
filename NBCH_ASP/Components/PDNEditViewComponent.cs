using Microsoft.AspNetCore.Mvc;
using System;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NBCH_ASP.Models.NBCH.PDNEditViewComponent;
using NBCH_LIB.Interfaces;
using NBCH_LIB.Models.PDN;
using NBCH_LIB.SOAP.SOAPNBCH;

namespace NBCH_ASP.Components {
	/// <summary>
	/// Компонент для отображения ПДН с возможностью редактирования.
	/// </summary>
	public class PDNEditViewComponent : ViewComponent {
		/// <summary>
		/// Логгер.
		/// </summary>
		private readonly ILogger<PDNEditViewComponent> _Logger;
		
		/// <summary>
		///  Сервис НБКИ.
		/// </summary>
		private readonly IServicePDN _ServiceServicePDN;

		/// <summary>
		/// Конструктор.
		/// </summary>
		/// <param name="serviceServicePDN">Сервис НБКИ</param>
		/// <param name="logger">Логгер</param>
		public PDNEditViewComponent(IServicePDN serviceServicePDN, ILogger<PDNEditViewComponent> logger) {
			_ServiceServicePDN = serviceServicePDN;
			_Logger				= logger;
		}

		/// <summary>
		/// Отобразить ПДН.
		/// </summary>
		/// <param name="account1CCode">Номер договора 1С</param>
		/// <param name="accountDate">Дата договора</param>
		/// <param name="client1CCode">Код клиента 1С</param>
		/// <param name="canEdit">Разрешено редактирование</param>
		/// <returns>Представление содержащее ПДН</returns>
		public async Task<IViewComponentResult> InvokeAsync(string account1CCode, DateTime accountDate, string client1CCode, bool canEdit) {
			PDNEditViewComponentView pdnEditComponentView = new PDNEditViewComponentView {Disabled = !canEdit};
			
			CancellationToken cancellationToken	= CancellationToken.None;

			if (!string.IsNullOrEmpty(account1CCode))
				try {
					PDNInfoList pdnInfoList	= await _ServiceServicePDN.GetSavedPDNAsync(account1CCode, CancellationToken.None);
					if (pdnInfoList	== default && !string.IsNullOrEmpty(client1CCode) && accountDate != default) {
						pdnInfoList	= await _ServiceServicePDN.CalculatePDNAsync(account1CCode, accountDate, client1CCode, cancellationToken);
						if (pdnInfoList.ReportDate >= accountDate.AddDays(-SOAPNBCH.NBCHAnketaExpiredDay)) {
							await _ServiceServicePDN.SavePDNAsync(pdnInfoList, cancellationToken);
							pdnEditComponentView.Message		= @"Данные обновлены";
						}
						else {
							pdnEditComponentView.Message = @"Для расчета ПДН, требуется обновить анкету НБКИ";
						}
					}
					else pdnEditComponentView.Message = @"У договора нет рассчитанного ПДН и нет данных для нового расчета";

					pdnEditComponentView.PDNCards		= pdnInfoList?.PDNCards ?? new PDNCard[0];
					pdnEditComponentView.PDNNonCards	= pdnInfoList?.PDNNonCards ?? new PDNNonCard[0];
				}
				catch (PDNAnketaNotFoundException) {
					pdnEditComponentView.Message = "Необходимо запросить анкету НБКИ";
				}
				catch (EndpointNotFoundException) {
					pdnEditComponentView.Message = "Не удалось подключиться к службе NBCH (расчет ПДН)";
				}
				catch (Exception exception) {
					string message = exception.InnerException?.Message ?? exception.Message;
					_Logger.LogError(
						exception,
						"Не удалось сформировать ПДН. account1CCode: {account1CCode}, client1CCode: {client1CCode} ошибка: {exceptionMessage}",
						account1CCode, client1CCode, message);
					pdnEditComponentView.Message = message;
				}

			return View(pdnEditComponentView);
		}
	}
}
