using System;

namespace NBCH_ASP.Models.NBCH.PDNEdit {
	/// <summary>
	/// Данные для отображения представления.
	/// </summary>
	public class PDNEditEdit {
		/// <summary>
		/// Номер договора 1С.
		/// </summary>
		public string Account1CCode {get; set;}

		/// <summary>
		/// Дата на которую формируется отчет.
		/// </summary>
		public DateTime ReportDate {get; set;}
	}
}
