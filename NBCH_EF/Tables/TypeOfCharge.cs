using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NBCH_EF.Tables.Interface;

namespace NBCH_EF.Tables {
	/// <summary>
	/// Канал перевода средств.
	/// </summary>
	[Table("TypeOfChargeDBs")]
	internal class TypeOfChargeDB : IDBTableName {
		/// <summary>
		/// Ключевое поле.
		/// </summary>
		[Key]
		[Column("ID")]
		public int ID { get; set; }

		/// <summary>
		/// Наименование.
		/// </summary>
		[Required]
		[MaxLength(20)]
		public string Name { get; set; }

		/// <summary>
		/// Договора.
		/// </summary>
		public virtual ICollection<Account1C> Account1C { get; set; }

	}
}