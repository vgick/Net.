using System;
using System.Security.Permissions;
using System.Threading;
using System.Threading.Tasks;
using NBCH_LIB.Interfaces;
using NBCH_LIB.Interfaces.WCF;
using NBCH_LIB.Logger;
using NBCH_LIB.Models.PDN;
using static NBCH_WCF.Services.Utils;
using static NBCH_WCF.Services.ServiceMethod;

namespace NBCH_WCF.Services {
	/// <summary>
	/// Сервис, реализующего интерфейс IServicePDN для работы с данными ПДН.
	/// </summary>
	public class WCFServicePDN : IServicePDNWCF {
		/// <summary>
		/// Данные ПДН.
		/// </summary>
		/// <param name="accounts">Список договоров 1С</param>
		/// <returns>ПДН договоров</returns>
		[PrincipalPermission(SecurityAction.Demand, Role = @"admin")]
		public PdnResult[] GetPDNPercents(string[] accounts) =>
			ExecuteWithTryCatch<PdnResult[], WCFServiceRegistrar>(() => ServicePDN.GetPDNPercents(accounts),
				new LogShortMessage("Ошибка вызова метода. Метод: {methodName} accounts: {accounts}.",
					"GetPDNPercents", string.Join(", ", accounts ?? new [] {"null"})));

		/// <summary>
		/// Данные ПДН асинхронно.
		/// </summary>
		/// <param name="accounts">Список договоров 1С</param>
		/// <returns>ПДН договоров</returns>
		[PrincipalPermission(SecurityAction.Demand, Role = @"admin")]
		public async Task<PdnResult[]> GetPDNPercentsAsync(string[] accounts) =>
			await ExecuteWithTryCatchAsync<PdnResult[], WCFServiceRegistrar>(() =>
					ServicePDN.GetPDNPercentsAsync(accounts),
				new LogShortMessage("Ошибка вызова метода. Метод: {methodName} accounts: {accounts}.",
					"GetPDNPercentsAsync", string.Join(", ", accounts ?? new [] {"null"})));

		/// <summary>
		/// Вернуть все записи в которых ПДН больше 50%.
		/// </summary>
		/// <returns>Номера договоров</returns>
		[PrincipalPermission(SecurityAction.Demand, Role = @"admin")]
		public string[] GetFullRecordOver50P() =>
			ExecuteWithTryCatch<string[], WCFServiceRegistrar>(() => ServicePDN.GetFullRecordOver50P(),
				new LogShortMessage("Ошибка вызова метода. Метод: {methodName}.", "GetFullRecordOver50P"));

		/// <summary>
		/// Вернуть все записи в которых ПДН больше 50% асинхронно.
		/// </summary>
		/// <returns>Номера договоров</returns>
		[PrincipalPermission(SecurityAction.Demand, Role = @"admin")]
		public async Task<string[]> GetFullRecordOver50PAsync() =>
			await ExecuteWithTryCatchAsync<string[], WCFServiceRegistrar>(() =>
					ServicePDN.GetFullRecordOver50PAsync(),
				new LogShortMessage("Ошибка вызова метода. Метод: {methodName}.", "GetFullRecordOver50PAsync"));

		/// <summary>
		/// Рассчитать ПДН по номеру клиента 1С.
		/// </summary>
		/// <param name="account1CCode">Код договора 1С</param>
		/// <param name="accountDate">Дата договора 1С</param>
		/// <param name="client1CCode">Код клиента 1С</param>
		/// <returns>ПДН и дата анкеты, на основании которой был рассчитан ПДН</returns>
		[PrincipalPermission(SecurityAction.Demand, Role = @"admin")]
		public PDNInfoList CalculatePDN(string account1CCode, DateTime accountDate, string client1CCode) =>
			ExecuteWithTryCatch<PDNInfoList, WCFServiceRegistrar>(() => 
				ServicePDN.CalculatePDN(account1CCode, accountDate, client1CCode),
				new LogShortMessage("Ошибка вызова метода. Метод: {methodName} account1CCode: {account1CCode}," +
				                    " accountDate: {accountDate}, client1CCode: {client1CCode}.",
					"CalculatePDN", account1CCode, accountDate, client1CCode));

		/// <summary>
		/// Рассчитать ПДН по номеру клиента 1С асинхронно.
		/// </summary>
		/// <param name="account1CCode">Код договора 1С</param>
		/// <param name="accountDate">Дата договора 1С</param>
		/// <param name="client1CCode">Код клиента 1С</param>
		/// <returns>ПДН и дата анкеты, на основании которой был рассчитан ПДН</returns>
		[PrincipalPermission(SecurityAction.Demand, Role = @"admin")]
		public async Task<PDNInfoList> CalculatePDNAsync(string account1CCode, DateTime accountDate, string client1CCode) =>
			await ExecuteWithTryCatchAsync<PDNInfoList, WCFServiceRegistrar>(() =>
					ServicePDN.CalculatePDNAsync(account1CCode, accountDate, client1CCode),
				new LogShortMessage("Ошибка вызова метода. Метод: {methodName} account1CCode: {account1CCode}," +
				                    " accountDate: {accountDate}, client1CCode: {client1CCode}.",
					"CalculatePDNAsync", account1CCode, accountDate, client1CCode));

		/// <summary>
		/// Получить сохраненный ПДН по номеру договору 1С.
		/// </summary>
		/// <param name="account1CCode">Номер договора из 1С</param>
		/// <returns>ПДН и дата анкеты, на основании которой был рассчитан ПДН</returns>
		[PrincipalPermission(SecurityAction.Demand, Role = @"admin")]
		public PDNInfoList GetSavedPDN(string account1CCode) =>
			ExecuteWithTryCatch<PDNInfoList, WCFServiceRegistrar>(() => ServicePDN.GetSavedPDN(account1CCode),
				new LogShortMessage("Ошибка вызова метода. Метод: {methodName} account1CCode: {account1CCode}.",
					"GetSavedPDN", account1CCode));

		/// <summary>
		/// Получить сохраненный ПДН по номеру договору 1С асинхронно.
		/// </summary>
		/// <param name="account1CCode">Номер договора из 1С</param>
		/// <returns>ПДН и дата анкеты, на основании которой был рассчитан ПДН</returns>
		[PrincipalPermission(SecurityAction.Demand, Role = @"admin")]
		public async Task<PDNInfoList> GetSavedPDNAsync(string account1CCode) =>
			await ExecuteWithTryCatchAsync<PDNInfoList, WCFServiceRegistrar>(() =>
				ServicePDN.GetSavedPDNAsync(account1CCode),
				new LogShortMessage("Ошибка вызова метода. Метод: {methodName} account1CCode: {account1CCode}.",
					"GetSavedPDNAsync", account1CCode));

		/// <summary>
		/// Сохранить ПДН.
		/// </summary>
		/// <param name="pdnInfoList">Данные ПДН</param>
		[PrincipalPermission(SecurityAction.Demand, Role = @"admin")]
		public void SavePDN(PDNInfoList pdnInfoList) =>
			ExecuteWithTryCatch<WCFServiceRegistrar>(() => ServicePDN.SavePDN(pdnInfoList),
				new LogShortMessage("Ошибка вызова метода. Метод: {methodName} pdnInfoList: {pdnInfoList}.",
					"SavePDN", pdnInfoList));

		/// <summary>
		/// Сохранить ПДН асинхронно.
		/// </summary>
		/// <param name="pdnInfoList">Данные ПДН</param>
		[PrincipalPermission(SecurityAction.Demand, Role = @"admin")]
		public async Task SavePDNAsync(PDNInfoList pdnInfoList) =>
			await ExecuteWithTryCatchAsync<WCFServiceRegistrar>(() => ServicePDN.SavePDNAsync(pdnInfoList),
				new LogShortMessage("Ошибка вызова метода. Метод: {methodName} pdnInfoList: {pdnInfoList}.",
					"SavePDNAsync", pdnInfoList));

		/// <summary>
		/// Получить список договоров с ошибками при расчете ПДН.
		/// </summary>
		/// <returns>Список договоров с ошибками в расчете ПДН</returns>
		[PrincipalPermission(SecurityAction.Demand, Role = @"admin")]
		public PDNErrorAccountInfo[] GetAccountsWithPDNError() =>
			ExecuteWithTryCatch<PDNErrorAccountInfo[], WCFServiceRegistrar>(() =>
					ServicePDN.GetAccountsWithPDNError(),
				new LogShortMessage("Ошибка вызова метода. Метод: {methodName}.", "GetAccountsWithPDNError"));

		/// <summary>
		/// Получить список договоров с ошибками при расчете ПДН асинхронно.
		/// </summary>
		/// <returns>Список договоров с ошибками в расчете ПДН</returns>
		[PrincipalPermission(SecurityAction.Demand, Role = @"admin")]
		public async Task<PDNErrorAccountInfo[]> GetAccountsWithPDNErrorAsync() =>
			await ExecuteWithTryCatchAsync<PDNErrorAccountInfo[], WCFServiceRegistrar>(() =>
				ServicePDN.GetAccountsWithPDNErrorAsync(),
				new LogShortMessage("Ошибка вызова метода. Метод: {methodName}.", "GetAccountsWithPDNErrorAsync"));
	}
}
