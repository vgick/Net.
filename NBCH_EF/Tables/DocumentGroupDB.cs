using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NBCH_EF.Tables.Interface;

namespace NBCH_EF.Tables {
	/// <summary>
	/// Группа документов.
	/// </summary>
	[Table("DocumentGroupDBs")]
	internal class DocumentGroupDB : IDBTableID {
		/// <summary>
		/// Ключевое поле.
		/// </summary>
		[Key]
		[Column("ID")]
		public int ID { get; set; }

		/// <summary>
		/// Описание списка документов.
		/// </summary>
		[Required]
		[MaxLength(256)]
		public string Description {get; set;}

		/// <summary>
		/// Типы файлов, включенные в пакет документов.
		/// </summary>
		public List<FileDescriptionDB> FileDescriptionDB {get; set;} = new List<FileDescriptionDB>();
	}
}
