using NBCH_LIB.Models;

namespace NBCH_ASP.Models.PDF {
	/// <summary>
	/// Класс расширенный от Region, с чекбоксом.
	/// </summary>
	public class RegionWChecked : Region {
		/// <summary>
		/// Конструктор по умолчанию
		/// </summary>
		public RegionWChecked() { }

		/// <summary>
		/// Конструктор, для создания нового класса на основании родительского класса Region
		/// </summary>
		/// <param name="region">Объект класса Region</param>
		public RegionWChecked(Region region) {
			ADUsers	= region.ADUsers;
			ID		= region.ID;
			Name	= region.Name;
		}

		/// <summary>
		/// Состояние региона - выбран или не выбран
		/// </summary>
		public bool Checked { get; set; } = default;
	}
}
