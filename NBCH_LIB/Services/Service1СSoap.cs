using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NBCH_LIB.Interfaces;
using NBCH_LIB.Interfaces.WCF;
using NBCH_LIB.SOAP.SOAP1C;
using NBCH_LIB.SOAP.SOAP1C.GetAccountsList;
using NBCH_LIB.SOAP.SOAP1C.GetClientData;
using static NBCH_LIB.Logger.ExceptionLog;

namespace NBCH_LIB.Services {
	public class Service1СSoap : IService1СSoap, IService1СSoapWCF {
		/// <summary>
		/// Логгер.
		/// </summary>
		private readonly ILogger<Service1СSoap> _Logger;

		/// <summary>
		/// Источник данных для сервиса.
		/// </summary>
		private readonly IService1C _Service1C;

		/// <summary>
		/// Конструктор.
		/// </summary>
		/// <param name="service1C">Источник данных</param>
		/// <param name="loggerFactory">Фабрика логгера</param>
		public Service1СSoap(IService1C service1C, ILoggerFactory loggerFactory) {
			_Service1C	= service1C;
			_Logger		= loggerFactory.CreateLogger<Service1СSoap>();
		}

		/// <summary>
		/// Получить из 1С информацию по номеру договора.
		/// </summary>
		/// <param name="webServiceURLs">Список адресов, для подключения к веб службе</param>
		/// <param name="userName">Имя пользователя для подключения к веб службе</param>
		/// <param name="userPassword">Пароль для для подключения к веб службе</param>
		/// <param name="account1CCode">Номер договора в 1С</param>
		/// <returns>CreditDocumentNError описание кредитного договора и ошибки при получении данных если были</returns>
		public CreditDocumentNResult GetCreditDocument(string[] webServiceURLs, string userName, string userPassword,
			string account1CCode) {

			GetCreditDocumentCheckParams(webServiceURLs,  userName, account1CCode);

			CreditDocumentNResult result = SOAP1C.GetCreditDocument(webServiceURLs, userName, userPassword, account1CCode);

			if (result.CreditDocument?.Client?.ID1C != default)
				_Service1C.UpdateAccountAndClientInfo(result);

			return result;
		}

		/// <summary>
		/// Получить из 1С информацию по номеру договора асинхронно.
		/// </summary>
		/// <param name="webServiceURLs">Список адресов, для подключения к веб службе</param>
		/// <param name="userName">Имя пользователя для подключения к веб службе</param>
		/// <param name="userPassword">Пароль для для подключения к веб службе</param>
		/// <param name="account1CCode">Номер договора в 1С</param>
		/// <returns>CreditDocumentNError описание кредитного договора и ошибки при получении данных если были</returns>
		public async Task<CreditDocumentNResult> GetCreditDocumentAsync(string[] webServiceURLs, string userName,
			string userPassword, string account1CCode) => await GetCreditDocumentAsync(webServiceURLs, userName,
			userPassword, account1CCode, CancellationToken.None);

		/// <summary>
		/// Получить из 1С информацию по номеру договора асинхронно.
		/// </summary>
		/// <param name="webServiceURLs">Список адресов, для подключения к веб службе</param>
		/// <param name="userName">Имя пользователя для подключения к веб службе</param>
		/// <param name="userPassword">Пароль для для подключения к веб службе</param>
		/// <param name="account1CCode">Номер договора в 1С</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns>CreditDocumentNError описание кредитного договора и ошибки при получении данных если были</returns>
		public async Task<CreditDocumentNResult> GetCreditDocumentAsync(string[] webServiceURLs, string userName,
			string userPassword, string account1CCode, CancellationToken cancellationToken) {
			GetCreditDocumentCheckParams(webServiceURLs,  userName, account1CCode);

			CreditDocumentNResult result =
				await SOAP1C.GetCreditDocumentAsync(webServiceURLs, userName, userPassword, account1CCode, cancellationToken);

			if (result.CreditDocument?.Client?.ID1C != default)
				await _Service1C.UpdateAccountAndClientInfoAsync(result, cancellationToken);

			return result;
		}
		
		/// <summary>
		/// Проверка входных параметров GetCreditDocument.
		/// </summary>
		/// <param name="webServiceURLs">Адреса веб сервисов 1С</param>
		/// <param name="userName">Логин пользователя для подключения</param>
		/// <param name="account1CCode">Номер договора 1С</param>
		/// <exception cref="ArgumentNullException"></exception>
		private void GetCreditDocumentCheckParams(string[] webServiceURLs, string userName, string account1CCode) {
			if (webServiceURLs == default)
				LogAndThrowException<ArgumentNullException, Service1СSoap>(
					_Logger, nameof(webServiceURLs),
					"Не задан адрес веб службы 1С./* Метод {methodName}.*/",
					"GetCreditDocumentCheckParams");
	
			if (string.IsNullOrEmpty(account1CCode))
				LogAndThrowException<ArgumentNullException, Service1СSoap>(
					_Logger, nameof(account1CCode),
					"Не задан номер договора 1С./* Метод {methodName}.*/",
					"GetCreditDocumentCheckParams");
	
			if (string.IsNullOrEmpty(userName))
				LogAndThrowException<ArgumentNullException, Service1СSoap>(
					_Logger, nameof(userName),
					"Не задан логин для подключения к службе 1С./* Метод {methodName}.*/",
					"GetCreditDocumentCheckParams");
		}

		/// <summary>
		/// Загрузить ПДН % асинхронно.
		/// </summary>
		/// <param name="webServiceURLs">Адреса веб сервисов 1С</param>
		/// <param name="userName">Пользователь веб сервиса 1С</param>
		/// <param name="userPassword">Пароль веб сервиса 1С</param>
		/// <param name="account1CCode">Код договора 1С</param>
		/// <param name="pdnValue">Процент ПДН</param>
		public async Task LoadPDNFromFileAsync(string[] webServiceURLs, string userName, string userPassword,
			string account1CCode, double pdnValue) =>
			await LoadPDNFromFileAsync(webServiceURLs, userName, userPassword, account1CCode, pdnValue, CancellationToken.None);

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
		public AccountLegendNResult GetAccountsLegends(string[] webServiceURLs, string userName, string userPassword,
			DateTime dateFrom, DateTime dateTo, string sellPoint1CCode, int quantityParam, params string[] status) {

			GetAccountsLegendsCheckParams(webServiceURLs, userName, dateFrom, dateTo);

			return SOAP1C.GetAccountsLegends(webServiceURLs, userName, userPassword, dateFrom, dateTo, sellPoint1CCode,
				quantityParam, status);
		}

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
		public async Task<AccountLegendNResult> GetAccountsLegendsAsync(string[] webServiceURLs, string userName,
			string userPassword, DateTime dateFrom, DateTime dateTo, string sellPoint1CCode, int quantityParam,
			params string[] status) =>
			await GetAccountsLegendsAsync(webServiceURLs, userName, userPassword, dateFrom, dateTo, sellPoint1CCode,
				quantityParam, CancellationToken.None, status);

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
		/// <param name="cancellationToken">Токен отмены</param>
		/// <param name="status">Список статусов для отбора. Может быть пустым</param>
		/// <returns>Список договоров для проверки</returns>
		public async Task<AccountLegendNResult> GetAccountsLegendsAsync(string[] webServiceURLs, string userName,
			string userPassword, DateTime dateFrom, DateTime dateTo, string sellPoint1CCode, int quantityParam,
			CancellationToken cancellationToken, params string[] status) {

			GetAccountsLegendsCheckParams(webServiceURLs, userName, dateFrom, dateTo);
				
			return await SOAP1C.GetAccountsLegendsAsync(webServiceURLs, userName, userPassword, dateFrom, dateTo,
				sellPoint1CCode, quantityParam, cancellationToken, status);
		}

		/// <summary>
		/// Проверить входные параметры GetAccountsLegends.
		/// </summary>
		/// <param name="webServiceURLs">Адреса веб сервисов 1С</param>
		/// <param name="userName">Имя пользователя для подключения</param>
		/// <param name="dateFrom">Начало периода отбора</param>
		/// <param name="dateTo">Окончание периода отбора</param>
		/// <exception cref="ArgumentNullException"></exception>
		private void GetAccountsLegendsCheckParams(string[] webServiceURLs, string userName, DateTime dateFrom, DateTime dateTo) {
			if (webServiceURLs == default)
				LogAndThrowException<ArgumentNullException, Service1СSoap>(
					_Logger, nameof(webServiceURLs),
					"Не задан адрес веб службы 1С./* Метод {methodName}.*/",
					"GetAccountsLegendsCheckParams");

			if (string.IsNullOrEmpty(userName))
				LogAndThrowException<ArgumentNullException, Service1СSoap>(
					_Logger, nameof(userName),
					"Не задан логин для подключения к службе 1С./* Метод {methodName}.*/",
					"GetAccountsLegendsCheckParams");
			
			if (dateFrom == default)
				LogAndThrowException<ArgumentNullException, Service1СSoap>(
					_Logger, nameof(dateFrom),
					"Не задано начало периода отбора./* Метод {methodName}.*/",
					"GetAccountsLegendsCheckParams");
			
			if (dateTo == default)
				LogAndThrowException<ArgumentNullException, Service1СSoap>(
					_Logger, nameof(dateTo),
					"Не задано окончание периода отбора./* Метод {methodName}.*/",
					"GetAccountsLegendsCheckParams");
		}

		/// <summary>
		/// Загрузить ПДН %
		/// </summary>
		/// <param name="webServiceURLs">Адреса веб сервисов 1С</param>
		/// <param name="userName">Пользователь веб сервиса 1С</param>
		/// <param name="userPassword">Пароль веб сервиса 1С</param>
		/// <param name="account1CCode">Код договора 1С</param>
		/// <param name="pdnValue">Процент ПДН</param>
		public void LoadPDNFromFile(string[] webServiceURLs, string userName, string userPassword, string account1CCode, double pdnValue) {
			LoadPDNFromFileCheck(webServiceURLs, userName, account1CCode);

			CreditDocumentNResult result = SOAP1C.GetCreditDocument(webServiceURLs, userName, userPassword, account1CCode);

			if (result.CreditDocument?.Client?.ID1C != default) {

				_Service1C.UpdateAccountAndClientInfo(result);
				_Service1C.LoadPDNFromFile(result, pdnValue);
			}
		}

		/// <summary>
		/// Загрузить ПДН % асинхронно.
		/// </summary>
		/// <param name="webServiceURLs">Адреса веб сервисов 1С</param>
		/// <param name="userName">Пользователь веб сервиса 1С</param>
		/// <param name="userPassword">Пароль веб сервиса 1С</param>
		/// <param name="account1CCode">Код договора 1С</param>
		/// <param name="pdnValue">Процент ПДН</param>
		/// <param name="cancellationToken">Токен отмены</param>
		public async Task LoadPDNFromFileAsync(string[] webServiceURLs, string userName, string userPassword,
			string account1CCode, double pdnValue, CancellationToken cancellationToken) {
			LoadPDNFromFileCheck(webServiceURLs, userName, account1CCode);

			CreditDocumentNResult result =
				await SOAP1C.GetCreditDocumentAsync(webServiceURLs, userName, userPassword, account1CCode, cancellationToken);

			if (result.CreditDocument?.Client?.ID1C != default) {

				await _Service1C.UpdateAccountAndClientInfoAsync(result, cancellationToken);
				await _Service1C.LoadPDNFromFileAsync(result, pdnValue, cancellationToken);
			}
		}

		/// <summary>
		/// Проверить входные данные LoadPDNFromFile.
		/// </summary>
		/// <param name="webServiceURLs">Адреса веб сервисов 1С</param>
		/// <param name="userName">Пользователь веб сервиса 1С</param>
		/// <param name="account1CCode">Код договора 1С</param>
		private void LoadPDNFromFileCheck(string[] webServiceURLs, string userName, string account1CCode) {
			if (webServiceURLs == default)
				LogAndThrowException<ArgumentNullException, Service1СSoap>(
					_Logger, nameof(webServiceURLs),
					"Не задан адрес веб службы 1С./* Метод {methodName}.*/",
					"GetCreditDocumentCheckParams");
	
			if (string.IsNullOrEmpty(account1CCode))
				LogAndThrowException<ArgumentNullException, Service1СSoap>(
					_Logger, nameof(account1CCode),
					"Не задан номер договора 1С./* Метод {methodName}.*/",
					"GetCreditDocumentCheckParams");
	
			if (string.IsNullOrEmpty(userName))
				LogAndThrowException<ArgumentNullException, Service1СSoap>(
					_Logger, nameof(userName),
					"Не задан логин для подключения к службе 1С./* Метод {methodName}.*/",
					"GetCreditDocumentCheckParams");
		}
	}
}