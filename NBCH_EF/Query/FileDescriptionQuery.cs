using System;

namespace NBCH_EF.Query {
	/// <summary>
	/// Документы по договору в разрезе.
	/// </summary>
	public class FileDescriptionQuery {
		/// <summary>
		/// ID документа.
		/// </summary>
		public int FDescID { get; set; }

		/// <summary>
		/// Описание документа.
		/// </summary>
		public string FDescDescription { get; set; }

		/// <summary>
		/// ID файла.
		/// </summary>
		public int? FRegitrarID { get; set; }

		/// <summary>
		/// Имя файла.
		/// </summary>
		public string FileName { get; set; }

		/// <summary>
		/// Дата загрузки файла.
		/// </summary>
		public DateTime? UploadDate { get; set; }

		/// <summary>
		/// Порядок вывода файла.
		/// </summary>
		public int SortOrder { get; set; }

		/// <summary>
		/// Имя КЭ.
		/// </summary>
		public string InspectorName { get; set; }

		/// <summary>
		/// Код клиента 1С.
		/// </summary>
		public string Client1CCode { get; set; }
	}
}
