using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace NBCH_ASP.Models.Middleware {
	/// <summary>
	/// Не используется. Список ролей вернул из конфига в код.
	/// Нет большого смысла оставлять для настройки.
	/// Сервис авторизации.
	/// </summary>
	public class AuthService {
		/// <summary>
		/// Следующий сервис middleware
		/// </summary>
		private readonly RequestDelegate _RequestDelegate;

		/// <summary>
		/// Конфигурация
		/// </summary>
		private static IConfiguration _Config;

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="requestDelegate">RequestDelegate</param>
		/// <param name="config">Конфигурация</param>
		public AuthService(RequestDelegate requestDelegate, IConfiguration config) {
			_RequestDelegate	= requestDelegate;
			_Config				= config;
		}

		/// <summary>
		/// Middleware сервис авторизации
		/// </summary>
		/// <param name="httpContext">HttpContext</param>
		/// <returns></returns>
		public async Task InvokeAsync(HttpContext httpContext){
			string actionName		= httpContext.Request.RouteValues["action"]?.ToString() ?? String.Empty;
			string controllerName	= httpContext.Request.RouteValues["controller"]?.ToString() ?? String.Empty;

			if (actionName == String.Empty || controllerName == String.Empty){
				httpContext.Response.StatusCode	= 403;
				await _RequestDelegate(httpContext);
				return;
			}

			#region Выборка ролей
			// Роли Атрибута авторизации для данного контроллера и действия
			Assembly assembly	= Assembly.GetExecutingAssembly();
			var authRoles	= assembly.GetTypes().Where(p => typeof(Controller).IsAssignableFrom(p))
				.SelectMany(p => p.GetMethods())
				.Where(method =>
					method.IsPublic
					// Атрибут авторизации
					&& method.IsDefined(typeof(AuthAttribute))
					// Метод (атрибут ActionName или имя действия) действия совпадает с текущим методом действия
					&& (((ActionNameAttribute)method.GetCustomAttributes(typeof(ActionNameAttribute), false).FirstOrDefault())?.Name ?? method.Name).Equals(actionName)
					// Контроллер совпадает с текущим
					&& method.ReflectedType.Name.Substring(0, method.ReflectedType.Name.Length - @"Controller".Length) == controllerName)
				// Выбираем роли
				.Select(p => p.GetCustomAttributes(typeof(AuthAttribute), false));
			#endregion
			
			// Отладочный код
			string str = String.Empty;
			foreach (var roles in authRoles) {
				foreach (AuthAttribute role in roles) {
					if (AuthAttribute.IsInGroup(httpContext, role.Role, _Config)) {
						str	= GetHTML($"<h2>YES</h2>");
						break;
					}
				}

				if (str != String.Empty) break;
			}

			int count = 0;
			foreach (var role in authRoles) count++;
			if (str == String.Empty && count > 0) {
				httpContext.Response.StatusCode = 403; //str = getHTML("NO");
				
				await httpContext.Response.WriteAsync("<!DOCTYPE html>" +
				"<html lang='ru'>" +
				"<head><meta charset=\"utf-8\"></head>" +
				"<body>" +
				"Ошибка: 405; Доступ запрещен" +
				"</body>" +
				"</html>");
				return;
			}

			await _RequestDelegate(httpContext);
		}

		/// <summary>
		/// Отладочное сообщение
		/// </summary>
		/// <param name="html">Текст сообщения</param>
		/// <returns></returns>
		private static string GetHTML(string html){
			string res	= @"<html>";
			res			+= html;
			res			+= @"</html>";

			return res;
		}
	}

	/// <summary>
	/// Метод расширения для класса IApplicationBuilder
	/// </summary>
	public static class Extention {
		/// <summary>
		/// Использовать кастомный сервис авторизации
		/// </summary>
		/// <param name="app">IApplicationBuilder</param>
		public static void UseAuthService (this IApplicationBuilder app) => app.UseMiddleware<AuthService>();
	}

}
