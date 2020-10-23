using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace NBCH_LIB.Models {
	/// <summary>
	/// Пользователь AD
	/// </summary>
	[DataContract]
	public class ADUser {
		/// <summary>
		/// Ключевое поле.
		/// </summary>
		[DataMember]
		[Required]
		public int ID { get; set; }

		/// <summary>
		/// Имя пользователя.
		/// </summary>
		[DataMember]
		[Required]
		public string ADName { get; set; }

		/// <summary>
		/// Список регионов пользователя.
		/// </summary>
		[DataMember]
		public Region[] Regions { get; set; }
	}
}
