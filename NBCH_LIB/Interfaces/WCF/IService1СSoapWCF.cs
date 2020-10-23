using System;
using System.ServiceModel;
using System.Threading.Tasks;
using NBCH_LIB.SOAP.SOAP1C.GetAccountsList;
using NBCH_LIB.SOAP.SOAP1C.GetClientData;

namespace NBCH_LIB.Interfaces.WCF {
	/// <summary>
	/// Контракт для работы в веб службой 1С
	/// </summary>
	[ServiceContract]
	public interface IService1СSoapWCF : IWCFContract {
		/// <summary>
		/// Получить из 1С информацию по договору.
		/// </summary>
		/// <param name="webServiceURLs">Адреса веб сервисов 1С</param>
		/// <param name="userName">Пользователь веб сервиса 1С</param>
		/// <param name="userPassword">Пароль веб сервиса 1С</param>
		/// <param name="account1CCode">Код договора 1С</param>
		/// <returns>Информация по договору 1С</returns>
		[OperationContract]
		CreditDocumentNResult GetCreditDocument(string[] webServiceURLs, string userName, string userPassword, string account1CCode);

		/// <summary>
		/// Получить из 1С информацию по договору асинхронно.
		/// </summary>
		/// <param name="webServiceURLs">Адреса веб сервисов 1С</param>
		/// <param name="userName">Пользователь веб сервиса 1С</param>
		/// <param name="userPassword">Пароль веб сервиса 1С</param>
		/// <param name="account1CCode">Код договора 1С</param>
		/// <returns>Информация по договору 1С</returns>
		[OperationContract(Name = "GetCreditDocumentAsync")]
		Task<CreditDocumentNResult> GetCreditDocumentAsync(string[] webServiceURLs, string userName, string userPassword,
				string account1CCode);

		/// <summary>
		/// Загрузить ПДН %
		/// </summary>
		/// <param name="webServiceURLs">Адреса веб сервисов 1С</param>
		/// <param name="userName">Пользователь веб сервиса 1С</param>
		/// <param name="userPassword">Пароль веб сервиса 1С</param>
		/// <param name="account1CCode">Код договора 1С</param>
		/// <param name="pdnValue">Процент ПДН</param>
		[OperationContract]
		void LoadPDNFromFile(string[] webServiceURLs, string userName, string userPassword, string account1CCode, double pdnValue);

		/// <summary>
		/// Загрузить ПДН % асинхронно.
		/// </summary>
		/// <param name="webServiceURLs">Адреса веб сервисов 1С</param>
		/// <param name="userName">Пользователь веб сервиса 1С</param>
		/// <param name="userPassword">Пароль веб сервиса 1С</param>
		/// <param name="account1CCode">Код договора 1С</param>
		/// <param name="pdnValue">Процент ПДН</param>
		[OperationContract(Name = "LoadPDNFromFileAsync")]
		Task LoadPDNFromFileAsync(string[] webServiceURLs, string userName, string userPassword, string account1CCode,
			double pdnValue);

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
		[OperationContract]
		AccountLegendNResult GetAccountsLegends(string[] webServiceURLs, string userName, string userPassword,
			DateTime dateFrom, DateTime dateTo, string sellPoint1CCode, int quantityParam, params string[] status);

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
		[OperationContract(Name = "GetAccountsLegendsAsync")]
		Task<AccountLegendNResult> GetAccountsLegendsAsync(string[] webServiceURLs, string userName, string userPassword,
			DateTime dateFrom, DateTime dateTo, string sellPoint1CCode, int quantityParam, params string[] status);
	}
}
