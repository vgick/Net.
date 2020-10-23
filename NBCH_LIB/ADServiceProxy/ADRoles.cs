using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;

namespace NBCH_LIB.ADServiceProxy {
	/// <summary>
	/// Прокси со списком ролей	пользователей
	/// </summary>
	public class ProxyADRoles {
		/// <summary>
		/// Время через которое требуется обновить права пользователя из AD
		/// </summary>
		private const int _UpdateADRolesTimeoutInMinutes = 60;
		/// <summary>
		/// Роли пользователей в AD (своего рода паттерн прокси)
		/// </summary>
		private readonly ConcurrentDictionary<string, RolesTimes> _ADLoginAndRoles = new ConcurrentDictionary<string, RolesTimes>();
		public string[] this[string login] {
			get {
				if (_ADLoginAndRoles.Keys.Contains(login)) {
					if (_ADLoginAndRoles[login].UpdateTime.AddMinutes(_UpdateADRolesTimeoutInMinutes) > DateTime.Now)
						return _ADLoginAndRoles[login].Roles;
				}

				RolesTimes rolesTimes	= _ADLoginAndRoles.Keys.Contains(login) ? _ADLoginAndRoles[login] : new RolesTimes();
				rolesTimes.UpdateTime	= DateTime.Now;
				rolesTimes.Roles		= GetUsersADRoles(login);

				if (!_ADLoginAndRoles.Keys.Contains(login)) _ADLoginAndRoles.TryAdd(login, rolesTimes);
				_ADLoginAndRoles[login]	= rolesTimes;

				return _ADLoginAndRoles[login].Roles;
			}
		}

		/// <summary>
		/// Получить список ролей пользователя AD
		/// </summary>
		/// <param name="adLogin">логин AD</param>
		/// <returns>Список ролей из AD</returns>
		private string[] GetUsersADRoles(string adLogin) {
			using (PrincipalContext ctx = new PrincipalContext(ContextType.Domain)) {
				UserPrincipal user	= UserPrincipal.FindByIdentity(ctx, adLogin);

				if (user != null) {
					// Только такая реализация. ToArray() и подобное не подходят, из за бага, который MS исправлять
					// не планируют в виду "разумного" способа обхода проблемы (игнорирования исключения???).
					// А ведь на багу ещё наткнуться надо (плавающий баг) и понять от чего у пользователя проблемы. Козлы...
					// FYI https://stackoverflow.com/questions/39525430/system-directoryservices-accountmanagement-nomatchingprincipalexception-an-erro
					List<string> list	= new List<string>();
					PrincipalSearchResult<Principal> groups	= user.GetAuthorizationGroups();
					IEnumerator<Principal> itemGroup = groups.GetEnumerator();
					using (itemGroup) {
						while (itemGroup.MoveNext()) {
							try {
								Principal p = itemGroup.Current;
								list.Add(p.Name);
							}
							catch (NoMatchingPrincipalException ) {
								// Это фишка решения...
								continue;
							}
						}

						return list.ToArray();
					}
				}
			}

			return new string[0];
		}

		/// <summary>
		/// Структура элемента кэша
		/// </summary>
		private struct RolesTimes {
			/// <summary>
			///  Время крайнего обновления данных пользователя из AD
			/// </summary>
			public DateTime UpdateTime {get; set;}

			/// <summary>
			/// Список ролей пользователя в AD
			/// </summary>
			public string[] Roles {get; set;}
		}
	}
}
