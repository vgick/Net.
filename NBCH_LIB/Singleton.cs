using System.Collections.Generic;

namespace NBCH_LIB {
	/// <summary>
	/// Сервис для работы с AD
	/// </summary>
	public static class Singleton<T> where T : class, new() {
		/// <summary>
		/// Список прокси с кэшем.
		/// </summary>
		private static readonly List<T> _Singleton = new List<T>();

		/// <summary>
		/// Вернуть объект класса.
		/// </summary>
		/// <returns>Объект</returns>
		public static T Values {
			get {
				foreach (T proxy in _Singleton) {
					if (proxy is T) {
						return proxy;
					}
				}

				T newProxy = new T();
				_Singleton.Add(newProxy);

				return newProxy;
			}
		}
	}
}
