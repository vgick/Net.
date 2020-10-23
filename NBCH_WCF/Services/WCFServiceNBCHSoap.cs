using System.Security.Permissions;
using System.Threading;
using System.Threading.Tasks;
using NBCH_LIB.Interfaces;
using NBCH_LIB.Interfaces.WCF;
using NBCH_LIB.Logger;
using NBCH_LIB.SOAP.SOAPNBCH;
using static NBCH_WCF.Services.Utils;
using static NBCH_WCF.Services.ServiceMethod;

namespace NBCH_WCF.Services {
	/// <summary>
	/// Сервис, реализующего интерфейс IServiceNBCH для работы с веб службой НБКИ.
	/// </summary>
	public class WCFServiceNBCHSoap : IServiceNBCHsoapWCF {
		/// <summary>
		/// Получить КИ клиента из НБКИ.
		/// </summary>
		/// <param name="url">Урл веб службы</param>
		/// <param name="request">Данные клиента</param>
		/// <param name="account1CCode">Номер договора в 1С</param>
		/// <param name="client1CCode">Код клиента в 1С</param>
		/// <param name="clientTimeZone">Часовой пояс, откуда пришел запрос</param>
		/// <returns>КИ из НБКИ</returns>
		[PrincipalPermission(SecurityAction.Demand, Role = @"admin")]
		public Report GetReport(string url, ProductRequest request, string account1CCode, string client1CCode, int clientTimeZone) =>
			ExecuteWithTryCatch<Report, WCFServiceNBCHSoap>(() =>
				ServiceNBCHsoap.GetReport(url, request, account1CCode, client1CCode, clientTimeZone),
				new LogShortMessage("Ошибка вызова метода. Метод: {methodName}, url: {url}, request: {request}," +
				                    " account1CCode: {account1CCode}, client1CCode: {client1CCode}," +
				                    " clientTimeZone: {clientTimeZone}.",
					"GetReport", url, request, account1CCode, client1CCode, clientTimeZone));

		/// <summary>
		/// Получить КИ клиента из НБКИ асинхронно.
		/// </summary>
		/// <param name="url">Урл веб службы</param>
		/// <param name="request">Данные клиента</param>
		/// <param name="account1CCode">Номер договора в 1С</param>
		/// <param name="client1CCode">Код клиента в 1С</param>
		/// <param name="clientTimeZone">Часовой пояс, откуда пришел запрос</param>
		/// <returns>КИ из НБКИ</returns>
		[PrincipalPermission(SecurityAction.Demand, Role = @"admin")]
		public async Task<Report> GetReportAsync(string url, ProductRequest request, string account1CCode,
			string client1CCode, int clientTimeZone)	=>

			await ExecuteWithTryCatchAsync<Report, WCFServiceNBCHSoap>(() =>
				ServiceNBCHsoap.GetReportAsync(url, request, account1CCode, client1CCode, clientTimeZone),
				new LogShortMessage("Ошибка вызова метода. Метод: {methodName}, url: {url}, request: {request}," +
				                    " account1CCode: {account1CCode}, client1CCode: {client1CCode}," +
				                    " clientTimeZone: {clientTimeZone}.",
					"GetReportAsync", url, request, account1CCode, client1CCode, clientTimeZone));
	}
}
