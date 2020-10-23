using System.Xml.Serialization;

namespace NBCH_LIB.SOAP.SOAP1C.GetClientData {
	/// <summary>
	/// Описание метода "GetClientDataResponse" SOAP сообщения
	/// </summary>
	public class GetClientDataResponse {
		/// <summary>
		/// Информация по кредитному договору
		/// </summary>
		[XmlElement("return")]
		public CreditDocument CreditDocument { get; set; }
	}
}
