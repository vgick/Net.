using System.Xml.Serialization;

namespace NBCH_LIB.SOAP.SOAPNBCH {
	/// <summary>
	/// Анкета НБКИ.
	/// </summary>
	public class Report {
		public SubjectReply SubjectReply { get; set; }
		/// <summary>
		/// Данные о клиенте (ФИО, дата место рождения...).
		/// </summary>
		[XmlElement]
		public PersonReply[] PersonReply { get; set; }

		/// <summary>
		/// Документы клиента.
		/// </summary>
		[XmlElement("IdReply")]
		public IdReply[] Documents { get; set; }

		/// <summary>
		/// Адреса клиента.
		/// </summary>
		[XmlElement]
		public AddressReply[] AddressReply {get; set;}

		[XmlElement]
		public InquiryReply[] InquiryReply {get; set;}

		public OwnInquiries OwnInquiries {get; set;}

		public string addOns {get; set;}

		public Calc calc {get; set;} = new Calc();

		public string groups {get; set;}

		public string inqControlNum {get; set;}

		public string MemberCodeStatus {get; set;}

		/// <summary>
		/// Информация по счетам клиента из НБКИ.
		/// </summary>
		[XmlElement]
		public AccountReply[] AccountReply {get; set;}

		/// <summary>
		/// Ошибка при выполнении к сервису НБКИ.
		/// </summary>
		[XmlIgnore]
		public string ErrorMessage {get; set;}

		/// <summary>
		/// Код клиента в 1С.
		/// </summary>
		[XmlIgnore]
		public string Client1CCode {get; set;}
	}
}
