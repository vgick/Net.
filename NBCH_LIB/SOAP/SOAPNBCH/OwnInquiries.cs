using System.Xml.Serialization;

namespace NBCH_LIB.SOAP.SOAPNBCH {
	public class OwnInquiries {
		[XmlElement]
		public Inquiry[] Inquiry { get; set; }
	}
}