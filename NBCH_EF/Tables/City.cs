using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NBCH_EF.Tables.Interface;

namespace NBCH_EF.Tables {
	/// <summary>
	/// Города.
	/// </summary>
	[Table("Cities")]
	internal class City : IDBTableName {
		/// <summary>
		/// ID записи.
		/// </summary>
		[Key]
		[Column("ID")]
		public int ID { get; set; }

		/// <summary>
		/// Наименование города.
		/// </summary>
		[Required]
		public string Name { get; set; }

		/// <summary>
		/// Договор.
		/// </summary>
		public virtual ICollection<Account1C> Account1C { get; set; }
	}
}