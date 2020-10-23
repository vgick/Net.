using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace NBCH_ASP.Infrastructure.DataFromConfigurationFile.ISecrets {
	public class SecretNBCH : ISecretNBCH {
		/// <summary>
		/// Конфигурация подключения из файла настроек
		/// </summary>
		public string Organization1CName {get; set;}
		/// <summary>
		/// Код участника
		/// </summary>
		public string MemberCode {get; set;}
		/// <summary>
		/// Логин
		/// </summary>
		public string UserId {get; set;}
		// todo: убрать из файла конфигурации
		/// <summary>
		/// Пароль пользователя
		/// </summary>
		public string Password {get; set;}
		/// <summary>
		/// Получить настройки подключения к НБКИ
		/// </summary>
		/// <param name="configuration">Файл конфигурации</param>
		/// <returns>Настройки подключения к НБКИ для организаций</returns>
		public static ISecretNBCH[] GetSecretNBCHs(IConfiguration configuration) {
			IConfigurationSection configurationSection	= configuration.GetSection(Startup.WebServiceNBCH);
			ISecretNBCH[] webServiceNBCHSecrets			= configurationSection.Get<List<SecretNBCH>>().ToArray();

			return webServiceNBCHSecrets;
		}
		/// <summary>
		/// Получить данные для подключения по имени организации
		/// </summary>
		/// <param name="configuration">Конфигурация</param>
		/// <param name="organizationName">Имя организации</param>
		/// <returns>Данные для подключения</returns>
		public static ISecretNBCH GetSecretNBCH(IConfiguration configuration, string organizationName){
			IConfigurationSection configurationSection	= configuration.GetSection(Startup.WebServiceNBCH);
			ISecretNBCH webServiceNBCHSecrets			= configurationSection.Get<List<SecretNBCH>>().FirstOrDefault(i => i.Organization1CName.Equals(organizationName));

			return webServiceNBCHSecrets;
		}
	}
}
