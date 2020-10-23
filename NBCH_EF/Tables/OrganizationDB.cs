using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NBCH_EF.Tables.Interface;

namespace NBCH_EF.Tables {
	/// <summary>
	/// Организации, заключающие договоры.
	/// </summary>
	internal class OrganizationDB : IDBTableName {
		/// <summary>
		/// ID записи.
		/// </summary>
		[Key]
		public int ID { get; set; }
		/// <summary>
		/// Наименование организации.
		/// </summary>
		[Required]
		[MaxLength(128)]
		public string Name { get; set; }

		/// <summary>
		/// Договора.
		/// </summary>
		public virtual ICollection<Account1C> Account1C { get; set; }
	}
}