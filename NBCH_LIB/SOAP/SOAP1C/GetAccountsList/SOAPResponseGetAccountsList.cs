using System.Xml.Serialization;

namespace NBCH_LIB.SOAP.SOAP1C.GetAccountsList {
	/// <summary>
	/// SOAP ответ.
	/// </summary>
	[XmlRoot(ElementName = "Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
	public class SOAPResponseGetAccountsList : Response1C {
		/// <summary>
		/// Тело SOAP сообщения.
		/// </summary>
		public SOAPBodyGetAccountsList Body { get; set; }
	}
}
