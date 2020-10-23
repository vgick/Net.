using System.Xml.Serialization;

namespace NBCH_LIB.SOAP.SOAPNBCH {
	[XmlRoot("product")]
	public class ProductRequest {
		/// <summary>
		/// Версия пакета обмена данными.
		/// </summary>
		[XmlIgnore]
		public static string RequestVersion {get; set;} = "2";

		[XmlElement("prequest")]
		public Prequest Prequest {get; set;} = new Prequest();
	}
}
