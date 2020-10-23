using System.Runtime.Serialization;

namespace NBCH_LIB.Models {
	/// <summary>
	/// Точка заключения сделки.
	/// </summary>
	[DataContract]
	public class SellPoint {
		/// <summary>
		/// Наименование точки.
		/// </summary>
		[DataMember]
		public string SellPointName { get; set; }

		/// <summary>
		/// Код 1С.
		/// </summary>
		[DataMember]
		public string SellPoint1CCode { get; set; }
	}
}
