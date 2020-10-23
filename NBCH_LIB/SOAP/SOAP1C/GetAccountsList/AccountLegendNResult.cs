using System.Runtime.Serialization;
using NBCH_LIB.SOAP.SOAPProxy;

namespace NBCH_LIB.SOAP.SOAP1C.GetAccountsList {
	/// <summary>
	/// Данные веб сервиса 1С
	/// Список договоров и ошибки, которые могли случиться при выполнении запроса.
	/// </summary>
	[DataContract]
	public class AccountLegendNResult : ISOAPData {
		/// <summary>
		/// Договора.
		/// </summary>
		[DataMember]
		public AccountLegend[] AccountLegends { get; set;} = new AccountLegend[0];

		/// <summary>
		/// Ошибки при выполнении запросов к серверам 1С.
		/// </summary>
		[DataMember]
		public string[] Errors {get; set;} = new string[0];

		/// <summary>
		/// Реализация интерфейса клонирования.
		/// </summary>
		/// <returns></returns>
		public object Clone() {
			AccountLegendNResult accountLegendNResult = new AccountLegendNResult {
				AccountLegends	= CloneAccountLegend(),
				Errors			= new string[Errors.Length]
			};
			Errors.CopyTo(accountLegendNResult.Errors, 0);

			return accountLegendNResult;
		}

		/// <summary>
		/// Клонирование списка договоров.
		/// </summary>
		/// <returns></returns>
		private AccountLegend[] CloneAccountLegend() {
			AccountLegend[] result = new AccountLegend[this.AccountLegends.Length];
			for (var i = 0; i < result.Length; i++) {
				result[i]	= (AccountLegend)this.AccountLegends[i].Clone();
			}

			return result;
		}
	}
}
