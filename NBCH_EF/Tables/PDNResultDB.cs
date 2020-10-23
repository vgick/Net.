using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Extensions.Logging;
using NBCH_EF.Tables.Interface;
using NBCH_LIB.Models.PDN;

namespace NBCH_EF.Tables {
	/// <summary>
	/// Рассчитанное ПДН.
	/// </summary>
	internal class PDNResultDB : IDBTableID {
		/// <summary>
		/// Ключевое поле.
		/// </summary>
		[Key]
		public int ID { get; set; }

		/// <summary>
		/// Договор.
		/// </summary>
		[Required]
		[Column("Account1C_Account1CCode")]
		public string Account1CID { get; set; }
		public Account1C Account1C { get; set;}

		/// <summary>
		/// Процент долговой нагрузки.
		/// </summary>
		[Required]
		public double Percent { get; set; }

		/// <summary>
		/// Приведение PDNResultDB к PdnResult.
		/// </summary>
		/// <param name="pdnResultDB"></param>
		public static explicit operator PdnResult(PDNResultDB pdnResultDB) {
			if (pdnResultDB?.Account1C == default) {
				ILogger<PDNResultDB> logger = MKKContext.LoggerFactory.CreateLogger<PDNResultDB>();
				logger.LogError("Не удалось привести PDNResultDB {PDNResultDB} к PdnResult.", pdnResultDB);
				return default;
			}

			PdnResult result	= new PdnResult() {
				Account		= pdnResultDB.Account1C.Account1CCode,
				PDNPercent	= pdnResultDB.Percent
			};

			return result;
		}

	}
}
