using NBCH_LIB.Models;

namespace NBCH_ASP.Models.NBCH.SearchClient {
	public class SearchClientModel {
		/// <summary>
		/// Строка поиска.
		/// </summary>
		public string SearchString {get; set;}

		/// <summary>
		/// Список клиентов, найденных по фильтру.
		/// </summary>
		public SearchClientList[] SearchClientList {get; set;}
	}
}
