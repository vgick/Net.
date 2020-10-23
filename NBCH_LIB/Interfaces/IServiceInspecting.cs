
using System;
using System.Threading;
using System.Threading.Tasks;
using NBCH_LIB.Models.Inspecting;

namespace NBCH_LIB.Interfaces {
	/// <summary>
	/// Сервис для логирования работы проверяющих сотрудников.
	/// </summary>
	public interface IServiceInspecting {
		/// <summary>
		/// Получить список проверяющих сотрудников по списку договоров. 
		/// </summary>
		/// <param name="accounts"></param>
		/// <returns>Последняя запись - номер договора/сотрудник</returns>
		InspectorAccount[] GedInspectorsByAccountList(string[] accounts);

		/// <summary>
		/// Получить список проверяющих сотрудников по списку договоров асинхронно. 
		/// </summary>
		/// <param name="accounts"></param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns>Последняя запись - номер договора/сотрудник</returns>
		Task<InspectorAccount[]>
			GedInspectorsByAccountListAsync(string[] accounts, CancellationToken cancellationToken);


		/// <summary>
		/// Установить проверяющего.
		/// </summary>
		/// <param name="account1CCode">Номер договора 1С</param>
		/// <param name="login">Логин проверяющего</param>
		/// <param name="assignDate">Дата операции</param>
		/// <param name="timeZone">Часовой пояс проверяющего</param>
		void SetInspection(string account1CCode, string login, DateTime assignDate, int timeZone);

		/// <summary>
		/// Установить проверяющего асинхронно.
		/// </summary>
		/// <param name="account1CCode">Номер договора 1С</param>
		/// <param name="login">Логин проверяющего</param>
		/// <param name="assignDate">Дата операции</param>
		/// <param name="timeZone">Часовой пояс проверяющего</param>
		/// <param name="cancellationToken">Токен отмены</param>
		Task SetInspectionAsync(string account1CCode, string login, DateTime assignDate, int timeZone,
			CancellationToken cancellationToken);
	}
}