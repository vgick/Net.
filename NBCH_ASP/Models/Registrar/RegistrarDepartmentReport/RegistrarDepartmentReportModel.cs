using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using NBCH_LIB.Models.Registrar;

namespace NBCH_ASP.Models.Registrar.RegistrarDepartmentReport {
	public class RegistrarDepartmentReportModel {
		/// <summary>
		/// Список регионов
		/// </summary>
		public SelectList RegionsWebServiceListName {get; set;}

		/// <summary>
		/// Точки заключения договоров.
		/// </summary>
		public SelectList SellPoints { get; set; }

		/// <summary>
		/// Дата начала выборки.
		/// </summary>
		public DateTime DateFrom { get; set; }

		/// <summary>
		/// Дата окончания выборки.
		/// </summary>
		public DateTime DateTo{ get; set; }

		/// <summary>
		/// Документы по загруженным договорам.
		/// </summary>
		public AccountsForCheck[] AccountsForCheck { get; set; } = new AccountsForCheck[0];
	}
}
