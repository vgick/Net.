
namespace NBCH_EF.Query {
	/// <summary>
	/// Доступы к документам.
	/// </summary>
	public class RoleListQuery {
		/// <summary>
		/// ID документа.
		/// </summary>
		public int FDescID { get; set; }

		/// <summary>
		/// Описание документа.
		/// </summary>
		public string FDescDescription { get; set; }

		/// <summary>
		/// Роли с доступом на чтение.
		/// </summary>
		public string ReadAccess { get; set; }

		/// <summary>
		/// Роли с доступом на запись.
		/// </summary>
		public string WriteAccess { get; set; }

		/// <summary>
		/// Порядок сортировки.
		/// </summary>
		public int SortOrder { get; set; }
	}
}
