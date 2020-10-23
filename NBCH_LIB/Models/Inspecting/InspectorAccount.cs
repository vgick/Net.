using System.Runtime.Serialization;

namespace NBCH_LIB.Models.Inspecting {
	/// <summary>
	/// Последняя запись по договору и проверяющему сотруднику. 
	/// </summary>
	[DataContract]
	public class InspectorAccount {
		/// <summary>
		/// Номер договора 1С.
		/// </summary>
		public string Account1CCode { get; set; }
		
		/// <summary>
		/// Проверяющий сотрудник.
		/// </summary>
		public string Inspector { get; set; }
	}
}