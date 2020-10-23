using System.Linq;
using NBCH_EF.Helpers;
using NBCH_EF.Tables;
using NBCH_LIB;
using NBCH_LIB.SOAP.SOAP1C.GetAccountsList;

namespace NBCH_ASP.Models.WebAPI.AccountsListApi {
	/// <summary>
	/// Модель API AccountsListApi.
	/// </summary>
	public class AccountLegendNResultApi {
		public AccountLegendNResultApi(AccountLegendNResult accountLegendNResult) {
			AccountLegendApi	= accountLegendNResult.AccountLegends.Select(i => new AccountLegendApi(i)).ToArray();
			Errors				= accountLegendNResult.Errors;
			Role				= null;
		}

		/// <summary>
		/// Ошибки вызова веб сервиса.
		/// </summary>
		public string[] Errors { get; set; }

		/// <summary>
		/// Список договоров.
		/// </summary>
		public AccountLegendApi[] AccountLegendApi { get; set; }

		/// <summary>
		/// Роль пользователя.
		/// </summary>
		public PermissionLevelE? Role { get; set; }
		
	}
}