using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading;
using System.Threading.Tasks;
using NBCH_LIB.Interfaces;
using NBCH_LIB.Interfaces.WCF;
using NBCH_LIB.SOAP.SOAP1C.GetAccountsList;
using NBCH_LIB.SOAP.SOAP1C.GetClientData;

namespace NBCH_LIB.WCFProxy {
	/// <summary>
	/// Прокси класс для работы с WCF службой 1С
	/// </summary>
	public class Service1CsoapProxy : ClientBase<IService1СSoapWCF>, IService1СSoapWCF {
		#region Конструкторы
		public Service1CsoapProxy() { }
		public Service1CsoapProxy(string endpointName) : base(endpointName) { }
		public Service1CsoapProxy(Binding binding, EndpointAddress remoteAddress) : base(binding, remoteAddress) { }
		#endregion
		/// <summary>
		/// Получить из 1С информацию по номеру договора.
		/// </summary>
		/// <param name="webServiceURLs">Список адресов, для подключения к веб службе</param>
		/// <param name="userName">Имя пользователя для подключения к веб службе</param>
		/// <param name="userPassword">Пароль для для подключения к веб службе</param>
		/// <param name="account1CCode">Номер договора в 1С</param>
		/// <returns>CreditDocumentNError описание кредитного договора и ошибки при получении данных если были</returns>
		public CreditDocumentNResult GetCreditDocument(string[] webServiceURLs, string userName, string userPassword, string account1CCode) =>
			Channel.GetCreditDocument(webServiceURLs, userName, userPassword, account1CCode);

		/// <summary>
		/// Получить из 1С информацию по номеру договора асинхронно.
		/// </summary>
		/// <param name="webServiceURLs">Список адресов, для подключения к веб службе</param>
		/// <param name="userName">Имя пользователя для подключения к веб службе</param>
		/// <param name="userPassword">Пароль для для подключения к веб службе</param>
		/// <param name="account1CCode">Номер договора в 1С</param>
		/// <returns>CreditDocumentNError описание кредитного договора и ошибки при получении данных если были</returns>
		public async Task<CreditDocumentNResult> GetCreditDocumentAsync(string[] webServiceURLs, string userName,
			string userPassword, string account1CCode) =>
			await Channel.GetCreditDocumentAsync(webServiceURLs, userName, userPassword, account1CCode);

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
		public AccountLegendNResult GetAccountsLegends(string[] webServiceURLs, string userName, string userPassword, DateTime dateFrom, DateTime dateTo,
			string sellPoint1CCode, int quantityParam, params string[] status) =>
			Channel.GetAccountsLegends(webServiceURLs, userName, userPassword, dateFrom, dateTo, sellPoint1CCode, quantityParam, status);

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
			await Channel.GetAccountsLegendsAsync(webServiceURLs, userName, userPassword, dateFrom, dateTo,
				sellPoint1CCode, quantityParam, status);

		/// <summary>
		/// Загрузить ПДН %
		/// </summary>
		/// <param name="webServiceURLs">Адреса веб сервисов 1С</param>
		/// <param name="userName">Пользователь веб сервиса 1С</param>
		/// <param name="userPassword">Пароль веб сервиса 1С</param>
		/// <param name="account1CCode">Код договора 1С</param>
		/// <param name="pdnValue">Процент ПДН</param>
		public void LoadPDNFromFile(string[] webServiceURLs, string userName, string userPassword, string account1CCode, double pdnValue) =>
			Channel.LoadPDNFromFile(webServiceURLs, userName, userPassword, account1CCode, pdnValue);

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
			await Channel.LoadPDNFromFileAsync(webServiceURLs, userName, userPassword, account1CCode, pdnValue);
	}
}
