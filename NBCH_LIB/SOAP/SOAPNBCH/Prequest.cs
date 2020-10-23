using System.Xml.Serialization;

namespace NBCH_LIB.SOAP.SOAPNBCH {
	public class Prequest {
		[XmlElement("req")]
		public Req Req { get; set; } = new Req();
	}
}
