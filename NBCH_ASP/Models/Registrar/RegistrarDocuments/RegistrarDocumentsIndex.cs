using Microsoft.AspNetCore.Mvc.Rendering;
using NBCH_LIB.Models;

namespace NBCH_ASP.Models.Registrar.RegistrarDocuments {
	public class RegistrarDocumentsIndex {
		/// <summary>
		/// Список клиентов в договоре.
		/// </summary>
		public Client[] Clients {get; set;} = new Client[0];

		/// <summary>
		/// Код договора 1С.
		/// </summary>
		public string Account1CCode {get; set;}

		/// <summary>
		/// Список регионов 1С серверов.
		/// </summary>
		public SelectList RegionsWebServiceListName {get; set;}

		/// <summary>
		/// Ошибки при запросе данных.
		/// </summary>
		public string[] Errors {get; set;}
	}
}
