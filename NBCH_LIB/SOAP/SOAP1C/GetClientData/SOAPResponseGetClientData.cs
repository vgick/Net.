using System.Xml.Serialization;

namespace NBCH_LIB.SOAP.SOAP1C.GetClientData {
	/// <summary>
	/// SOAP ответ 
	/// </summary>
	[XmlRoot(ElementName = "Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
	public class SOAPResponseGetClientData : Response1C {
		/// <summary>
		/// Тело SOAP сообщения
		/// </summary>
		public SOAPBodyGetClientData Body { get; set; }
	}
}
