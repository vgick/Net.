using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NBCH_EF.Tables.Interface;
using NBCH_LIB.Models.Registrar;

namespace NBCH_EF.Tables {
	[Table("RegistrarFileDBs")]
	internal class RegistrarFileDB : IDBTableID {
		/// <summary>
		/// Ключевое поле
		/// </summary>
		[Column("ID")]
		public int ID {get; set;}

		/// <summary>
		/// Договор 1С к которому привязан документ.
		/// </summary>
		[Required]
		[Column("Account1C_Account1CCode")]
		public string Account1CAccount1CCode { get; set; }
		public Account1C Account1C {get; set;}

		/// <summary>
		/// К кому привязан документ.
		/// </summary>
		[Required]
		[Column("Client_ID")]
		public int ClientID { get; set; }
		public ClientDB Client {get; set;}

		/// <summary>
		/// Вид документа.
		/// </summary>
		[Required]
		[Column("FileDescriptionDB_ID")]
		public int FileDescriptionDbID { get; set; }
		public FileDescriptionDB FileDescriptionDB {get; set;}

		/// <summary>
		/// Дата добавления.
		/// </summary>
		[Required]
		[Column(TypeName = "datetime")]
		public DateTime UploadDate {get; set;}

		/// <summary>
		/// Кто добавил файл.
		/// </summary>
		[Required]
		[Column("AuthorName_ID")]
		public int AuthorNameID { get; set; }
		public ADLoginsDB AuthorName {get; set;}

		/// <summary>
		/// Имя файла при загрузке.
		/// </summary>
		[Required]
		[MaxLength(128)]
		public string FileName {get; set;}

		/// <summary>
		/// Содержимое файла.
		/// </summary>
		[Required]
		public byte[] File {get; set;}

		/// <summary>
		/// Часовой пояс пользователя, загрузившего файл.
		/// </summary>
		[Required]
		public int TimeZone { get; set; }

		/// <summary>
		/// Файл помечен как удаленный.
		/// </summary>
		public bool Delete { get; set; }

		/// <summary>
		/// Приведение RegistrarFile к RegistrarFileDB.
		/// </summary>
		/// <param name="value">RegistrarFileDB</param>
		public static implicit operator RegistrarFile(RegistrarFileDB value){
			RegistrarFile registrarFile	= new RegistrarFile() {
				AuthorName	= value.AuthorName.Name,
				FileName	= value.FileName,
				UploadDate	= value.UploadDate,
				ID			= value.ID
			};

			return registrarFile;
		}
	}
}
