using System;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace NBCH_ASP.Models.PDF {
	/// <summary>
	/// Меню администратора пользователей/регионов.
	/// </summary>
	public class AdminMenu {
		public static string GetUsersListStatus(ViewContext context) => GetMenuItemStatus(context, "ADUsersList");
		public static string GetRegionsListStatus(ViewContext context) => GetMenuItemStatus(context, "RegionsList");
		private static string GetMenuItemStatus(ViewContext context, string menuItem){
			return String.Equals(context?.ViewData["SelectedMenuItem"], menuItem) ? "active" : "";
		}
	}
}
