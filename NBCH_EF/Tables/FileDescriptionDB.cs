using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using NBCH_EF.Tables.Interface;
using NBCH_LIB.Models.Registrar;

namespace NBCH_EF.Tables {
	/// <summary>
	/// Описание загружаемого файла.
	/// </summary>
	[Table("FileDescriptionDBs")]
	internal class FileDescriptionDB : IDBTableID {
		/// <summary>
		/// Ключевое поле.
		/// </summary>
		[Key]
		[Column("ID")]
		public int ID { get; set; }

		/// <summary>
		/// Описание типа файла.
		/// </summary>
		[Required]
		[MaxLength(256)]
		public string Description {get; set;}

		/// <summary>
		/// Роли, которым доступен просмотр документов.
		/// </summary>
		public IList<ADRoleDB> ReadADRoles { get; set; } = new List<ADRoleDB>();

		/// <summary>
		/// Роли, которым доступно добавление документов.
		/// </summary>
		public IList<ADRoleDB> WriteADRoles { get; set; } = new List<ADRoleDB>();

		/// <summary>
		/// Порядок сортировки.
		/// </summary>
		public int SortOrder { get; set; }

		/// <summary>
		/// Принадлежность к группе документов.
		/// </summary>
		[Column("DocumentGroupDB_ID")]
		public int? DocumentGroupDBID { get; set; }
		public DocumentGroupDB DocumentGroupDB { get; set; }

		/// <summary>
		/// Приведение FileDescriptionIDB к FileDescription.
		/// </summary>
		/// <param name="value">Объект типа FileDescriptionIDB</param>
		public static explicit operator FileDescription(FileDescriptionDB value) {
			FileDescription fileDescription = new FileDescription() {
				Descrioption = value.Description,
				ID = value.ID,
				ReadADRoles = value.ReadADRoles.Select(i => i.Role).ToArray(),
				WriteADRoles = value.WriteADRoles.Select(i => i.Role).ToArray()
			};

			return fileDescription;
		}

		/// <summary>
		/// Приведение типа FileDescription к FileDescriptionIDB.
		/// </summary>
		/// <param name="value"></param>
		public static explicit operator FileDescriptionDB(FileDescription value) {
			FileDescriptionDB fileDescriptionIDB = new FileDescriptionDB() {
				Description = value.Descrioption,
				ReadADRoles = value.ReadADRoles.Select(i => new ADRoleDB() { Role = i }).ToList(),
				WriteADRoles = value.ReadADRoles.Select(i => new ADRoleDB() { Role = i }).ToList()
			};

			return fileDescriptionIDB;
		}
	}
}
