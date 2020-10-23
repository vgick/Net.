using System;
using System.Xml.Serialization;

namespace NBCH_LIB.SOAP.SOAPNBCH {
	public class Calc {
		/// <summary>
		/// Всего активных.
		/// </summary>
		public string totalAccts { get; set; }

		/// <summary>
		/// Активных на сумму.
		/// </summary>
		public string totalActiveBalanceAccounts { get; set; }

		/// <summary>
		/// Негативных.
		/// </summary>
		public string negativeRating { get; set; }

		public string totalLegalItems { get; set; }

		/// <summary>
		/// Банкротсвт.
		/// </summary>
		public string totalBankruptcies { get; set; }

		public string MyPropertytotalInquiries { get; set; }

		public string recentInquiries { get; set; }

		public string collectionsInquiries { get; set; }

		public string mostRecentInqText { get; set; }

		public string reportIssueDateTime { get; set; }

		public string totalIPRecords { get; set; }

		public string totalRejectedIPRecords { get; set; }

		public string totalApprovedIPRecords { get; set; }

		public string totalDefaultFlagIPRecords { get; set; }

		/// <summary>
		/// Дата отчета. Устанавливается при получении отчета из НБКИ.
		/// </summary>
		[XmlIgnore]
		public DateTime ReportDate { get; set; }

	}
}