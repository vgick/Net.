using System.ComponentModel.DataAnnotations;

namespace NBCH_ASP.Models.Registrar.RegistrarRolesAdministration {
	/// <summary>
	/// Модель для добавления нового описания файла.
	/// </summary>
	public class AddFileDescriptionModel {
		/// <summary>
		/// Описание файла
		/// </summary>
		[Required]
		public string Description {get; set;}

		/// <summary>
		/// Группа документа
		/// </summary>
		[Required]
		public string DocumentGroup {get; set;}
	}
}
