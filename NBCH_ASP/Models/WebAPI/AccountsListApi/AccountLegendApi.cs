using NBCH_LIB.SOAP.SOAP1C.GetAccountsList;

namespace NBCH_ASP.Models.WebAPI.AccountsListApi {
	public class AccountLegendApi : AccountLegend {
		public AccountLegendApi(AccountLegend accountLegend) {
			doc_number			= accountLegend.doc_number;
			doc_date			= accountLegend.doc_date;
			city				= accountLegend.city;
			date_status			= accountLegend.date_status;
			date_status_acting	= accountLegend.date_status_acting;
			date_status_closed	= accountLegend.date_status_closed;
			doc_status			= accountLegend.doc_status;
			doc_type			= accountLegend.doc_type;
			fio					= accountLegend.fio;
			organization_code	= accountLegend.organization_code;
			organization_name	= accountLegend.organization_name;
		}

		/// <summary>
		/// Проверяющий сотрудник.
		/// </summary>
		public string inspector { get; set; }
	}
}