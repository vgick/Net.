using System;

namespace NBCH_LIB.ADServiceProxy {
	// EF Core не поддерживает одну таблицу на класс с наследованием всех полей, поэтому интерфейс.

	/// <summary>
	/// Описание логина в AD
	/// </summary>
	public interface IADLogin {
		/// <summary>
		/// Время обновления данных пользователя из AD
		/// </summary>
		DateTime UpdateDate { get; set; }

		/// <summary>
		/// Полное имя пользователя в AD
		/// </summary>
		string Name { get; set; }
	}
}
