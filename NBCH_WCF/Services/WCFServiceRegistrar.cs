using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Threading;
using System.Threading.Tasks;
using NBCH_LIB.Interfaces.WCF;
using NBCH_LIB.Logger;
using NBCH_LIB.Models;
using NBCH_LIB.Models.Registrar;
using static NBCH_WCF.Services.Utils;
using static NBCH_WCF.Services.ServiceMethod;

namespace NBCH_WCF.Services {
	/// <summary>
	/// Сервис, реализующего интерфейс IServiceRegistrar для работы с архивом документов.
	/// </summary>
	public class WCFServiceRegistrar : IServiceRegistrarWCF {
		/// <summary>
		/// Добавить группу документов.
		/// </summary>
		/// <param name="groupName">Имя группы</param>
		[PrincipalPermission(SecurityAction.Demand, Role = @"admin")]
		public void AddDocumentGroup(string groupName) =>
			ExecuteWithTryCatch<WCFServiceRegistrar>(() => ServiceRegistrar.AddDocumentGroup(groupName),
				new LogShortMessage("Ошибка вызова метода. Метод: {methodName}, groupName: {groupName}.",
					"AddDocumentGroup", groupName));
		
		/// <summary>
		/// Добавить группу документов асинхронно.
		/// </summary>
		/// <param name="groupName">Имя группы</param>
		[PrincipalPermission(SecurityAction.Demand, Role = @"admin")]
		public async  Task AddDocumentGroupAsync(string groupName) =>
			await ExecuteWithTryCatchAsync<WCFServiceRegistrar>( () => ServiceRegistrar.AddDocumentGroupAsync(groupName),
				new LogShortMessage("Ошибка вызова метода. Метод: {methodName}, groupName: {groupName}.",
					"AddDocumentGroupAsync", groupName));

		/// <summary>
		/// Добавить описание типа файла.
		/// </summary>
		/// <param name="documentsGroup">Группа файлов</param>
		/// <param name="fileDescription">Описание файла</param>
		/// <returns>Новая запись</returns>
		[PrincipalPermission(SecurityAction.Demand, Role = @"admin")]
		public FileDescription AddFileDescription(string documentsGroup, FileDescription fileDescription) =>
			ExecuteWithTryCatch<FileDescription, WCFServiceRegistrar>(() =>
					ServiceRegistrar.AddFileDescription(documentsGroup, fileDescription),
				new LogShortMessage("Ошибка вызова метода. Метод: {methodName}, groupName: {documentsGroup}" +
				                    " fileDescription: {fileDescription}.",
					"AddFileDescription", documentsGroup, fileDescription));

		/// <summary>
		/// Добавить описание типа файла асинхронно.
		/// </summary>
		/// <param name="documentsGroup">Группа файлов</param>
		/// <param name="fileDescription">Описание файла</param>
		/// <returns>Новая запись</returns>
		[PrincipalPermission(SecurityAction.Demand, Role = @"admin")]
		public async Task<FileDescription> AddFileDescriptionAsync(string documentsGroup, FileDescription fileDescription) =>
			await ExecuteWithTryCatchAsync<FileDescription, WCFServiceRegistrar>(() =>
					ServiceRegistrar.AddFileDescriptionAsync(documentsGroup, fileDescription),
				new LogShortMessage("Ошибка вызова метода. Метод: {methodName}, groupName: {documentsGroup}" +
				                    " fileDescription: {fileDescription}.",
					"AddFileDescriptionAsync", documentsGroup, fileDescription));

		/// <summary>
		/// Получить информацию по загруженным документам.
		/// </summary>
		/// <param name="accounts">Список договоров</param>
		/// <returns>Информация по загруженным документам</returns>
		[PrincipalPermission(SecurityAction.Demand, Role = @"admin")]
		public AccountsForCheck[] GetAccountsInfoForCheckDocuments(string[] accounts) =>
			ExecuteWithTryCatch<AccountsForCheck[], WCFServiceRegistrar>(() =>
				ServiceRegistrar.GetAccountsInfoForCheckDocuments(accounts),
				new LogShortMessage("Ошибка вызова метода. Метод: {methodName}, accounts: {accounts}.",
					"GetAccountsInfoForCheckDocuments", string.Join(", ", accounts ?? new [] {"null"})));

		/// <summary>
		/// Получить информацию по загруженным документам асинхронно.
		/// </summary>
		/// <param name="accounts">Список договоров</param>
		/// <returns>Информация по загруженным документам</returns>
		[PrincipalPermission(SecurityAction.Demand, Role = @"admin")]
		public async Task<AccountsForCheck[]> GetAccountsInfoForCheckDocumentsAsync(string[] accounts) =>
			await ExecuteWithTryCatchAsync<AccountsForCheck[], WCFServiceRegistrar>( () =>
					ServiceRegistrar.GetAccountsInfoForCheckDocumentsAsync(accounts),
				new LogShortMessage("Ошибка вызова метода. Метод: {methodName}, accounts: {accounts}.",
					"GetAccountsInfoForCheckDocumentsAsync", string.Join(", ", accounts ?? new [] {"null"})));

		/// <summary>
		/// Получить список всех групп документов.
		/// </summary>
		/// <returns>Список групп</returns>
		[PrincipalPermission(SecurityAction.Demand, Role = @"admin")]
		public string[] GetDocumentGroups() =>
			ExecuteWithTryCatch<string[], WCFServiceRegistrar>(() => ServiceRegistrar.GetDocumentGroups(),
				new LogShortMessage("Ошибка вызова метода. Метод: {methodName}.",
					"GetDocumentGroups"));

		/// <summary>
		/// Получить список всех групп документов асинхронно.
		/// </summary>
		/// <returns>Список групп</returns>
		[PrincipalPermission(SecurityAction.Demand, Role = @"admin")]
		public async Task<string[]> GetDocumentGroupsAsync() =>
			await ExecuteWithTryCatchAsync<string[], WCFServiceRegistrar>(() =>
				ServiceRegistrar.GetDocumentGroupsAsync(),
				new LogShortMessage("Ошибка вызова метода. Метод: {methodName}.",
					"GetDocumentGroupsAsync"));

		/// <summary>
		/// Получить список документов по номеру договора 1С и списку клиентов из договора.
		/// </summary>
		/// <param name="adLogin">Логин пользователя, кто запрашивает документы</param>
		/// <param name="account1CCode">Код договора 1С</param>
		/// <param name="users">Клиенты</param>
		/// <returns>Список документов по договору клиента</returns>
		[PrincipalPermission(SecurityAction.Demand, Role = @"admin")]
		public Dictionary<Client, RegistrarDocument[]> GetDocumentsByAccountAndClients1C(string adLogin,
			string account1CCode, Client[] users) =>
			
			ExecuteWithTryCatch<Dictionary<Client, RegistrarDocument[]>, WCFServiceRegistrar>(() =>
				ServiceRegistrar.GetDocumentsByAccountAndClients1C(adLogin, account1CCode, users),
				new LogShortMessage("Ошибка вызова метода. Метод: {methodName}, adLogin: {adLogin}," +
				                    " account1CCode: {account1CCode}, clients: {clients}.",
					"GetDocumentsByAccountAndClients1C", adLogin, account1CCode, users));

		/// <summary>
		/// Получить список документов по номеру договора 1С и списку клиентов из договора асинхронно.
		/// </summary>
		/// <param name="adLogin">Логин пользователя, кто запрашивает документы</param>
		/// <param name="account1CCode">Код договора 1С</param>
		/// <param name="users">Клиенты</param>
		/// <returns>Список документов по договору клиента</returns>
		[PrincipalPermission(SecurityAction.Demand, Role = @"admin")]
		public async Task<Dictionary<Client, RegistrarDocument[]>> GetDocumentsByAccountAndClients1CAsync(string adLogin,
			string account1CCode, Client[] users) =>
	
			await ExecuteWithTryCatchAsync<Dictionary<Client, RegistrarDocument[]>, WCFServiceRegistrar>(() =>
				ServiceRegistrar.GetDocumentsByAccountAndClients1CAsync(adLogin, account1CCode, users),
				new LogShortMessage("Ошибка вызова метода. Метод: {methodName}, adLogin: {adLogin}," +
				                    " account1CCode: {account1CCode}, clients: {clients}.",
					"GetDocumentsByAccountAndClients1CAsync", adLogin, account1CCode, users));

		/// <summary>
		/// Получить список файлов по номеру договора 1С, клиенту из договора и коду документа.
		/// </summary>
		/// <param name="adLogin">Логин пользователя, кто запрашивает документы</param>
		/// <param name="account1CCode">Код договора 1С</param>
		/// <param name="user">Клиент</param>
		/// <param name="documentID">Код документа</param>
		/// <returns>Список файлов по договору клиента и номеру документа</returns>
		[PrincipalPermission(SecurityAction.Demand, Role = @"admin")]
		public RegistrarDocument GetDocumentsByAccountAndClients1CAndDocumentID(string adLogin, string account1CCode,
			Client user, int documentID) =>
			ExecuteWithTryCatch<RegistrarDocument, WCFServiceRegistrar>(() =>
				ServiceRegistrar.GetDocumentsByAccountAndClients1CAndDocumentID(adLogin, account1CCode, user, documentID),
				new LogShortMessage("Ошибка вызова метода. Метод: {methodName}, adLogin: {adLogin}," +
				                    " account1CCode: {account1CCode}, client: {clients}, documentID: {documentID}.",
					"GetDocumentsByAccountAndClients1CAndDocumentID", adLogin, account1CCode, user, documentID));

		/// <summary>
		/// Получить список файлов по номеру договора 1С, клиенту из договора и коду документа асинхронно.
		/// </summary>
		/// <param name="adLogin">Логин пользователя, кто запрашивает документы</param>
		/// <param name="account1CCode">Код договора 1С</param>
		/// <param name="user">Клиент</param>
		/// <param name="documentID">Код документа</param>
		/// <returns>Список файлов по договору клиента и номеру документа</returns>
		[PrincipalPermission(SecurityAction.Demand, Role = @"admin")]
		public async Task<RegistrarDocument> GetDocumentsByAccountAndClients1CAndDocumentIDAsync(string adLogin,
			string account1CCode, Client user, int documentID) =>
			await ExecuteWithTryCatchAsync<RegistrarDocument, WCFServiceRegistrar>(() =>
				ServiceRegistrar.GetDocumentsByAccountAndClients1CAndDocumentIDAsync(adLogin, account1CCode, user,
					documentID),
				new LogShortMessage("Ошибка вызова метода. Метод: {methodName}, adLogin: {adLogin}," +
				                    " account1CCode: {account1CCode}, client: {clients}, documentID: {documentID}.",
					"GetDocumentsByAccountAndClients1CAndDocumentIDAsync", adLogin, account1CCode, user, documentID));

		/// <summary>
		/// Получить список документов по имени группы документов.
		/// </summary>
		/// <param name="documentsGroup">Группа документов</param>
		/// <returns>Список типов файлов в группе</returns>
		[PrincipalPermission(SecurityAction.Demand, Role = @"admin")]
		public FileDescription[] GetFilesDescriptionsByDocumentGroupName(string documentsGroup) =>
			ExecuteWithTryCatch<FileDescription[], WCFServiceRegistrar>(() =>
				ServiceRegistrar.GetFilesDescriptionsByDocumentGroupName(documentsGroup),
				new LogShortMessage("Ошибка вызова метода. Метод: {methodName}, documentsGroup: {documentsGroup},",
					"GetFilesDescriptionsByDocumentGroupName", documentsGroup));
		

		/// <summary>
		/// Получить список документов по имени группы документов асинхронно.
		/// </summary>
		/// <param name="documentsGroup">Группа документов</param>
		/// <returns>Список типов файлов в группе</returns>
		[PrincipalPermission(SecurityAction.Demand, Role = @"admin")]
		public async Task<FileDescription[]> GetFilesDescriptionsByDocumentGroupNameAsync(string documentsGroup) =>
			await ExecuteWithTryCatchAsync<FileDescription[], WCFServiceRegistrar>(() =>
				ServiceRegistrar.GetFilesDescriptionsByDocumentGroupNameAsync(documentsGroup),
				new LogShortMessage("Ошибка вызова метода. Метод: {methodName}, documentsGroup: {documentsGroup},",
					"GetFilesDescriptionsByDocumentGroupNameAsync", documentsGroup));

		/// <summary>
		/// Получить список фотографий клиента.
		/// </summary>
		/// <param name="client1CCode">Код клиента 1С</param>
		/// <param name="excludeAccount">Исключить фотографию из выбранного договора</param>
		/// <param name="adLogin">Пользователь, запрашивающий список</param>
		/// <returns>Список - ID файла и дата загрузки</returns>
		[PrincipalPermission(SecurityAction.Demand, Role = @"admin")]
		public Dictionary<int, DateTime> GetPhotoList(string client1CCode, string excludeAccount, string adLogin) =>
			ExecuteWithTryCatch<Dictionary<int, DateTime>, WCFServiceRegistrar>(() =>
				ServiceRegistrar.GetPhotoList(client1CCode, excludeAccount, adLogin),
				new LogShortMessage("Ошибка вызова метода. Метод: {methodName}, client1CCode: {client1CCode}," +
				                    " excludeAccount: {excludeAccount}, adLogin: {adLogin}.",
					"GetPhotoList", client1CCode, excludeAccount, adLogin));

		/// <summary>
		/// Получить список фотографий клиента асинхронно.
		/// </summary>
		/// <param name="client1CCode">Код клиента 1С</param>
		/// <param name="excludeAccount">Исключить фотографию из выбранного договора</param>
		/// <param name="adLogin">Пользователь, запрашивающий список</param>
		/// <returns>Список - ID файла и дата загрузки</returns>
		[PrincipalPermission(SecurityAction.Demand, Role = @"admin")]
		public async Task<Dictionary<int, DateTime>> GetPhotoListAsync(string client1CCode, string excludeAccount,
			string adLogin) =>
			await ExecuteWithTryCatchAsync<Dictionary<int, DateTime>, WCFServiceRegistrar>(() =>
				ServiceRegistrar.GetPhotoListAsync(client1CCode, excludeAccount, adLogin),
				new LogShortMessage("Ошибка вызова метода. Метод: {methodName}, client1CCode: {client1CCode}," +
				                    " excludeAccount: {excludeAccount}, adLogin: {adLogin}.",
					"GetPhotoListAsync", client1CCode, excludeAccount, adLogin));

		/// <summary>
		/// Получить файл из базы по его ID.
		/// </summary>
		/// <param name="adLogin">AD логин пользователя, запрашивающего файл</param>
		/// <param name="idFile">ID файла</param>
		/// <returns>Имя и содержимое файла</returns>
		[PrincipalPermission(SecurityAction.Demand, Role = @"admin")]
		public RegistrarFileData GetRegistrarFile(string adLogin, int idFile) =>
			ExecuteWithTryCatch<RegistrarFileData, WCFServiceRegistrar>(() =>
				ServiceRegistrar.GetRegistrarFile(adLogin, idFile),
				new LogShortMessage("Ошибка вызова метода. Метод: {methodName}, idFile: {idFile} adLogin: {adLogin}.",
					"GetRegistrarFile", idFile, adLogin));

		/// <summary>
		/// Получить файл из базы по его ID асинхронно.
		/// </summary>
		/// <param name="adLogin">AD логин пользователя, запрашивающего файл</param>
		/// <param name="idFile">ID файла</param>
		/// <returns>Имя и содержимое файла</returns>
		[PrincipalPermission(SecurityAction.Demand, Role = @"admin")]
		public async Task<RegistrarFileData> GetRegistrarFileAsync(string adLogin, int idFile) =>
			await ExecuteWithTryCatchAsync<RegistrarFileData, WCFServiceRegistrar>(() =>
				ServiceRegistrar.GetRegistrarFileAsync(adLogin, idFile),
				new LogShortMessage("Ошибка вызова метода. Метод: {methodName}, idFile: {idFile} adLogin: {adLogin}.",
					"GetRegistrarFileAsync", idFile, adLogin));

		/// <summary>
		/// Получить точки продаж в разрезе организаций.
		/// Можно передавать дату по-умолчанию.
		/// </summary>
		/// <param name="dateFrom">Дата с которой делается выборка по организациям</param>
		/// <param name="dateTo">Дата по которую делается выборка по организациям</param>
		/// <param name="organizations">Организации</param>
		/// <returns>Точки продаж в разрезе организаций</returns>
		[PrincipalPermission(SecurityAction.Demand, Role = @"admin")]
		public SellPoint[] GetSellPoints(DateTime dateFrom, DateTime dateTo, string[] organizations) =>
			ExecuteWithTryCatch<SellPoint[], WCFServiceRegistrar>(() =>
				ServiceRegistrar.GetSellPoints(dateFrom, dateTo, organizations),
				new LogShortMessage("Ошибка вызова метода. Метод: {methodName}, dateFrom: {dateFrom}," +
				                    " dateTo: {dateTo}, organizations: {organizations}.",
					"GetSellPoints", dateFrom, dateTo, organizations));

		/// <summary>
		/// Получить точки продаж в разрезе организаций асинхронно.
		/// Можно передавать дату по-умолчанию.
		/// </summary>
		/// <param name="dateFrom">Дата с которой делается выборка по организациям</param>
		/// <param name="dateTo">Дата по которую делается выборка по организациям</param>
		/// <param name="organizations">Организации</param>
		/// <returns>Точки продаж в разрезе организаций</returns>
		[PrincipalPermission(SecurityAction.Demand, Role = @"admin")]
		public async Task<SellPoint[]> GetSellPointsAsync(DateTime dateFrom, DateTime dateTo, string[] organizations) =>
			await ExecuteWithTryCatchAsync<SellPoint[], WCFServiceRegistrar>(() =>
				ServiceRegistrar.GetSellPointsAsync(dateFrom, dateTo, organizations),
				new LogShortMessage("Ошибка вызова метода. Метод: {methodName}, dateFrom: {dateFrom}," +
				                    " dateTo: {dateTo}, organizations: {organizations}.",
					"GetSellPointsAsync", dateFrom, dateTo, organizations));

		/// <summary>
		/// Пометить файл как удаленный.
		/// </summary>
		/// <param name="adLogin"> AD логин пользователя</param>
		/// <param name="idFile">ID пользователя</param>
		[PrincipalPermission(SecurityAction.Demand, Role = @"admin")]
		public void MarkFileAsDeleted(string adLogin, int idFile) =>
			ExecuteWithTryCatch<WCFServiceRegistrar>(() => ServiceRegistrar.MarkFileAsDeleted(adLogin, idFile),
				new LogShortMessage("Ошибка вызова метода. Метод: {methodName}, adLogin: {adLogin}, idFile: {idFile}.",
					"MarkFileAsDeleted", adLogin, idFile));

		/// <summary>
		/// Пометить файл как удаленный асинхронно.
		/// </summary>
		/// <param name="adLogin"> AD логин пользователя</param>
		/// <param name="idFile">ID пользователя</param>
		[PrincipalPermission(SecurityAction.Demand, Role = @"admin")]
		public async Task MarkFileAsDeletedAsync(string adLogin, int idFile) =>
			await ExecuteWithTryCatchAsync<WCFServiceRegistrar>(
				() => ServiceRegistrar.MarkFileAsDeletedAsync(adLogin, idFile),
				new LogShortMessage("Ошибка вызова метода. Метод: {methodName}, adLogin: {adLogin}, idFile: {idFile}.",
					"MarkFileAsDeletedAsync", adLogin, idFile));

		/// <summary>
		/// Обновить описание файла.
		/// </summary>
		/// <param name="fileDescription">Описание файла</param>
		[PrincipalPermission(SecurityAction.Demand, Role = @"admin")]
		public void UpdateFileDescription(FileDescription fileDescription) =>
			ExecuteWithTryCatch<WCFServiceRegistrar>(() => ServiceRegistrar.UpdateFileDescription(fileDescription),
				new LogShortMessage("Ошибка вызова метода. Метод: {methodName}, fileDescription: {fileDescription}.",
					"UpdateFileDescription", fileDescription));

		/// <summary>
		/// Обновить описание файла асинхронно.
		/// </summary>
		/// <param name="fileDescription">Описание файла</param>
		[PrincipalPermission(SecurityAction.Demand, Role = @"admin")]
		public async Task UpdateFileDescriptionAsync(FileDescription fileDescription) =>
			await ExecuteWithTryCatchAsync<WCFServiceRegistrar>(() =>
				ServiceRegistrar.UpdateFileDescriptionAsync(fileDescription),
				new LogShortMessage("Ошибка вызова метода. Метод: {methodName}, fileDescription: {fileDescription}.",
					"UpdateFileDescriptionAsync", fileDescription));

		/// <summary>
		/// Загрузить в архив на сервер файлы.
		/// </summary>
		/// <param name="adLogin">Логин AD пользователя, осуществляющего загрузку</param>
		/// <param name="account1CCode">Код клиента 1С, к которому привязаны документы</param>
		/// <param name="client1CCode">Код договора, к которому привязаны документы</param>
		/// <param name="idFileDescription">ID описание загружаемого файла.</param>
		/// <param name="files">Загружаемые файлы (имя, содержимое)</param>
		/// <param name="clientTimeZone">Часовой пояс на клиентской машине</param>
		[PrincipalPermission(SecurityAction.Demand, Role = @"admin")]
		public void UploadRegistrarFiles(string adLogin, string account1CCode, string client1CCode,
			int idFileDescription, Dictionary<string, byte[]> files, int clientTimeZone) =>
			
			ExecuteWithTryCatch<WCFServiceRegistrar>(() => 
				ServiceRegistrar.UploadRegistrarFiles(adLogin, account1CCode, client1CCode, idFileDescription, files,
					clientTimeZone),
				new LogShortMessage("Ошибка вызова метода. Метод: {methodName}, adLogin: {adLogin}," +
		                    " account1CCode: {account1CCode}, client1CCode: {client1CCode}," +
		                    " idFileDescription: {idFileDescription}, clientTimeZone: {clientTimeZone}, files: {files}.",
					"UploadRegistrarFilesAsync", adLogin, client1CCode, adLogin, idFileDescription,
					clientTimeZone, string.Join(", ", files?.Keys.ToArray() ?? new [] {"null"})));

		/// <summary>
		/// Загрузить в архив на сервер файлы асинхронно.
		/// </summary>
		/// <param name="adLogin">Логин AD пользователя, осуществляющего загрузку</param>
		/// <param name="account1CCode">Код клиента 1С, к которому привязаны документы</param>
		/// <param name="client1CCode">Код договора, к которому привязаны документы</param>
		/// <param name="idFileDescription">ID описание загружаемого файла.</param>
		/// <param name="files">Загружаемые файлы (имя, содержимое)</param>
		/// <param name="clientTimeZone">Часовой пояс на клиентской машине</param>
		[PrincipalPermission(SecurityAction.Demand, Role = @"admin")]
		public async Task UploadRegistrarFilesAsync(string adLogin, string account1CCode, string client1CCode,
			int idFileDescription, Dictionary<string, byte[]> files, int clientTimeZone) =>
			await ExecuteWithTryCatchAsync<WCFServiceRegistrar>(() => 
				ServiceRegistrar.UploadRegistrarFilesAsync(adLogin, account1CCode, client1CCode, idFileDescription,
					files, clientTimeZone),
				new LogShortMessage("Ошибка вызова метода. Метод: {methodName}, adLogin: {adLogin}," +
							" account1CCode: {account1CCode}, client1CCode: {client1CCode}," +
							" idFileDescription: {idFileDescription}, clientTimeZone: {clientTimeZone}, files: {files}.",
					"UploadRegistrarFilesAsync", adLogin, client1CCode, adLogin, idFileDescription,
					clientTimeZone, string.Join(", ", files?.Keys.ToArray()  ?? new [] {"null"})));
	}
}
