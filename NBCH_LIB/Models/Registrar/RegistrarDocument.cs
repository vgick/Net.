using System.Collections.Generic;
using System.Runtime.Serialization;

namespace NBCH_LIB.Models.Registrar {
	[DataContract]
	public class RegistrarDocument {
		/// <summary>
		/// ID документа.
		/// </summary>
		[DataMember]
		public int ID {get; set;}

		/// <summary>
		/// Документ
		/// </summary>
		[DataMember]
		public string FileDescription {get; set;}

		/// <summary>
		/// Имена сохраненных файлов.
		/// </summary>
		[DataMember]
		public List<RegistrarFile> Files {get; set;}


		/// <summary>
		/// Разрешено добавление документа.
		/// </summary>
		[DataMember]
		public bool WriteAccess { get; set; }


		/// <summary>
		/// Проверить на равенство.
		/// </summary>
		/// <param name="obj">Сравниваемый объект</param>
		/// <returns></returns>
		public override bool Equals(object obj) {
			RegistrarDocument item = obj as RegistrarDocument;

			if (item == null) {
				return false;
			}

			return this.ID.Equals(item.ID) && (Files?.Count ?? 0) == (item.Files?.Count ?? 0);
		}

		/// <summary>
		/// Получить хэш.
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode() {
			return ID;
		}
	}
}