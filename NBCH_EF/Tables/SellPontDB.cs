using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NBCH_EF.Tables.Interface;
using NBCH_LIB.Models;

namespace NBCH_EF.Tables {
	/// <summary>
	/// Точка заключения сделки.
	/// </summary>
	[Table("SellPontDBs")]
	internal class SellPontDB : IDBTableIDNameW1CCode {
		/// <summary>
		/// Ключевое поле.
		/// </summary>
		[Key]
		[Column("ID")]
		public int ID { get; set; }

		/// <summary>
		/// Наименование точки заключения сделки.
		/// </summary>
		[Required]
		[MaxLength(128)]
		public string Name { get; set; }

		/// <summary>
		/// Код 1С.
		/// </summary>
		[Required]
		[MaxLength(10)]
		public string Code1C { get; set; }

		/// <summary>
		/// Договора.
		/// </summary>
		public virtual ICollection<Account1C> Account1C { get; set; }

		public static implicit operator SellPoint(SellPontDB value) {
			SellPoint result	= new SellPoint() {
				SellPoint1CCode	= value.Code1C,
				SellPointName	= value.Name
			};

			return result;
		}
	}
}