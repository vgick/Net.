using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NBCH_EF.Tables {
	/// <summary>
	/// Связь доступных регионов и пользователей AD.
	/// </summary>
	[Table("ADUserRegionRelations")]
	internal class ADUserRegionRelation {
		/// <summary>
		/// Код региона.
		/// </summary>
		[Required]
		[Column("RegionID")]
		public int RegionID { get; set; }
		public RegionDB Region { get; set; }

		/// <summary>
		/// Код пользователя AD.
		/// </summary>
		[Required]
		[Column("ADUserID")]
		public int ADUserID { get; set; }
		public ADUserDB ADUser { get; set; }

		/// <summary>
		/// Пользователь выбрал регион для просмотра в своем интерфейсе.
		/// </summary>
		[Required]
		public bool ShowToUser { get; set; }
	}
}
