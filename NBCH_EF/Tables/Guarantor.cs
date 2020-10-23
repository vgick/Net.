using System.ComponentModel.DataAnnotations.Schema;
using NBCH_EF.Tables.Interface;

namespace NBCH_EF.Tables {
	/// <summary>
	/// Таблица поручителей.
	/// </summary>
	[Table("GuarantorDBs")]
	internal class GuarantorDB: IDBTable {
		/// <summary>
		/// Поручитель.
		/// </summary>
		[Column("ClientDBID")]
		public int ClientDBID { get; set; }
		public ClientDB Client { get; set; }

		/// <summary>
		/// Договор в котором клиент является поручителем.
		/// </summary>
		[Column("Account1CID")]
		public string Account1CID { get; set; }
		public Account1C Account { get; set; }
	}
}
