using System.Collections.Generic;

namespace NBCH_ASP.Infrastructure.DataFromConfigurationFile.ISecrets {
	/// <summary>
	/// Данные для подключения к сервисам 1С
	/// </summary>
	public interface ISecret1C {
		/// <summary>
		/// Логин для подключения к сервису 1С
		/// </summary>
		string Login {get; set;}
		// todo: убрать из настроек
		/// <summary>
		/// Пароль для подключения к сервису 1С
		/// </summary>
		string Password {get; set;}
		/// <summary>
		/// Список регионов и привязанных к ним серверам для подключения
		/// </summary>
		Dictionary<string, string[]> Servers {get; set;}
	}
}
