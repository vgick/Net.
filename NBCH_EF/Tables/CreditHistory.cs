using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NBCH_EF.Tables.Interface;

namespace NBCH_EF.Tables {
	[Table(name: "CreditHistories")]
	internal class CreditHistory : IDBTableID {
		/// <summary>
		///  ID записи.
		/// </summary>
		[Key]
		[Column("ID")]
		public int ID {get; set;}

		/// <summary>
		/// Дата и время загрузки файла.
		/// </summary>
		[Required]
		[Column(TypeName = "datetime")]
		public DateTime Date {get; set;}

		/// <summary>
		/// Клиент, которому принадлежит анкета.
		/// </summary>
		[Required]
		[Column("Client_ID")]
		public int? ClientID { get; set; }
		public ClientDB Client {get; set;}

		/// <summary>
		/// Номер договора в 1С.
		/// </summary>
		[Column("Account1CID_Account1CCode")]
		public string Account1CidAccount1CCode { get; set; }
		public Account1C Account1CID {get; set;}

		/// <summary>
		/// Подписанная анкета.
		/// </summary>
		[Required]
		[Column("SignedXML")]
		public byte[] SignedXML {get; set;}

		/// <summary>
		/// Анкета без подписи.
		/// </summary>
		[Column("UnSignedXML")]
		public byte[] UnSignedXML { get; set; }

		/// <summary>
		/// Локальное время на клиенте.
		/// </summary>
		public int ClientTimeZone {get; set;}

		/// <summary>
		/// Код ошибка НБКИ.
		/// </summary>
		public string ErrorCode {get; set;}

		/// <summary>
		/// Текст ошибки НБКИ.
		/// </summary>
		public string ErrorText {get; set;}
	}
}
