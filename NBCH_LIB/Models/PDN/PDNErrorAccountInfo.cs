using System;
using System.Runtime.Serialization;

namespace NBCH_LIB.Models.PDN {
	/// <summary>
	/// Информация о договорах с ошибками при расчете ПДН.
	/// </summary>
	[DataContract]
	public class PDNErrorAccountInfo {
		/// <summary>
		/// Номер проблемного договора.
		/// 
		/// </summary>
		[DataMember]
		public string Account1CCode {get; set;}

		/// <summary>
		/// Дата расчета ПДН.
		/// </summary>
		[DataMember]
		public DateTime ReportDate {get; set;}

		/// <summary>
		/// Анкета на основании которой идет расчет ПДН.
		/// </summary>
		[DataMember]
		public int CreditHistoryID {get; set;}
	}
}
