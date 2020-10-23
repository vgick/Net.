using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace NBCH_LIB.SOAP.SOAPNBCH {
	public class IdReq {
		/// <summary>
		/// Номер документа.
		/// </summary>
		[XmlElement("idNum")]
		public string DocumentNumber {get; set;}

		/// <summary>
		/// Тип документа.
		/// </summary>
		[XmlElement("idType")]
		public string DocumentType {get; set;}

		/// <summary>
		/// Серия документа.
		/// </summary>
		[XmlElement("seriesNumber")]
		public string DocumentSeries {get; set;}

		/// <summary>
		/// Город выдачи документа (да, судя по примеру город).
		/// </summary>
		[XmlElement("issueCountry")]
		public string IssueCountry {get; set;}

		/// <summary>
		/// Дата выдачи документа.
		/// </summary>
		[XmlElement("issueDate")]
		public string IssueDate {get; set;}

		/// <summary>
		/// Место выдачи документа.
		/// </summary>
		[XmlElement("issueAuthority")]
		public string IssueAuthority {get; set;}

		/// <summary>
		/// Дата выдачи документа в формате DateTime (заполняет и берет данные из issueDate).
		/// </summary>
		[XmlIgnore]
		public DateTime issueDateDateTime {
			get => SOAPNBCH.StringToDateTime(IssueDate);
			set => IssueDate = SOAPNBCH.DateTimeToString(value);
		}
	}

	/// <summary>
	/// Тип документа
	/// </summary>
	public enum DocumentType {
		[Description("Российский паспорт")]
		RussianPassport	= 21

	}
}
