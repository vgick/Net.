using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NBCH_EF.Helpers;
using NBCH_EF.Tables;
using NBCH_LIB;
using NBCH_LIB.Interfaces;
using NBCH_LIB.Interfaces.WCF;
using NBCH_LIB.Logger;
using NBCH_LIB.Models.PDN;
using NBCH_LIB.SOAP.SOAPNBCH;
using static NBCH_EF.MKKContext;
using static NBCH_LIB.Logger.ExceptionLog;

namespace NBCH_EF.Services {
	public class EFServicePDN : IServicePDN, IServicePDNWCF {
		/// <summary>
		/// Логгер.
		/// </summary>
		private static readonly ILogger<EFServicePDN> _Logger;

		/// <summary>
		/// Статический конструктор.
		/// </summary>
		static EFServicePDN() {
			_Logger	= MKKContext.LoggerFactory.CreateLogger<EFServicePDN>();
		}

		/// <summary>
		/// Данные ПДН
		/// </summary>
		/// <param name="accounts">Список договоров 1С</param>
		/// <returns>ПДН договоров</returns>
		public PdnResult[] GetPDNPercents(string[] accounts) {
			List<PDNResultDB> pdnList	= new List<PDNResultDB>();
			Object lockObject			= new object();

			int countOfIteration = (int)Math.Ceiling(((accounts.Length / (decimal)MaxCountOfParameters)));

			Parallel.For(0, countOfIteration, (part) => {
				IEnumerable<string> accountPart	= accounts.Skip(MaxCountOfParameters * part).Take(MaxCountOfParameters);
				PDNResultDB[] pdnPart			= GetPdnsAsync(accountPart, CancellationToken.None).ResultAndThrowException();

				lock (lockObject) {
					pdnList.AddRange(pdnPart);
				}
			});

			return pdnList.AsParallel().Select(i => (PdnResult)i).ToArray();
		}

		/// <summary>
		/// Данные ПДН асинхронно.
		/// </summary>
		/// <param name="accounts">Список договоров 1С</param>
		/// <returns>ПДН договоров</returns>
		public async Task<PdnResult[]> GetPDNPercentsAsync(string[] accounts) =>
			await GetPDNPercentsAsync(accounts, CancellationToken.None);

		/// <summary>
		/// Данные ПДН асинхронно.
		/// </summary>
		/// <param name="accounts">Список договоров 1С</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns>ПДН договоров</returns>
		public async Task<PdnResult[]> GetPDNPercentsAsync(string[] accounts, CancellationToken cancellationToken) => 
			await Task.Run(() => GetPDNPercents(accounts), cancellationToken);

		/// <summary>
		/// Получить список ПДН асинхронно.
		/// </summary>
		/// <param name="accounts">Договора для которых необходимо получить ПДН</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns>Записи ПДН</returns>
		private static async Task<PDNResultDB[]> GetPdnsAsync(IEnumerable<string> accounts, CancellationToken cancellationToken) {
			using (IDBSource dbSource = new MKKContext()) {
				return await dbSource.PDNResultDBs.
						AsNoTracking().
						Include(i => i.Account1C).
						Where(i => accounts.Contains(i.Account1C.Account1CCode)).
						Select(i => i).
						ToArrayAndLogErrorAsync<PDNResultDB, EFServicePDN>(cancellationToken);
			}
		}

		/// <summary>
		/// Вернуть все записи в которых ПДН больше 50%
		/// </summary>
		public string[] GetFullRecordOver50P() => GetFullRecordOver50PAsync(CancellationToken.None).ResultAndThrowException();

		/// <summary>
		/// Вернуть все записи в которых ПДН больше 50% асинхронно.
		/// </summary>
		public async Task<string[]> GetFullRecordOver50PAsync() => await GetFullRecordOver50PAsync(CancellationToken.None);

		/// <summary>
		/// Вернуть все записи в которых ПДН больше 50% асинхронно.
		/// </summary>
		/// <param name="cancellationToken">Токен отмены</param>
		public async Task<string[]> GetFullRecordOver50PAsync(CancellationToken cancellationToken) {
			using (IDBSource dbSource = new MKKContext()) {
				return await dbSource.PDNResultDBs.
					AsNoTracking().
					Include(i => i.Account1C).
					Where(i => i.Percent >= 50).
					Select(i => i.Account1C.Account1CCode).
					ToArrayAndLogErrorAsync<string, EFServicePDN>(cancellationToken);
			}
		}

		/// <summary>
		/// Рассчитать ПДН по коду клиента 1С.
		/// </summary>
		/// <param name="accountDate">Дата заведения счета</param>
		/// <param name="client1CCode">Код клиента 1С</param>
		/// <param name="account1CCode">Код договора 1С, для которого требуется рассчитать ПДН</param>
		/// <returns>ПДН и дата анкеты, на основании которой был рассчитан ПДН</returns>
		public PDNInfoList CalculatePDN(string account1CCode, DateTime accountDate, string client1CCode) =>
			CalculatePDNAsync(account1CCode, accountDate, client1CCode, CancellationToken.None).ResultAndThrowException();

		/// <summary>
		/// Рассчитать ПДН по коду клиента 1С асинхронно.
		/// </summary>
		/// <param name="accountDate">Дата заведения счета</param>
		/// <param name="client1CCode">Код клиента 1С</param>
		/// <param name="account1CCode">Код договора 1С, для которого требуется рассчитать ПДН</param>
		/// <returns>ПДН и дата анкеты, на основании которой был рассчитан ПДН</returns>
		public async Task<PDNInfoList> CalculatePDNAsync(string account1CCode, DateTime accountDate, string client1CCode) =>
			await CalculatePDNAsync(account1CCode, accountDate, client1CCode, CancellationToken.None);

		/// <summary>
		/// Рассчитать ПДН по коду клиента 1С асинхронно.
		/// </summary>
		/// <param name="accountDate">Дата заведения счета</param>
		/// <param name="client1CCode">Код клиента 1С</param>
		/// <param name="account1CCode">Код договора 1С, для которого требуется рассчитать ПДН</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns>ПДН и дата анкеты, на основании которой был рассчитан ПДН</returns>
		public async Task<PDNInfoList> CalculatePDNAsync(string account1CCode, DateTime accountDate, string client1CCode,
			CancellationToken cancellationToken) {
			
			CalculatePDNCheckParams(account1CCode, accountDate, client1CCode);
			
			PDN[] pdnInfos;
			DateTime reportDate;
			int creditHistoryID;

			CreditHistory creditHistory = 
				await EFServiceNBCH.GetCreditHistoryByAccount1CCodeAsync<EFServicePDN>(client1CCode, accountDate, cancellationToken);
			if (creditHistory != default) {
				ProductResponse productResponse = EFServiceNBCH.GetNBCHXml(creditHistory);

				pdnInfos		= PDN.CreatePDNObgect(productResponse?.Preply?.Report?.AccountReply, Helper.EndOfDay(creditHistory.Date));
				reportDate		= creditHistory.Date;
				creditHistoryID	= creditHistory.ID;
			}
			else { throw new PDNAnketaNotFoundException("Нет подходящей для расчета анкеты НБКИ.\n"); }

			return new PDNInfoList(pdnInfos) { ReportDate = reportDate, CreditHistoryID = creditHistoryID, Account1CID = account1CCode, Account1CDate = accountDate };
		}

		private void CalculatePDNCheckParams(string account1CCode, DateTime accountDate, string client1CCode) {
			if (string.IsNullOrEmpty(client1CCode))
				LogAndThrowException<ArgumentNullException, EFServicePDN>(
					_Logger, nameof(client1CCode),
					"Не задан код клиента 1С./* Метод {methodName}.*/",
					"CalculatePDNCheckParams");

			if (string.IsNullOrEmpty(account1CCode))
				LogAndThrowException<ArgumentNullException, EFServicePDN>(
					_Logger, nameof(account1CCode),
					"Не задан номер договора 1С./* Метод {methodName}.*/",
					"CalculatePDNCheckParams");

			if (accountDate == default)
				LogAndThrowException<ArgumentNullException, EFServicePDN>(
					_Logger, nameof(accountDate),
					"Не задана дата договора 1С./* Метод {methodName}.*/",
					"CalculatePDNCheckParams");
		}

		/// <summary>
		/// Получить сохраненный ПДН по номеру договору 1С.
		/// </summary>
		/// <param name="account1CCode">Номер договора из 1С</param>
		/// <returns>ПДН и дата анкеты, на основании которой был рассчитан ПДН</returns>
		public PDNInfoList GetSavedPDN(string account1CCode) =>
			GetSavedPDNAsync(account1CCode, CancellationToken.None).ResultAndThrowException();

		/// <summary>
		/// Получить сохраненный ПДН по номеру договору 1С асинхронно.
		/// </summary>
		/// <param name="account1CCode">Номер договора из 1С</param>
		/// <returns>ПДН и дата анкеты, на основании которой был рассчитан ПДН</returns>
		public async Task<PDNInfoList> GetSavedPDNAsync(string account1CCode) =>
			await GetSavedPDNAsync(account1CCode, CancellationToken.None);

		/// <summary>
		/// Получить сохраненный ПДН по номеру договору 1С асинхронно.
		/// </summary>
		/// <param name="account1CCode">Номер договора из 1С</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns>ПДН и дата анкеты, на основании которой был рассчитан ПДН</returns>
		public async Task<PDNInfoList> GetSavedPDNAsync(string account1CCode, CancellationToken cancellationToken) {
			GetSavedPDNCheckParams(account1CCode);

			using (IDBSource dbSource = new MKKContext()) {
				Account1C account1C	= await FindAccountAsync(account1CCode, cancellationToken, i => i.PDNData);
				if ((account1C?.PDNCreditHistoryAnket ?? default) != default) {
					PDNData[] pdnData			= await dbSource.PDNDatas.
						Include(i => i.Account1C).
						Where(i => i.Account1C.Account1CCode.Equals(account1CCode)).
						ToArrayAndLogErrorAsync<PDNData, EFServicePDN>(cancellationToken);
					
					CreditHistory creditHistory	= await dbSource.CreditHistories.Where(i => i.ID == account1C.PDNCreditHistoryAnket).
						FirstOrDefaultAndLogErrorAsync<CreditHistory, EFServicePDN>(cancellationToken);

					return new PDNInfoList {
						PDNCards		= pdnData.Where(i => i.PDNCalculateType == PDNCalculateType.Card)
							.Select(i => (PDNCard)i).ToArray(),
						PDNNonCards		= pdnData.Where(i => i.PDNCalculateType == PDNCalculateType.NonCard).Select(i => {
							PDNNonCard result = i;
							return result;
						}).ToArray(),
						CreditHistoryID	= creditHistory.ID,
						ReportDate		= creditHistory.Date,
						Account1CID		= account1CCode
					};
				}
			}

			return default;
		}

		/// <summary>
		/// Проверить входные параметры GetSavedPDN
		/// </summary>
		/// <param name="account1CCode">Номер договора</param>
		private void GetSavedPDNCheckParams(string account1CCode) {
			if (string.IsNullOrEmpty(account1CCode))
				LogAndThrowException<ArgumentNullException, EFServicePDN>(
					_Logger, nameof(account1CCode),
					"Не задан номер договора 1С./* Метод {methodName}.*/",
					"GetSavedPDNCheckParams");
		}

		/// <summary>
		/// Сохранить ПДН.
		/// </summary>
		/// <param name="pdnInfoList">Данные ПДН</param>
		public void SavePDN(PDNInfoList pdnInfoList) =>
			SavePDNAsync(pdnInfoList, CancellationToken.None).WaitAndThrowException();

		/// <summary>
		/// Сохранить ПДН асинхронно.
		/// </summary>
		/// <param name="pdnInfoList">Данные ПДН</param>
		public async Task SavePDNAsync(PDNInfoList pdnInfoList) =>
			await SavePDNAsync(pdnInfoList, CancellationToken.None);

		/// <summary>
		/// Сохранить ПДН асинхронно.
		/// </summary>
		/// <param name="pdnInfoList">Данные ПДН</param>
		/// <param name="cancellationToken">Токен отмены</param>
		public async Task SavePDNAsync(PDNInfoList pdnInfoList, CancellationToken cancellationToken) {
			SavePDNCheckParams(pdnInfoList);

			using (IDBSource dbSource = new MKKContext()) {
				CreditHistory creditHistory	= await dbSource.CreditHistories.
					Where(i => i.ID == pdnInfoList.CreditHistoryID).
					FirstOrDefaultAndLogErrorAsync<CreditHistory, EFServicePDN>(cancellationToken);
				
				Account1C account1C			= await FindAccountAndLogErrorAsync<EFServicePDN>(
					pdnInfoList.Account1CID,
					cancellationToken,
					i => i.City, i => i.PDNData, i => i.GuarantorDBs,
					i => i.Client, i => i.Organization);

				if (account1C == default) {
					if (pdnInfoList.Account1CDate == default)
						LogAndThrowException<ArgumentNullException, EFServicePDN>(
							_Logger, nameof(pdnInfoList.Account1CID),
							"В базе нет информации по договору '{pdnInfoList.Account1CID}' и нет данных о дате договора, для его добавления./* Метод {methodName}, договор {pdnInfoList.Account1CID}.*/",
							"SavePDNAsync");

					account1C	= new Account1C() {
						Account1CCode	= pdnInfoList.Account1CID,
						DateTime		= pdnInfoList.Account1CDate,
					};
					await dbSource.Account1Cs.AddAsync(account1C, cancellationToken);
					await dbSource.SaveChangesAndLogErrorAsync<EFServicePDN>(new LogShortMessage(
						"При добавлении ПДН, договор не найден, а новый создать не удалось./* Метод {methodName}, pdnInfoList {pdnInfoList}.*/",
						"SavePDNAsync", pdnInfoList),
						cancellationToken
					);
				}
				else {
					await DeletePDNDataAsync(account1C, cancellationToken);
					AttachAndLogError<Account1C, EFServicePDN>(dbSource, account1C);
				}

				List<PDNData> pdnData = GetPDNToSave(account1C, pdnInfoList, cancellationToken);

				account1C.Payments				= pdnData.Sum(i => i?.Payment ?? 0);
				account1C.PDNCreditHistoryAnket	= creditHistory.ID;
				account1C.PDNError				= pdnData.Any(i => !String.IsNullOrEmpty(i.Error));
				account1C.PDNManual				= pdnInfoList.Manual;
				account1C.PDNAccept				= pdnInfoList.PDNAccept;

				foreach (PDNData data in pdnData) {
					await dbSource.PDNDatas.AddAsync(data, cancellationToken);
				}

				await dbSource.SaveChangesAndLogErrorAsync<EFServiceNBCH>(new LogShortMessage(
					"Не удалось сохранить данные ПДН./* Метод {methodName} pdnInfoList: {pdnInfoList}.*/",
					"SavePDNAsync", pdnInfoList),
					cancellationToken
				);
			}
		}

		/// <summary>
		/// Подготовить ПДН для сохранения в БД
		/// </summary>
		/// <param name="account1C">Договор</param>
		/// <param name="pdnInfoList">ПДН</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns></returns>
		private List<PDNData> GetPDNToSave(Account1C account1C, PDNInfoList pdnInfoList, CancellationToken cancellationToken) {
			List<PDNData> pdnData = new List<PDNData>();
			pdnData.AddRange(pdnInfoList.PDNCards.AsParallel().WithCancellation(cancellationToken).Select(i => {
				PDNData data	= i;
				data.Account1C	= account1C;
				data.Payment	= Math.Round(i.CalculatePayment(), 2);
				return data;
			}));
			pdnData.AddRange(pdnInfoList.PDNNonCards.AsParallel().WithCancellation(cancellationToken).Select(i => {
				PDNData data	= i;
				data.Account1C	= account1C;
				data.Payment	= Math.Round(i.CalculatePayment(pdnInfoList.ReportDate, pdnInfoList.PDNAccept), 2);
				return data;
			}));

			return pdnData;
		}

		/// <summary>
		/// Удалить данные ПДН по договору асинхронно.
		/// </summary>
		/// <param name="account1C">Договор 1с</param>
		/// <param name="cancellationToken">Токен отмены</param>
		private async Task DeletePDNDataAsync(Account1C account1C, CancellationToken cancellationToken) {
			using (IDBSource dbSource = new MKKContext()) {
				AttachAndLogError<Account1C, EFServicePDN>(dbSource, account1C);

				PDNData[] pdnDatas = account1C.PDNData?.ToArray() ?? new PDNData[0];
				if (pdnDatas.Length > 0) {
					account1C.PDNCreditHistoryAnket	= default;
					account1C.PDNData				= default;
					account1C.PDNError				= default;
					account1C.PDNManual				= false;
					account1C.PDNAccept				= false;
					dbSource.PDNDatas.RemoveRange(pdnDatas);

					await dbSource.SaveChangesAsync(cancellationToken);
				}
			}
		}

		/// <summary>
		/// Проверка входных параметров SavePDNAsync
		/// </summary>
		/// <param name="pdnInfoList"></param>
		private static void SavePDNCheckParams(PDNInfoList pdnInfoList) {
			if (pdnInfoList == default)
				LogAndThrowException<ArgumentNullException, EFServicePDN>(
					_Logger, nameof(pdnInfoList),
					"Данные для сохранения расчета ПДН не заданы./* Метод {methodName}.*/",
					"SavePDNCheckParams");

			if (pdnInfoList?.Account1CID == default)
				LogAndThrowException<ArgumentNullException, EFServicePDN>(
					_Logger, nameof(pdnInfoList.Account1CID),
					"Не задан номер договора 1С для сохранения ПДН./* Метод {methodName}, pdnInfoList {pdnInfoList}.*/",
					"SavePDNCheckParams", pdnInfoList);
		}

		/// <summary>
		/// Получить список договоров с ошибками при расчете ПДН.
		/// </summary>
		/// <returns>Список договоров с ошибками в расчете ПДН</returns>
		public PDNErrorAccountInfo[] GetAccountsWithPDNError() =>
			GetAccountsWithPDNErrorAsync(CancellationToken.None).ResultAndThrowException();

		/// <summary>
		/// Получить список договоров с ошибками при расчете ПДН асинхронно.
		/// </summary>
		/// <returns>Список договоров с ошибками в расчете ПДН</returns>
		public async Task<PDNErrorAccountInfo[]> GetAccountsWithPDNErrorAsync() =>
			await GetAccountsWithPDNErrorAsync(CancellationToken.None);

		/// <summary>
		/// Получить список договоров с ошибками при расчете ПДН асинхронно.
		/// </summary>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns>Список договоров с ошибками в расчете ПДН</returns>
		public async Task<PDNErrorAccountInfo[]> GetAccountsWithPDNErrorAsync(CancellationToken cancellationToken) {
			using (IDBSource dbSource = new MKKContext()) {
				return await
					(from acc in dbSource.Account1Cs.AsNoTracking()
						join ch in dbSource.CreditHistories on acc.PDNCreditHistoryAnket equals ch.ID
						where acc.PDNError == true && acc.PDNAccept == false
						orderby acc.DateTime descending
						select new PDNErrorAccountInfo { Account1CCode = acc.Account1CCode, ReportDate = ch.Date, CreditHistoryID = acc.PDNCreditHistoryAnket }).
					ToArrayAndLogErrorAsync<PDNErrorAccountInfo, EFServicePDN>(cancellationToken);
			}
		}
	}
}
