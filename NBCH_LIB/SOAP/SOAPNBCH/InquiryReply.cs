
namespace NBCH_LIB.SOAP.SOAPNBCH {
	public class InquiryReply {
		public string inqControlNum { get; set; }

		public string inquiryPeriod { get; set; }

		public string inqPurpose { get; set; }

		public string inqPurposeText { get; set; }

		public string inqAmount { get; set; }

		public string currencyCode { get; set; }

		public string userReference { get; set; }

		public string freezeFlag { get; set; }

		public string suppressFlag { get; set; }
	}
}