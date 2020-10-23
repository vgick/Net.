using System.Xml.Serialization;

namespace NBCH_LIB.SOAP.SOAPNBCH {
	[XmlRoot("product")]
	public class ProductResponse {
		[XmlElement("prequest")]
		public Prequest Prequest { get; set; } = new Prequest();

		[XmlElement("preply")]
		public Preply Preply { get; set; } = new Preply();
	}
}
