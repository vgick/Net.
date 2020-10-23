using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace NBCH_LIB.Models.Registrar {
	/// <summary>
	/// Имя загруженного файла и его содержимое.
	/// </summary>
	[DataContract]
	public struct RegistrarFileData {
		/// <summary>
		/// Имя загруженного файла.
		/// </summary>
		[DataMember]
		[Required]
		public string FileName { get; set; }

		/// <summary>
		/// Содержимое файла.
		/// </summary>
		[DataMember]
		[Required]
		public byte[] Data { get; set; }
	}
}
