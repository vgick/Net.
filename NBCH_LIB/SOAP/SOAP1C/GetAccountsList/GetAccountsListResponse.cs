using System.Xml.Serialization;

namespace NBCH_LIB.SOAP.SOAP1C.GetAccountsList {
	public class GetAccountsListResponse {
		/// <summary>
		/// Список договоров 1С.
		/// </summary>
		[XmlElement("return")]
		public ReturnClass Return { get; set;}
	}
}