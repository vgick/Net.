using NBCH_ASP.Infrastructure;

namespace NBCH_ASP.Models.Registrar.RegistrarRolesAdministration {
	/// <summary>
	/// Модель для индексной страницы
	/// </summary>
	public class RegistrarRolesAdministrationIndex {
		/// <summary>
		/// Группа документов.
		/// </summary>
		public string DocumentGroup	{get; set;}

		/// <summary>
		/// Документы.
		/// </summary>
		public string[] Documents {get; set;}

		/// <summary>
		/// Права на чтения для выбранного типа документа.
		/// </summary>
		public string[] ReadADRoles {get; set;} = new string[0];

		/// <summary>
		/// Права на добавление для выбранного типа документа
		/// </summary>
		public string[] WriteADRoles {get; set;} = new string[0];

		/// <summary>
		/// Список ролей ВЛФ.
		/// </summary>
		public HelperASP.ADSimpleDescription[] ADRolesVLF {get;} = HelperASP.GetADRolesVLF();
	}
}
