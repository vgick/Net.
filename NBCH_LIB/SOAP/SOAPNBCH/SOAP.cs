using System;
using System.IO;
using System.Net;
using System.Security.Cryptography.Pkcs;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace NBCH_LIB.SOAP.SOAPNBCH {
	public static class SOAPNBCH {
		#region Ошибки НБКИ. есть ошибки "1", "001", которые не имеют ничего общего...
		/// <summary>
		/// В базе НБКИ клиент с такими данными не найден.
		/// </summary>
		public static readonly string ClientNotFoundNBCH	= "1";
		#endregion
		/// <summary>
		/// Часовой пояс Москвы.  Необходимо для корректировки даты при отправки запросов в НБКИ, работающих по Московскому времени.
		/// </summary>
		public static readonly int MoscowTimeZone			= 3;

		/// <summary>
		/// Кол-во минут, через которое можно повторить запрос.
		/// </summary>
		public static readonly int ReportCanReaplyMinute	= 2;

		/// <summary>
		/// Кол-во дней по истечении которых, для расчета ПДН необходимо обновить анкету НБКИ.
		/// </summary>
		public static readonly int NBCHAnketaExpiredDay		= 5;

		/// <summary>
		/// Тестовый сервер НБКИ. Использовать исключительно для отладочных целей.
		/// </summary>
		public static string TestServiceURL			= "http://alpha.demo.nbki.ru/products/B2BRequestServlet";

		/// <summary>
		/// Рабочий сервер НБКИ. Использовать исключительно для отладочных целей.
		/// </summary>
		public static string ProductServiceURL		= "https://icrs.nbki.ru/products/B2BRequestServlet";

		/// <summary>
		/// Получить подписанный НБКИ отчет асинхронно.
		/// </summary>
		/// <param name="url">Урл сервиса</param>
		/// <param name="requestProd">Запрос в НБКИ</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns>Подписанная КИ клиента</returns>
		public static async Task<byte[]> GetSignedReportAsync(string url, ProductRequest requestProd, CancellationToken cancellationToken){
			byte[] serializedObject					= SerializeProductRequest(requestProd);
			ServicePointManager.SecurityProtocol	= SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;// | SecurityProtocolType.Tls13;

			WebRequest request = WebRequest.Create(url);
			using (MemoryStream outStream = new MemoryStream(serializedObject)) {
				request.Credentials	= CredentialCache.DefaultCredentials;
				request.Method		= "POST";
				request.ContentType	= "text/xml";
				using (cancellationToken.Register(() => request.Abort(), useSynchronizationContext: false)) {
					Stream data			= await request.GetRequestStreamAsync();
					await outStream.CopyToAsync(data);
					data.Close();
				}
			}

			WebResponse response	= await request.GetResponseAsync();
			Stream responseStream	= response.GetResponseStream();
			MemoryStream inStream	= new MemoryStream();
			using (cancellationToken.Register(() => request.Abort(), useSynchronizationContext: false)) {
				await responseStream.CopyToAsync(inStream);
			}

			return inStream.ToArray();
		}

		/// <summary>
		/// Получить подписанный НБКИ отчет.
		/// </summary>
		/// <param name="url">Урл сервиса</param>
		/// <param name="requestProd">Запрос в НБКИ</param>
		/// <returns>Подписанная КИ клиента</returns>
		public static byte[] GetSignedReport(string url, ProductRequest requestProd) {
			byte[] serializedObject = SerializeProductRequest(requestProd);
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;// | SecurityProtocolType.Tls13;

			WebRequest request = WebRequest.Create(url);
			using (MemoryStream outStream = new MemoryStream(serializedObject)) {
				request.Credentials = CredentialCache.DefaultCredentials;
				request.Method = "POST";
				request.ContentType = "text/xml";
				Stream data = request.GetRequestStream();
				outStream.CopyTo(data);
				data.Close();
			}

			WebResponse response = request.GetResponse();
			Stream responseStream = response.GetResponseStream();
			MemoryStream inStream = new MemoryStream();
			responseStream.CopyTo(inStream);

			return inStream.ToArray();
		}


		/// <summary>
		/// Сериализация объекта ProductResponse.
		/// </summary>
		/// <param name="productRequest">объект ProductResponse</param>
		/// <returns>Массив байт (сериализованного объекта ProductResponse)</returns>
		private static byte[] SerializeProductRequest(ProductRequest productRequest) {
			XmlSerializer serializer	= new XmlSerializer(typeof(ProductRequest));
			MemoryStream memoryStream	= new MemoryStream();
			XmlWriter xmlWriter			= new XmlTextWriter(memoryStream, Encoding.GetEncoding("windows-1251"));
			serializer.Serialize(xmlWriter, productRequest);

			return memoryStream.ToArray();
		}
		/// <summary>
		/// Десериализация объекта ProductResponse.
		/// </summary>
		/// <param name="productResponse">Массив байт (сериализованного объекта ProductResponse)</param>
		/// <returns>объект ProductResponse</returns>
		public static ProductResponse DeserializeProductRequest(byte[] productResponse) {
			MemoryStream memoryStream	= new MemoryStream(productResponse);
			XmlSerializer serializer	= new XmlSerializer(typeof(ProductResponse));
			return (ProductResponse)serializer.Deserialize(memoryStream);
		}
		/// <summary>
		/// Удалить подпись из ответа.
		/// </summary>
		/// <param name="response">подписанный ответ</param>
		/// <returns>Очищенный от подписи ответ</returns>
		public static byte[] RemoveSignature(byte[] response){
			SignedCms cms	= new SignedCms();
			cms.Decode(response);
			//cms.CheckSignature(true);

			return cms.ContentInfo.Content;
		}
		/// <summary>
		/// Преобразовать DateTime в строку даты для НБКИ
		/// </summary>
		/// <param name="dateTime">DateTime</param>
		/// <returns>Строка даты в НБКИ</returns>
		public static string DateTimeToString(DateTime dateTime){
			// todo: вернуть regex
			if (dateTime == default) throw new ArgumentException("Wrong DateTime value");
			return $"{dateTime.Year}-{dateTime.Month.ToString("00")}-{dateTime.Day.ToString("00")}";
		}
		/// <summary>
		/// Преобразовать строку даты из НБКИ в DateTime.
		/// </summary>
		/// <param name="date">Строка даты в НБКИ</param>
		/// <returns>DateTime</returns>
		public static DateTime StringToDateTime(String date){
			if (String.IsNullOrEmpty(date)) return default;

			// todo: вернуть regex
			//Regex regex = new Regex(@"(\d\d\d\d)-((0[1-9]|1[012])-(0[1-9]|[12]\d)|(0[13-9]|1[012])-30|(0[13578]|1[02])-31)");
			//MatchCollection matches = regex.Matches(date);

			//foreach (var item in matches) {
				string[] parsedDate = date.Split('-');
				return new DateTime(Int32.Parse(parsedDate[0]), Int32.Parse(parsedDate[1]), Int32.Parse(parsedDate[2]));
			//}

			//throw new ArgumentException($"Wrong Date value({date}). Format (yyyy-mm-dd)");
		}


		public static string Date1CToDateNBCH(string date1C){
			// todo: добавить проверку regex
			string year		= date1C.Substring(0, 4);
			string month	= date1C.Substring(4, 2);
			string day		= date1C.Substring(6, 2);

			return $"{year}-{month}-{day}";
		}

	}
}
