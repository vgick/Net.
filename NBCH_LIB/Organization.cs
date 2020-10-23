using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using NBCH_LIB.ADServiceProxy;

namespace NBCH_LIB {
	public static class Organization {
		/// <summary>
		/// Получить список доступных организация по логину пользователя
		/// </summary>
		/// <param name="login">Логин пользователя</param>
		/// <returns>Список организаций</returns>
		public static Organizations[] OrganizationsByLogin(string login) {
			string[] roles	= Singleton<ProxyADRoles>.Values[login];

			List<Organizations> organizations	= new List<Organizations>();

			foreach (KeyValuePair<string, Organizations> item in ADRoleOrgName) {
				if (roles.Contains(item.Key)) organizations.Add(item.Value);
			}

			return organizations.ToArray();
		}

		/// <summary>
		/// Получить список доступных организаций по логину пользователя, и выставить признак выбранных элементов
		/// </summary>
		/// <param name="login">Логин пользователя</param>
		/// <param name="checkedElementNames">Список выбранных организация</param>
		/// <returns>Список организаций с признаком выбранных</returns>
		public static Dictionary<Organizations, bool> OrganizationsByLogin(string login, string[] checkedElementNames) {
			Organizations[] organizations	= OrganizationsByLogin(login);

			Dictionary<Organizations, bool> result	= new Dictionary<Organizations, bool>();

			foreach (Organizations organization in organizations) {
				bool itemChecked	= checkedElementNames.Contains(organization.ToString());
				result.Add(organization, itemChecked);
			}

			return result;
		}

		/// <summary>
		/// Связь роли AD с перечислением организации
		/// </summary>
		private static Dictionary<string, Organizations> ADRoleOrgName = new Dictionary<string, Organizations> {
				{"vlf-org-vlf", Organizations.vlf},
				{"vlf-org-lider", Organizations.ldr}
			};

		/// <summary>
		/// Организации 
		/// </summary>
		public enum Organizations {
			[Description("ООО ORG2")]
			[Display(Name = "ORG2")]
			vlf,
			[Description("ООО ORG1")]
			[Display(Name = "ORG1")]
			ldr
		}
	}
}
