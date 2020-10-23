using Microsoft.AspNetCore.Mvc.Rendering;

namespace NBCH_ASP.Infrastructure.Registrar {
	public static class RegistrarMenu {
		/// <summary>
		/// Пункт меню - архив документов.
		/// </summary>
		/// <param name="context">Контекст</param>
		/// <returns>Статус пункта меню</returns>
		public static string GetArchiveItemStatus(ViewContext context) => GetStatus(context, "Archive");

		/// <summary>
		/// Пункт меню - отчет руководителя.
		/// </summary>
		/// <param name="context">Контекст</param>
		/// <returns>Статус пункта меню</returns>
		public static string GetDepartmentReportItemStatus(ViewContext context) => GetStatus(context, "DepartmentReport");

		/// <summary>
		/// Статус пункта меню.
		/// </summary>
		/// <param name="context">Контекст</param>
		/// <param name="itemName">Имя пункта меню</param>
		/// <returns>Статус пункта меню</returns>
		private static string  GetStatus(ViewContext context, string itemName) => ((string)context?.ViewData["RegistrarMenuItem"] ?? "") == itemName ? "active" : "";
	}
}
