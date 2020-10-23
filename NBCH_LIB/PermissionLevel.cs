using System.Linq;
using NBCH_LIB.ADServiceProxy;

namespace NBCH_LIB {
	public static class PermissionLevel {
		/// <summary>
		/// Роль AD сотрудника SB.
		/// </summary>
		private const string _SBAdRole = "vlf-sb";

		/// <summary>
		/// Роль AD сотрудника SB.
		/// </summary>
		private const string _AdminRole = "vlf-wcf-service-admin";

		/// <summary>
		/// Получить уровень доступа пользователя.
		/// </summary>
		/// <param name="login">AD логин пользователя</param>
		/// <returns>Уровень доступа пользователя</returns>
		public static int GetInt(string login) {
			PermissionLevelE permissionLevel	= Get(login);
			return (int)permissionLevel;
		}

		/// <summary>
		/// Получить роль пользователя.
		/// </summary>
		/// <param name="login">AD логин пользователя</param>
		/// <returns>Уровень доступа пользователя</returns>
		public static PermissionLevelE Get(string login) {
			string[] userRoles	= Singleton<ProxyADRoles>.Values[login];
			
			if (userRoles.Contains(_AdminRole)) return PermissionLevelE.Admin;
			if (userRoles.Contains(_SBAdRole)) return PermissionLevelE.Security;
			return PermissionLevelE.CreditInspector;
		}
	}
	
	/// <summary>
	/// Уровень доступа.
	/// </summary>
	public enum PermissionLevelE {
		/// <summary>
		/// Кредитный инспектор.
		/// </summary>
		CreditInspector	= 100,

		/// <summary>
		/// Сотрудник СБ.
		/// </summary>
		Security		= 200,
		
		/// <summary>
		/// Администратор. 
		/// </summary>
		Admin			= 1000
	}
}