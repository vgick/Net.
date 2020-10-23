using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using NBCH_LIB.SOAP.SOAP1C.GetAccountsList;
using NBCH_LIB.SOAP.SOAP1C.GetClientData;
using NBCH_LIB.SOAP.SOAPProxy;

namespace NBCH_LIB.SOAP.SOAP1C {
	public static class SOAP1C {
		/// <summary>
		/// Десериализовать СОАП ответ 1С.
		/// </summary>
		/// <param name="response">Данные для десериализации</param>
		/// <returns>Десериализованный объект</returns>
		public static T DeserializeResponse<T>(byte[] response) where T: Response1C {
			MemoryStream memoryStream	= new MemoryStream(response);
			XmlSerializer serializer	= new XmlSerializer(typeof(T));
			T soapResponse				= (T)serializer.Deserialize(memoryStream);

			return soapResponse;
		}

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
		public static AccountLegendNResult GetAccountsLegends(string[] webServiceURLs, string userName, string userPassword, DateTime dateFrom, DateTime dateTo, string sellPoint1CCode, int quantityParam, params string[] status) {
			SOAPProxy.SOAPProxy soapProxy		= Singleton<SOAPProxy.SOAPProxy>.Values;
			AccountLegendMethod serviceSbTable	= new AccountLegendMethod(userName, userPassword, dateFrom, dateTo, sellPoint1CCode, quantityParam, status);
			AccountLegendNResult result			= GetDataFromService(webServiceURLs, serviceSbTable, soapProxy);

			return result;
		}

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
		/// <param name="cancellationToken">Токен отмены</param>
		/// <param name="status">Список статусов для отбора. Может быть пустым</param>
		/// <returns>Список договоров для проверки</returns>
		// todo: сделать прокси асинхронным.
		public static async Task<AccountLegendNResult> GetAccountsLegendsAsync(string[] webServiceURLs, string userName,
			string userPassword, DateTime dateFrom, DateTime dateTo, string sellPoint1CCode, int quantityParam,
			CancellationToken cancellationToken, params string[] status) {
			
			return await Task.Run(() =>
				GetAccountsLegends(webServiceURLs, userName, userPassword, dateFrom, dateTo, sellPoint1CCode, quantityParam, status),
				cancellationToken);
		}


		/// <summary>
		/// Получить данные с прокси веб сервиса.
		/// </summary>
		/// <param name="webServiceURLs">Адреса серверов</param>
		/// <param name="service">Веб сервис</param>
		/// <param name="soapProxy">Прокси</param>
		/// <returns></returns>
		private static T GetDataFromService<T>(string[] webServiceURLs, SOAPMethod<T> service, SOAPProxy.SOAPProxy soapProxy) where T : ISOAPData, new() {
			T result	= new T();
			List<string> errors		= new List<string>();

			foreach (string server in webServiceURLs) {
				result	= soapProxy.GetData(service, server);
				if (result.Errors.Length == 0) {
					break;
				}
				errors.AddRange(result.Errors);
			}

			result.Errors = errors.ToArray();
			return result;
		}

		/// <summary>
		/// Получить из 1С информацию по номеру договора.
		/// </summary>
		/// <param name="webServiceURLs">Список адресов, для подключения к веб службе</param>
		/// <param name="userName">Имя пользователя для подключения к веб службе</param>
		/// <param name="userPassword">Пароль для для подключения к веб службе</param>
		/// <param name="account1CCode">Номер договора в 1С</param>
		/// <returns>CreditDocumentNError описание кредитного договора и ошибки при получении данных если были</returns>
		public static CreditDocumentNResult GetCreditDocument(string[] webServiceURLs, string userName, string userPassword, string account1CCode) =>
			GetCreditDocumentAsync(webServiceURLs, userName, userPassword, account1CCode, CancellationToken.None).ResultAndThrowException();

		/// <summary>
		/// Получить из 1С информацию по номеру договора асинхронно.
		/// </summary>
		/// <param name="webServiceURLs">Список адресов, для подключения к веб службе</param>
		/// <param name="userName">Имя пользователя для подключения к веб службе</param>
		/// <param name="userPassword">Пароль для для подключения к веб службе</param>
		/// <param name="account1CCode">Номер договора в 1С</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns>CreditDocumentNError описание кредитного договора и ошибки при получении данных если были</returns>
		public static async Task<CreditDocumentNResult> GetCreditDocumentAsync(string[] webServiceURLs, string userName,
			string userPassword, string account1CCode, CancellationToken cancellationToken) {
			string request = SOAPQuery.GetCreditDocumentSoapMessage(account1CCode);

			(byte[] response, string[] errors) response =
				await GetDataFromWebServiceAsync(webServiceURLs, userName, userPassword, request, cancellationToken);

			if (response.response == default || response.response.Length == 0)
				return new CreditDocumentNResult() { CreditDocument = default, Errors = response.errors };

			SOAPResponseGetClientData soapResponse;
			try {
				soapResponse = DeserializeResponse<SOAPResponseGetClientData>(response.response);
			}
			catch (Exception ex) {
				return new CreditDocumentNResult() { CreditDocument = default, Errors = new string[] { ex.Message } };
			}


			return new CreditDocumentNResult() { CreditDocument = soapResponse?.Body?.GetClientDataResponse?.CreditDocument ?? default, Errors = response.errors };
		}

		/// <summary>
		/// Получить из вебсирвиса данные по переданному сообщению.
		/// </summary>
		/// <param name="webServiceURL">Список адресов, для подключения к веб службе</param>
		/// <param name="userName">Имя пользователя для подключения к веб службе</param>
		/// <param name="userPassword">Пароль для для подключения к веб службе</param>
		/// <param name="requestString">Сообщение к вев сервису</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns>(полученное сообщение, ошибки при получении данных)</returns>
		private static async Task<(byte[] response, string[] error)> GetDataFromWebServiceAsync(string[] webServiceURL,
			string userName, string userPassword, string requestString, CancellationToken cancellationToken) {
			WebClient client	= new WebClient() {Credentials = new NetworkCredential(userName, userPassword)};

			byte[] request		= Encoding.UTF8.GetBytes(requestString);
			byte[] response		= default;
			List<string> errors	= new List<string>();

			foreach (string url in webServiceURL) {
				try {
					using (cancellationToken.Register(client.CancelAsync)) {
						response = await client.UploadDataTaskAsync(url, request);
					}

					if (response.Length == 0) errors.Add($"При подключении к серверу 1С '{url}' получен пустой ответ. Возможно неверные настройки подключения к сервису 1С.");
					else break;
				}
				catch (Exception exception) {
					errors.Add(exception.ToString());
				}
			}

			return (response, errors.ToArray());
		}

		/// <summary>
		/// Преобразовать DateTime в строку даты для 1С.
		/// </summary>
		/// <param name="dateTime">DateTime</param>
		/// <returns>Строка даты в НБКИ</returns>
		public static string DateTimeToString(DateTime dateTime){
			// todo: вернуть regex
			if (dateTime == default) throw new ArgumentException("Wrong DateTime value");
			return $"{dateTime.Year}{dateTime.Month:00}{dateTime.Day:00}";
		}

		/// <summary>
		/// Преобразовать строку даты из 1С в DateTime.
		/// </summary>
		/// <param name="date">Строка даты в НБКИ</param>
		/// <returns>DateTime</returns>
		public static DateTime StringToDateTime(string date){
			if (String.IsNullOrEmpty(date)) throw new ArgumentNullException(nameof(date), "Wrong Date value");
			if (date.Length != 8) throw new ArgumentException("Wrong Date value. Example: yyymmmdd");

			// todo: вернуть regex

			//Regex regex = new Regex(@"(\d\d\d\d)-((0[1-9]|1[012])-(0[1-9]|[12]\d)|(0[13-9]|1[012])-30|(0[13578]|1[02])-31)");
			//MatchCollection matches = regex.Matches(date);

			int year	= int.Parse(date.Substring(0, 4));
			int month	= int.Parse(date.Substring(4, 2));
			int day		= int.Parse(date.Substring(6, 2));

			return new DateTime(year, month, day);
		}

		/// <summary>
		/// Преобразовать дату НБКИ в дату 1С.
		/// </summary>
		/// <param name="nbchDate">Дата в формате НБКИ</param>
		/// <returns>Дата в формате 1С</returns>
		public static string NBCHDateToDate1C(string nbchDate){
			return nbchDate.Replace("-", String.Empty);
		}

		/// <summary>
		/// Статусы договоров 1С.
		/// </summary>
		public enum AccountStatus {
			[Description("Действующий")]
			Open,
			[Description("Закрыт")]
			Close,
			[Description("Новый")]
			New,
			[Description("НаПроверкеСБ")]
			CheckSB,
			[Description("НаВерификации")]
			Verification,
			[Description("ПодписаниеДоговораКлиентом")]
			OnClientAssign
		}

	}
}
