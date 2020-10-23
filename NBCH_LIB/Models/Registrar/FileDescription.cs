using System.Runtime.Serialization;

namespace NBCH_LIB.Models.Registrar {
	/// <summary>
	/// Описание типа загружаемого файла.
	/// </summary>
	[DataContract]
	public class FileDescription {
		/// <summary>
		/// Ключевое поле.
		/// </summary>
		[DataMember]
		public int ID {get; set;}

		/// <summary>
		/// Описание.
		/// </summary>
		[DataMember]
		public string Descrioption {get; set;}

		/// <summary>
		/// AD роли с доступом на чтение.
		/// </summary>
		[DataMember]
		public string[] ReadADRoles {get; set;} = new string[0];

		/// <summary>
		/// AD роли с доступом на добавление.
		/// </summary>
		[DataMember]
		public string[] WriteADRoles {get; set;} = new string[0];
	}
}
