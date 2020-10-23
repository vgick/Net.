using System.Xml.Serialization;

namespace NBCH_LIB.SOAP.SOAP1C.GetAccountsList {
	/// <summary>
	/// Тело SOAP сообщения.
	/// </summary>
	public class SOAPBodyGetAccountsList {
		/// <summary>
		/// Метод "GetTableSBResponse" SOAP сообщения.
		/// </summary>
		[XmlElement("GetAccountsListResponse", Namespace = "ns_credit")]
		public GetAccountsListResponse GetAccountsListResponse { get; set; }
	}
}
