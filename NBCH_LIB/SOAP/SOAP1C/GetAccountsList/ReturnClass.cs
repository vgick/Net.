using System.Xml.Serialization;

namespace NBCH_LIB.SOAP.SOAP1C.GetAccountsList {
	public class ReturnClass {
		[XmlElement("SBList", Namespace = "Loan")]
		public AccountLegend[] AccountLegends { get; set;}
	}
}