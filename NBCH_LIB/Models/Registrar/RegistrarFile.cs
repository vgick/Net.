using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace NBCH_LIB.Models.Registrar {
	/// <summary>
	/// Информация по загруженному файлу.
	/// </summary>
	[DataContract]
	public struct RegistrarFile {
		/// <summary>
		/// Ключ записи в БД.
		/// </summary>
		[DataMember]
		[Required]
		public int ID {get; set;}

		/// <summary>
		/// Имя файла.
		/// </summary>
		[DataMember]
		[Required]
		[MaxLength(128)]
		public string FileName {get; set;}

		/// <summary>
		/// Кто загрузил файл.
		/// </summary>
		[DataMember]
		[MaxLength(128)]
		public string AuthorName {get; set;}

		/// <summary>
		/// Дата загрузки файла.
		/// </summary>
		[DataMember]
		[Required]
		public DateTime UploadDate { get; set; }

		/// <summary>
		/// Код клиента 1С.
		/// </summary>
		[DataMember]
		[Required]
		public string Client1CCode { get; set; }
	}
}
