using System.Runtime.Serialization;

namespace NBCH_LIB.Models.PDN {
	/// <summary>
	/// Сервис по работе с PDN
	/// </summary>
	[DataContract]
	public class PdnResult {
		/// <summary>
		/// Договор 1С.
		/// </summary>
		[DataMember]
		public string Account { get; set; }

		/// <summary>
		/// Процент ПДН.
		/// </summary>
		[DataMember]
		public double PDNPercent { get; set; }
	}
}
