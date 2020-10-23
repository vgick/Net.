using System.Security.Permissions;
using System.Threading;
using System.Threading.Tasks;
using NBCH_LIB.Interfaces;
using NBCH_LIB.Interfaces.WCF;
using NBCH_LIB.Logger;
using NBCH_LIB.Models;
using NBCH_LIB.SOAP.SOAPNBCH;
using static NBCH_WCF.Services.Utils;
using static NBCH_WCF.Services.ServiceMethod;

namespace NBCH_WCF.Services {
	/// <summary>
	/// Сервис, реализующего интерфейс IServiceNBCH для работы с анкетами НБКИ.
	/// </summary>
	public class WCFServiceNBCH : IServiceNBCHWCF {
		/// <summary>
		/// Вернуть номер последней, сохраненной анкеты.
		/// </summary>
		/// <param name="client1CCode">Код клиента 1С</param>
		/// <returns>Номер последней анкеты</returns>
		[PrincipalPermission(SecurityAction.Demand, Role = @"admin")]
		public int GetClientCreditHistoryID(string client1CCode) =>
			ExecuteWithTryCatch<int, WCFServiceNBCH>(() => ServiceNBCH.GetClientCreditHistoryID(client1CCode),
				new LogShortMessage("Ошибка вызова метода. Метод: {methodName}, client1CCode: {client1CCode}.",
					"GetClientCreditHistoryID", client1CCode));

		/// <summary>
		/// Вернуть номер последней, сохраненной анкеты асинхронно.
		/// </summary>
		/// <param name="client1CCode">Код клиента 1С</param>
		/// <returns>Номер последней анкеты</returns>
		[PrincipalPermission(SecurityAction.Demand, Role = @"admin")]
		public async Task<int> GetClientCreditHistoryIDAsync(string client1CCode) =>
			await ExecuteWithTryCatchAsync<int, WCFServiceNBCH>(() =>
				ServiceNBCH.GetClientCreditHistoryIDAsync(client1CCode),
				new LogShortMessage("Ошибка вызова метода. Метод: {methodName}, client1CCode: {client1CCode}.",
					"GetClientCreditHistoryIDAsync", client1CCode));

		/// <summary>
		/// Получить список кредитных историй по коду клиента.
		/// </summary>
		/// <param name="client1CCode">Код клиента 1С</param>
		/// <returns>Список кредитных анкет</returns>
		[PrincipalPermission(SecurityAction.Demand, Role = @"admin")]
		public CreditHistoryInfo[] GetCreditHistoryList(string client1CCode) =>
			ExecuteWithTryCatch<CreditHistoryInfo[], WCFServiceNBCH>(() =>
				ServiceNBCH.GetCreditHistoryList(client1CCode),
				new LogShortMessage("Ошибка вызова метода. Метод: {methodName}, client1CCode: {client1CCode}.",
					"GetCreditHistoryList", client1CCode));

		/// <summary>
		/// Получить список кредитных историй по коду клиента асинхронно.
		/// </summary>
		/// <param name="client1CCode">Код клиента 1С</param>
		/// <returns>Список кредитных анкет</returns>
		[PrincipalPermission(SecurityAction.Demand, Role = @"admin")]
		public async Task<CreditHistoryInfo[]> GetCreditHistoryListAsync(string client1CCode) =>
			await ExecuteWithTryCatchAsync<CreditHistoryInfo[], WCFServiceNBCH>(() =>
				ServiceNBCH.GetCreditHistoryListAsync(client1CCode),
				new LogShortMessage("Ошибка вызова метода. Метод: {methodName}, client1CCode: {client1CCode}.",
					"GetCreditHistoryListAsync", client1CCode));

		/// <summary>
		/// Получить список сохраненных КИ клиента по коду анкеты.
		/// </summary>
		/// <param name="creditHistoryID">Код анкеты в базе</param>
		/// <returns>Список сохраненных КИ</returns>
		[PrincipalPermission(SecurityAction.Demand, Role = @"admin")]
		public CreditHistoryInfo[] GetCreditHistoryListByCreditHistoryID(int creditHistoryID) =>
			ExecuteWithTryCatch<CreditHistoryInfo[], WCFServiceNBCH>(() =>
				ServiceNBCH.GetCreditHistoryListByCreditHistoryID(creditHistoryID),
				new LogShortMessage("Ошибка вызова метода. Метод: {methodName}, creditHistoryID: {creditHistoryID}.",
					"GetCreditHistoryListByCreditHistoryID", creditHistoryID));

		/// <summary>
		/// Получить список сохраненных КИ клиента по коду анкеты асинхронно.
		/// </summary>
		/// <param name="creditHistoryID">Код анкеты в базе</param>
		/// <returns>Список сохраненных КИ</returns>
		[PrincipalPermission(SecurityAction.Demand, Role = @"admin")]
		public async Task<CreditHistoryInfo[]> GetCreditHistoryListByCreditHistoryIDAsync(int creditHistoryID) =>
			await ExecuteWithTryCatchAsync<CreditHistoryInfo[], WCFServiceNBCH>(() =>
				ServiceNBCH.GetCreditHistoryListByCreditHistoryIDAsync(creditHistoryID),
				new LogShortMessage("Ошибка вызова метода. Метод: {methodName}, creditHistoryID: {creditHistoryID}.",
					"GetCreditHistoryListByCreditHistoryIDAsync", creditHistoryID));

		/// <summary>
		/// Получить КИ по ID.
		/// </summary>
		/// <param name="creditHistoryID"></param>
		/// <returns>Кредитная история</returns>
		[PrincipalPermission(SecurityAction.Demand, Role = @"admin")]
		public Report GetSavedReport(int creditHistoryID) =>
			ExecuteWithTryCatch<Report, WCFServiceNBCH>(() => ServiceNBCH.GetSavedReport(creditHistoryID),
				new LogShortMessage("Ошибка вызова метода. Метод: {methodName}, creditHistoryID: {creditHistoryID}.",
					"GetSavedReport", creditHistoryID));

		/// <summary>
		/// Получить КИ по ID асинхронно.
		/// </summary>
		/// <param name="creditHistoryID"></param>
		/// <returns>Кредитная история</returns>
		[PrincipalPermission(SecurityAction.Demand, Role = @"admin")]
		public async Task<Report> GetSavedReportAsync(int creditHistoryID) =>
			await ExecuteWithTryCatchAsync<Report, WCFServiceNBCH>(() =>
					ServiceNBCH.GetSavedReportAsync(creditHistoryID),
				new LogShortMessage("Ошибка вызова метода. Метод: {methodName}, creditHistoryID: {creditHistoryID}.",
					"GetSavedReportAsync", creditHistoryID));

		/// <summary>
		/// Найти клиентов по совпадению ФИО.
		/// </summary>
		/// <param name="fio"></param>
		/// <returns></returns>
		[PrincipalPermission(SecurityAction.Demand, Role = @"admin")]
		public SearchClientList[] SearchClient(string fio) =>
			ExecuteWithTryCatch<SearchClientList[], WCFServiceNBCH>(() => ServiceNBCH.SearchClient(fio),
				new LogShortMessage("Ошибка вызова метода. Метод: {methodName}, fio: {fio}.", "SearchClient", fio));

		/// <summary>
		/// Найти клиентов по совпадению ФИО асинхронно.
		/// </summary>
		/// <param name="fio"></param>
		/// <returns></returns>
		[PrincipalPermission(SecurityAction.Demand, Role = @"admin")]
		public async Task<SearchClientList[]> SearchClientAsync(string fio) =>
			await ExecuteWithTryCatchAsync<SearchClientList[], WCFServiceNBCH>(() =>
				ServiceNBCH.SearchClientAsync(fio),
				new LogShortMessage("Ошибка вызова метода. Метод: {methodName}, fio: {fio}.",
					"SearchClientAsync", fio));

		/// <summary>
		/// Сохранить кредитную историю асинхронно.
		/// </summary>
		/// <param name="account1CCode">Номер кредитного договора в 1С</param>
		/// <param name="client1CCode">Код клиента 1С</param>
		/// <param name="signedXml">Подписанные НБКИ данные</param>
		/// <param name="unSignedXml">Анкета НБКИ без подписи</param>
		/// <param name="clientTimeZone">Часовой пояс на клиенте</param>
		/// <param name="error">Ошибка из сервиса НБКИ</param>
		[PrincipalPermission(SecurityAction.Demand, Role = @"admin")]
		public async Task WriteCreditHistoryAsync(string account1CCode, string client1CCode, byte[] signedXml,
			byte[] unSignedXml, int clientTimeZone, CTErr error) =>
			
			await ExecuteWithTryCatchAsync<WCFServiceNBCH>(() =>
				ServiceNBCH.WriteCreditHistoryAsync(account1CCode, client1CCode, signedXml, unSignedXml, clientTimeZone,
					error),
				new LogShortMessage("Ошибка вызова метода. Метод: {methodName}, account1CCode: {account1CCode}," +
				            " client1CCode: {client1CCode}, signedXml {signedXml} байт, unSignedXml {unSignedXml} байт," +
				            " clientTimeZone: {clientTimeZone}, error: {error} .",
					"WriteCreditHistoryAsync", account1CCode, client1CCode,
					signedXml?.Length.ToString() ?? "null", unSignedXml?.Length.ToString() ?? "null",
					clientTimeZone, error));
	}
}
