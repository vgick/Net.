using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using NBCH_EF.Tables.Interface;
using NBCH_LIB.Models;

namespace NBCH_EF.Tables {
	/// <summary>
	/// Пользователи, которые добавили запись.
	/// </summary>
	[Table("ADUserDBs")]
	internal class ADUserDB : IDBTableID {
		public ADUserDB() => ADUserRegionRelations = new List<ADUserRegionRelation>();

		/// <summary>
		/// Ключевое поле.
		/// </summary>
		[Key]
		[Column("ID")]
		public int ID { get; set; }

		/// <summary>
		/// Имя пользователя.
		/// </summary>
		[Required]
		[MaxLength(100)]
		[Column("ADName")]
		public string ADName { get; set; }

		/// <summary>
		/// Список регионов, доступных пользователю.
		/// </summary>
		public List<ADUserRegionRelation> ADUserRegionRelations { get; set; }

		/// <summary>
		/// Преобразование ADUser из сервиса в ADUser DB.
		/// </summary>
		/// <param name="adUserDB">На входе пользователь из БД</param>
		public static implicit operator ADUser(ADUserDB adUserDB) {
			ADUser user = new ADUser {
				ADName	= adUserDB.ADName,
				ID		= adUserDB.ID,
				Regions	= adUserDB.ADUserRegionRelations.Select(i => new Region {Name = i.Region.Name, ID = i.RegionID})
					.ToArray()
			};

			return user;
		}
	}
}
