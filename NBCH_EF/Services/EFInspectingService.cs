using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NBCH_EF.Helpers;
using NBCH_EF.Tables;
using NBCH_LIB;
using NBCH_LIB.Interfaces;
using NBCH_LIB.Models.Inspecting;
using static NBCH_EF.MKKContext;
using static NBCH_LIB.Logger.ExceptionLog;

namespace NBCH_EF.Services {
	/// <summary>
	/// Сервис для логирования работы проверяющих сотрудников.
	/// </summary>
	public class EFServiceInspecting: IServiceInspecting {
		/// <summary>
		/// Логгер.
		/// </summary>
		private static readonly ILogger<EFServiceInspecting> _Logger;

		/// <summary>
		/// Статический конструктор.
		/// </summary>
		static EFServiceInspecting() {
			_Logger = MKKContext.LoggerFactory.CreateLogger<EFServiceInspecting>();
		}
		
		/// <summary>
		/// Получить список проверяющих сотрудников по списку договоров. 
		/// </summary>
		/// <param name="accounts"></param>
		/// <returns>Последняя запись - номер договора/сотрудник</returns>
		public InspectorAccount[] GedInspectorsByAccountList(string[] accounts) =>
			GedInspectorsByAccountListAsync(accounts, CancellationToken.None).ResultAndThrowException();

		/// <summary>
		/// Получить список проверяющих сотрудников по списку договоров асинхронно. 
		/// </summary>
		/// <param name="accounts"></param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns>Последняя запись - номер договора/сотрудник</returns>
		public async Task<InspectorAccount[]> GedInspectorsByAccountListAsync(string[] accounts,
			CancellationToken cancellationToken) {
			
			GedInspectorsByAccountListCheck(accounts);

			using (IDBSource dbSource = new MKKContext()) {
				List<AccountInspectingDB> accountInspectors =
					await dbSource.
						AccountInspectingDB.
						AsNoTracking().
						Include(i => i.Account1C).
						Include(i => i.ADLoginsDB).
						Where(i => accounts.Contains(i.Account1CID)).
						ToListAndLogErrorAsync<AccountInspectingDB, EFServiceInspecting>(cancellationToken);

				InspectorAccount[] result = accountInspectors.
					OrderByDescending(o => o.OperationDate).
					GroupBy(g => g.Account1C).
					Select(i => new InspectorAccount() {
						Account1CCode	= i.Key.Account1CCode,
						Inspector		= i.Select(g => g).FirstOrDefault()?.ADLoginsDB.Name
					}).
					ToArray();

				return result;
			}
		}

		/// <summary>
		/// Проверить входные параметры метода GedInspectorsByAccountList.
		/// </summary>
		/// <param name="accounts"></param>
		private static void GedInspectorsByAccountListCheck(string[] accounts) {
			if (accounts == default || accounts.Length == 0)
				LogAndThrowException<ArgumentNullException, EFServiceInspecting>(
					_Logger, nameof(accounts),
					"Не заданы договора, для которых требуется получить список проверяющих сотрудников./* Метод {methodName}.*/",
					"GedInspectorsByAccountListCheck");
		}

		/// <summary>
		/// Установить проверяющего.
		/// </summary>
		/// <param name="account1CCode">Номер договора 1С</param>
		/// <param name="login">Логин проверяющего</param>
		/// <param name="assignDate">Дата операции</param>
		/// <param name="timeZone">Часовой пояс проверяющего</param>
		public void SetInspection(string account1CCode, string login, DateTime assignDate, int timeZone) =>
			SetInspectionAsync(account1CCode, login, assignDate, timeZone, CancellationToken.None).WaitAndThrowException();

		/// <summary>
		/// Установить проверяющего асинхронно.
		/// </summary>
		/// <param name="account1CCode">Номер договора 1С</param>
		/// <param name="login">Логин проверяющего</param>
		/// <param name="assignDate">Дата операции</param>
		/// <param name="timeZone">Часовой пояс проверяющего</param>
		/// <param name="cancellationToken">Токен отмены</param>
		public async Task SetInspectionAsync(string account1CCode, string login, DateTime assignDate, int timeZone,
			CancellationToken cancellationToken) {

			SetInspectionCheckParams(login, assignDate, timeZone);

			using (IDBSource dbSource	= new MKKContext()) {
				Account1C account1C	= await FindAccountAndLogErrorAsync<EFServiceInspecting>(account1CCode, cancellationToken);
				ADLoginsDB adLogin	= Singleton<ProxyADLoginsDB>.Values[login];
				
				await dbSource.AccountInspectingDB.AddAsync(
					new AccountInspectingDB() {
						Account1CID		= account1C.Account1CCode,
						OperationDate	= assignDate,
						TimeZone		= timeZone,
						UserPermission	= PermissionLevel.GetInt(login),
						ADLoginsDBID	= adLogin.ID
					},
					cancellationToken);

				await dbSource.SaveChangesAndLogErrorAsync<EFServiceInspecting>(cancellationToken);
			}
		}

		/// <summary>
		/// Проверить входные параметры SetInspection.
		/// </summary>
		/// <param name="login">Логин проверяющего</param>
		/// <param name="assignDate">Дата операции</param>
		/// <param name="timeZone">Часовой пояс проверяющего</param>
		private void SetInspectionCheckParams(string login, DateTime assignDate, int timeZone) {
			if (string.IsNullOrEmpty(login))
				LogAndThrowException<ArgumentNullException, EFServiceInspecting>(
					_Logger, nameof(login),
					"Не задан логин проверяющего сотрудника./* Метод {methodName}.*/",
					"SetInspectionCheckParams");
			
			if (assignDate == default)
				LogAndThrowException<ArgumentNullException, EFServiceInspecting>(
					_Logger, nameof(assignDate),
					"Не задана дата операции./* Метод {methodName}.*/",
					"SetInspectionCheckParams");

			if (timeZone == default)
				LogAndThrowException<ArgumentNullException, EFServiceInspecting>(
					_Logger, nameof(timeZone),
					"Не задан часовой пояс проверяющего./* Метод {methodName}.*/",
					"SetInspectionCheckParams");

			if (PermissionLevel.Get(login) != PermissionLevelE.Security)
				LogAndThrowException<Exception, EFServiceInspecting>(
					_Logger, "",
					"У пользователя нет прав осуществлять проверку./* Метод {methodName}, пользователь {login}.*/",
					"SetInspectionCheckParams", login);
		}
	}
}