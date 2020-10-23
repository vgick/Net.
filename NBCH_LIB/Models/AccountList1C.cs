using System.Runtime.Serialization;

namespace NBCH_LIB.Models {
	/// <summary>
	/// Список договоров из 1С.
	/// </summary>
	[DataContract]
	public class AccountList1C {
		/// <summary>
		/// Код договора 1С.
		/// </summary>
		[DataMember]
		public string Account1CCode {get; set;}

		/// <summary>
		/// Статус договора.
		/// </summary>
		[DataMember]
		public string Status {get; set;}
	}
}
