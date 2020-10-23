using System.Collections.Generic;
using System.Linq;

namespace NBCH_ASP.Models.PDF.ADUsers {
	/// <summary>
	/// Модель для контроллера Users c индексной страницы.
	/// </summary>
	public class ADUsersIndexModel {
		/// <summary>
		/// Список пользователей системы.
		/// </summary>
		private IEnumerable<ADUserMVC> _ADUsers;

		/// <summary>
		/// Метод , для работы со списком пользователей системы.
		/// </summary>
		public IEnumerable<ADUserMVC> ADUsers {
			get => _ADUsers;
			set{
				_ADUsers	= value;
				RecordCount	= _ADUsers?.Count() ?? 0;
			}
		
		}

		/// <summary>
		/// Количество пользователей системы.
		/// </summary>
		public int RecordCount {get; private set;}
	}
}
