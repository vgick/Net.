using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NBCH_EF.Helpers;
using NBCH_EF.Tables;
using NBCH_EF.Tables.Interface;
using NBCH_LIB;
using NBCH_LIB.Logger;
using static NBCH_LIB.Organization;

namespace NBCH_EF {
	/// <summary>
	/// Класс реализующий интерфейсы для работы с БД
	/// </summary>
	internal partial class MKKContext {
		/// <summary>
		/// Найти объект по ID.
		/// </summary>
		/// <param name="id">ID объекта</param>
		/// <param name="loggedMessage">Описание места и состоянии объекта вызвавшего исключение</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <param name="includes">Инклуды</param>
		/// <returns>Запись в БД</returns>
		internal static async Task<TEntity> FindDBRecordByIDdAndLogErrorAsync<TEntity, TLoggedClass>(int id,
			LoggedMessage<TLoggedClass> loggedMessage, CancellationToken cancellationToken,
			params Expression<Func<TEntity, object>>[] includes) where TEntity : class, IDBTableID where TLoggedClass : class {
			
			using (IDBSource source = new MKKContext()) {
				IQueryable<TEntity> query = ((DbContext)source).Set<TEntity>().
					AsNoTracking().
					Where(i => i.ID.Equals(id));
				foreach (var include in includes) {
					query = query.Include(include);
				}

				return await query.SingleOrDefaultAndErrorAsync(loggedMessage, cancellationToken);
			}
		}


		/// <summary>
		/// Найти объект по ID.
		/// </summary>
		/// <param name="id">ID объекта</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <param name="includes">Инклуды</param>
		/// <returns>Запись в БД</returns>
		internal static async Task<TEntity> FindDBRecordByIDdAndLogErrorAsync<TEntity, TLoggedClass>(int id,
			CancellationToken cancellationToken, params Expression<Func<TEntity, object>>[] includes)
			where TEntity : class, IDBTableID where TLoggedClass : class {

			ILogger<TLoggedClass> logger				= LoggerFactory.CreateLogger<TLoggedClass>();
			LoggedMessage<TLoggedClass> loggedMessage	=
				new LoggedMessage<TLoggedClass>(
					logger,
					"Ошибка поиска объекта в БД./* Объект {TEntity}, id {id}.*/",
					typeof(TEntity), id);

			return await FindDBRecordByIDdAndLogErrorAsync(id, loggedMessage,cancellationToken, includes);
		}

		/// <summary>
		/// Найти объект по ID.
		/// </summary>
		/// <param name="id">ID объекта</param>
		/// <param name="includes">Инклуды</param>
		/// <returns>Запись в БД</returns>
		private static TEntity FindDBRecordByID<TEntity>(int id, params Expression<Func<TEntity, object>>[] includes)
			where TEntity : class, IDBTableID {
			
			using (IDBSource source = new MKKContext()) {
				IQueryable<TEntity> query = ((DbContext)source).Set<TEntity>().
							AsNoTracking().
							Where(i => i.ID.Equals(id));

				foreach (var include in includes) {
					query	= query.Include(include);
				}

				return query.SingleOrDefault();
			}
		}

		/// <summary>
		/// Запрос для поиска записи по ID.
		/// </summary>
		/// <typeparam name="TEntity">Тип данных</typeparam>
		/// <param name="dbSource">источник данных</param>
		/// <param name="id">Код записи</param>
		/// <returns></returns>
		private static IQueryable<TEntity> GetSingleRecordQuery<TEntity>(IDBSource dbSource, int id) where TEntity : class, IDBTableID {
			return ((DbContext)dbSource).
						Set<TEntity>().
						AsNoTracking().
						Where(i => i.ID.Equals(id));
		}

		/// <summary>
		/// Запрос для поиска единичной записи по имени.
		/// </summary>
		/// <typeparam name="TEntity">Тип данных</typeparam>
		/// <param name="dbSource">Источник данных</param>
		/// <param name="name">Имя</param>
		/// <returns>Запись</returns>
		private static IQueryable<TEntity> GetSingleRecordByNameQuery<TEntity>(IDBSource dbSource, string name) where TEntity : class, IDBTableName {
			return ((DbContext)dbSource).
						Set<TEntity>().
						AsNoTracking().
						Where(i => i.Name.Equals(name));
		}

		/// <summary>
		/// Найти запись в БД по имени асинхронно. Если записи нет, то создать новую с Name = параметру name.
		/// </summary>
		/// <typeparam name="TEntity">Таблица</typeparam>
		/// <typeparam name="TLoggedClass">Класс логируемого объекта</typeparam>
		/// <param name="name">Имя записи</param>
		/// <param name="loggedMessage">Описание места и состоянии объекта вызвавшего исключение</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns>Запись с искомым ID</returns>
		internal static async Task<TEntity> FindRequiredDBRecordByNameAndLogErrorAsync<TEntity, TLoggedClass>(string name,
			LoggedMessage<TLoggedClass> loggedMessage, CancellationToken cancellationToken) where TEntity : class, IDBTableName, new() where TLoggedClass : class {
			using (IDBSource dbSource = new MKKContext()) {
				TEntity dbRecord	= await GetSingleRecordByNameQuery<TEntity>(dbSource, name).
					SingleOrDefaultAndErrorAsync(loggedMessage, cancellationToken);

				if (dbRecord != default) return dbRecord;

				TEntity newRecord	= new TEntity() { Name = name };
				await ((DbContext)dbSource).Set<TEntity>().AddAsync(newRecord, cancellationToken);
				await dbSource.SaveChangesAndLogErrorAsync(loggedMessage, cancellationToken);

				return newRecord;
			}
		}

		/// <summary>
		/// Найти запись в БД по имени асинхронно. Если записи нет, то создать новую с Name = параметру name.
		/// </summary>
		/// <typeparam name="TEntity">Таблица</typeparam>
		/// <typeparam name="TLoggedClass">Класс логируемого объекта</typeparam>
		/// <param name="name">Имя записи</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns>Запись с искомым ID</returns>
		internal static async Task<TEntity> FindRequiredDBRecordByNameAndLogErrorAsync<TEntity, TLoggedClass>(string name,
			CancellationToken cancellationToken) where TEntity : class, IDBTableName, new() where TLoggedClass : class {
			ILogger<TLoggedClass> logger = MKKContext.LoggerFactory.CreateLogger<TLoggedClass>();
			LoggedMessage<TLoggedClass> loggedMessage =
				new LoggedMessage<TLoggedClass>(logger, "Ошибка поиска объекта в БД./* Объект {TEntity}, значение {name}.*/", typeof(TEntity), name);

			return await FindRequiredDBRecordByNameAndLogErrorAsync<TEntity, TLoggedClass>(name, loggedMessage, cancellationToken);
		}

		/// <summary>
		/// Найти запись в БД по имени асинхронно. Если записи нет, то создать новую с Name = параметру name.
		/// </summary>
		/// <typeparam name="TEntity"></typeparam>
		/// <param name="name">Имя записи</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns>Запись с искомым ID</returns>
		internal static async Task<TEntity> FindRequiredDBRecordByNameAsync<TEntity>(string name,
			CancellationToken cancellationToken) where TEntity : class, IDBTableName, new() {
			
			using (IDBSource dbSource = new MKKContext()) {
				TEntity dbRecord = await GetSingleRecordByNameQuery<TEntity>(dbSource, name).
					SingleOrDefaultAsync(cancellationToken);

				if (dbRecord != default) return dbRecord;

				TEntity newRecord = new TEntity() { Name = name };
				await ((DbContext)dbSource).Set<TEntity>().AddAsync(newRecord, cancellationToken);
				await ((DbContext)dbSource).SaveChangesAsync(cancellationToken);

				return newRecord;
			}
		}

		/// <summary>
		/// Найти запись в БД по имени асинхронно. Если записи нет, то создать новую с Name = параметру name, Code1C = code1C.
		/// </summary>
		/// <typeparam name="TEntity">Таблица</typeparam>
		/// <typeparam name="TLoggedClass">Класс логируемого объекта</typeparam>
		/// <param name="name">Имя записи</param>
		/// <param name="code1C">Код 1С</param>
		/// <param name="loggedMessage">Описание места и состоянии объекта вызвавшего исключение</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns>Запись с искомым ID</returns>
		internal static async Task<TEntity> FindRequiredDBRecordByNameAndLogErrorAsync<TEntity, TLoggedClass>(string name,
			string code1C, LoggedMessage<TLoggedClass> loggedMessage, CancellationToken cancellationToken)
			where TEntity : class, IDBTableIDNameW1CCode, new() where TLoggedClass : class {
			
			using (IDBSource dbSource = new MKKContext()) {
				TEntity dbRecord = await GetSingleRecordByNameQuery<TEntity>(dbSource, name).
					SingleOrDefaultAndErrorAsync(loggedMessage, cancellationToken);

				if (dbRecord == default) {
					TEntity newRecord = new TEntity() { Name = name, Code1C = code1C };

					await ((DbContext)dbSource).Set<TEntity>().AddAsync(newRecord, cancellationToken);
					await dbSource.SaveChangesAndLogErrorAsync(loggedMessage, cancellationToken);

					return newRecord;
				}

				return dbRecord;
			}
		}

		/// <summary>
		/// Найти запись в БД по имени асинхронно. Если записи нет, то создать новую с Name = параметру name, Code1C = code1C.
		/// </summary>
		/// <typeparam name="TEntity">Таблица</typeparam>
		/// <typeparam name="TLoggedClass">Класс логируемого объекта</typeparam>
		/// <param name="name">Имя записи</param>
		/// <param name="code1C">Код 1С</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns>Запись с искомым ID</returns>
		internal static async Task<TEntity> FindRequiredDBRecordByNameAndLogErrorAsync<TEntity, TLoggedClass>(string name,
			string code1C, CancellationToken cancellationToken) where TEntity : class, IDBTableIDNameW1CCode, new() where TLoggedClass : class {
			ILogger<TLoggedClass> logger				= LoggerFactory.CreateLogger<TLoggedClass>();
			LoggedMessage<TLoggedClass> loggedMessage	=
				new LoggedMessage<TLoggedClass>(logger, "Ошибка поиска объекта в БД./* Объект {TEntity}, значение {name}, код 1С {code1C}.*/", typeof(TEntity), name, code1C);

			return await FindRequiredDBRecordByNameAndLogErrorAsync<TEntity, TLoggedClass>(name, code1C, loggedMessage, cancellationToken);
		}


		/// <summary>
		/// Найти запись в БД по имени асинхронно. Если записи нет, то создать новую с Name = параметру name, Code1C = code1C.
		/// </summary>
		/// <typeparam name="TEntity"></typeparam>
		/// <param name="name">Имя записи</param>
		/// <param name="code1C">Код 1С</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns>Запись с искомым ID</returns>
		internal static async Task<TEntity> FindRequiredDBRecordByNameAsync<TEntity>(string name, string code1C,
			CancellationToken cancellationToken) where TEntity : class, IDBTableIDNameW1CCode, new() {
			using (IDBSource dbSource = new MKKContext()) {
				TEntity dbRecord = await GetSingleRecordByNameQuery<TEntity>(dbSource, name).
					SingleOrDefaultAsync(cancellationToken);

				if (dbRecord == default) {
					TEntity newRecord = new TEntity() { Name = name, Code1C = code1C };

					await ((DbContext)dbSource).Set<TEntity>().AddAsync(newRecord, cancellationToken);
					await dbSource.SaveChangesAsync(cancellationToken);

					return newRecord;
				}

				return dbRecord;
			}
		}

		/// <summary>
		/// Запрос для выборки клиента.
		/// </summary>
		/// <param name="dbSource">Источник данных</param>
		/// <param name="code1C">Код записи</param>
		/// <returns>Запрос</returns>
		private static IQueryable<ClientDB> ClientDBQuery(IDBSource dbSource, string code1C) {
			return dbSource.Clients.
						AsNoTracking().
						Where(i => i.Code1C.Equals(code1C));
		}

		/// <summary>
		/// Найти клиента по ID из 1С.
		/// </summary>
		/// <param name="code1C">Код клиента</param>
		/// <returns>Запись в БД</returns>
		private static ClientDB FindClient(string code1C) {
			using (IDBSource source = new MKKContext()) {
				return ClientDBQuery(source, code1C).SingleOrDefault();
			}
		}

		/// <summary>
		/// Найти асинхронно клиента по ID из 1С.
		/// </summary>
		/// <param name="code1C">Код клиента</param>
		/// <param name="loggedMessage">Описание места и состоянии объекта вызвавшего исключение</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns>Запись в БД</returns>
		internal static async Task<ClientDB> FindClientAndLogErrorAsync<TLoggedClass>(string code1C,
			LoggedMessage<TLoggedClass> loggedMessage, CancellationToken cancellationToken) where TLoggedClass : class {
			using (IDBSource source = new MKKContext()) {
				return await ClientDBQuery(source, code1C).SingleOrDefaultAndErrorAsync(loggedMessage, cancellationToken);
			}
		}

		/// <summary>
		/// Найти асинхронно клиента по ID из 1С.
		/// </summary>
		/// <param name="code1C">Код клиента</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns>Запись в БД</returns>
		internal static async Task<ClientDB> FindClientAndLogErrorAsync<TLoggedClass>(string code1C,
			CancellationToken cancellationToken) where TLoggedClass : class {
			
			using (IDBSource source = new MKKContext()) {
				ILogger<TLoggedClass> logger = LoggerFactory.CreateLogger<TLoggedClass>();
				LoggedMessage<TLoggedClass> loggedMessage =
					new LoggedMessage<TLoggedClass>(logger, "Ошибка поиска клиента в БД./* Код клиента {client1CCode}.*/", code1C);

				return await ClientDBQuery(source, code1C).SingleOrDefaultAndErrorAsync(loggedMessage, cancellationToken);
			}
		}


		/// <summary>
		/// Запрос для выборки договора.
		/// </summary>
		/// <param name="dbSource">Источник данных</param>
		/// <param name="account1CCode">Код договора 1С</param>
		/// <param name="includes">Объекты для включения в выборку</param>
		/// <returns>Запрос</returns>
		internal static IQueryable<Account1C> AccountDBQuery(IDBSource dbSource, string account1CCode, params Expression<Func<Account1C, object>>[] includes) {
			IQueryable<Account1C> query = dbSource.Account1Cs.
						AsNoTracking().
						Where(i => i.Account1CCode.Equals(account1CCode));

			foreach (var include in includes) {
				query = query.Include(include);
			}

			return query;
		}

		/// <summary>
		/// Найти асинхронно договор по коду 1С.
		/// </summary>
		/// <param name="account1CCode">Номер договора</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <param name="includes">Инклуды</param>
		/// <returns>Запись в БД</returns>
		internal static async Task<Account1C> FindAccountAsync(string account1CCode, CancellationToken cancellationToken,
			params Expression<Func<Account1C, object>>[] includes) {
			
			using (IDBSource source = new MKKContext()) {
				return await AccountDBQuery(source, account1CCode, includes).SingleOrDefaultAsync(cancellationToken);
			}
		}

		/// <summary>
		/// Найти асинхронно договор по коду 1С.
		/// </summary>
		/// <typeparam name="TLoggedClass">Тип логируемого объекта</typeparam>
		/// <param name="account1CCode">Номер договора 1С</param>
		/// <param name="loggedMessage">Описание места и состоянии объекта вызвавшего исключение</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <param name="includes">Инклуды</param>
		/// <returns></returns>
		internal static async Task<Account1C> FindAccountAndLogErrorAsync<TLoggedClass>(string account1CCode,
			LoggedMessage<TLoggedClass> loggedMessage, CancellationToken cancellationToken,
			params Expression<Func<Account1C, object>>[] includes) where TLoggedClass : class {
			
			using (IDBSource source = new MKKContext()) {
				return await AccountDBQuery(source, account1CCode, includes).
					SingleOrDefaultAndErrorAsync(loggedMessage, cancellationToken);
			}
		}

		/// <summary>
		/// Найти асинхронно договор по коду 1С.
		/// </summary>
		/// <typeparam name="TLoggedClass">Тип логируемого объекта</typeparam>
		/// <param name="account1CCode">Номер договора 1С</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <param name="includes">Инклуды</param>
		/// <returns></returns>
		internal static async Task<Account1C> FindAccountAndLogErrorAsync<TLoggedClass>(string account1CCode,
			CancellationToken cancellationToken, params Expression<Func<Account1C, object>>[] includes) where TLoggedClass : class {
			
			ILogger<TLoggedClass> logger				= LoggerFactory.CreateLogger<TLoggedClass>();
			LoggedMessage<TLoggedClass> loggedMessage	=
				new LoggedMessage<TLoggedClass>(logger, "Ошибка поиска договора в БД./* Код договора {account1CCode}.*/", account1CCode);

			return await FindAccountAndLogErrorAsync(account1CCode, loggedMessage, cancellationToken, includes);
		}

		/// <summary>
		/// Найти договор по коду 1С.
		/// </summary>
		/// <param name="account1CCode">Код клиента</param>
		/// <param name="includes">Инклуды</param>
		/// <returns>Запись в БД</returns>
		internal static Account1C FindAccount(string account1CCode, params Expression<Func<Account1C, object>>[] includes) {
			using (IDBSource source = new MKKContext()) {
				return AccountDBQuery(source, account1CCode, includes).SingleOrDefault();
			}
		}

		/// <summary>
		/// Получить описание файла по ID асинхронно.
		/// </summary>
		/// <param name="idFileDescription"></param>
		/// <param name="loggedMessage">Описание места и состоянии объекта вызвавшего исключение</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns></returns>
		internal static async Task<FileDescriptionDB> FindFileDescriptionDBAndLogErrorAsync<TLoggedClass>(int idFileDescription,
			LoggedMessage<TLoggedClass> loggedMessage, CancellationToken cancellationToken) where TLoggedClass : class{
			using (IDBSource source = new MKKContext()) {
				return await source.FileDescriptionDBs.
					AsNoTracking().
					Where(i => i.ID.Equals(idFileDescription)).
					SingleOrDefaultAndErrorAsync(loggedMessage, cancellationToken);
			}
		}

		/// <summary>
		/// Получить описание файла по ID асинхронно.
		/// </summary>
		/// <param name="idFileDescription"></param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns></returns>
		internal static async Task<FileDescriptionDB> FindFileDescriptionDBAndLogErrorAsync<TLoggedClass>(int idFileDescription,
			CancellationToken cancellationToken) where TLoggedClass : class {
			
			ILogger<TLoggedClass> logger				= LoggerFactory.CreateLogger<TLoggedClass>();
			LoggedMessage<TLoggedClass> loggedMessage	=
				new LoggedMessage<TLoggedClass>(logger, "Ошибка поиска описания файла в БД./* Код описания {idFileDescription}.*/", idFileDescription);

			using (IDBSource source = new MKKContext()) {
				return await source.FileDescriptionDBs.
					AsNoTracking().
					Where(i => i.ID.Equals(idFileDescription)).
					SingleOrDefaultAndErrorAsync(loggedMessage, cancellationToken);
			}
		}

		/// <summary>
		/// Выполнить запрос/процедуру асинхронно.
		/// </summary>
		/// <typeparam name="TEntity"></typeparam>
		/// <param name="query">Запрос</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns></returns>
		internal static async Task<TEntity[]> ExecSqlAsync<TEntity>(string query, CancellationToken cancellationToken)
			where TEntity: class {
			
			using (IDBSource source = new MKKContext()) {
				return await ((DbContext)source).Set<TEntity>().FromSqlRaw(query).ToArrayAsync(cancellationToken);
			}
		}

		/// <summary>
		/// Выполнить запрос/процедуру асинхронно и при необходимости логировать ошибки.
		/// </summary>
		/// <typeparam name="TEntity">Результат выполнения запроса</typeparam>
		/// <typeparam name="TLoggedClass">Класс логируемого объекта</typeparam>
		/// <param name="query">Запрос</param>
		/// <param name="loggedMessage">Описание места и состоянии объекта вызвавшего исключение</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns></returns>
		internal static async Task<TEntity[]> ExecSqlAndLogErrorAsync<TEntity, TLoggedClass>(string query,
			LoggedMessage<TLoggedClass> loggedMessage, CancellationToken cancellationToken) where TEntity : class where TLoggedClass : class {
			
			using (IDBSource source = new MKKContext()) {
				return await ((MKKContext)source).Set<TEntity>().FromSqlRaw(query).
					ToArrayAndLogErrorAsync(loggedMessage, cancellationToken);
			}
		}

		/// <summary>
		/// Получить значение из потока и приатачить.
		/// </summary>
		/// <typeparam name="TEntity">Тип данных</typeparam>
		/// <typeparam name="TLoggedClass">Класс логируемого объекта</typeparam>
		/// <param name="dbContext">Контекст БД</param>
		/// <param name="valueOInTask">Поток с данными</param>
		/// <param name="loggedMessage">Описание места и состоянии объекта вызвавшего исключение</param>
		/// <returns>Приатаченное значение</returns>
		internal static async Task<TEntity> AttachFromTaskAndLogErrorAsync<TEntity, TLoggedClass>(IDBSource dbContext,
			Task<TEntity> valueOInTask, LoggedMessage<TLoggedClass> loggedMessage) where TEntity : class where TLoggedClass : class {
			TEntity attachedValue;
			try {
				attachedValue = await valueOInTask;
				((MKKContext)dbContext).Set<TEntity>().Attach(attachedValue);
			}
			catch (Exception exception) {
				loggedMessage.LogMessage("Ошибка работы с БД. Исключение: {ContextException}", exception);
				throw;
			}

			return attachedValue;
		}

		/// <summary>
		/// Получить значение из потока и приатачить.
		/// </summary>
		/// <typeparam name="TEntity">Тип данных</typeparam>
		/// <typeparam name="TLoggedClass">Класс логируемого объекта</typeparam>
		/// <param name="dbContext">Контекст БД</param>
		/// <param name="valueOInTask">Поток с данными</param>
		/// <returns>Приатаченное значение</returns>
		internal static async Task<TEntity> AttachFromTaskAndLogErrorAsync<TEntity, TLoggedClass>(IDBSource dbContext,
			Task<TEntity> valueOInTask) where TEntity : class where TLoggedClass : class {
			
			TEntity attachedValue;
			try {
				attachedValue = await valueOInTask;
				((MKKContext)dbContext).Set<TEntity>().Attach(attachedValue);
			}
			catch (Exception exception) {
				ILogger<TLoggedClass> logger	= LoggerFactory.CreateLogger<TLoggedClass>();
				logger.LogError(
					exception,
					"Ошибка работы с БД при аттаче объекта./* Тип объекта {TEntity}, исключение: {ContextException}*/",
					typeof(TEntity), exception.Message);
				throw;
			}

			return attachedValue;
		}

		/// <summary>
		/// Получить значение из потока и приатачить.
		/// </summary>
		/// <typeparam name="TEntity">Тип данных</typeparam>
		/// <param name="dbContext">Контекст БД</param>
		/// <param name="valueOInTask">Поток с данными</param>
		/// <returns>Приатаченное значение</returns>
		internal static async Task<TEntity> AttachFromTaskAsync<TEntity>(IDBSource dbContext, Task<TEntity> valueOInTask) where TEntity : class {
			var attachedValue	= await valueOInTask;
			((MKKContext)dbContext).Set<TEntity>().Attach(attachedValue);

			return attachedValue;
		}

		/// <summary>
		/// Приатачить значение, при возникновении исключения, залогировать и пробросить дальше.
		/// </summary>
		/// <typeparam name="TEntity">Тип данных</typeparam>
		/// <typeparam name="TLoggedClass">Класс логируемого объекта</typeparam>
		/// <param name="dbContext">Контекст БД</param>
		/// <param name="value">Поток с данными</param>
		/// <param name="loggedMessage">Описание места и состоянии объекта вызвавшего исключение</param>
		/// <returns>Приатаченное значение</returns>
		internal static TEntity AttachAndLogError<TEntity, TLoggedClass>(IDBSource dbContext, TEntity value,
			LoggedMessage<TLoggedClass> loggedMessage) where TEntity : class where TLoggedClass : class {
			
			try {
				((MKKContext)dbContext).Set<TEntity>().Attach(value);
			}
			catch (Exception exception) {
				loggedMessage.LogMessage("Ошибка работы с аттаче объекта. Исключение: {ContextException}", exception);
				throw;
			}

			return value;
		}

		/// <summary>
		/// Приатачить значение, при возникновении исключения, залогировать и пробросить дальше.
		/// </summary>
		/// <typeparam name="TEntity">Тип данных</typeparam>
		/// <typeparam name="TLoggedClass">Класс логируемого объекта</typeparam>
		/// <param name="dbContext">Контекст БД</param>
		/// <param name="value">Поток с данными</param>
		/// <returns>Приатаченное значение</returns>
		internal static TEntity AttachAndLogError<TEntity, TLoggedClass>(IDBSource dbContext, TEntity value)
			where TEntity : class where TLoggedClass : class {
			
			try {
				((MKKContext)dbContext).Set<TEntity>().Attach(value);
			}
			catch (Exception exception) {
				ILogger<TLoggedClass> logger = MKKContext.LoggerFactory.CreateLogger<TLoggedClass>();
				logger.LogError(
					exception,
					"Ошибка работы с БД при аттаче объекта./* Тип объекта {TEntity}, значение объекта {value}," +
					" исключение: {ContextException}*/",
					typeof(TEntity), value, exception.Message);
				throw;
			}

			return value;
		}

		/// <summary>
		/// Проверить доступ к договору по организации и логину пользователя.
		/// </summary>
		/// <param name="adLogin">Логин пользователя</param>
		/// <param name="account1CCode">Номер договора 1С</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns></returns>
		internal static async Task<bool> HaveAccessToAccountAsync(string adLogin, string account1CCode,
			CancellationToken cancellationToken) {
			
			Account1C account		= await FindAccountAsync(account1CCode, cancellationToken, i => i.Organization);
			Organizations[] orgs	= OrganizationsByLogin(adLogin);

			foreach (Organizations organizations in orgs) {
				if (account.Organization.Name.Equals(organizations.GetDescription())) return true;
			}

			return false;
		}
	}
}
