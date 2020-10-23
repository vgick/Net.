using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;
using NBCH_LIB.Interfaces;

namespace NBCH_LIB.WCFProxy {
	/// <summary>
	/// Прокси класс
	/// </summary>
	public class PDFSaverProxy : ClientBase<IPDFSaver>, IPDFSaver {
		#region Конструкторы
		public PDFSaverProxy() { }
		public PDFSaverProxy(string endpointName) : base(endpointName) { }
		public PDFSaverProxy(Binding binding, EndpointAddress remoteAddress) : base(binding, remoteAddress) { }
		#endregion
		/// <summary>
		/// Получить PDF файл.
		/// </summary>
		/// <param name="clientFIO">Имя файла</param>
		/// <returns>содержимое файла</returns>
		public byte[][] GetPDFByName(string clientFIO) => Channel.GetPDFByName(clientFIO);

		/// <summary>
		/// Получить PDF файл асинхронно.
		/// </summary>
		/// <param name="clientFIO">Имя файла</param>
		/// <returns>Содержимое файла</returns>
		public async Task<byte[][]> GetPDFByNameAsync(string clientFIO) => await Channel.GetPDFByNameAsync(clientFIO);

		/// <summary>
		/// Сохранить PDF в базе.
		/// </summary>
		/// <param name="clientFIO">ФИО клиента</param>
		/// <param name="data">Файл в двоичном виде</param>
		/// <param name="date">Дата сохранения файла на диск</param>
		/// <param name="user">Пользователь загрузивший файл</param>
		/// <param name="region">Регион пользователя</param>
		/// <returns>Результат</returns>
		public void SavePDF(string clientFIO, byte[] data, DateTime date, string user, string region)
				=> Channel.SavePDF(clientFIO, data, date, user, region);

		/// <summary>
		/// Сохранить PDF в базе асинхронно.
		/// </summary>
		/// <param name="clientFIO">ФИО клиента</param>
		/// <param name="data">Файл в двоичном виде</param>
		/// <param name="date">Дата сохранения файла на диск</param>
		/// <param name="user">Пользователь загрузивший файл</param>
		/// <param name="region">Регион пользователя</param>
		/// <returns>Результат</returns>
		public async Task SavePDFAsync(string clientFIO, byte[] data, DateTime date, string user, string region)
				=> await Channel.SavePDFAsync(clientFIO, data, date, user, region);
	}
}
