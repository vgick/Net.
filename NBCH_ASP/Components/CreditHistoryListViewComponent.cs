using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NBCH_LIB.Interfaces;
using NBCH_LIB.Models;

namespace NBCH_ASP.Components {
	/// <summary>
	/// Компонент для отрисовки списка сохраненных анкет НБКИ.
	/// </summary>
	public class CreditHistoryListViewComponent : ViewComponent {
		/// <summary>
		/// Логгер.
		/// </summary>
		private readonly ILogger<CreditHistoryListViewComponent> _Logger;
		
		/// <summary>
		/// Сервис НБКИ.
		/// </summary>
		private readonly IServiceNBCH _ServiceNBCH;

		/// <summary>
		/// Конструктор.
		/// </summary>
		/// <param name="serviceNBCH">Сервис НБКИ</param>
		/// <param name="logger">Логгер</param>
		public CreditHistoryListViewComponent(IServiceNBCH serviceNBCH, ILogger<CreditHistoryListViewComponent> logger) {
			_ServiceNBCH	= serviceNBCH;
			_Logger			= logger;
		}

		/// <summary>
		/// Список анкет НБКИ.
		/// </summary>
		/// <param name="client1CCode">Код клиента 1С</param>
		/// <returns>Представление списка сохраненных анкет НБКИ</returns>
		public async Task<IViewComponentResult> InvokeAsync(string client1CCode) {
			CreditHistoryInfo[] creditHistorys	= new CreditHistoryInfo[0];
			if (!string.IsNullOrEmpty(client1CCode))
				try { creditHistorys = await _ServiceNBCH.GetCreditHistoryListAsync(client1CCode, CancellationToken.None); }
				catch (Exception exception) {
					_Logger.LogError(
						exception,
						"Не удалось получить список анкет НБКИ. client1CCode: {client1CCode}, ошибка: {exceptionMessage}",
						client1CCode, exception.Message);
				}
				

			return View(creditHistorys);
		}
	}
}
