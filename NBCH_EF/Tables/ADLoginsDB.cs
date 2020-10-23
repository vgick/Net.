using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NBCH_EF.Tables.Interface;
using NBCH_LIB.ADServiceProxy;

namespace NBCH_EF.Tables {
	/// <summary>
	/// Описание пользователя из AD.
	/// </summary>
	[Table("ADLoginsDBs")]
	internal class ADLoginsDB : IDBTableID, IADLogin {
		/// <summary>
		/// Ключевое поле.
		/// </summary>
		[Key]
		[Column("ID")]
		public int ID { get; set; }

		/// <summary>
		/// Логин пользователя AD.
		/// </summary>
		[Required]
		[MaxLength(128)]
		public string Login {get; set;}

		/// <summary>
		/// Имя пользователя AD.
		/// </summary>
		[Required]
		[MaxLength(128)]
		public string Name {get; set;}

		/// <summary>
		/// Дата обновления записи.
		/// </summary>
		[Column(TypeName = "datetime")]
		public DateTime UpdateDate {get; set;}
		
		/// <summary>
		/// Проверяющий.
		/// </summary>
		public List<AccountInspectingDB> AccountInspectingDbs { get; set; } = new List<AccountInspectingDB>();
		
	}
}
