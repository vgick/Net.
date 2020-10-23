using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NBCH_EF.Helpers;
using NBCH_EF.Tables;
using NBCH_LIB;
using NBCH_LIB.Interfaces.WCF;
using NBCH_LIB.Logger;
using NBCH_LIB.Models;
using NBCH_LIB.Models.PDN;
using NBCH_LIB.SOAP.SOAPNBCH;
using static NBCH_EF.MKKContext;
using static NBCH_LIB.Helper;
using static NBCH_LIB.Logger.ExceptionLog;

namespace NBCH_EF.Services {
	public class EFServiceNBCH : IServiceNBCHFull {
		/// <summary>
		/// Логгер.
		/// </summary>
		private static readonly ILogger<EFServiceNBCH> _Logger;

		static EFServiceNBCH() {
			_Logger = MKKContext.LoggerFactory.CreateLogger<EFServiceNBCH>();
		}

		/// <summary>
		/// Получить КИ по ID асинхронно.
		/// </summary>
		/// <param name="creditHistoryID">ID кредитной истории в базе</param>
		/// <returns>Кредитная история</returns>
		public Report GetSavedReport(int creditHistoryID) =>
			GetSavedReportAsync(creditHistoryID, CancellationToken.None).ResultAndThrowException();

		/// <summary>
		/// Получить КИ по ID асинхронно.
		/// </summary>
		/// <param name="creditHistoryID">ID кредитной истории в базе</param>
		/// <returns>Кредитная история</returns>
		public async Task<Report> GetSavedReportAsync(int creditHistoryID) =>
			await GetSavedReportAsync(creditHistoryID, CancellationToken.None);

		/// <summary>
		/// Получить КИ по ID асинхронно.
		/// </summary>
		/// <param name="creditHistoryID">ID кредитной истории в базе</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns>Кредитная история</returns>
		public async Task<Report> GetSavedReportAsync(int creditHistoryID, CancellationToken cancellationToken) {
			GetSavedReportCheckParams(creditHistoryID);

			Report report;

			CreditHistory creditHistory	= await GetCreditHistoryByIDAsync(creditHistoryID, cancellationToken);
			if (creditHistory != default) {
				ProductResponse productResponse	= GetNBCHXml(creditHistory);

				report	= productResponse.Preply.Report ?? new Report();
				report.ErrorMessage		= productResponse.Preply?.Error?.CtErr?.Text ?? "";
				report.Client1CCode		= creditHistory.Client.Code1C;
				report.calc.ReportDate	= creditHistory.Date;
			}
			else {
				report = new Report { ErrorMessage = "Анкета не найдена" };
			}

			return report;
		}

		/// <summary>
		/// Проверить входные параметры GetSavedReport
		/// </summary>
		/// <param name="creditHistoryID"></param>
		private void GetSavedReportCheckParams(int creditHistoryID) {
			if (creditHistoryID == default)
				LogAndThrowException<ArgumentNullException, EFServiceNBCH>(
					_Logger, nameof(creditHistoryID),
					"Не задан ID кредитной анкеты./* Метод {methodName}.*/",
					"GetSavedReportCheckParams");
		}

		/// <summary>
		/// Получить КИ по ID асинхронно.
		/// </summary>
		/// <param name="creditHistoryID"></param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns>Кредитная история</returns>
		private async Task<CreditHistory> GetCreditHistoryByIDAsync(int creditHistoryID, CancellationToken cancellationToken) =>
			await FindDBRecordByIDdAndLogErrorAsync<CreditHistory, EFServiceNBCH>(creditHistoryID, cancellationToken, i => i.Client);


		/// <summary>
		/// Получить список сохраненных КИ по коду клиента 1С.
		/// </summary>
		/// <param name="client1CCode">Код клиента 1С</param>
		/// <returns>Список сохраненных КИ</returns>
		public CreditHistoryInfo[] GetCreditHistoryList(string client1CCode) =>
			GetCreditHistoryListAsync(client1CCode, CancellationToken.None).ResultAndThrowException();

		/// <summary>
		/// Получить список сохраненных КИ по коду клиента 1С асинхронно.
		/// </summary>
		/// <param name="client1CCode">Код клиента 1С</param>
		/// <returns>Список сохраненных КИ</returns>
		public async Task<CreditHistoryInfo[]> GetCreditHistoryListAsync(string client1CCode) =>
			await GetCreditHistoryListAsync(client1CCode, CancellationToken.None);

		/// <summary>
		/// Получить список сохраненных КИ по коду клиента 1С асинхронно.
		/// </summary>
		/// <param name="client1CCode">Код клиента 1С</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns>Список сохраненных КИ</returns>
		public async Task<CreditHistoryInfo[]> GetCreditHistoryListAsync(string client1CCode, CancellationToken cancellationToken) {
			GetCreditHistoryListCheckParams(client1CCode);

			using (IDBSource dbSource = new MKKContext()) {
				return await dbSource.CreditHistories.
					AsNoTracking().
					Where(i => i.Client.Code1C.Equals(client1CCode)).
					Select(i => new CreditHistoryInfo() {
						CreditHistoryID	= i.ID,
						Date			= i.Date,
						ErrorText		= i.ErrorText,
						ErrorCode		= i.ErrorCode
					}).
					OrderByDescending(o => o.Date).
					ToArrayAndLogErrorAsync<CreditHistoryInfo, EFServiceNBCH>(cancellationToken);
			}
		}

		/// <summary>
		/// Проверить входные параметры GetCreditHistoryList
		/// </summary>
		/// <param name="client1CCode"></param>
		private void GetCreditHistoryListCheckParams(string client1CCode) {
			if (string.IsNullOrEmpty(client1CCode))
				LogAndThrowException<ArgumentNullException, EFServiceNBCH>(
					_Logger, nameof(client1CCode),
					"Не задан код клиента 1C./* Метод {methodName}.*/",
					"GetCreditHistoryListCheckParams");
		}

		/// <summary>
		/// Поиск клиента в базе.
		/// </summary>
		/// <param name="fio">часть имени клиента</param>
		/// <returns>Совпадения</returns>
		public SearchClientList[] SearchClient(string fio) =>
			SearchClientAsync(fio, CancellationToken.None).ResultAndThrowException();

		/// <summary>
		/// Поиск клиента в базе.
		/// </summary>
		/// <param name="fio">часть имени клиента</param>
		/// <returns>Совпадения</returns>
		public async Task<SearchClientList[]> SearchClientAsync(string fio) =>
			await SearchClientAsync(fio, CancellationToken.None);

		/// <summary>
		/// Поиск клиента в базе.
		/// </summary>
		/// <param name="fio">часть имени клиента</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns>Совпадения</returns>
		public async Task<SearchClientList[]> SearchClientAsync(string fio, CancellationToken cancellationToken) {
			SearchClientAsyncCheckParams(fio);

			using (IDBSource dbSource = new MKKContext()) {
				return await dbSource.Clients.
					AsNoTracking().
					Where(i => i.FIO.Contains(fio)).
					Select(i => new SearchClientList() {
						BirthDate	= i.BirthDate,
						FIO			= i.FIO,
						ClientID	= i.ID,
						ClientID1C	= i.Code1C
					}
				).ToArrayAndLogErrorAsync<SearchClientList, EFServiceNBCH>(cancellationToken);
			}
		}

		/// <summary>
		/// Проверить входные параметры SearchClient.
		/// </summary>
		/// <param name="fio">Часть фио клиента</param>
		private void SearchClientAsyncCheckParams(string fio) {
			if (string.IsNullOrEmpty(fio))
				LogAndThrowException<ArgumentNullException, EFServiceNBCH>(
					_Logger, nameof(fio),
					"Не задана часть ФИО для поиска./* Метод {methodName}.*/",
					"SearchClientAsyncCheckParams");
		}

		/// <summary>
		/// Получить список сохраненных КИ клиента по коду анкеты асинхронно.
		/// </summary>
		/// <param name="creditHistoryID">Код анкеты в базе</param>
		/// <returns>Список сохраненных КИ</returns>
		public CreditHistoryInfo[] GetCreditHistoryListByCreditHistoryID(int creditHistoryID) =>
			GetCreditHistoryListByCreditHistoryIDAsync(creditHistoryID, CancellationToken.None).ResultAndThrowException();

		/// <summary>
		/// Получить список сохраненных КИ клиента по коду анкеты асинхронно.
		/// </summary>
		/// <param name="creditHistoryID">Код анкеты в базе</param>
		/// <returns>Список сохраненных КИ</returns>
		public async Task<CreditHistoryInfo[]> GetCreditHistoryListByCreditHistoryIDAsync(int creditHistoryID) =>
			await GetCreditHistoryListByCreditHistoryIDAsync(creditHistoryID, CancellationToken.None);

		/// <summary>
		/// Получить список сохраненных КИ клиента по коду анкеты асинхронно.
		/// </summary>
		/// <param name="creditHistoryID">Код анкеты в базе</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns>Список сохраненных КИ</returns>
		public async Task<CreditHistoryInfo[]> GetCreditHistoryListByCreditHistoryIDAsync(int creditHistoryID,
			CancellationToken cancellationToken) {
			GetCreditHistoryListByCreditHistoryIDCheckParams(creditHistoryID);

			int clientID = (await GetCreditHistoryByIDAsync(creditHistoryID, cancellationToken))?.ID ?? default;
			if (clientID == default) return new CreditHistoryInfo[0];

			using (IDBSource dbSource = new MKKContext()) {
				return await dbSource.CreditHistories.
					AsNoTracking().
					Where(i => i.Client.ID == clientID).
					Select(i => new CreditHistoryInfo() {
						CreditHistoryID	= i.ID,
						Date			= i.Date,
						ErrorText		= i.ErrorText,
						ErrorCode		= i.ErrorCode
					}).
					OrderByDescending(o => o.Date).
					ToArrayAndLogErrorAsync<CreditHistoryInfo, EFServiceNBCH>(cancellationToken);
			}
		}

		/// <summary>
		/// Проверить входные параметры GetCreditHistoryListByCreditHistoryID
		/// </summary>
		/// <param name="creditHistoryID"></param>
		private void GetCreditHistoryListByCreditHistoryIDCheckParams(int creditHistoryID) {
			if (creditHistoryID == default)
				LogAndThrowException<ArgumentNullException, EFServiceNBCH>(
					_Logger, nameof(creditHistoryID),
					"Не задан ID кредитной анкеты./* Метод {methodName}.*/",
					"GetCreditHistoryListByCreditHistoryIDCheckParams");
		}

		/// <summary>
		/// Вернуть номер последней, сохраненной анкеты асинхронно.
		/// </summary>
		/// <param name="client1CCode">Код клиента 1С</param>
		/// <returns>Номер последней анкеты</returns>
		public int GetClientCreditHistoryID(string client1CCode) =>
			GetClientCreditHistoryIDAsync(client1CCode, CancellationToken.None).ResultAndThrowException();

		/// <summary>
		/// Вернуть номер последней, сохраненной анкеты асинхронно.
		/// </summary>
		/// <param name="client1CCode">Код клиента 1С</param>
		/// <returns>Номер последней анкеты</returns>
		public async Task<int> GetClientCreditHistoryIDAsync(string client1CCode) =>
			await GetClientCreditHistoryIDAsync(client1CCode, CancellationToken.None);

		/// <summary>
		/// Сохранить кредитную историю асинхронно.
		/// </summary>
		/// <param name="account1CCode">Номер кредитного договора в 1С</param>
		/// <param name="client1CCode">Код клиента 1С</param>
		/// <param name="signedXml">Подписанные НБКИ данные</param>
		/// <param name="unSignedXml">Анкета НБКИ без подписи</param>
		/// <param name="clientTimeZone">Часовой пояс на клиенте</param>
		/// <param name="error">Ошибка из сервиса НБКИ</param>
		public async Task WriteCreditHistoryAsync(string account1CCode, string client1CCode, byte[] signedXml,
			byte[] unSignedXml, int clientTimeZone, CTErr error) =>
			await WriteCreditHistoryAsync(account1CCode, client1CCode, signedXml, unSignedXml, clientTimeZone, error,
				CancellationToken.None);

		/// <summary>
		/// Вернуть номер последней, сохраненной анкеты асинхронно.
		/// </summary>
		/// <param name="client1CCode">Код клиента 1С</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns>Номер последней анкеты</returns>
		public async Task<int> GetClientCreditHistoryIDAsync(string client1CCode, CancellationToken cancellationToken) {
			GetClientCreditHistoryIDCheckParams(client1CCode);

			using (IDBSource dbSource = new MKKContext()) {
				return await(from cl in dbSource.Clients.AsNoTracking()
					join ch in dbSource.CreditHistories on cl.ID equals ch.Client.ID
					where cl.Code1C.Equals(client1CCode)
					select ch.ID).
					MaxAsync(cancellationToken);
			}
		}

		/// <summary>
		/// Проверить входные параметры GetCreditHistoryList
		/// </summary>
		/// <param name="client1CCode"></param>
		private void GetClientCreditHistoryIDCheckParams(string client1CCode) {
			if (string.IsNullOrEmpty(client1CCode))
				LogAndThrowException<ArgumentNullException, EFServiceNBCH>(
					_Logger, nameof(client1CCode),
					"Не задан код клиента 1C./* Метод {methodName}.*/",
					"GetClientCreditHistoryIDCheckParams");
		}

		/// <summary>
		/// Получить кредитную историю по коду клиента 1С асинхронно.
		/// Берутся анкеты полученные в день заведения договора, или ранее полученные по этому клиенту.
		/// </summary>
		/// <param name="client1CCode">Код клиента 1С</param>
		/// <param name="accountDate">Дата расчета</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns>Кредитная история</returns>
		internal static async Task<CreditHistory> GetCreditHistoryByAccount1CCodeAsync<TLoggerClass>(string client1CCode,
				DateTime accountDate, CancellationToken cancellationToken) where TLoggerClass: class {
			GetCreditHistoryByAccount1CCodeCheckParams<TLoggerClass>(client1CCode, accountDate);

			using (IDBSource dbSource = new MKKContext()) {
				CreditHistory[] creditHistorys = await
					(from acc in dbSource.Account1Cs.AsNoTracking()
						join ch in dbSource.CreditHistories on acc.Account1CCode equals ch.Account1CID.Account1CCode
						join cl in dbSource.Clients on ch.Client.ID equals cl.ID
						orderby ch.Date descending
						where string.Equals(cl.Code1C, client1CCode) && (string.IsNullOrEmpty(ch.ErrorCode) || ch.ErrorCode == SOAPNBCH.ClientNotFoundNBCH)
						select ch).
					ToArrayAndLogErrorAsync<CreditHistory, EFServicePDN>(cancellationToken);

				foreach (var creditHistory in creditHistorys) {
					if (accountDate.AddDays(PDN.MaxDayAfter) >= creditHistory.Date)
						return creditHistory;
				}
			}

			return default;
		}

		private static void GetCreditHistoryByAccount1CCodeCheckParams<TLoggerClass>(string client1CCode, DateTime accountDate) where TLoggerClass: class{
			if (string.IsNullOrEmpty(client1CCode))
				LogAndThrowException<ArgumentNullException, TLoggerClass>(
					MKKContext.LoggerFactory.CreateLogger<TLoggerClass>(),
					nameof(client1CCode),
					"Не задан код клиента 1С./* Метод {methodName}.*/",
					"GetCreditHistoryByAccount1CCodeCheckParams");

			if (accountDate == default)
				LogAndThrowException<ArgumentNullException, TLoggerClass>(
					MKKContext.LoggerFactory.CreateLogger<TLoggerClass>(),
					nameof(accountDate),
					"Не задана дата договора 1С./* Метод {methodName}.*/",
					"CalculatePDNCheckParams");
		}

		/// <summary>
		/// Получить данные анкеты из БД.
		/// </summary>
		/// <param name="creditHistory">Анкета в БД</param>
		/// <returns></returns>
		internal static ProductResponse GetNBCHXml(CreditHistory creditHistory) {
			byte[] clearResponse;

			if (creditHistory.UnSignedXML == default) {
				byte[] signedResponse	= creditHistory.SignedXML;
				clearResponse			= SOAPNBCH.RemoveSignature(signedResponse);
			}
			else {
				clearResponse = creditHistory.UnSignedXML;
			}

			ProductResponse productResponse = SOAPNBCH.DeserializeProductRequest(clearResponse);
			return productResponse;
		}


		/// <summary>
		/// Сохранить кредитную историю асинхронно.
		/// </summary>
		/// <param name="account1CCode">Номер кредитного договора в 1С</param>
		/// <param name="client1CCode">Код клиента 1С</param>
		/// <param name="signedXml">Подписанные НБКИ данные</param>
		/// <param name="unSignedXml">Анкета НБКИ без подписи</param>
		/// <param name="clientTimeZone">Часовой пояс на клиенте</param>
		/// <param name="error">Ошибка из сервиса НБКИ</param>
		/// <param name="cancellationToken">Токен отмены</param>
		public async Task WriteCreditHistoryAsync(string account1CCode, string client1CCode, byte[] signedXml,
			byte[] unSignedXml, int clientTimeZone, CTErr error, CancellationToken cancellationToken) {
			WriteCreditHistoryCheckParams(account1CCode, client1CCode, signedXml, unSignedXml, clientTimeZone);

			ClientDB client		= new ClientDB() { Code1C = client1CCode };
			Account1C account1C	= new Account1C() { Account1CCode = account1CCode };
			int serverTimeZone	= ServerTimeZone;

			CreditHistory creditHistory	= new CreditHistory() {
				Account1CID		= account1C,
				Client			= client,
				SignedXML		= signedXml,
				UnSignedXML		= unSignedXml,
				ClientTimeZone	= clientTimeZone,
				Date			= DateTime.Now.AddHours(clientTimeZone - serverTimeZone),
				ErrorCode		= error?.Code,
				ErrorText		= error?.Text
			};

			await SaveCreditHistoryAsync(creditHistory, cancellationToken);
		}

		/// <summary>
		/// Проверить входные параметры WriteCreditHistoryAsync.
		/// </summary>
		/// <param name="account1CCode">Номер договора 1С</param>
		/// <param name="client1CCode">Код клиента 1С</param>
		/// <param name="signedXml">Файл НБКИ подписанный</param>
		/// <param name="unSignedXml">Файл НБКИ без подписи</param>
		/// <param name="clientTimeZone">Часовой пояс клиента</param>
		private static void WriteCreditHistoryCheckParams(string account1CCode, string client1CCode, byte[] signedXml, byte[] unSignedXml, int clientTimeZone) {
			if (string.IsNullOrEmpty(client1CCode))
				LogAndThrowException<ArgumentNullException, EFServiceNBCH>(
					_Logger, nameof(client1CCode),
					"Не задан код клиента 1C./* Метод {methodName}.*/",
					"WriteCreditHistoryCheckParams");

			if (string.IsNullOrEmpty(account1CCode))
				LogAndThrowException<ArgumentNullException, EFServiceNBCH>(
					_Logger, nameof(client1CCode),
					"Не задан номер договора 1C./* Метод {methodName}.*/",
					"WriteCreditHistoryCheckParams");

			if (signedXml == default)
				LogAndThrowException<ArgumentNullException, EFServiceNBCH>(
					_Logger, nameof(signedXml),
					"Нет подписанной анкеты НБКИ./* Метод {methodName}.*/",
					"WriteCreditHistoryCheckParams");

			if (unSignedXml == default)
				LogAndThrowException<ArgumentNullException, EFServiceNBCH>(
					_Logger, nameof(unSignedXml),
					"Нет анкеты НБКИ без подписи анкеты./* Метод {methodName}.*/",
					"WriteCreditHistoryCheckParams");

			if (clientTimeZone == default)
				LogAndThrowException<ArgumentNullException, EFServiceNBCH>(
					_Logger, nameof(clientTimeZone),
					"Не задан часовой пояс клиента./* Метод {methodName}.*/",
					"WriteCreditHistoryCheckParams");
		}

		/// <summary>
		/// Сохранить кредитную анкету в базу. В переданном объекте CreditHistory, обязательны поля - код клиента и номер договора асинхронно.
		/// </summary>
		/// <param name="creditHistory">Кредитная анкета</param>
		/// <param name="cancellationToken">Токен отмены</param>
		private async Task SaveCreditHistoryAsync(CreditHistory creditHistory, CancellationToken cancellationToken) {
			using (IDBSource dbSource = new MKKContext()) {
				Task<ClientDB> presentClientTask		= FindClientAndLogErrorAsync<EFServiceNBCH>(creditHistory.Client.Code1C, cancellationToken);
				Task<Account1C> presentAccount1CTask	= FindAccountAndLogErrorAsync<EFServiceNBCH>(creditHistory.Account1CID.Account1CCode, cancellationToken);

				ClientDB presentClient		= await AttachFromTaskAndLogErrorAsync<ClientDB, EFServicePDN>(dbSource, presentClientTask);
				Account1C presentAccount1C	= await AttachFromTaskAndLogErrorAsync<Account1C, EFServiceNBCH>(dbSource, presentAccount1CTask);

				if (presentClient == default)
					LogAndThrowException<Exception, EFServiceNBCH>(
						_Logger, "",
						"В базе нет клиента с кодом 1С {creditHistory.Client.Code1C}./* Метод {methodName}.*/",
						creditHistory.Client.Code1C, "SaveCreditHistoryAsync");

				if (presentAccount1C == default)
					LogAndThrowException<Exception, EFServiceNBCH>(
						_Logger, "",
						"В базе нет договора с кодом 1С {creditHistory.Account1CID.Account1CCode}./* Метод {methodName}.*/",
						creditHistory.Account1CID.Account1CCode, "SaveCreditHistoryAsync");

				creditHistory.Client		= presentClient;
				creditHistory.Account1CID	= presentAccount1C;
				await dbSource.CreditHistories.AddAsync(creditHistory, cancellationToken);

				await dbSource.SaveChangesAndLogErrorAsync<EFServiceNBCH>(new LogShortMessage(
					"Не удалось сохранить анкету НБКИ./* Метод {methodName}, creditHistory {creditHistory}.*/",
					"SaveCreditHistoryAsync", creditHistory),
					cancellationToken
				);
			}
		}
	}
}
