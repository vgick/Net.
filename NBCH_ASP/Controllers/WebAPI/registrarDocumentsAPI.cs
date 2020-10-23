using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NBCH_ASP.Infrastructure;
using NBCH_LIB.Interfaces;
using NBCH_LIB.Models;
using NBCH_LIB.Models.Registrar;

namespace NBCH_ASP.Controllers.WebAPI {
	#if !(DEBUG)
	[Authorize(Roles = @"admin")]
	#endif
	[Route("api/[controller]")]
	[ApiController]
	public class RegistrarDocumentsApi : ControllerBase {
		private readonly ILogger<RegistrarDocumentsApi> _Logger;

		/// <summary>
		/// Сервис для работы с хранилищем документов
		/// </summary>
		private readonly IServiceRegistrar _ServiceRegistrar;

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="serviceRegistrar">Сервис по работе с хранилищем документов</param>
		/// <param name="logger">Логгер</param>
		public RegistrarDocumentsApi(IServiceRegistrar serviceRegistrar, ILogger<RegistrarDocumentsApi> logger) {
			_ServiceRegistrar	= serviceRegistrar;
			_Logger				= logger;
		}

		/// <summary>
		/// Получить список документов по номеру договора и контрагенту
		/// </summary>
		/// <param name="account1CCode">Номер договора</param>
		/// <param name="client1CCode">Код контрагента</param>
		/// <returns>Список документов</returns>
		[HttpGet("{account1CCode}/{client1CCode}")]
		public async Task<IActionResult> Get(string account1CCode, string client1CCode) {
			Client client	= new Client() {Code1C = client1CCode};

			Dictionary<Client, RegistrarDocument[]> result = default;
			try {
				result = await _ServiceRegistrar.GetDocumentsByAccountAndClients1CAsync(
					HelperASP.Login(User),
					account1CCode,
					new Client[] { client },
					CancellationToken.None
				);
			}
			catch (Exception exception) {
				_Logger.LogError(
					exception,
					"Ошибка получения списка документов. Account1CCode: {account1CCode}. Пользователь: {login}," +
					" Client1CCode: {client1CCode} Ошибка: {exceptionMessage}, StackTrace: {StackTrace}",
					account1CCode, HelperASP.Login(User), client1CCode,  exception.Message, exception.StackTrace);

				return StatusCode(StatusCodes.Status400BadRequest, exception.Message);
			}
			return Ok(result[result.Keys.First()]);
		}

		/// <summary>
		/// Получить список загруженных файлов по номеру договора, коду контрагента и коду документа
		/// </summary>
		/// <param name="account1CCode">Номер договора</param>
		/// <param name="client1CCode">Код клиента</param>
		/// <param name="documentId">Код документа</param>
		/// <returns>Список файлов</returns>
		[HttpGet("{account1CCode}/{client1CCode}/{documentID}")]
		public async Task<IActionResult> Get(string account1CCode, string client1CCode, int documentId) {
			Client client = new Client() { Code1C = client1CCode };

			RegistrarDocument result	= default;
			try {
				result = await _ServiceRegistrar.GetDocumentsByAccountAndClients1CAndDocumentIDAsync(
					HelperASP.Login(User),
					account1CCode,
					client,
					documentId,
					CancellationToken.None);
			}
			catch (Exception exception) {
				_Logger.LogError(
					exception,
					"Ошибка получения списка файлов. Account1CCode: {account1CCode}, DocumentId {documentId}," +
						" Пользователь: {login}, Client1CCode: {client1CCode} Ошибка: {exceptionMessage}," +
						" StackTrace: {StackTrace}",
					account1CCode, documentId, HelperASP.Login(User), client1CCode,  exception.Message,
						exception.StackTrace);
				return StatusCode(StatusCodes.Status400BadRequest, exception.Message);
			}
			return Ok(result);
		}
	}
}
