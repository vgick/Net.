using Microsoft.AspNetCore.Authorization;
using NBCH_LIB.SOAP.SOAPNBCH;

namespace NBCH_ASP.Models.NBCH.ArchiveCH {
	[Authorize(Roles = @"role,roleadmin")]
	public class ArchiveCHModel {
		/// <summary>
		/// Отчет из НБКИ
		/// </summary>
		public Report Report {get; set;}
		/// <summary>
		/// Персональные данные клиента
		/// </summary>
		public ClientPersonalInfo ClientPersonalInfo {get; set;} = new ClientPersonalInfo();

	}
}
