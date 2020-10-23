using System.Xml.Serialization;

namespace NBCH_LIB.SOAP.SOAPNBCH {
	/// <summary>
	/// Ошибка.
	/// </summary>
	public class CTErr {
		/// <summary>
		/// Код ошибки.
		/// </summary>
		[XmlElement("Code")]
		public string Code { get; set; }

		/// <summary>
		/// Текст ошибки.
		/// </summary>
		[XmlElement("Text")]
		public string Text { get; set; }
	}
}