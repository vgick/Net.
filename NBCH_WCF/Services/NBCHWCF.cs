using System;
using NBCHLibrary.Interfaces;
using NBCHWCF.Source;
using static NBCHWCF.Source.Security;
using static NBCHWCF.Source.Utils;

namespace NBCHWCFNS {
	public class NBCHWCF  : IPDFSaver {
		/// <summary>
		/// Группа доступа из конфигурационного файла
		/// </summary>
		private static string AccessGroup = "WCFPermissionADGroup";
		/// <summary>
		/// Хранилище PDF файлов
		/// </summary>
		static public IPDFSaver PDFSaver	{get; set;} = default;
		/// <summary>
		/// Конструктор без параметров
		/// </summary>
		public NBCHWCF(){}
		/// <summary>
		/// Конструктор. На вход подается хранилище PDF файлов.
		/// </summary>
		/// <param name="pdfSaver"></param>
		public NBCHWCF(IPDFSaver pdfSaver) => PDFSaver = pdfSaver;
		/// <summary>
		/// Получить PDF файл
		/// </summary>
		/// <param name="fileName">имя файла</param>
		/// <returns>содержимое файла</returns>
		public byte[][] GetPDFByName(string clientFIO) {
			if (PDFSaver == default) ThrowFaultException(new Exception("Source base not set"));
			CheckAccess(AccessGroup);
			if (String.IsNullOrEmpty(clientFIO)) ThrowFaultException(new ArgumentNullException());
			return ExecuteWithTryCatch(() => PDFSaver.GetPDFByName(clientFIO));
		}
		/// <summary>
		/// Сохранить PDF в базе
		/// </summary>
		/// <param name="fileName">Имя базы</param>
		/// <param name="data">Файл в двоичном виде</param>
		/// <returns>Результат</returns>
		public void SavePDF(string clientFIO, byte[] data, DateTime date, string user, string region) {
			if (PDFSaver == default) ThrowFaultException(new Exception("Source base not set"));
			CheckAccess(AccessGroup);
			if (String.IsNullOrEmpty(clientFIO) || data == default || date == default || String.IsNullOrEmpty(user) || String.IsNullOrEmpty(region))
				ThrowFaultException(new ArgumentNullException());
			ExecuteWithTryCatch(() => PDFSaver.SavePDF(clientFIO, data, date, user, region));
		}
	}
}
