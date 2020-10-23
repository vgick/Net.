using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Xml.Serialization;
using NBCH_LIB.SOAP.SOAPProxy;

namespace NBCH_LIB.SOAP.SOAP1C {
	public abstract class SOAPMethod<SOAPType> where SOAPType : ISOAPData {
		/// <summary>
		/// Десериализовать СОАП ответ 1С.
		/// </summary>
		/// <param name="response">Данные для десериализации</param>
		/// <returns>Десериализованный объект</returns>
		protected static T DeserializeResponse<T>(byte[] response) where T : Response1C {
			MemoryStream memoryStream = new MemoryStream(response);
			XmlSerializer serializer = new XmlSerializer(typeof(T));
			T soapResponse = (T)serializer.Deserialize(memoryStream);

			return soapResponse;
		}

		/// <summary>
		/// Дополнительный ключ для кэша.
		/// </summary>
		public abstract string AdditionKey { get; }

		/// <summary>
		/// Получить из вебсирвиса данные по переданному сообщению
		/// </summary>
		/// <param name="webServiceURL">Список адресов, для подключения к веб службе</param>
		/// <param name="userName">Имя пользователя для подключения к веб службе</param>
		/// <param name="userPassword">Пароль для для подключения к веб службе</param>
		/// <param name="requestString">Сообщение к вев сервису</param>
		/// <returns>(полученное сообщение, ошибки при получении данных)</returns>
		protected static SOAPResponse GetDataFromWebService(string userName, string userPassword, string requestString, params string[] webServiceURL) {
			if (webServiceURL == default || webServiceURL.Length == 0) throw new ArgumentNullException(nameof(webServiceURL),"url is not set");
			if (string.IsNullOrEmpty(requestString)) throw new ArgumentNullException(nameof(requestString),"Request string is not set");

			WebClient client = new WebClient() { Credentials = new NetworkCredential(userName, userPassword) };

			byte[] request	= Encoding.UTF8.GetBytes(requestString);
			byte[] response	= default;
			List<string> errors = new List<string>();

			foreach (string url in webServiceURL) {
				try {
					response = client.UploadData(url, request);

					if (response.Length == 0) errors.Add($"При подключении к серверу '{url}' получен пустой ответ. Возможно неверные настройки подключения к сервису");
					else break;
				}
				catch (Exception exception) {
					errors.Add(exception.Message);
				}
			}

			return new SOAPResponse() { Response = response, Errors = errors.ToArray()};
		}

		/// <summary>
		/// Выполнить запрос и получить данные.
		/// </summary>
		/// <returns>Словарь - сервер/результат выполнения запроса</returns>
		public abstract SOAPType GetData(string server);
	}
}
