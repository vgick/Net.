using System.Xml.Serialization;

namespace NBCH_LIB.SOAP.SOAPNBCH {
	public class Error {
		[XmlElement("ctErr")]
		public CTErr CtErr {get; set;} = new CTErr();
	}
}