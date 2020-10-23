using System;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NBCH_ASP.Infrastructure;
using NBCH_ASP.Models.NBCH.SearchClient;
using NBCH_LIB.Interfaces;

namespace NBCH_ASP.Controllers.NBCH
{
	[Authorize(Roles = @"role,roleadmin")]
	public class SearchClientController : Controller {
		private readonly ILogger<SearchClientController> _Logger;
		/// <summary>
		/// Сервис для работы данными НБКИ.
		/// </summary>
		private readonly IServiceNBCH _ServiceNBCH;

		/// <summary>
		/// Конструктор.
		/// </summary>
		/// <param name="serviceNBCH">Сервис для работы с данными НБКИ</param>
		/// <param name="logger">Логгер</param>
		public SearchClientController(IServiceNBCH serviceNBCH, ILogger<SearchClientController> logger) {
			_ServiceNBCH    = serviceNBCH;
			_Logger        = logger;
		}

		/// <summary>
		/// Форма запроса данных.
		/// </summary>
		/// <returns></returns>
		public IActionResult Index() {
			return View();
		}

		/// <summary>
		/// Результат поиска.
		/// </summary>
		/// <param name="searchString">Строка поиска</param>
		/// <returns></returns>
		[HttpPost]
		public async Task<IActionResult> Index(string searchString) {
			SearchClientModel searchClientModel = new SearchClientModel {SearchString = searchString};
			if (!string.IsNullOrEmpty(searchString))
				try { searchClientModel.SearchClientList  = await _ServiceNBCH.SearchClientAsync(searchString, CancellationToken.None); }
				catch (Exception exception) {
					_Logger.LogError(
						exception,
						"Не удалось получить список клиентов. Пользователь: {login},  строка поиска: {searchString}, ошибка: {exceptionMessage}, StackTrace: {StackTrace}, exception: {exception}.",
						HelperASP.Login(User), searchString, exception.Message, exception.StackTrace, exception);
				}

			return View(searchClientModel);
		}
	}
}