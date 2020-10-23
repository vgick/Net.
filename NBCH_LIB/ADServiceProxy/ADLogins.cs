using System;
using System.Collections.Concurrent;
using System.DirectoryServices.AccountManagement;

namespace NBCH_LIB.ADServiceProxy {
	/// <summary>
	/// Прокси данных пользователей из AD / базы данных
	/// </summary>
	public class ProxyADLogins {
		/// <summary>
		/// Время через которое необходимо сверить данные в БД в днях
		/// </summary>
		protected static int UpdateIntervalInDay = 1;

		/// <summary>
		/// Закэшированные данные
		/// </summary>
		protected ConcurrentDictionary<string, IADLogin> _ADLogins {get; set;} = new ConcurrentDictionary<string, IADLogin>();

		/// <summary>
		/// Блокировка объекта на время запроса данных
		/// </summary>
		protected readonly object Locker = new object();

		/// <summary>
		/// Получить данные из кэша
		/// </summary>
		/// <param name="login">Поиск по логину</param>
		/// <returns>Данные из кэша</returns>
		public ADLogin this[string login] {
			get{
				if (login.IndexOf('\\') == -1) throw new ArgumentException($"Неверный формат логина AD. Логин должен быть вида: \"domain\\login\". Передан логин \"{login}\"");

				login = login.Substring(login.IndexOf('\\'));

				lock (Locker) {
					if (_ADLogins.Keys.Contains(login)) {
						if (_ADLogins[login].UpdateDate.AddDays(UpdateIntervalInDay) < DateTime.Now) {
							_ADLogins[login].UpdateDate	= DateTime.Now;
							_ADLogins[login].Name		= GetNameByLogin(login);
						}
					}
					else {
						ADLogin adLogins = new ADLogin() {
							Name		= GetNameByLogin(login),
							UpdateDate	= DateTime.Now
						};

						_ADLogins.TryAdd(login, adLogins);
					}
				}

				return (ADLogin)_ADLogins[login];
			}
		}

		/// <summary>
		/// Получить из AD полное имя пользователя
		/// </summary>
		/// <param name="login"></param>
		/// <returns>Полное имя пользователя</returns>
		protected static string GetNameByLogin(string login) {
			PrincipalContext ctx	= new PrincipalContext(ContextType.Domain);
			UserPrincipal user		= UserPrincipal.FindByIdentity(ctx, login);

			if (user != null)
				return user.DisplayName;
			
			return string.Empty;
		}
	}
}
