namespace NBCH_ASP.Infrastructure.DataFromConfigurationFile.ISecrets {
	public interface ISecretNBCH {
		/// <summary>
		/// Конфигурация подключения из файла настроек
		/// </summary>
		string Organization1CName {get; set;}

		/// <summary>
		/// Код участника
		/// </summary>
		string MemberCode {get; set;}

		/// <summary>
		/// Логин
		/// </summary>
		string UserId {get; set;}

		/// <summary>
		/// Пароль пользователя
		/// </summary>
		string Password {get; set;}
	}
}
