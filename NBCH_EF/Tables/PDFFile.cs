using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NBCH_EF.Tables.Interface;

namespace NBCH_EF.Tables {
	/// <summary>
	/// Описание PDF в БД.
	/// </summary>
	[Table("PDFFiles")]
	internal class PDFFile : IDBTableID {
		/// <summary>
		/// Ключевое поле.
		/// </summary>
		[Key]
		[Column("ID")]
		public int ID { get; set; }

		/// <summary>
		/// Дата добавления файла.
		/// </summary>
		[Required]
		[Column(TypeName = "datetime")]
		public DateTime Date { get; set; }

		/// <summary>
		/// Клиент.
		/// </summary>
		[Required]
		[Column("Client_ID")]
		public int ClientID { get; set; }
		public ClientDB Client { get; set; }

		/// <summary>
		/// Содержимое файла.
		/// </summary>
		[Required]
		public byte[] Data { get; set; }

		/// <summary>
		/// Пользователь добавивший запись.
		/// </summary>
		[Required]
		[Column("ADUser_ID")]
		public int AduserID { get; set; }
		public ADUserDB ADUser { get; set; }

		/// <summary>
		/// Регион с которого пришла анкета.
		/// </summary>
		[Required]
		[Column("Region_ID")]
		public int RegionID { get; set; }
		public RegionDB Region { get; set; }
	}
}
