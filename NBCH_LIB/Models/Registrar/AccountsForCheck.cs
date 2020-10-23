using System.Runtime.Serialization;

namespace NBCH_LIB.Models.Registrar {
	/// <summary>
	/// Сводная информация по загруженным договорам.
	/// </summary>
	[DataContract]
	public class AccountsForCheck {
		/// <summary>
		/// Код договора 1С.
		/// </summary>
		[DataMember]
		public string Account1CCode { get; set; }

		/// <summary>
		/// Код клиента.
		/// </summary>
		[DataMember]
		public int ClientID { get; set; }

		/// <summary>
		/// Согласие НБКИ.
		/// </summary>
		[DataMember]
		public bool NBCH { get; set; }

		/// <summary>
		/// Паспорт.
		/// </summary>
		[DataMember]
		public bool Passport { get; set; }

		/// <summary>
		/// Анкета
		/// </summary>
		[DataMember]
		public bool Profile { get; set; }

		/// <summary>
		/// Договор.
		/// </summary>
		[DataMember]
		public bool Contract { get; set; }

		/// <summary>
		/// График платежей.
		/// </summary>
		[DataMember]
		public bool PaymentSchedule { get; set; }

		/// <summary>
		/// Дополнительное соглашение.
		/// </summary>
		[DataMember]
		public bool AddionalAgreement { get; set; }

		/// <summary>
		/// Расходник.
		/// </summary>
		[DataMember]
		public bool CashWarrant { get; set; }

		/// <summary>
		/// Прочие документы.
		/// </summary>
		[DataMember]
		public bool OverScans { get; set; }

		/// <summary>
		/// Фотография клиента.
		/// </summary>
		[DataMember]
		public bool Photo { get; set; }

		/// <summary>
		/// Выдача наличными.
		/// </summary>
		[DataMember]
		public bool Cash { get; set; }
	}
}
