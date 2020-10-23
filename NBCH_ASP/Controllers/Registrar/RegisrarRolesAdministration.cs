using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NBCH_ASP.Models.Registrar;
using NBCH_ASP.Models.Registrar.RegistrarRolesAdministration;
using NBCH_LIB.Interfaces;
using NBCH_LIB.Models.Registrar;

namespace NBCH_ASP.Controllers.Registrar {
	/// <summary>
	/// Контроллер для настройки прав пользователей для работы с пакетом документов.
	/// </summary>
	[Authorize(Roles = @"admin")]
	public class RegistrarRolesAdministrationController : Controller {
		/// <summary>
		/// Отобразить информацию по документу (вариант POST запроса).
		/// </summary>
		public const string GetDirectoryInfo	= "GetDocumentInfo";

		/// <summary>
		/// Обновить в БД информацию по документу (вариант POST запроса).
		/// </summary>
		public const string UpdateDirectoryInfo	= "UpdateDocumentInfo";

		/// <summary>
		/// Сервис для работы с архивом.
		/// </summary>
		private readonly IServiceRegistrar _ServiceRegistrar;

		/// <summary>
		/// Конструктор.
		/// </summary>
		/// <param name="serviceRegistrar">Сервис для работы с документами</param>
		public RegistrarRolesAdministrationController(IServiceRegistrar serviceRegistrar) {
			_ServiceRegistrar	= serviceRegistrar;
		}

		/// <summary>
		/// Отображение описание выбранного/первого файла в категории.
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public async Task<IActionResult> FileDescription() {
			RegistrarRolesAdministrationIndex index	= new RegistrarRolesAdministrationIndex();
			FileDescription[] fileDescriptions		=
				await _ServiceRegistrar.GetFilesDescriptionsByDocumentGroupNameAsync(
					Presets.DocumentGroup1CAccount,
					CancellationToken.None);

			string selectedDocument			= fileDescriptions.Length > 0 ? fileDescriptions[0].Descrioption : "";
			FileDescription fileDescription	= fileDescriptions.FirstOrDefault(i => i.Descrioption.Equals(selectedDocument));

			index.Documents		= fileDescriptions.Select(i => i.Descrioption).ToArray();
			index.DocumentGroup	= Presets.DocumentGroup1CAccount;
			index.ReadADRoles	= fileDescriptions.FirstOrDefault(i => i.Descrioption.Equals(selectedDocument))?.ReadADRoles ?? new string[0];
			index.WriteADRoles	= fileDescriptions.FirstOrDefault(i => i.Descrioption.Equals(selectedDocument))?.WriteADRoles ?? new string[0];

			ViewData["SelectedDocument"] = selectedDocument;

			return View(index);
		}

		/// <summary>
		/// Отображение/редактирование описание выбранного файла.
		/// </summary>
		/// <param name="documentGroup">Группа документов</param>
		/// <param name="selectedDocument">Выбранный документ</param>
		/// <param name="readADRolesVLF">роли для чтения выбранного документа</param>
		/// <param name="writeADRolesVLF">роли для добавления выбранного документа</param>
		/// <param name="submitType">Отобразить/обновить описание выбранного файла</param>
		/// <returns>Описание файла</returns>
		[HttpPost]
		public async Task<ActionResult> FileDescription(string documentGroup, string selectedDocument, string[] readADRolesVLF, string[] writeADRolesVLF, string submitType) {
			RegistrarRolesAdministrationIndex index	= new RegistrarRolesAdministrationIndex();
			FileDescription[] fileDescriptions		=
				await _ServiceRegistrar.GetFilesDescriptionsByDocumentGroupNameAsync(
					Presets.DocumentGroup1CAccount,
					CancellationToken.None);

			FileDescription fileDescription	= fileDescriptions.FirstOrDefault(i => i.Descrioption.Equals(selectedDocument));

			if (submitType == UpdateDirectoryInfo && fileDescription != default){
				fileDescription.ReadADRoles		= readADRolesVLF;
				fileDescription.WriteADRoles	= writeADRolesVLF;
				await _ServiceRegistrar.UpdateFileDescriptionAsync(fileDescription, CancellationToken.None);
			}

			ViewData["SelectedDocument"] = selectedDocument;

			index.Documents		= fileDescriptions.Select(i => i.Descrioption).ToArray();
			index.DocumentGroup	= documentGroup;
			index.ReadADRoles	= fileDescriptions.FirstOrDefault(i => i.Descrioption.Equals(selectedDocument))?.ReadADRoles ?? new string[0];
			index.WriteADRoles	= fileDescriptions.FirstOrDefault(i => i.Descrioption.Equals(selectedDocument))?.WriteADRoles ?? new string[0];
			return View(index);
		}

		/// <summary>
		/// Форма добавления нового описания файла.
		/// </summary>
		/// <param name="documentGroup"></param>
		/// <returns>Форма для заполнения</returns>
		[HttpGet]
		public IActionResult AddFileDescription(string documentGroup) {
			AddFileDescriptionModel model = new AddFileDescriptionModel {DocumentGroup = documentGroup};

			return View(model);
		}

		/// <summary>
		/// Добавление нового описания файла.
		/// </summary>
		/// <param name="documentGroup">Группа документов для описания</param>
		/// <param name="description">Описание</param>
		/// <returns>Представление</returns>
		[HttpPost]
		public async Task<IActionResult> AddFileDescription(string documentGroup, string description) {
			FileDescription fileDescription	= new FileDescription() {Descrioption = description};
			await _ServiceRegistrar.AddFileDescriptionAsync(documentGroup, fileDescription, CancellationToken.None);

			return RedirectToAction("FileDescription");
		}
	}
}
