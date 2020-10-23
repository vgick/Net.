using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using NBCH_EF.Tables.Interface;
using NBCH_LIB.Models;

namespace NBCH_EF.Tables {
	/// <summary>
	/// Регион с которого пришла анкета.
	/// </summary>
	[Table("RegionDBs")]
	internal class RegionDB : IDBTableName {
		public RegionDB() => ADUserRegionRelations = new List<ADUserRegionRelation>();
		/// <summary>
		/// Ключевое поле
		/// </summary>
		[Key]
		[Column("ID")]
		public int ID { get; set; }

		/// <summary>
		/// Наименование региона.
		/// </summary>
		[Required]
		[MaxLength(100)]
		public string Name { get; set; }

		/// <summary>
		/// Список пользователей, у которых есть доступ к данным региона.
		/// </summary>
		public List<ADUserRegionRelation> ADUserRegionRelations { get; set; }

		/// <summary>
		/// Преобразовать Region из сервиса в RegionDB из БД.
		/// </summary>
		/// <param name="regionDB"></param>
		public static implicit operator Region(RegionDB regionDB) {
			Region region = new Region {
				Name	= regionDB.Name,
				ID		= regionDB.ID,
				ADUsers	= regionDB.ADUserRegionRelations
					.Select(i => new ADUser {ADName = i.ADUser.ADName, ID = i.ADUserID}).ToArray()
			};

			return region;
		}
	}
}
