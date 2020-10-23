using System;

namespace NBCH_LIB.ADServiceProxy {
	/// <summary>
	/// Описание логина в AD
	/// </summary>
	public class ADLogin : IADLogin {
		/// <summary>
		/// Время обновления данных пользователя из AD
		/// </summary>
		public DateTime UpdateDate { get; set;}

		/// <summary>
		/// Полное имя пользователя в AD
		/// </summary>
		public string Name { get; set; }
	}
}
