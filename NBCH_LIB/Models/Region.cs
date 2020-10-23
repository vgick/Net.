using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace NBCH_LIB.Models {
	/// <summary>
	/// Регион
	/// </summary>
	[DataContract]
	public class Region {
		/// <summary>
		/// Ключевое поле.
		/// </summary>
		[Required]
		[DataMember]
		public int ID { get; set; }

		/// <summary>
		/// Регион.
		/// </summary>
		[DataMember]
		[Required]
		public string Name { get; set; }

		/// <summary>
		/// Список пользователей региона.
		/// </summary>
		[DataMember]
		public ADUser[] ADUsers { get; set; }
	}
}
