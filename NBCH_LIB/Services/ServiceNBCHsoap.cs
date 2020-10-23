using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NBCH_LIB.Interfaces;
using NBCH_LIB.Interfaces.WCF;
using NBCH_LIB.SOAP.SOAPNBCH;
using static NBCH_LIB.Logger.ExceptionLog;
using static NBCH_LIB.Helper;

namespace NBCH_LIB.Services {
	public class ServiceNBCHsoap: IServiceNBCHsoap, IServiceNBCHsoapWCF {
		/// <summary>
		/// Логгер.
		/// </summary>
		private readonly ILogger<ServiceNBCHsoap> _Logger;

		/// <summary>
		/// Сервис для работы с данными НБКИ.
		/// </summary>
		private readonly IServiceNBCH _ServiceNBCH;
		public ServiceNBCHsoap(IServiceNBCH serviceNBCH, ILoggerFactory loggerFactory) {
			_ServiceNBCH	= serviceNBCH;
			_Logger			= loggerFactory.CreateLogger<ServiceNBCHsoap>();
		}

		/// <summary>
		/// Получить КИ клиента из НБКИ.
		/// </summary>
		/// <param name="url">Урл веб службы</param>
		/// <param name="request">Данные клиента</param>
		/// <param name="account1CCode">Номер договора в 1С</param>
		/// <param name="client1CCode">Код клиента в 1С</param>
		/// <param name="clientTimeZone">Часовой пояс, откуда пришел запрос</param>
		/// <returns>КИ из НБКИ</returns>
		public Report GetReport(string url, ProductRequest request, string account1CCode, string client1CCode,
			int clientTimeZone) =>
			GetReportAsync(url, request, account1CCode, client1CCode, clientTimeZone, CancellationToken.None).
				ResultAndThrowException();

		/// <summary>
		/// Получить КИ клиента из НБКИ асинхронно.
		/// </summary>
		/// <param name="url">Урл веб службы</param>
		/// <param name="request">Данные клиента</param>
		/// <param name="account1CCode">Номер договора в 1С</param>
		/// <param name="client1CCode">Код клиента в 1С</param>
		/// <param name="clientTimeZone">Часовой пояс, откуда пришел запрос</param>
		/// <returns>КИ из НБКИ</returns>
		public async Task<Report> GetReportAsync(string url, ProductRequest request, string account1CCode,
			string client1CCode, int clientTimeZone) =>
			await GetReportAsync(url, request, account1CCode, client1CCode, clientTimeZone);

		/// <summary>
		/// Получить КИ клиента из НБКИ асинхронно.
		/// </summary>
		/// <param name="url">Урл веб службы</param>
		/// <param name="request">Данные клиента</param>
		/// <param name="account1CCode">Номер договора в 1С</param>
		/// <param name="client1CCode">Код клиента в 1С</param>
		/// <param name="clientTimeZone">Часовой пояс, откуда пришел запрос</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns>КИ из НБКИ</returns>
		public async Task<Report> GetReportAsync(string url, ProductRequest request, string account1CCode,
			string client1CCode, int clientTimeZone, CancellationToken cancellationToken) {
			GetReportCheckParams(url, request, account1CCode, client1CCode, clientTimeZone);

			int serverTimeZone = ServerTimeZone;

			if (!await ReportIsExpiredAsync(client1CCode, clientTimeZone, cancellationToken)) {
				Report nullReport = new Report {
					ErrorMessage = $"КИ НБКИ по одному клиенту можно запрашивать не чаще чем раз в {SOAPNBCH.ReportCanReaplyMinute} минут",
					calc = { ReportDate = DateTime.Now.AddHours(clientTimeZone - serverTimeZone) }
				};
				return nullReport;
			}

			request	= UpdateDateToMoscowTimeZone(request);

			ProductResponse productResponse	= default;
			byte[] signedResponse	= default;
			byte[] clearResponse	= default;
			
			try {
				signedResponse	= await SOAPNBCH.GetSignedReportAsync(url, request, cancellationToken);
				clearResponse	= SOAPNBCH.RemoveSignature(signedResponse);
				productResponse	= SOAPNBCH.DeserializeProductRequest(clearResponse) ?? new ProductResponse();
			}
			catch (Exception exception) {
				LogAndThrowException<Exception, ServiceNBCHsoap>(
					_Logger, "",
					"Ошибка запроса анкеты НБКИ. Ошибка {ExceptionMessage}./* Метод: {methodName}, запрос: {request}," +
					" url: {url}, Exception: {Exception}.*/",
					exception.Message, "GetReportAsync", request, url, exception);
			}

			try {
				await _ServiceNBCH.WriteCreditHistoryAsync(account1CCode, client1CCode, signedResponse,
					clearResponse, clientTimeZone, productResponse?.Preply?.Error?.CtErr, cancellationToken);
			}
			catch (Exception exception) {
				LogAndThrowException<Exception, ServiceNBCHsoap>(
					_Logger, "",
					"Ошибка сохранения анкеты НБКИ. Ошибка {ExceptionMessage}./* Метод: {methodName}," +
					" account1CCode: {account1CCode}, client1CCode: {client1CCode}.*/",
					exception.Message, "GetReportAsync", account1CCode, client1CCode);
			}

			Report report			= productResponse?.Preply?.Report ?? new Report();
			report.ErrorMessage		= productResponse?.Preply?.Error?.CtErr.Code == SOAPNBCH.ClientNotFoundNBCH ?
				"В базе НБКИ, клиент с такими данными не найден" : productResponse?.Preply?.Error?.CtErr.Text;
			report.calc.ReportDate	= DateTime.Now.AddHours(clientTimeZone - serverTimeZone);
			report.Client1CCode		= client1CCode;

			return report;
		}

		/// <summary>
		/// Изменить дату и время согласия клиента в соответствии с Московским временем.
		/// </summary>
		/// <param name="request">Запрос в НБКИ</param>
		/// <returns></returns>
		private ProductRequest UpdateDateToMoscowTimeZone(ProductRequest request) {
			DateTime consentDate	= SOAPNBCH.StringToDateTime(request.Prequest.Req.InquiryReq.ConsentReq.consentDate);
			consentDate				= EndOfDay(consentDate);
			DateTime moscowDate		= DateTime.Now.AddHours(SOAPNBCH.MoscowTimeZone - ServerTimeZone);
			consentDate				= consentDate > moscowDate ? consentDate.AddDays(-1) : consentDate;

			request.Prequest.Req.InquiryReq.ConsentReq.consentDate = SOAPNBCH.DateTimeToString(consentDate);

			return request;
		}

		/// <summary>
		/// Можно ли повторить повторить запрос анкеты НБКИ. Чтобы часто не запрашивать анкеты асинхронно.
		/// </summary>
		/// <param name="client1CCode">Код клиента из 1С базы</param>
		/// <param name="clientTimeZone">Часовой пояс клиента</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns>Можно или нет повторить запрос</returns>
		private async Task<bool> ReportIsExpiredAsync(string client1CCode, int clientTimeZone, CancellationToken cancellationToken) {
			DateTime lastCreditHistoryDate = (await _ServiceNBCH.GetCreditHistoryListAsync(client1CCode, cancellationToken)).
				Where(i => string.IsNullOrEmpty(i.ErrorCode) || i.ErrorCode == SOAPNBCH.ClientNotFoundNBCH).
				OrderByDescending(i => i.Date).
				Select(i => i.Date).
				FirstOrDefault();

			if (lastCreditHistoryDate != default) {
				return lastCreditHistoryDate.AddHours(ServerTimeZone - clientTimeZone).AddMinutes(SOAPNBCH.ReportCanReaplyMinute) < DateTime.Now;
			}

			return true;
		}

		/// <summary>
		/// Проверить корректность входных данных метода ReportCheckParams.
		/// </summary>
		/// <param name="url">URL сервиса</param>
		/// <param name="request">Запрос</param>
		/// <param name="account1CCode">Номер договора</param>
		/// <param name="client1CCode">Код клиента</param>
		/// <param name="clientTimeZone">Часовой пояс клиента</param>
		private void GetReportCheckParams(string url, ProductRequest request, string account1CCode, string client1CCode, int clientTimeZone) {
			if (url == default)
				LogAndThrowException<ArgumentNullException, ServiceNBCHsoap>(
					_Logger, nameof(url),
					"Не задан адрес веб службы НБКИ./* Метод {methodName}.*/",
					"GetReportCheckParams");
	
			if (string.IsNullOrEmpty(account1CCode))
				LogAndThrowException<ArgumentNullException, ServiceNBCHsoap>(
					_Logger, nameof(account1CCode),
					"Не задан номер договора 1С./* Метод {methodName}.*/",
					"GetReportCheckParams");
	
			if (string.IsNullOrEmpty(client1CCode))
				LogAndThrowException<ArgumentNullException, ServiceNBCHsoap>(
					_Logger, nameof(client1CCode),
					"Не задан код клиента 1С./* Метод {methodName}.*/",
					"GetReportCheckParams");
			
			if (request == default)
				LogAndThrowException<ArgumentNullException, ServiceNBCHsoap>(
					_Logger, nameof(client1CCode),
					"Нет данных для запроса анкеты НБКИ./* Метод {methodName}.*/",
					"GetReportCheckParams");

			if (clientTimeZone == default)
				LogAndThrowException<ArgumentNullException, ServiceNBCHsoap>(
					_Logger, nameof(client1CCode),
					"Не задан часовой пояс клиента./* Метод {methodName}.*/",
					"GetReportCheckParams");
		}
	}
}
