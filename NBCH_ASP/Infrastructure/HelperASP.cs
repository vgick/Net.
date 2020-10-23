using NBCH_ASP.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace NBCH_ASP.Infrastructure {
	public static class HelperASP {
		/// <summary>
		/// Префикс ролей AD, для настройки прав в системе Архива.
		/// </summary>
		private const string VLF_AD_PREFIX	= "vlf-serviceRegistrar-role-";

		private const int _TimeOut = 300_000;

		/// <summary>
		/// Возвращает имя контроллера без постфикса "Controller".
		/// </summary>
		/// <param name="type"></param>
		/// <returns>Имя без слова "Controller"</returns>
		public static string ControllerNameFromClass(Type type) {
			if (!type.Name.EndsWith("Controller", StringComparison.OrdinalIgnoreCase)) throw new ArgumentException(@"The class name must end with ""Controller""");
			return type.Name.Substring(0, type.Name.Length - "Controller".Length);
		}

		/// <summary>
		/// Цвет надписи-предупреждения.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string NegativeLabel(int value) => value != 0 ? " text-danger font-weight-bold" : "";

		/// <summary>
		/// Получить список ролей AD привязанных к VLF (начинаются с vlf-).
		/// </summary>
		/// <returns>Список всех ролей AD</returns>
		public static ADSimpleDescription[] GetADRolesVLF() {
			PrincipalContext pcRoot	= new PrincipalContext(ContextType.Domain);
			GroupPrincipal qbeGroup	= new GroupPrincipal(pcRoot);
			PrincipalSearcher srch	= new PrincipalSearcher(qbeGroup);

			ADSimpleDescription[] adRoles	= srch.FindAll().
				Where(i => i is GroupPrincipal && i.Name.StartsWith(VLF_AD_PREFIX)).
				Select(i => new ADSimpleDescription() {Name = i.Name, Description = i.Description}).ToArray();

			return adRoles;
		}

		/// <summary>
		/// Упрощенное описание роли AD.
		/// </summary>
		public struct ADSimpleDescription{
			/// <summary>
			/// Роль в AD
			/// </summary>
			public string Name {get; set;}
			/// <summary>
			/// Описание роли в AD
			/// </summary>
			public string Description {get; set;}
		}

		/// <summary>
		/// Логин пользователя.
		/// </summary>
		/// <param name="user"></param>
		/// <returns></returns>
		public static string Login(ClaimsPrincipal user = null) {
			#if DEBUG
				return	@"login";
			#else
				return user?.Identity.Name ?? "";
			#endif
		}

		// public static async Task<(T result, string error)> GetFromAwait<T>(Task<T> task) {
		// 	bool success = false;
		// 	try {
		// 		success	= await Task.Run(() => task.Wait(_TimeOut));
		// 	}
		// 	catch (Exception exception) {
		// 		throw exception.InnerException ?? exception;
		// 	}
		//
		// 	return !success ? (default, $"Время вызова метода вышло ({_TimeOut/1000}) секунд.") : (task.Result, "");
		// }
	}
}
