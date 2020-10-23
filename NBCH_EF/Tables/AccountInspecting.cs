using System;

namespace NBCH_EF.Tables {
	/**
	 * Таблица связи проверяющего сотрудника и договора.
	 */
	internal class AccountInspectingDB {
		/// <summary>
		/// Ключевое поле.
		/// </summary>
		public int ID { get; set; }

		/// <summary>
		/// Дата/время операции.
		/// </summary>
		public DateTime OperationDate { get; set; }

		/**
		 * Договор.
		 */
		public string Account1CID { get; set; }
		public Account1C Account1C { get; set; }

		/**
		 * Проверяющий сотрудник.
		 */
		public int ADLoginsDBID { get; set; }
		public ADLoginsDB ADLoginsDB { get; set; }

		/**
		 * Уровень доступа пользователя.
		 * Необходим, если будут добавлены другие звенья проверки, помимо сотрудника СБ.
		 */
		public int UserPermission { get; set; }

		/// <summary>
		/// Часовой пояс клиента.
		/// </summary>
		public int TimeZone { get; set; }
	}
}