using System.Xml.Serialization;

namespace NBCH_LIB.SOAP.SOAPNBCH {
	public class Preply {
		[XmlElement("report")]
		public Report Report {get; set;}

		[XmlElement("err")]
		public Error Error {get; set;} = new Error();
	}
}
