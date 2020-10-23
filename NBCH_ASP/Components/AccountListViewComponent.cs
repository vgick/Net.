using Microsoft.AspNetCore.Mvc;
using NBCH_ASP.Models;
using static NBCH_LIB.SOAP.SOAP1C.SOAP1C;

namespace NBCH_ASP.Components {
	/// <summary>
	/// Компонент для отображения списка договоров из 1С на проверке.
	/// </summary>
	public class AccountListViewComponent : ViewComponent {
		/// <summary>
		/// Отобразить договора на проверку.
		/// </summary>
		/// <param name="region">Регион (для выбора приоритетного сервера)</param>
		/// <param name="accountStatus">Статусы договоров для фильтра</param>
		/// <returns>Список счетов</returns>
		public IViewComponentResult Invoke(string region, params AccountStatus[] accountStatus) {
			AccountList accountList = new AccountList {Region = region, AccountStatus = accountStatus};
			return View(accountList);
		}
	}
}
