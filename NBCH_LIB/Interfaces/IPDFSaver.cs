using System;
using System.ServiceModel;
using System.Threading.Tasks;
using NBCH_LIB.Interfaces.WCF;

namespace NBCH_LIB.Interfaces {
	
	[ServiceContract]
	public interface IPDFSaver : IWCFContract {
		/// <summary>
		/// Сохранить PDF в базе.
		/// </summary>
		/// <param name="clientFIO">ФИО клиента</param>
		/// <param name="data">Файл в двоичном виде</param>
		/// <param name="date">Дата сохранения файла на диск</param>
		/// <param name="user">Пользователь загрузивший файл</param>
		/// <param name="region">Регион пользователя</param>
		/// <returns>Результат</returns>
		[OperationContract]
		void SavePDF(string clientFIO, byte[] data, DateTime date, string user, string region);

		/// <summary>
		/// Сохранить PDF в базе.
		/// </summary>
		/// <param name="clientFIO">ФИО клиента</param>
		/// <param name="data">Файл в двоичном виде</param>
		/// <param name="date">Дата сохранения файла на диск</param>
		/// <param name="user">Пользователь загрузивший файл</param>
		/// <param name="region">Регион пользователя</param>
		/// <returns>Результат</returns>
		[OperationContract]
		Task SavePDFAsync(string clientFIO, byte[] data, DateTime date, string user, string region);

		/// <summary>
		/// Получить PDF файл.
		/// </summary>
		/// <param name="clientFIO">Имя файла</param>
		/// <returns>содержимое файла</returns>
		[OperationContract]
		byte[][] GetPDFByName(string clientFIO);


		/// <summary>
		/// Получить PDF файл асинхронно.
		/// </summary>
		/// <param name="clientFIO">Имя файла</param>
		/// <returns>содержимое файла</returns>
		[OperationContract]
		Task<byte[][]> GetPDFByNameAsync(string clientFIO);
	}
}
