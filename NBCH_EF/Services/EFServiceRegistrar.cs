using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NBCH_EF.Helpers;
using NBCH_EF.Query;
using NBCH_EF.Tables;
using NBCH_LIB;
using NBCH_LIB.ADServiceProxy;
using NBCH_LIB.Interfaces;
using NBCH_LIB.Interfaces.WCF;
using NBCH_LIB.Logger;
using NBCH_LIB.Models;
using NBCH_LIB.Models.Registrar;
using static NBCH_EF.MKKContext;
using static NBCH_LIB.Logger.ExceptionLog;
using static NBCH_LIB.Helper;

namespace NBCH_EF.Services {
	/// <summary>
	/// Сервис по работе с архивом.
	/// </summary>
	public class EFServiceRegistrar: IServiceRegistrar, IServiceRegistrarWCF {
		/// <summary>
		/// Логгер.
		/// </summary>
		private static readonly ILogger<EFServiceRegistrar> _Logger;

		/// <summary>
		/// Статический конструктор.
		/// </summary>
		static EFServiceRegistrar() {
			_Logger = MKKContext.LoggerFactory.CreateLogger<EFServiceRegistrar>();
		}

		/// <summary>
		/// На сколько глубоко смотреть историю договоров.
		/// </summary>
		public const int DayForDepthSellPoints = 30;

		/// <summary>
		/// Получить список всех групп документов.
		/// </summary>
		public string[] GetDocumentGroups() => GetDocumentGroupsAsync(CancellationToken.None).ResultAndThrowException();

		/// <summary>
		/// Получить список всех групп документов асинхронно.
		/// </summary>
		public async Task<string[]> GetDocumentGroupsAsync() => await GetDocumentGroupsAsync(CancellationToken.None);

		/// <summary>
		/// Получить список всех групп документов асинхронно.
		/// </summary>
		/// <param name="cancellationToken">Токен отмены</param>
		public async Task<string[]> GetDocumentGroupsAsync(CancellationToken cancellationToken) {
			using (IDBSource dbSource = new MKKContext()) {
				return await dbSource.DocumentGroupDBs.Select(i => i.Description)
					.ToArrayAndLogErrorAsync<string, EFServiceRegistrar>(cancellationToken);
			}
		}

		/// <summary>
		/// Добавить группу документов асинхронно.
		/// </summary>
		/// <param name="groupName">Название группы</param>
		/// <returns></returns>
		public async Task AddDocumentGroupAsync(string groupName) =>
			await AddDocumentGroupAsync(groupName, CancellationToken.None);

		/// <summary>
		/// Получить список документов по имени группы документов.
		/// </summary>
		/// <param name="documentsGroup">Группа документов</param>
		/// <returns>Список типов файлов в группе</returns>
		public FileDescription[] GetFilesDescriptionsByDocumentGroupName(string documentsGroup) =>
			GetFilesDescriptionsByDocumentGroupNameAsync(documentsGroup, CancellationToken.None).ResultAndThrowException();

		/// <summary>
		/// Получить список документов по имени группы документов асинхронно.
		/// </summary>
		/// <param name="documentsGroup">Группа документов</param>
		/// <returns>Список типов файлов в группе</returns>
		public async Task<FileDescription[]> GetFilesDescriptionsByDocumentGroupNameAsync(string documentsGroup) =>
			await GetFilesDescriptionsByDocumentGroupNameAsync(documentsGroup, CancellationToken.None);
		
		/// <summary>
		/// Получить список документов по имени группы документов асинхронно.
		/// </summary>
		/// <param name="documentsGroup">Группа документов</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns>Список типов файлов в группе</returns>
		public async Task<FileDescription[]> GetFilesDescriptionsByDocumentGroupNameAsync(string documentsGroup,
			CancellationToken cancellationToken) {
			GetFilesDescriptionsByDocumentGroupNameCheckParams(documentsGroup);

			using (IDBSource dbSource = new MKKContext()) {
				DocumentGroupDB documentGroupDB = await dbSource.DocumentGroupDBs.
					Where(i => i.Description.Equals(documentsGroup)).
					Include(i => i.FileDescriptionDB.Select(j => j.ReadADRoles)).
					Include(i => i.FileDescriptionDB.Select(j => j.WriteADRoles)).
					Select(i => i).FirstOrDefaultAndLogErrorAsync<DocumentGroupDB, EFServiceRegistrar>(cancellationToken);

				return documentGroupDB?.FileDescriptionDB?.Select(i => (FileDescription)i).ToArray() ?? new FileDescription[0];
			}
		}

		/// <summary>
		/// Проверить входные параметры метода GetFilesDescriptionsByDocumentGroupName.
		/// </summary>
		/// <param name="documentsGroup">Группа документов</param>
		private static void GetFilesDescriptionsByDocumentGroupNameCheckParams(string documentsGroup) {
			if (string.IsNullOrEmpty(documentsGroup))
				LogAndThrowException<ArgumentNullException, EFServiceRegistrar>(_Logger,
					nameof(documentsGroup),
					"Не задана группа документов./* Метод {methodName}.*/",
					"GetFilesDescriptionsByDocumentGroupNameCheckParams");
		}

		/// <summary>
		/// Добавить группу документов.
		/// </summary>
		/// <param name="groupName">Название группы</param>
		public void AddDocumentGroup(string groupName) =>
			AddDocumentGroupAsync(groupName, CancellationToken.None).WaitAndThrowException();

		/// <summary>
		/// Добавить группу документов асинхронно.
		/// </summary>
		/// <param name="groupName">Название группы</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns></returns>
		public async Task AddDocumentGroupAsync(string groupName, CancellationToken cancellationToken) {
			AddDocumentGroupAsyncCheckParams(groupName);

			using (IDBSource dbSource = new MKKContext()) {
				if (await dbSource.DocumentGroupDBs.
					AnyAsync(i => i.Description.Equals(groupName), cancellationToken))
					LogAndThrowException<Exception, EFServiceRegistrar>(_Logger,
						"",
						"Группа с именем '{groupName}' уже существует./* Метод {methodName}.*/",
						groupName, "AddDocumentGroupAsync");

				await dbSource.DocumentGroupDBs.AddAsync(new DocumentGroupDB() { Description = groupName });
				await dbSource.SaveChangesAndLogErrorAsync<EFServiceRegistrar>(new LogShortMessage(
					"Не удалось добавить новую группу./* Метод {methodName}, группа документов {groupName}.*/",
					"AddDocumentGroupAsync", groupName),
					cancellationToken);
			}
		}

		/// <summary>
		/// Проверить входные параметры метода AddDocumentGroupAsync.
		/// </summary>
		/// <param name="groupName">Наименование новой группы</param>
		private static void AddDocumentGroupAsyncCheckParams(string groupName) {
			if (string.IsNullOrEmpty(groupName))
				LogAndThrowException<ArgumentNullException, EFServiceRegistrar>(_Logger,
					nameof(groupName),
					"Не задано имя группы документов./* Метод {methodName}.*/",
					"AddDocumentGroupAsync");
		}

		/// <summary>
		/// Добавить описание типа файла
		/// </summary>
		/// <param name="documentsGroup">Группа файлов</param>
		/// <param name="fileDescription">Описание файла</param>
		/// <returns>Новая запись</returns>
		public FileDescription AddFileDescription(string documentsGroup, FileDescription fileDescription) =>
			AddFileDescriptionAsync(documentsGroup, fileDescription, CancellationToken.None).ResultAndThrowException();

		/// <summary>
		/// Добавить описание типа файла асинхронно.
		/// </summary>
		/// <param name="documentsGroup">Группа файлов</param>
		/// <param name="fileDescription">Описание файла</param>
		/// <returns>Новая запись</returns>
		public async Task<FileDescription> AddFileDescriptionAsync(string documentsGroup, FileDescription fileDescription) =>
			await AddFileDescriptionAsync(documentsGroup, fileDescription, CancellationToken.None);

		/// <summary>
		/// Добавить описание типа файла асинхронно.
		/// </summary>
		/// <param name="documentsGroup">Группа файлов</param>
		/// <param name="fileDescription">Описание файла</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns>Новая запись</returns>
		public async Task<FileDescription> AddFileDescriptionAsync(string documentsGroup, FileDescription fileDescription,
			CancellationToken cancellationToken) {
			AddFileDescriptionAsyncCheckParams(documentsGroup, fileDescription);

			using (IDBSource dbSource = new MKKContext()) {
				DocumentGroupDB documentGroupDB	= await dbSource.DocumentGroupDBs.
					Where(i => i.Description.Equals(documentsGroup)).
					Include(i => i.FileDescriptionDB).
					FirstOrDefaultAndLogErrorAsync<DocumentGroupDB, EFServiceRegistrar>(cancellationToken);

				if (documentGroupDB == default)
					LogAndThrowException<ArgumentNullException, EFServiceRegistrar>(_Logger,
						nameof(documentGroupDB),
						"Группа документов с наименованием {documentsGroup} не существует./* Метод {methodName}, fileDescription: {fileDescription}.*/",
						"AddFileDescriptionAsync", new ExLogValue() {LogValue = default, ExValue = "null"});
				
				if (documentGroupDB?.FileDescriptionDB?.Where(i => i.Description.Equals(fileDescription.Descrioption)).Any() ?? false)
					LogAndThrowException<Exception, EFServiceRegistrar>(_Logger,
						nameof(documentGroupDB),
						"Тип файлов с описанием '{fileDescription}' в группе файлов '{documentsGroup}' уже существует./* Метод {methodName}, fileDescription: {fileDescription}.*/",
						fileDescription.Descrioption, documentsGroup, "AddFileDescriptionAsync", new ExLogValue() { LogValue = fileDescription, ExValue = "null"});

				FileDescriptionDB fileDescriptionDB	= (FileDescriptionDB)fileDescription;
				documentGroupDB?.FileDescriptionDB?.Add(fileDescriptionDB);
				await dbSource.FileDescriptionDBs.AddAsync(fileDescriptionDB, cancellationToken);

				await dbSource.SaveChangesAndLogErrorAsync<EFServiceRegistrar>(new LogShortMessage(
					message: "Не удалось найти группу документа./* Метод {methodName}, documentsGroup: {documentsGroup}, fileDescription: {fileDescription}.*/",
					"AddFileDescriptionAsync", documentsGroup, fileDescription),
					cancellationToken
				);

				DocumentGroupDB documentsGroupsBD =
					await (dbSource.DocumentGroupDBs.
						Where(i => i.Description.Equals(documentsGroup)).
						FirstOrDefaultAndLogErrorAsync<DocumentGroupDB, EFServiceRegistrar>(cancellationToken));

				return documentsGroupsBD?.
					FileDescriptionDB.
					Where(i => i.Description.Equals(fileDescription.Descrioption)).
					Select(i => (FileDescription)i).
					FirstOrDefault();
			}
		}

		/// <summary>
		/// Проверить входные параметры метода AddFileDescriptionAsyncCheckParams
		/// </summary>
		/// <param name="documentsGroup">Группа файлов</param>
		/// <param name="fileDescription">Описание файла</param>
		private static void AddFileDescriptionAsyncCheckParams(string documentsGroup, FileDescription fileDescription) {
			if (string.IsNullOrEmpty(documentsGroup))
				LogAndThrowException<ArgumentNullException, EFServiceRegistrar>(_Logger,
					nameof(documentsGroup),
					"Не задана группа документов./* Метод {methodName}.*/",
					"AddFileDescriptionAsync");

			if (fileDescription == default)
				LogAndThrowException<ArgumentNullException, EFServiceRegistrar>(_Logger,
					nameof(fileDescription),
					"Не задано описание типа файла./* Метод {methodName}.*/",
					"AddFileDescriptionAsync");
		}

		/// <summary>
		/// Обновить описание файла
		/// </summary>
		/// <param name="fileDescription">Описание файла</param>
		public void UpdateFileDescription(FileDescription fileDescription) =>
			UpdateFileDescriptionAsync(fileDescription, CancellationToken.None).WaitAndThrowException();

		/// <summary>
		/// Обновить описание файла асинхронно.
		/// </summary>
		/// <param name="fileDescription">Описание файла</param>
		public async Task UpdateFileDescriptionAsync(FileDescription fileDescription) =>
			await UpdateFileDescriptionAsync(fileDescription, CancellationToken.None);

		/// <summary>
		/// Обновить описание файла асинхронно.
		/// </summary>
		/// <param name="fileDescription">Описание файла</param>
		/// <param name="cancellationToken">Токен отмены</param>
		public async Task UpdateFileDescriptionAsync(FileDescription fileDescription, CancellationToken cancellationToken) {
			UpdateFileDescriptionCheckParams(fileDescription);

			using (IDBSource dbSource = new MKKContext()) {
				FileDescriptionDB fileDescriptionDB = await dbSource.FileDescriptionDBs.
					Where(i => i.ID == fileDescription.ID).
					Include(i => i.ReadADRoles).
					Include(i => i.WriteADRoles).
					FirstOrDefaultAndLogErrorAsync<FileDescriptionDB, EFServiceRegistrar>(cancellationToken);
				if (fileDescriptionDB == default)
					LogAndThrowException<Exception, EFServiceRegistrar>(_Logger,
						"",
						"Невозможно обновить запись описания файла./* Запись с кодом {fileDescription.ID} в БД не существует. Метод {methodName}.*/",
						fileDescription.ID, "UpdateFileDescriptionAsync");

				ADRoleDB[] readADRoleDBs	= fileDescriptionDB.ReadADRoles.ToArray();
				ADRoleDB[] writeADRoleDBs	= fileDescriptionDB.WriteADRoles.ToArray();
				dbSource.ADRoleDBs.RemoveRange(readADRoleDBs);
				dbSource.ADRoleDBs.RemoveRange(writeADRoleDBs);

				fileDescriptionDB.Description	= fileDescription.Descrioption;
				fileDescriptionDB.ReadADRoles	= fileDescription.ReadADRoles.Select(i => new ADRoleDB() { Role = i }).ToList();
				fileDescriptionDB.WriteADRoles	= fileDescription.WriteADRoles.Select(i => new ADRoleDB() { Role = i }).ToList();

				await dbSource.SaveChangesAndLogErrorAsync<EFServiceRegistrar>(new LogShortMessage(
					message: "Не удалось обновить описание файла./* Метод {methodName}, fileDescription: {fileDescription}.*/",
					"UpdateFileDescriptionAsync", new ExLogValue() { LogValue = fileDescription, ExValue = fileDescription.ID }),
					cancellationToken
				);
			}
		}

		/// <summary>
		/// Проверить входные параметры UpdateFileDescriptionAsync.
		/// </summary>
		/// <param name="fileDescription">Описание файла</param>
		private static void UpdateFileDescriptionCheckParams(FileDescription fileDescription) {
			if (fileDescription == default)
				LogAndThrowException<ArgumentNullException, EFServiceRegistrar>(_Logger,
					nameof(fileDescription),
					"Не задано описание типа файла./* Метод {methodName}.*/",
					"UpdateFileDescriptionCheckParams");

			if ((fileDescription?.ID ?? default) == default)
				LogAndThrowException<ArgumentNullException, EFServiceRegistrar>(_Logger,
					nameof(fileDescription),
					"ID файла описания, не задано./* Метод {methodName}, fileDescription: {fileDescription}.*/",
					"UpdateFileDescriptionCheckParams", new ExLogValue() { LogValue = fileDescription, ExValue = fileDescription?.ID });
		}

		/// <summary>
		/// Получить список документов по номеру договора 1С и списку клиентов из договора.
		/// </summary>
		/// <param name="adLogin">Логин пользователя, кто запрашивает документы</param>
		/// <param name="account1CCode">Код договора 1С</param>
		/// <param name="users">Клиенты</param>
		/// <returns>Список документов по договору клиента</returns>
		public Dictionary<Client, RegistrarDocument[]> GetDocumentsByAccountAndClients1C(string adLogin, string account1CCode, Client[] users) =>
			GetDocumentsByAccountAndClients1CAsync(adLogin, account1CCode, users, CancellationToken.None).ResultAndThrowException();

		/// <summary>
		/// Получить список документов по номеру договора 1С и списку клиентов из договора асинхронно.
		/// </summary>
		/// <param name="adLogin">Логин пользователя, кто запрашивает документы</param>
		/// <param name="account1CCode">Код договора 1С</param>
		/// <param name="users">Клиенты</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns>Список документов по договору клиента</returns>
		public async Task<Dictionary<Client, RegistrarDocument[]>> GetDocumentsByAccountAndClients1CAsync(
			string adLogin, string account1CCode, Client[] users) =>

			await GetDocumentsByAccountAndClients1CAsync(adLogin, account1CCode, users, CancellationToken.None);

		/// <summary>
		/// Получить список документов по номеру договора 1С и списку клиентов из договора асинхронно.
		/// </summary>
		/// <param name="adLogin">Логин пользователя, кто запрашивает документы</param>
		/// <param name="account1CCode">Код договора 1С</param>
		/// <param name="users">Клиенты</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns>Список документов по договору клиента</returns>
		public async Task<Dictionary<Client, RegistrarDocument[]>> GetDocumentsByAccountAndClients1CAsync(string adLogin,
			string account1CCode, Client[] users, CancellationToken cancellationToken) {
			GetDocumentsByAccountAndClients1CCheckParams(adLogin, account1CCode, users);

			if (!await HaveAccessToAccountAsync(adLogin, account1CCode, cancellationToken)) {
				LogAndThrowException<ArgumentNullException, EFServiceRegistrar>(_Logger,
					"",
					"У пользователя нет прав для доступа к договору./* Метод {methodName}, логин: {adLogin}, договор {account1CCode}.*/",
					"GetDocumentsByAccountAndClients1CAsync", adLogin, account1CCode);
			}

			Dictionary<Client, RegistrarDocument[]> registrarDocuments = new Dictionary<Client, RegistrarDocument[]>();
			foreach (Client client in users) {
				RegistrarDocument[] registrarDocument =
					await GetDocumentsByAccountAndClients1CAsync(adLogin, account1CCode, client, cancellationToken);
				registrarDocuments.Add(await ClientBy1CCodeAsync(client.Code1C, cancellationToken), registrarDocument);
			}

			return registrarDocuments;
		}

		/// <summary>
		/// Проверить входные параметры GetDocumentsByAccountAndClients1CAsync
		/// </summary>
		/// <param name="adLogin">Логин пользователя, кто запрашивает документы</param>
		/// <param name="account1CCode">Код договора 1С</param>
		/// <param name="users">Клиенты</param>
		private static void GetDocumentsByAccountAndClients1CCheckParams(string adLogin, string account1CCode, Client[] users) {
			if (string.IsNullOrEmpty(account1CCode))
				LogAndThrowException<ArgumentNullException, EFServiceRegistrar>(_Logger,
					nameof(account1CCode),
					"Не задан договор 1С./* Метод {methodName}.*/", 
					"GetDocumentsByAccountAndClients1CCheckParams");

			if (string.IsNullOrEmpty(adLogin))
				LogAndThrowException<ArgumentNullException, EFServiceRegistrar>(_Logger,
					nameof(adLogin),
					"Не задан пользователь./* Метод {methodName}.*/", 
					"GetDocumentsByAccountAndClients1CCheckParams");

			if (users == default || !users.Any())
				LogAndThrowException<ArgumentNullException, EFServiceRegistrar>(_Logger,
					nameof(adLogin),
					"Не задана информация по клиенту./* Метод {methodName}, клиенты {users}.*/",
					"GetDocumentsByAccountAndClients1CCheckParams", new ExLogValue() {LogValue = users, ExValue = "null"});
		}

		/// <summary>
		/// Получить запись клиента ио коду из 1С асинхронно.
		/// </summary>
		/// <param name="client1CCode">Код клиента из 1С</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns>Запись из базы</returns>
		private async Task<Client> ClientBy1CCodeAsync(string client1CCode, CancellationToken cancellationToken) {
			using (IDBSource dbSource = new MKKContext()) {
				return (Client)(await dbSource.Clients.
					AsNoTracking().
					Where(i => i.Code1C.Equals(client1CCode)).
					FirstOrDefaultAndLogErrorAsync<ClientDB, EFServiceRegistrar>(cancellationToken));
			}
		}

		/// <summary>
		/// Получить список документов по номеру договора 1С и клиенту из договора
		/// </summary>
		/// <param name="adLogin">Логин пользователя, кто запрашивает документы</param>
		/// <param name="account1CCode">Код договора 1С</param>
		/// <param name="user">Клиент</param>
		/// <param name="documentID">Код документа</param>
		/// <returns>Список документов по договору клиента</returns>
		private RegistrarDocument[] GetDocumentsByAccountAndClients1C(string adLogin, string account1CCode, Client user, int documentID = default) =>
			GetDocumentsByAccountAndClients1CAsync(adLogin, account1CCode, user, CancellationToken.None, documentID).ResultAndThrowException();


		/// <summary>
		/// Получить список документов по номеру договора 1С и клиенту из договора
		/// </summary>
		/// <param name="adLogin">Логин пользователя, кто запрашивает документы</param>
		/// <param name="account1CCode">Код договора 1С</param>
		/// <param name="user">Клиент</param>
		/// <param name="documentID">Код документа</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns>Список документов по договору клиента</returns>
		private static async Task<RegistrarDocument[]> GetDocumentsByAccountAndClients1CAsync(string adLogin,
			string account1CCode, Client user, CancellationToken cancellationToken,  int documentID = default) {
			LoggedMessage<EFServiceRegistrar> loggerMessage = new LoggedMessage<EFServiceRegistrar>(
				_Logger,
				"Не удалось получить список документов по договору./* Метод {methodName}, пользователь {adLogin}, ID документа {documentID}, клиент {user}.*/  Договор {account1CCode}",
				"GetDocumentsByAccountAndClients1CAsync", adLogin, documentID, new ExLogValue() { LogValue = user, ExValue = user?.Code1C }, account1CCode);

			Task<RoleListQuery[]> documentsRolesTask	= 
				ExecSqlAndLogErrorAsync<RoleListQuery, EFServiceRegistrar>("exec RegistrarRoles", loggerMessage, cancellationToken);
			Task<FileDescriptionQuery[]> filesTask		= ExecSqlAndLogErrorAsync<FileDescriptionQuery, EFServiceRegistrar>
				($"exec RegistrarGetFilesList '{account1CCode}', '{user?.Code1C}', {documentID}", loggerMessage, cancellationToken);

			RoleListQuery[] documentsRoles	= await documentsRolesTask;
			FileDescriptionQuery[] files	= await filesTask;

			string[] userADRoles = GetUserADRoles(adLogin);

			RegistrarDocument[] registrarDocuments	= documentsRoles.OrderBy(i => i.SortOrder)
				.Where(i => userADRoles.Contains(i.ReadAccess) & files.Any(j => j.FDescID.Equals(i.FDescID)))
				.Select(i => new RegistrarDocument() { ID = i.FDescID, FileDescription = i.FDescDescription }).Distinct()
				.ToArray();

			registrarDocuments.AsParallel().WithCancellation(cancellationToken).ForAll(document => {
				document.WriteAccess = documentsRoles.Any(role => 
					role.FDescID.Equals(document.ID) && userADRoles.Contains(role.WriteAccess));

				document.Files = files.Where(file => file.FDescID.Equals(document.ID) && file.FRegitrarID != null)
					.Select(j => new RegistrarFile() {
						ID				= (int)j.FRegitrarID,
						FileName		= j.FileName,
						UploadDate		= (DateTime)j.UploadDate,
						AuthorName		= j.InspectorName,
						Client1CCode	= j.Client1CCode
					}).ToList();
			});

			return registrarDocuments.ToArray();
		}

		/// <summary>
		/// Получить через прокси класс роли пользователя.
		/// </summary>
		/// <param name="adLogin">Логин пользователя</param>
		/// <returns>Роли пользователя</returns>
		private static string[] GetUserADRoles(string adLogin) {
			try { return Singleton<ProxyADRoles>.Values[adLogin]; }
			catch (Exception exception) {
				LoggedMessage<EFServiceRegistrar> logMessage = new LoggedMessage<EFServiceRegistrar>(
					_Logger,
					"Не удалось получить список ролей./* Метод {methodName} adLogin: {adLogin}.*/",
					"GetUserADRoles", adLogin);
				logMessage.LogMessage($"{Environment.NewLine}" + "Описание ошибки: {exception}", exception);
				throw;
			}
		}

		/// <summary>
		/// Получить список документов по номеру договора 1С и клиенту из договора
		/// </summary>
		/// <param name="adLogin">Логин пользователя, кто запрашивает документы</param>
		/// <param name="account1CCode">Код договора 1С</param>
		/// <param name="user">Клиент</param>
		/// <param name="documentID">Код документа</param>
		/// <returns>Список документов по договору клиента</returns>
		public async Task<RegistrarDocument> GetDocumentsByAccountAndClients1CAndDocumentIDAsync(string adLogin,
			string account1CCode, Client user, int documentID) =>

			await GetDocumentsByAccountAndClients1CAndDocumentIDAsync(adLogin, account1CCode, user, documentID,
				CancellationToken.None); 

		/// <summary>
		/// Загрузить файлы в архив на сервере.
		/// </summary>
		/// <param name="adLogin">Логин AD пользователя, осуществляющего загрузку</param>
		/// <param name="account1CCode">Код клиента 1С, к которому привязаны документы</param>
		/// <param name="client1CCode">Код договора, к которому привязаны документы</param>
		/// <param name="idFileDescription">Описание загружаемого файла.</param>
		/// <param name="files">Загружаемые файлы (имя, содержимое)</param>
		/// <param name="clientTimeZone">Часовой пояс на клиентской машине</param>
		public void UploadRegistrarFiles(string adLogin, string account1CCode, string client1CCode, int idFileDescription, Dictionary<string, byte[]> files, int clientTimeZone) =>
			UploadRegistrarFilesAsync(adLogin, account1CCode, client1CCode, idFileDescription, files, clientTimeZone,
				CancellationToken.None).WaitAndThrowException();

		/// <summary>
		/// Загрузить файлы в архив на сервере асинхронно.
		/// </summary>
		/// <param name="adLogin">Логин AD пользователя, осуществляющего загрузку</param>
		/// <param name="account1CCode">Код клиента 1С, к которому привязаны документы</param>
		/// <param name="client1CCode">Код договора, к которому привязаны документы</param>
		/// <param name="idFileDescription">Описание загружаемого файла.</param>
		/// <param name="files">Загружаемые файлы (имя, содержимое)</param>
		/// <param name="clientTimeZone">Часовой пояс на клиентской машине</param>
		public async Task UploadRegistrarFilesAsync(string adLogin, string account1CCode, string client1CCode,
			int idFileDescription,
			Dictionary<string, byte[]> files, int clientTimeZone) =>

			await UploadRegistrarFilesAsync(adLogin, account1CCode, client1CCode, idFileDescription, files,
				clientTimeZone, CancellationToken.None);

		/// <summary>
		/// Загрузить файлы в архив на сервере асинхронно.
		/// </summary>
		/// <param name="adLogin">Логин AD пользователя, осуществляющего загрузку</param>
		/// <param name="account1CCode">Код клиента 1С, к которому привязаны документы</param>
		/// <param name="client1CCode">Код договора, к которому привязаны документы</param>
		/// <param name="idFileDescription">Описание загружаемого файла.</param>
		/// <param name="files">Загружаемые файлы (имя, содержимое)</param>
		/// <param name="clientTimeZone">Часовой пояс на клиентской машине</param>
		/// <param name="cancellationToken">Токен отмены</param>
		public async Task UploadRegistrarFilesAsync(string adLogin, string account1CCode, string client1CCode,
			int idFileDescription, Dictionary<string, byte[]> files, int clientTimeZone, CancellationToken cancellationToken) {
			UploadRegistrarFilesCheckParams(adLogin, account1CCode, client1CCode, idFileDescription, files, clientTimeZone);

			using (IDBSource dbSource = new MKKContext()) {
				Task<ClientDB> clientTask				= FindClientAndLogErrorAsync<EFServiceRegistrar>(client1CCode, cancellationToken);
				Task<Account1C> accountTask				= FindAccountAndLogErrorAsync<EFServiceRegistrar>(account1CCode, cancellationToken);
				Task<FileDescriptionDB> fileDescTask	= FindFileDescriptionDBAndLogErrorAsync<EFServiceRegistrar>(idFileDescription, cancellationToken);

				ClientDB client					= await AttachFromTaskAndLogErrorAsync<ClientDB, EFServiceRegistrar>(dbSource, clientTask);
				Account1C account				= await AttachFromTaskAndLogErrorAsync<Account1C, EFServiceRegistrar>(dbSource, accountTask);
				FileDescriptionDB fileDesc		= await AttachFromTaskAndLogErrorAsync<FileDescriptionDB, EFServiceRegistrar>(dbSource, fileDescTask);

				if (client == default) LogAndThrowException<Exception, EFServiceRegistrar>(_Logger, "", "Клиент с кодом 1С {client1CCode} в базе не найден./* Метод {methodName}.*/", client1CCode, "UploadRegistrarFilesAsync");
				if (account == default) LogAndThrowException<Exception, EFServiceRegistrar>(_Logger, "", "Договор с кодом 1С {account1CCode} в базе не найден./* Метод {methodName}.*/", account1CCode, "UploadRegistrarFilesAsync");
				if (fileDesc == default) LogAndThrowException<Exception, EFServiceRegistrar>(_Logger, "", "Описание файла с кодом {idFileDescription} в базе не найдено./* Метод {methodName}.*/", idFileDescription, "UploadRegistrarFilesAsync");
				
				ADLoginsDB adLoginsDB	= GetUserADLogin(adLogin);
				AttachAndLogError<ADLoginsDB, EFServiceRegistrar>(dbSource, adLoginsDB);

				int serverTimeZone		= ServerTimeZone;

				foreach (KeyValuePair<string, byte[]> file in files) {
					RegistrarFileDB registrarFileDB = new RegistrarFileDB() {
						Account1C			= account,
						Client				= client,
						FileDescriptionDB	= fileDesc,
						AuthorName			= adLoginsDB,
						FileName			= file.Key,
						File				= file.Value,
						UploadDate			= DateTime.Now.AddHours(clientTimeZone - serverTimeZone),
						TimeZone			= clientTimeZone
					};

					await dbSource.RegistrarFileDBs.AddAsync(registrarFileDB, cancellationToken);
				}

				await dbSource.SaveChangesAndLogErrorAsync<EFServiceRegistrar>(new LogShortMessage(
					message: "Не удалось загрузить файлы на сервер./* Метод {methodName}, пользователь {adLogin}, тип файла {idFileDescription}, код client1CCode {client1CCode}, файлов {files}, часовой пояс {clientTimeZone}.*/  Договор 1С {account1CCode}",
					"UploadRegistrarFilesAsync", adLogin, idFileDescription, client1CCode, files.Count, clientTimeZone, account1CCode),
					cancellationToken
				);
			}
		}

		/// <summary>
		/// Получить через прокси класс логин пользователя.
		/// </summary>
		/// <param name="adLogin">Логин пользователя</param>
		/// <returns>Роли пользователя</returns>
		private static ADLoginsDB GetUserADLogin(string adLogin) {
			try { return Singleton<ProxyADLoginsDB>.Values[adLogin]; }
			catch (Exception exception) {
				LoggedMessage<EFServiceRegistrar> logMessage = new LoggedMessage<EFServiceRegistrar>(
					_Logger,
					"Не удалось получить логин пользователя./* Метод {methodName} adLogin: {adLogin}.*/",
					"GetUserADLogin", adLogin);
				logMessage.LogMessage($"{Environment.NewLine}" + "Описание ошибки: {exception}", exception);
				throw;
			}
		}

		/// <summary>
		/// Проверить входные параметры для метода UploadRegistrarFilesAsync.
		/// </summary>
		/// <param name="adLogin">Логин AD пользователя, осуществляющего загрузку</param>
		/// <param name="account1CCode">Код клиента 1С, к которому привязаны документы</param>
		/// <param name="client1CCode">Код договора, к которому привязаны документы</param>
		/// <param name="idFileDescription">Описание загружаемого файла.</param>
		/// <param name="files">Загружаемые файлы (имя, содержимое)</param>
		/// <param name="clientTimeZone">Часовой пояс клиента</param>
		private static void UploadRegistrarFilesCheckParams(string adLogin, string account1CCode, string client1CCode,
			int idFileDescription, Dictionary<string, byte[]> files, int clientTimeZone) {
			
			if (clientTimeZone == default)
				LogAndThrowException<ArgumentNullException, EFServiceRegistrar>(_Logger,
					nameof(clientTimeZone),
					"Не задан часовой пояс клиента./* Метод {methodName}.*/",
					"UploadRegistrarFilesCheckParams");

			if (String.IsNullOrEmpty(account1CCode))
				LogAndThrowException<ArgumentNullException, EFServiceRegistrar>(_Logger,
					nameof(account1CCode),
					"Не задан договор 1С, для которого загружаются документы./* Метод {methodName}.*/",
					"UploadRegistrarFilesCheckParams");

			if (String.IsNullOrEmpty(adLogin))
				LogAndThrowException<ArgumentNullException, EFServiceRegistrar>(_Logger,
					nameof(adLogin),
					"Не задан пользователь AD, загружающий документы./* Метод {methodName}.*/",
					"UploadRegistrarFilesCheckParams");

			if (String.IsNullOrEmpty(client1CCode))
				LogAndThrowException<ArgumentNullException, EFServiceRegistrar>(_Logger,
					nameof(client1CCode),
					"Не задан клиент 1С, для которого загружаются документы./* Метод {methodName}.*/",
					"UploadRegistrarFilesCheckParams");

			if (idFileDescription == default)
				LogAndThrowException<ArgumentNullException, EFServiceRegistrar>(_Logger,
					nameof(idFileDescription),
					"Не задан вид загружаемых файлов./* Метод {methodName}.*/",
					"UploadRegistrarFilesCheckParams");

			if (files == default)
				LogAndThrowException<ArgumentNullException, EFServiceRegistrar>(_Logger,
					nameof(files),
					"Нет загружаемых файлов./* Метод {methodName}.*/",
					"UploadRegistrarFilesCheckParams");
		}

		/// <summary>
		/// Получить файл из базы по его ID
		/// </summary>
		/// <param name="adLogin">AD логин пользователя, запрашивающего файл</param>
		/// <param name="idFile">ID файла</param>
		/// <returns>Имя и содержимое файла</returns>
		public RegistrarFileData GetRegistrarFile(string adLogin, int idFile) =>
			GetRegistrarFileAsync(adLogin, idFile, CancellationToken.None).ResultAndThrowException();


		/// <summary>
		/// Получить файл из базы по его ID асинхронно.
		/// </summary>
		/// <param name="adLogin">AD логин пользователя, запрашивающего файл</param>
		/// <param name="idFile">ID файла</param>
		/// <returns>Имя и содержимое файла</returns>
		public async Task<RegistrarFileData> GetRegistrarFileAsync(string adLogin, int idFile) =>
			await GetRegistrarFileAsync(adLogin, idFile, CancellationToken.None);

		/// <summary>
		/// Получить файл из базы по его ID асинхронно.
		/// </summary>
		/// <param name="adLogin">AD логин пользователя, запрашивающего файл</param>
		/// <param name="idFile">ID файла</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns>Имя и содержимое файла</returns>
		public async Task<RegistrarFileData> GetRegistrarFileAsync(string adLogin, int idFile, CancellationToken cancellationToken) {
			GetRegistrarFileCheckParams(adLogin, idFile);

			using (IDBSource dbSource = new MKKContext()) {
				string[] userADRoles = GetUserADRoles(adLogin);

				RegistrarFileDB registrarFileDB = await dbSource.RegistrarFileDBs.
					Where(i => i.ID.Equals(idFile)).
					Include(i => i.FileDescriptionDB.ReadADRoles).
					FirstOrDefaultAndLogErrorAsync<RegistrarFileDB, EFServiceRegistrar>(cancellationToken);

				if (registrarFileDB == default)
					LogAndThrowException<ArgumentOutOfRangeException, EFServiceRegistrar>(
						_Logger, nameof(idFile),
						"Файл не найден./* Метод {methodName}, ID файла {idFile}.*/",
						"GetRegistrarFileAsync", idFile);

				if (!registrarFileDB.FileDescriptionDB.ReadADRoles.Any(i => userADRoles.Contains(i.Role)))
					LogAndThrowException<Exception, EFServiceRegistrar>(
						_Logger, "",
						"У пользователя {adLogin} нет прав на просмотр файла./* Метод {methodName}, ID файла {idFile}.*/",
						adLogin, "GetRegistrarFileAsync", idFile);

				return new RegistrarFileData() {
					FileName	= registrarFileDB.FileName,
					Data		= registrarFileDB.File
				};
			}
		}


		/// <summary>
		/// Проверить корректность входных параметров GetRegistrarFileAsync.
		/// </summary>
		/// <param name="adLogin"> AD логин пользователя</param>
		/// <param name="idFile">ID пользователя</param>
		private static void GetRegistrarFileCheckParams(string adLogin, int idFile) {
			if (string.IsNullOrEmpty(adLogin))
				LogAndThrowException<ArgumentNullException, EFServiceRegistrar>(
					_Logger, nameof(adLogin),
					"Не задан пользователь AD, запрашивающий документ./* Метод {methodName}.*/",
					"GetRegistrarFileCheckParams");
			if (idFile == default)
				LogAndThrowException<ArgumentNullException, EFServiceRegistrar>(
					_Logger, nameof(idFile),
					"Не задан запрашиваемый файл./* Метод {methodName}.*/",
					"GetRegistrarFileCheckParams");
		}

		/// <summary>
		/// Пометить файл как удаленный
		/// </summary>
		/// <param name="adLogin"> AD логин пользователя</param>
		/// <param name="idFile">ID пользователя</param>
		public void MarkFileAsDeleted(string adLogin, int idFile) =>
			MarkFileAsDeletedAsync(adLogin, idFile, CancellationToken.None).WaitAndThrowException();

		/// <summary>
		/// Пометить файл как удаленный асинхронно.
		/// </summary>
		/// <param name="adLogin"> AD логин пользователя</param>
		/// <param name="idFile">ID пользователя</param>
		public async Task MarkFileAsDeletedAsync(string adLogin, int idFile) =>
			await MarkFileAsDeletedAsync(adLogin, idFile, CancellationToken.None);

		/// <summary>
		/// Пометить файл как удаленный асинхронно.
		/// </summary>
		/// <param name="adLogin"> AD логин пользователя</param>
		/// <param name="idFile">ID пользователя</param>
		/// <param name="cancellationToken">Токен отмены</param>
		public async Task MarkFileAsDeletedAsync(string adLogin, int idFile, CancellationToken cancellationToken) {
			MarkFileAsDeletedCheckParams(adLogin, idFile);

			LoggedMessage<EFServiceRegistrar> loggerMessage = new LoggedMessage<EFServiceRegistrar>(
				_Logger,
				"Не удалось удалить файл из БД./* Метод {methodName}, пользователь {adLogin}, ID файла {idFile}.*/",
				"MarkFileAsDeletedAsync", adLogin, idFile);

			using (IDBSource dbSource = new MKKContext()) {
				ADLoginsDB adLoginsDB = Singleton<ProxyADLoginsDB>.Values[adLogin];

				RegistrarFileDB registrarFileDB = await FindDBRecordByIDdAndLogErrorAsync<RegistrarFileDB, EFServiceRegistrar>(idFile, loggerMessage, cancellationToken, i => i.AuthorName);
				if (registrarFileDB == default)
					LogAndThrowException<Exception, EFServiceRegistrar>(_Logger, "", "Файла с ID '{idFile}' не найден./* Метод {methodName}.*/", idFile, "MarkFileAsDeletedAsync");
				if (!registrarFileDB.AuthorName.ID.Equals(adLoginsDB.ID))
					LogAndThrowException<Exception, EFServiceRegistrar>(_Logger, "", "Только автор этого документа может удалить файл./* Метод {methodName}.*/", idFile, "MarkFileAsDeletedAsync");

				registrarFileDB.Delete = true;
				dbSource.Entry(registrarFileDB).State = EntityState.Modified;
				await dbSource.SaveChangesAndLogErrorAsync(loggerMessage, cancellationToken);
			}
		}

		/// <summary>
		/// Проверить корректность входных параметров MarkFileAsDeletedAsync.
		/// </summary>
		/// <param name="adLogin">Логин пользователя</param>
		/// <param name="idFile">ID пользователя</param>
		private static void MarkFileAsDeletedCheckParams(string adLogin, int idFile) {
			if (string.IsNullOrEmpty(adLogin))
				LogAndThrowException<ArgumentNullException, EFServiceRegistrar>(
					_Logger, nameof(adLogin),
					"Не задан пользователь AD, удаляющий документ./* Метод {methodName}.*/",
					"MarkFileAsDeletedCheckParams");

			if (idFile == default)
				LogAndThrowException<ArgumentNullException, EFServiceRegistrar>(
					_Logger, nameof(adLogin),
					"Не задан файл, подлежащий удалению./* Метод {methodName}.*/",
					"MarkFileAsDeletedCheckParams");
		}

		/// <summary>
		/// Получить точки продаж в разрезе организаций
		/// Можно передавать дату по-умолчанию
		/// </summary>
		/// <param name="dateFrom">Дата с которой делается выборка по организациям</param>
		/// <param name="dateTo">Дата по которую делается выборка по организациям</param>
		/// <param name="organizations">Организации</param>
		/// <returns>Точки продаж в разрезе организаций</returns>
		public SellPoint[] GetSellPoints(DateTime dateFrom, DateTime dateTo, string[] organizations) =>
			GetSellPointsAsync(dateFrom, dateTo, organizations, CancellationToken.None).ResultAndThrowException();

		/// <summary>
		/// Получить точки продаж в разрезе организаций асинхронно.
		/// Можно передавать дату по-умолчанию
		/// </summary>
		/// <param name="dateFrom">Дата с которой делается выборка по организациям</param>
		/// <param name="dateTo">Дата по которую делается выборка по организациям</param>
		/// <param name="organizations">Организации</param>
		/// <returns>Точки продаж в разрезе организаций</returns>
		public async Task<SellPoint[]> GetSellPointsAsync(DateTime dateFrom, DateTime dateTo, string[] organizations) =>
			await GetSellPointsAsync(dateFrom, dateTo, organizations, CancellationToken.None);

		/// <summary>
		/// Получить точки продаж в разрезе организаций асинхронно.
		/// Можно передавать дату по-умолчанию
		/// </summary>
		/// <param name="dateFrom">Дата с которой делается выборка по организациям</param>
		/// <param name="dateTo">Дата по которую делается выборка по организациям</param>
		/// <param name="organizations">Организации</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns>Точки продаж в разрезе организаций</returns>
		public async Task<SellPoint[]> GetSellPointsAsync(DateTime dateFrom, DateTime dateTo, string[] organizations,
			CancellationToken cancellationToken) {
			
			if (dateFrom == default) dateFrom = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(-DayForDepthSellPoints);
			if (dateTo == default) dateTo = DateTime.Now;

			LoggedMessage<EFServiceRegistrar> loggerMessage = new LoggedMessage<EFServiceRegistrar>(
				_Logger,
				"Не удалось получить список точек./* Метод {methodName}, дата 'С' {dateFrom}, дата 'по' {dateTo}.*/",
				"GetSellPointsAsync", dateFrom, dateTo);

			using (IDBSource dbSource = new MKKContext()) {
				SellPontDB[] data = await dbSource.Account1Cs.
					Include(i => i.Organization).
					Where(i => i.DateTime >= dateFrom && i.DateTime <= dateTo && organizations.Contains(i.Organization.Name)).
					Select(i => i.SellPont).
					Distinct().
					ToArrayAndLogErrorAsync(loggerMessage, cancellationToken);

				return data.Select(i => (SellPoint)i).ToArray();
			}
		}


		/// <summary>
		/// Получить информацию по загруженным документам
		/// </summary>
		/// <param name="accounts">Список договоров</param>
		/// <returns>Информация по загруженным документам</returns>
		public AccountsForCheck[] GetAccountsInfoForCheckDocuments(string[] accounts) =>
			GetAccountsInfoForCheckDocumentsAsync(accounts, CancellationToken.None).ResultAndThrowException();

		/// <summary>
		/// Получить информацию по загруженным документам асинхронно.
		/// </summary>
		/// <param name="accounts">Список договоров</param>
		/// <returns>Информация по загруженным документам</returns>
		public async Task<AccountsForCheck[]> GetAccountsInfoForCheckDocumentsAsync(string[] accounts) =>
			await GetAccountsInfoForCheckDocumentsAsync(accounts, CancellationToken.None);

		/// <summary>
		/// Получить информацию по загруженным документам асинхронно.
		/// </summary>
		/// <param name="accounts">Список договоров</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns>Информация по загруженным документам</returns>
		public async Task<AccountsForCheck[]> GetAccountsInfoForCheckDocumentsAsync(string[] accounts,
			CancellationToken cancellationToken) {
			using (MKKContext dbSource = new MKKContext()) {
				SqlParameter parameter = CreateParam(accounts);

				LoggedMessage<EFServiceRegistrar> loggerMessage = new LoggedMessage<EFServiceRegistrar>(
					_Logger,
					"Не удалось получить список договоров с описанием загруженных документов./* Метод {methodName}, список договоров {accounts}.*/",
					"GetAccountsInfoForCheckDocumentsAsync", string.Join(", ", accounts ?? new [] {"null"}));

				return (await dbSource.AccountsForCheckQuery.FromSqlRaw("exec [dbo].[RegistrarGetDocumentsFlag] {0}", parameter).
						ToArrayAndLogErrorAsync(loggerMessage, cancellationToken)).
						OrderBy(i => i.Account1CCode).
						ToArray();
			}
		}

		/// <summary>
		/// Создать параметр для хранимой процедуры [RegistrarGetDocumentsFlag] на основании списка договоров
		/// </summary>
		/// <param name="accounts">Список договоров</param>
		/// <returns>Параметр для запроса</returns>
		private SqlParameter CreateParam(string[] accounts) {
			DataTable data = new DataTable();
			data.Columns.Add("Code1C", typeof(string));
			foreach (var r in accounts) {
				data.Rows.Add(r);
			}

			SqlParameter param = new SqlParameter("@accountList", data) { TypeName = "dbo.Account1CList" };

			return param;
		}

		/// <summary>
		/// Получить список файлов по номеру договора 1С, клиенту из договора и коду документа
		/// </summary>
		/// <param name="adLogin">Логин пользователя, кто запрашивает документы</param>
		/// <param name="account1CCode">Код договора 1С</param>
		/// <param name="user">Клиент</param>
		/// <param name="documentID">Код документа</param>
		/// <returns>Список файлов по договору клиента и номеру документа</returns>
		public RegistrarDocument GetDocumentsByAccountAndClients1CAndDocumentID(string adLogin, string account1CCode,
			Client user, int documentID) =>
			GetDocumentsByAccountAndClients1CAndDocumentIDAsync(adLogin, account1CCode, user, documentID, CancellationToken.None).
				ResultAndThrowException();

		/// <summary>
		/// Получить список файлов по номеру договора 1С, клиенту из договора и коду документа асинхронно.
		/// </summary>
		/// <param name="adLogin">Логин пользователя, кто запрашивает документы</param>
		/// <param name="account1CCode">Код договора 1С</param>
		/// <param name="user">Клиент</param>
		/// <param name="documentID">Код документа</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns>Список файлов по договору клиента и номеру документа</returns>
		public async Task<RegistrarDocument> GetDocumentsByAccountAndClients1CAndDocumentIDAsync(string adLogin,
			string account1CCode, Client user, int documentID, CancellationToken cancellationToken) {
			GetDocumentsByAccountAndClients1CAndDocumentIDCheckParams(adLogin, account1CCode, user, documentID);

			return (await GetDocumentsByAccountAndClients1CAsync(adLogin, account1CCode, user, cancellationToken, documentID)).FirstOrDefault();
		}

		/// <summary>
		/// Проверить входные параметры GetDocumentsByAccountAndClients1CAndDocumentIDAsync.
		/// </summary>
		/// <param name="adLogin">Логин пользователя, кто запрашивает документы</param>
		/// <param name="account1CCode">Код договора 1С</param>
		/// <param name="user">Клиент</param>
		/// <param name="documentID">Код документа</param>
		private static void GetDocumentsByAccountAndClients1CAndDocumentIDCheckParams(string adLogin, string account1CCode, Client user, int documentID) {
			if (string.IsNullOrEmpty(account1CCode))
				LogAndThrowException<ArgumentNullException, EFServiceRegistrar>(_Logger,
					nameof(account1CCode),
					"Не задан договор 1С, для которого необходимо получить список документов./* Метод {methodName}.*/",
					"GetDocumentsByAccountAndClients1CAndDocumentIDCheckParams");

			if (string.IsNullOrEmpty(adLogin))
				LogAndThrowException<ArgumentNullException, EFServiceRegistrar>(_Logger,
					nameof(adLogin),
					"Не задан пользователь, который запрашивает документы./* Метод {methodName}.*/",
					"GetDocumentsByAccountAndClients1CAndDocumentIDCheckParams");

			if (user == default)
				LogAndThrowException<ArgumentNullException, EFServiceRegistrar>(_Logger,
					nameof(user),
					"Не задана информации по клиенту 1C./* Метод {methodName}.*/",
					"GetDocumentsByAccountAndClients1CAndDocumentIDCheckParams");

			if (documentID == default)
				LogAndThrowException<ArgumentNullException, EFServiceRegistrar>(_Logger,
					nameof(documentID),
					"Не задан код документа./* Метод {methodName}.*/",
					"GetDocumentsByAccountAndClients1CAndDocumentIDCheckParams");
		}

		/// <summary>
		/// Получить список фотографий клиента
		/// </summary>
		/// <param name="client1CCode">Код клиента 1С</param>
		/// <param name="excludeAccount">Исключить фотографию из выбранного договора</param>
		/// <param name="adLogin">Пользователь, запрашивающий список</param>
		/// <returns>Список - ID файла и дата загрузки</returns>
		public Dictionary<int, DateTime> GetPhotoList(string client1CCode, string excludeAccount, string adLogin) =>
			GetPhotoListAsync(client1CCode, excludeAccount, adLogin, CancellationToken.None).ResultAndThrowException();

		/// <summary>
		/// Получить список фотографий клиента асинхронно.
		/// </summary>
		/// <param name="client1CCode">Код клиента 1С</param>
		/// <param name="excludeAccount">Исключить фотографию из выбранного договора</param>
		/// <param name="adLogin">Пользователь, запрашивающий список</param>
		/// <returns>Список - ID файла и дата загрузки</returns>
		public async Task<Dictionary<int, DateTime>> GetPhotoListAsync(string client1CCode, string excludeAccount,
			string adLogin) =>
			await GetPhotoListAsync(client1CCode, excludeAccount, adLogin, CancellationToken.None);

		/// <summary>
		/// Получить список фотографий клиента асинхронно.
		/// </summary>
		/// <param name="client1CCode">Код клиента 1С</param>
		/// <param name="excludeAccount">Исключить фотографию из выбранного договора</param>
		/// <param name="adLogin">Пользователь, запрашивающий список</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns>Список - ID файла и дата загрузки</returns>
		public async Task<Dictionary<int, DateTime>> GetPhotoListAsync(string client1CCode, string excludeAccount,
			string adLogin, CancellationToken cancellationToken) {
			
			GetPhotoListCheckParams(client1CCode, adLogin);
			excludeAccount = excludeAccount ?? "";

			await CheckPermissionAsync(adLogin, cancellationToken);

			LoggedMessage<EFServiceRegistrar> loggerMessage = new LoggedMessage<EFServiceRegistrar>(
				_Logger,
				"Не удалось получить список фотографий клиента./* Метод {methodName}, код клиента 1С {}, текущий договор {}, логин {}.*/",
				"GetPhotoListAsync", client1CCode, excludeAccount, adLogin);


			using (IDBSource dbSource = new MKKContext()) {
				return await dbSource.RegistrarFileDBs.
					AsNoTracking().
					Include(i => i.Client).
					Include(i => i.Account1C).
					Include(i => i.FileDescriptionDB).
					Where(i => i.Client.Code1C.Equals(client1CCode) && !i.Account1C.Account1CCode.Equals(excludeAccount) && i.FileDescriptionDB.Description.Equals(Presets.Photo)).
					Select(i => new { id = i.ID, uploadDate = i.UploadDate }).
					ToDictionaryAndLogErrorAsync(i => i.id, i => i.uploadDate, loggerMessage, cancellationToken);
			}
		}

		/// <summary>
		/// Проверить пользователя на наличие прав.
		/// </summary>
		/// <param name="adLogin"></param>
		/// <param name="cancellationToken">Токен отмены</param>
		private static async Task CheckPermissionAsync(string adLogin, CancellationToken cancellationToken) {
			using (IDBSource dbSource = new MKKContext()) {
				IEnumerable<ADRoleDB> readRoles = await dbSource.
					FileDescriptionDBs.
					AsNoTracking().
					Where(i => i.Description.Equals(Presets.Photo)).
					Select(i => i.ReadADRoles).
					FirstOrDefaultAndLogErrorAsync<IEnumerable<ADRoleDB>, EFServiceRegistrar>(cancellationToken);

				string[] userRoles = GetUserADRoles(adLogin);
				if (!readRoles?.Any(i => userRoles.Contains(i.Role)) ?? false)
					LogAndThrowException<Exception, EFServiceRegistrar>(_Logger,
						"",
						"У пользователя нет прав для просмотра фотографий клиентов./* Метод {methodName}, имя пользователя {adLogin}.*/",
						"GetPhotoListCheckParams", adLogin);
			}
		}

		/// <summary>
		/// Проверить входные параметры GetPhotoListCheckParamsAsync.
		/// </summary>
		/// <param name="client1CCode">Код клиента 1С</param>
		/// <param name="adLogin">Логин пользователя, запрашивающего права</param>
		private static void GetPhotoListCheckParams(string client1CCode, string adLogin) {
			if (string.IsNullOrEmpty(client1CCode))
				LogAndThrowException<ArgumentNullException, EFServiceRegistrar>(_Logger,
					nameof(client1CCode),
					"Не задан код клиента 1С./* Метод {methodName}.*/",
					"GetPhotoListCheckParams");

			if (string.IsNullOrEmpty(adLogin))
				LogAndThrowException<ArgumentNullException, EFServiceRegistrar>(_Logger,
					nameof(client1CCode),
					"Не задан пользователь, запрашивающий список файлов./* Метод {methodName}.*/",
					"GetPhotoListCheckParams");
		}
	}
}
