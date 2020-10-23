using System;
using System.Security.Permissions;
using System.Threading;
using System.Threading.Tasks;
using NBCH_LIB.Interfaces;
using NBCH_LIB.Interfaces.WCF;
using NBCH_LIB.Logger;
using NBCH_LIB.SOAP.SOAP1C.GetAccountsList;
using NBCH_LIB.SOAP.SOAP1C.GetClientData;
using static NBCH_WCF.Services.Utils;
using static NBCH_WCF.Services.ServiceMethod;

namespace NBCH_WCF.Services {
	/// <summary>
	/// Сервис, реализующего интерфейс IService1Csoap для работы с веб службой 1С.
	/// </summary>
	public class WCFService1CSoap : IService1СSoapWCF {
		/// <summary>
		/// Получить список договор на проверку.
		/// </summary>
		/// <param name="webServiceURLs">Адреса веб сервисов 1С</param>
		/// <param name="userName">Имя пользователя для подключения</param>
		/// <param name="userPassword">Пароль для подключения</param>
		/// <param name="dateFrom">Начало периода отбора</param>
		/// <param name="dateTo">Окончание периода отбора</param>
		/// <param name="sellPoint1CCode">Точка заключения сделки</param>
		/// <param name="quantityParam">Кол-во договоров в выборке</param>
		/// <param name="status">Список статусов для отбора. Может быть пустым</param>
		/// <returns>Список договоров для проверки</returns>
		[PrincipalPermission(SecurityAction.Demand, Role = @"admin")]
		public AccountLegendNResult GetAccountsLegends(string[] webServiceURLs, string userName, string userPassword,
			DateTime dateFrom, DateTime dateTo, string sellPoint1CCode, int quantityParam, params string[] status) =>
			
			ExecuteWithTryCatch<AccountLegendNResult, WCFService1CSoap>( () =>
				Service1СSoap.GetAccountsLegends(webServiceURLs, userName, userPassword, dateFrom, dateTo,
					sellPoint1CCode, quantityParam, status),
				new LogShortMessage("Ошибка вызова метода. Метод: {methodName}, webServiceURLs: {webServiceURLs}," +
				                    " userName: {userName}, dateFrom {dateFrom}, dateTo {dateTo}," +
				                    " sellPoint1CCode: {sellPoint1CCode}, quantityParam: {quantityParam}, status: {status}.",
					"GetAccountsLegends", string.Join(", ", webServiceURLs ?? new [] {"null"}), userName, dateFrom,
					dateTo, sellPoint1CCode, quantityParam, string.Join(", ", status ??new [] {"null"})));

		/// <summary>
		/// Получить список договор на проверку асинхронно.
		/// </summary>
		/// <param name="webServiceURLs">Адреса веб сервисов 1С</param>
		/// <param name="userName">Имя пользователя для подключения</param>
		/// <param name="userPassword">Пароль для подключения</param>
		/// <param name="dateFrom">Начало периода отбора</param>
		/// <param name="dateTo">Окончание периода отбора</param>
		/// <param name="sellPoint1CCode">Точка заключения сделки</param>
		/// <param name="quantityParam">Кол-во договоров в выборке</param>
		/// <param name="status">Список статусов для отбора. Может быть пустым</param>
		/// <returns>Список договоров для проверки</returns>
		[PrincipalPermission(SecurityAction.Demand, Role = @"admin")]
		public async Task<AccountLegendNResult> GetAccountsLegendsAsync(string[] webServiceURLs, string userName,
			string userPassword, DateTime dateFrom, DateTime dateTo, string sellPoint1CCode, int quantityParam,
			params string[] status)  =>
			
			await ExecuteWithTryCatchAsync<AccountLegendNResult, WCFService1CSoap>( () =>
				Service1СSoap.GetAccountsLegendsAsync(webServiceURLs, userName, userPassword, dateFrom, dateTo,
					sellPoint1CCode, quantityParam, status),
				new LogShortMessage("Ошибка вызова метода. Метод: {methodName}, webServiceURLs: {webServiceURLs}," +
				                    " userName: {userName}, dateFrom {dateFrom}, dateTo {dateTo}," +
				                    " sellPoint1CCode: {sellPoint1CCode}, quantityParam: {quantityParam}, status: {status}.",
					"GetAccountsLegendsAsync", string.Join(", ", webServiceURLs ?? new [] {"null"}),
					userName, dateFrom, dateTo, sellPoint1CCode, quantityParam,
					string.Join(", ", status ?? new [] {"null"})));

		/// <summary>
		/// Получить из 1С информацию по договору.
		/// </summary>
		/// <param name="webServiceURLs">Адреса веб сервисов 1С</param>
		/// <param name="userName">Пользователь веб сервиса 1С</param>
		/// <param name="userPassword">Пароль веб сервиса 1С</param>
		/// <param name="account1CCode">Код договора 1С</param>
		/// <returns>Информация по договору 1С</returns>
		[PrincipalPermission(SecurityAction.Demand, Role = @"admin")]
		public CreditDocumentNResult GetCreditDocument(string[] webServiceURLs, string userName, string userPassword,
			string account1CCode) =>
			
			ExecuteWithTryCatch<CreditDocumentNResult, WCFService1CSoap>( () =>
				Service1СSoap.GetCreditDocument(webServiceURLs, userName, userPassword, account1CCode),
					new LogShortMessage("Ошибка вызова метода. Метод: {methodName} webServiceURLs: {webServiceURLs}," +
					                    " userName: {userName}, account1CCode: {account1CCode}",
						"GetCreditDocument", string.Join(", ", webServiceURLs ?? new [] {"null"}),
						userName, account1CCode));

		/// <summary>
		/// Получить из 1С информацию по договору асинхронно.
		/// </summary>
		/// <param name="webServiceURLs">Адреса веб сервисов 1С</param>
		/// <param name="userName">Пользователь веб сервиса 1С</param>
		/// <param name="userPassword">Пароль веб сервиса 1С</param>
		/// <param name="account1CCode">Код договора 1С</param>
		/// <returns>Информация по договору 1С</returns>
		[PrincipalPermission(SecurityAction.Demand, Role = @"admin")]
		public async Task<CreditDocumentNResult> GetCreditDocumentAsync(string[] webServiceURLs, string userName,
			string userPassword, string account1CCode) => 
			await ExecuteWithTryCatchAsync<CreditDocumentNResult, WCFService1CSoap>( () =>
				Service1СSoap.GetCreditDocumentAsync(webServiceURLs, userName, userPassword, account1CCode),
					new LogShortMessage("Ошибка вызова метода. Метод: {methodName} webServiceURLs: {webServiceURLs}," +
					                    " userName: {userName}, account1CCode: {account1CCode}",
						"GetCreditDocument", string.Join(", ", webServiceURLs ?? new [] {"null"}),
						userName, account1CCode));

		/// <summary>
		/// Загрузить ПДН %
		/// </summary>
		/// <param name="webServiceURLs">Адреса веб сервисов 1С</param>
		/// <param name="userName">Пользователь веб сервиса 1С</param>
		/// <param name="userPassword">Пароль веб сервиса 1С</param>
		/// <param name="account1CCode">Код договора 1С</param>
		/// <param name="pdnValue">Процент ПДН</param>
		[PrincipalPermission(SecurityAction.Demand, Role = @"admin")]
		public void LoadPDNFromFile(string[] webServiceURLs, string userName, string userPassword, string account1CCode,
			double pdnValue) =>
			
			ExecuteWithTryCatch<WCFService1CSoap>( () => Service1СSoap.LoadPDNFromFile(webServiceURLs, userName,
					userPassword, account1CCode, pdnValue),
				new LogShortMessage("Ошибка вызова метода. Метод: {methodName} webServiceURLs: {webServiceURLs}," +
				                    " userName: {userName}, account1CCode: {account1CCode}, pdnValue: {pdnValue}.",
					"LoadPDNFromFile", string.Join(", ", webServiceURLs ?? new [] {"null"}),
					userName, account1CCode, pdnValue));

		/// <summary>
		/// Загрузить ПДН % асинхронно.
		/// </summary>
		/// <param name="webServiceURLs">Адреса веб сервисов 1С</param>
		/// <param name="userName">Пользователь веб сервиса 1С</param>
		/// <param name="userPassword">Пароль веб сервиса 1С</param>
		/// <param name="account1CCode">Код договора 1С</param>
		/// <param name="pdnValue">Процент ПДН</param>
		[PrincipalPermission(SecurityAction.Demand, Role = @"admin")]
		public async Task LoadPDNFromFileAsync(string[] webServiceURLs, string userName, string userPassword,
			string account1CCode, double pdnValue) =>
			await ExecuteWithTryCatchAsync<WCFService1CSoap>(
				() => Service1СSoap.LoadPDNFromFileAsync(webServiceURLs, userName, userPassword, account1CCode, pdnValue),
				new LogShortMessage("Ошибка вызова метода. Метод: {methodName} webServiceURLs: {webServiceURLs}," +
				                    " userName: {userName}, account1CCode: {account1CCode}, pdnValue: {pdnValue}.",
					"LoadPDNFromFileAsync", string.Join(", ", webServiceURLs ?? new [] {"null"}),
					userName, account1CCode, pdnValue));
	}
}
