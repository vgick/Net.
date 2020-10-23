using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static NBCH_LIB.SOAP.SOAP1C.SOAP1C;

namespace NBCH_ASP.Models {
	public class AccountList {

		/// <summary>
		/// Регион пользователя.
		/// </summary>
		public string Region { get; set; }

		/// <summary>
		/// Статусы договоров для отображения.
		/// </summary>
		public AccountStatus[] AccountStatus { get; set; }
	}
}
