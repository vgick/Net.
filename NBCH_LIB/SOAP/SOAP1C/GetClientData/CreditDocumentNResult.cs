using System.Runtime.Serialization;

namespace NBCH_LIB.SOAP.SOAP1C.GetClientData {
	/// <summary>
	/// Кредитный договор из 1С и ошибки (если были при подключении к одному из серверов).
	/// </summary>
	[DataContract]
	public class CreditDocumentNResult {
		/// <summary>
		/// Кредитный договор.
		/// </summary>
		[DataMember]
		public CreditDocument CreditDocument {get; set;}

		/// <summary>
		/// Ошибки если были при подключении к серверу.
		/// </summary>
		[DataMember]
		public string[] Errors {get; set;}
	}
}
