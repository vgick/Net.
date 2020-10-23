using System.Xml.Serialization;

namespace NBCH_LIB.SOAP.SOAP1C.GetClientData {
	/// <summary>
	/// Тело SOAP сообщения
	/// </summary>
	public class SOAPBodyGetClientData {
		/// <summary>
		/// Метода "GetClientDataResponse" SOAP сообщения
		/// </summary>
		[XmlElement("GetClientDataResponse", Namespace = "ns_credit")]
		public GetClientDataResponse GetClientDataResponse { get; set; }
	}
}
