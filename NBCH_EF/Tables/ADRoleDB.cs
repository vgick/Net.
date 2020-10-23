using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NBCH_EF.Tables.Interface;

namespace NBCH_EF.Tables {
	/// <summary>
	/// Роли AD.
	/// </summary>
	[Table("ADRoleDBs")]
	internal class ADRoleDB : IDBTableID {
		/// <summary>
		/// Ключевое поле.
		/// </summary>
		[Key]
		[Column("ID")]
		public int ID { get; set; }

		/// <summary>
		/// Роль.
		/// </summary>
		[Required]
		[MaxLength(256)]
		public string Role {get; set;}

		/// <summary>
		/// Права на чтение в таблице FileDescriptionDB
		/// </summary>
		[Column("FileDescriptionDB_ID")]
		public int? ReadADRolesID { get; set; }
		public virtual FileDescriptionDB ReadADRoles { get; set; }

		/// <summary>
		/// Права на запись в таблице FileDescriptionDB
		/// </summary>
		[Column("FileDescriptionDB_ID1")]
		public int? WriteADRolesID { get; set; }
		public virtual FileDescriptionDB WriteADRoles { get; set; }


	}
}
