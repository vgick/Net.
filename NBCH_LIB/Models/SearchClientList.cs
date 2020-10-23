using System;
using System.Runtime.Serialization;

namespace NBCH_LIB.Models {
	/// <summary>
	/// Результат поиска клиента в базе.
	/// </summary>
	[DataContract]
	public class SearchClientList {
		/// <summary>
		/// Код клиента в 1С.
		/// </summary>
		[DataMember]
		public string ClientID1C {get; set;}

		/// <summary>
		/// Код клиента в базе.
		/// </summary>
		[DataMember]
		public int ClientID {get; set;}

		/// <summary>
		/// ФИО клиента.
		/// </summary>
		[DataMember]
		public string FIO {get; set;}

		/// <summary>
		/// Дата рождения клиента.
		/// </summary>
		[DataMember]
		public DateTime BirthDate {get; set;}
	}
}
