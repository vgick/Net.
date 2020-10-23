using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NBCH_LIB.Interfaces;
using NBCH_LIB.Models;
using NBCH_LIB.Models.Registrar;

namespace NBCH_ASP.Components {
	/// <summary>
	/// Компонент для отображения списка файлов, загруженных в архив.
	/// </summary>
	public class RegistrarClientDocumentsViewComponent : ViewComponent {
		/// <summary>
		/// Логгер.
		/// </summary>
		private readonly ILogger<RegistrarClientDocumentsViewComponent> _Logger;
		/// <summary>
		/// Сервис для работы с архивом документов
		/// </summary>
		IServiceRegistrar ServiceRegistrar {get; set;}

		/// <summary>
		/// Конструктор.
		/// </summary>
		/// <param name="serviceRegistrar">Сервис для работы с архивом документов</param>
		/// <param name="logger">Логгер</param>
		public RegistrarClientDocumentsViewComponent(IServiceRegistrar serviceRegistrar, ILogger<RegistrarClientDocumentsViewComponent> logger) {
			ServiceRegistrar	= serviceRegistrar;
			_Logger				= logger;
		}

		/// <summary>
		/// Отобразить загруженные в архив документы с учетом прав пользователя, который запрашивает информацию.
		/// </summary>
		/// <param name="client">Клиент 1С</param>
		/// <param name="account1CCode">Код договора 1С</param>
		/// <returns>Представление</returns>
		public async Task<IViewComponentResult> InvokeAsync(Client client, string account1CCode) {
			Dictionary<Client, RegistrarDocument[]> documentsAll	= new Dictionary<Client, RegistrarDocument[]>();
			try {
				documentsAll = await ServiceRegistrar.
					GetDocumentsByAccountAndClients1CAsync(
						User.Identity?.Name,
						account1CCode,
						new Client[] { client },
						CancellationToken.None
					);
			}
			catch (Exception exception) {
				_Logger.LogError(
					exception,
					"Не удалось сформировать список файлов. client: {client}, account1CCode: {account1CCode} ошибка: {exceptionMessage}",
					client, account1CCode, exception.Message);
				ViewData["Error"]	= exception.Message;
			}
			
			KeyValuePair<Client, RegistrarDocument[]> documents		= new KeyValuePair<Client, RegistrarDocument[]>(Client.NullClient, new RegistrarDocument[0]);
			if (documentsAll.Keys.Count > 0) {
				documentsAll.Keys.First().AffiliationOfAccount	= client.AffiliationOfAccount;
				documents	= new KeyValuePair<Client, RegistrarDocument[]>(documentsAll.Keys.First(), documentsAll[documentsAll.Keys.First()]);
			}

			ViewData["Account1CCode"]	= account1CCode;

			return View(documents);
		}
	}
}
