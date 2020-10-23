using System.Xml.Serialization;

namespace NBCH_LIB.SOAP.SOAPNBCH {
	public class IdReply {
		public string serialNum { get; set; }

		public string fileSinceDt { get; set; }

		/// <summary>
		/// Номер документа.
		/// </summary>
		[XmlElement("idNum")]
		public string DocumentNumber { get; set; }

		/// <summary>
		/// Тип документа.
		/// </summary>
		[XmlElement("idType")]
		public string DocumentType { get; set; }

		public string idTypeText { get; set; }

		/// <summary>
		/// Серия документа.
		/// </summary>
		[XmlElement("seriesNumber")]
		public string DocumentSeries { get; set; }

		/// <summary>
		/// Город выдачи документа (да, судя по примеру город).
		/// </summary>
		[XmlElement("issueCountry")]
		public string IssueCountry { get; set; }

		/// <summary>
		/// Дата выдачи документа.
		/// </summary>
		[XmlElement("issueDate")]
		public string IssueDate { get; set; }

		/// <summary>
		/// Место выдачи документа.
		/// </summary>
		[XmlElement("issueAuthority")]
		public string IssueAuthority { get; set; }

		public string dataValidity { get; set; }

		public string lastUpdatedDt { get; set; }

		public string freezeFlag { get; set; }

		public string suppressFlag { get; set; }
		
		public static explicit operator IdReq (IdReply idReply){
			IdReq idReq	= new IdReq(){
				DocumentNumber	= idReply.DocumentNumber,
				DocumentSeries	= idReply.DocumentSeries,
				DocumentType	= idReply.DocumentType,
				IssueAuthority	= idReply.IssueAuthority,
				IssueCountry	= idReply.IssueCountry,
				IssueDate		= idReply.IssueDate
			};

			return idReq;
		}
	}
}