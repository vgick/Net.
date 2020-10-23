using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;

namespace NBCH_ASP.Infrastructure.DataFromConfigurationFile.ISecrets {
	/// <summary>
	/// Данные для подключения к сервисам 1С.
	/// </summary>
	public class Secret1C : ISecret1C {
		/// <summary>
		/// Логин для подключения к сервису 1С.
		/// </summary>
		public string Login {get; set;}

		/// <summary>
		/// Пароль для подключения к сервису 1С.
		/// </summary>
		public string Password {get; set;}

		/// <summary>
		/// Список серверов.
		/// </summary>
		public Dictionary<string, string[]> Servers {get; set;}

		/// <summary>
		/// Получить настройки 1С из файла конфигурации.
		/// </summary>
		/// <param name="configuration">Конфигурация программы</param>
		/// <returns>Настройки для подключения к 1С</returns>
		public static Secret1C GetSecret(IConfiguration configuration){
			IConfigurationSection configurationSection	= configuration.GetSection(Startup.WebService1С);
			return configurationSection.Get<Secret1C>();
		}

		/// <summary>
		/// Получить список регионов из файла конфигурации.
		/// </summary>
		/// <param name="secret1C">Конфигурация для подключения к серверам 1С</param>
		/// <param name="selectedItem">Выбранный элемент</param>
		/// <returns>Список регионов</returns>
		public static SelectList GetRegions(ISecret1C secret1C, string selectedItem) {
			List<string> regions	= new List<string>();

			foreach (KeyValuePair<string, string[]> regionDescription in secret1C.Servers) {
				regions.Add(regionDescription.Key);
			}

			SelectList list	= new SelectList(regions, selectedItem);

			return list;
		}
	}
}
