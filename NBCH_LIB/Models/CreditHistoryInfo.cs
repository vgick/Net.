using System;
using System.Runtime.Serialization;

namespace NBCH_LIB.Models {
	/// <summary>
	/// Список сохраненных анкет.
	/// </summary>
	[DataContract]
	public class CreditHistoryInfo {
		/// <summary>
		/// ID анкеты в базе.
		/// </summary>
		[DataMember]
		public int CreditHistoryID {get; set;}

		/// <summary>
		/// Дата сохраненной анкеты.
		/// </summary>
		[DataMember]
		public DateTime Date {get; set;}

		/// <summary>
		/// Ошибка при получении анкеты НБКИ (Код ошибки).
		/// </summary>
		[DataMember]
		public string ErrorCode {get; set;}

		/// <summary>
		/// Ошибка при получении анкеты НБКИ.
		/// </summary>
		[DataMember]
		public string ErrorText {get; set;}
	}
}
