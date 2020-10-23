using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading;
using System.Threading.Tasks;
using NBCH_LIB.Interfaces;
using NBCH_LIB.Interfaces.WCF;
using NBCH_LIB.Models;
using NBCH_LIB.Models.Registrar;

namespace NBCH_LIB.WCFProxy {
	/// <summary>
	/// Прокси класс для работы архивом документов
	/// </summary>
	public class ServiceRegistrarProxy : ClientBase<IServiceRegistrarWCF>, IServiceRegistrarWCF {
		#region Конструкторы
		public ServiceRegistrarProxy() { }
		public ServiceRegistrarProxy(string endpointName) : base(endpointName) { }
		public ServiceRegistrarProxy(Binding binding, EndpointAddress remoteAddress) : base(binding, remoteAddress) { }
		#endregion

		/// <summary>
		/// Получить список всех групп документов
		/// </summary>
		public string[] GetDocumentGroups() => Channel.GetDocumentGroups();

		/// <summary>
		/// Получить список всех групп документов асинхронно
		/// </summary>
		public async Task<string[]> GetDocumentGroupsAsync() =>
			await Channel.GetDocumentGroupsAsync();

		/// <summary>
		/// Получить список документов по имени группы документов
		/// </summary>
		/// <param name="documentsGroup">Группа документов</param>
		/// <returns>Список типов файлов в группе</returns>
		public FileDescription[] GetFilesDescriptionsByDocumentGroupName(string documentsGroup) =>
			Channel.GetFilesDescriptionsByDocumentGroupName(documentsGroup);

		/// <summary>
		/// Получить список документов по имени группы документов асинхронно
		/// </summary>
		/// <param name="documentsGroup">Группа документов</param>
		/// <returns>Список типов файлов в группе</returns>
		public async Task<FileDescription[]> GetFilesDescriptionsByDocumentGroupNameAsync(string documentsGroup) =>
			await Channel.GetFilesDescriptionsByDocumentGroupNameAsync(documentsGroup);

		/// <summary>
		/// Добавить группу документов
		/// </summary>
		public void AddDocumentGroup(string groupName)  => Channel.AddDocumentGroup(groupName);

		/// <summary>
		/// Добавить группу документов асинхронно
		/// </summary>
		public async Task AddDocumentGroupAsync(string groupName) => await Channel.AddDocumentGroupAsync(groupName);

		/// <summary>
		/// Добавить описание типа файла
		/// </summary>
		/// <param name="documentsGroup">Группа файлов</param>
		/// <param name="fileDescription">Описание файла</param>
		/// <returns>Новая запись</returns>
		public FileDescription AddFileDescription(string documentsGroup, FileDescription fileDescription) =>
			Channel.AddFileDescription(documentsGroup, fileDescription);

		/// <summary>
		/// Добавить описание типа файла асинхронно
		/// </summary>
		/// <param name="documentsGroup">Группа файлов</param>
		/// <param name="fileDescription">Описание файла</param>
		/// <returns>Новая запись</returns>
		public async Task<FileDescription> AddFileDescriptionAsync(string documentsGroup, FileDescription fileDescription) => 
			await Channel.AddFileDescriptionAsync(documentsGroup, fileDescription);

		/// <summary>
		/// Обновить описание файла
		/// </summary>
		/// <param name="fileDescription">Описание файла</param>
		public void UpdateFileDescription(FileDescription fileDescription) => Channel.UpdateFileDescription(fileDescription);

		/// <summary>
		/// Обновить описание файла асинхронно
		/// </summary>
		/// <param name="fileDescription">Описание файла</param>
		public async Task UpdateFileDescriptionAsync(FileDescription fileDescription) =>
			await Channel.UpdateFileDescriptionAsync(fileDescription);

		/// <summary>
		/// Получить список документов по номеру договора 1С и списку клиентов из договора
		/// </summary>
		/// <param name="adLogin">Логин пользователя, кто запрашивает документы</param>
		/// <param name="account1CCode">Код договора 1С</param>
		/// <param name="users">Клиенты</param>
		/// <returns>Список документов по договору клиента</returns>
		public Dictionary<Client, RegistrarDocument[]> GetDocumentsByAccountAndClients1C(string adLogin, string account1CCode,
			Client[] users) => Channel.GetDocumentsByAccountAndClients1C(adLogin, account1CCode, users);

		/// <summary>
		/// Получить список документов по номеру договора 1С и списку клиентов из договора асинхронно
		/// </summary>
		/// <param name="adLogin">Логин пользователя, кто запрашивает документы</param>
		/// <param name="account1CCode">Код договора 1С</param>
		/// <param name="users">Клиенты</param>
		/// <returns>Список документов по договору клиента</returns>
		public async Task<Dictionary<Client, RegistrarDocument[]>> GetDocumentsByAccountAndClients1CAsync(string adLogin,
			string account1CCode, Client[] users) =>
			await Channel.GetDocumentsByAccountAndClients1CAsync(adLogin, account1CCode, users);

		/// <summary>
		/// Загрузить в архив на сервер файлы.
		/// </summary>
		/// <param name="adLogin">Логин AD пользователя, осуществляющего загрузку</param>
		/// <param name="account1CCode">Код клиента 1С, к которому привязаны документы</param>
		/// <param name="client1CCode">Код договора, к которому привязаны документы</param>
		/// <param name="idFileDescription">Описание загружаемого файла.</param>
		/// <param name="files">Загружаемые файлы (имя, содержимое)</param>
		/// <param name="clientTimeZone">Часовой пояс на клиентской машине</param>
		public void UploadRegistrarFiles(string adLogin, string account1CCode, string client1CCode, int idFileDescription,
			Dictionary<string, byte[]> files, int clientTimeZone) =>
			Channel.UploadRegistrarFiles(adLogin, account1CCode, client1CCode, idFileDescription, files, clientTimeZone);

		/// <summary>
		/// Загрузить в архив на сервер файлы асинхронно.
		/// </summary>
		/// <param name="adLogin">Логин AD пользователя, осуществляющего загрузку</param>
		/// <param name="account1CCode">Код клиента 1С, к которому привязаны документы</param>
		/// <param name="client1CCode">Код договора, к которому привязаны документы</param>
		/// <param name="idFileDescription">Описание загружаемого файла.</param>
		/// <param name="files">Загружаемые файлы (имя, содержимое)</param>
		/// <param name="clientTimeZone">Часовой пояс на клиентской машине</param>
		public async Task UploadRegistrarFilesAsync(string adLogin, string account1CCode, string client1CCode,
			int idFileDescription, Dictionary<string, byte[]> files, int clientTimeZone) =>
			await Channel.UploadRegistrarFilesAsync(adLogin, account1CCode, client1CCode, idFileDescription,
				files, clientTimeZone);

		/// <summary>
		/// Получить файл из базы по его ID
		/// </summary>
		/// <param name="adLogin">AD логин пользователя, запрашивающего файл</param>
		/// <param name="idFile">ID файла</param>
		/// <returns>Имя и содержимое файла</returns>
		public RegistrarFileData GetRegistrarFile(string adLogin, int idFile) => Channel.GetRegistrarFile(adLogin, idFile);

		/// <summary>
		/// Получить файл из базы по его ID асинхронно.
		/// </summary>
		/// <param name="adLogin">AD логин пользователя, запрашивающего файл</param>
		/// <param name="idFile">ID файла</param>
		/// <returns>Имя и содержимое файла</returns>
		public async Task<RegistrarFileData> GetRegistrarFileAsync(string adLogin, int idFile) =>
			await Channel.GetRegistrarFileAsync(adLogin, idFile);

		/// <summary>
		/// Пометить файл как удаленный
		/// </summary>
		/// <param name="adLogin"> AD логин пользователя</param>
		/// <param name="idFile">ID пользователя</param>
		public void MarkFileAsDeleted(string adLogin, int idFile) => Channel.MarkFileAsDeleted(adLogin, idFile);

		/// <summary>
		/// Пометить файл как удаленный асинхронно.
		/// </summary>
		/// <param name="adLogin"> AD логин пользователя</param>
		/// <param name="idFile">ID пользователя</param>
		public async Task MarkFileAsDeletedAsync(string adLogin, int idFile) =>
			await Channel.MarkFileAsDeletedAsync(adLogin, idFile);

		/// <summary>
		/// Получить точки продаж в разрезе организаций
		/// Можно передавать дату по-умолчанию
		/// </summary>
		/// <param name="dateFrom">Дата с которой делается выборка по организациям</param>
		/// <param name="dateTo">Дата по которую делается выборка по организациям</param>
		/// <param name="organizations">Организации</param>
		/// <returns>Точки продаж в разрезе организаций</returns>
		public SellPoint[] GetSellPoints(DateTime dateFrom, DateTime dateTo, string[] organizations) =>
			Channel.GetSellPoints(dateFrom, dateTo, organizations);

		/// <summary>
		/// Получить точки продаж в разрезе организаций асинхронно.
		/// Можно передавать дату по-умолчанию
		/// </summary>
		/// <param name="dateFrom">Дата с которой делается выборка по организациям</param>
		/// <param name="dateTo">Дата по которую делается выборка по организациям</param>
		/// <param name="organizations">Организации</param>
		/// <returns>Точки продаж в разрезе организаций</returns>
		public async Task<SellPoint[]> GetSellPointsAsync(DateTime dateFrom, DateTime dateTo, string[] organizations) =>
			await Channel.GetSellPointsAsync(dateFrom, dateTo, organizations);

		/// <summary>
		/// Получить информацию по загруженным документам
		/// </summary>
		/// <param name="accounts">Список договоров</param>
		/// <returns>Информация по загруженным документам</returns>
		public AccountsForCheck[] GetAccountsInfoForCheckDocuments(string[] accounts) =>
			Channel.GetAccountsInfoForCheckDocuments(accounts);

		/// <summary>
		/// Получить информацию по загруженным документам асинхронно.
		/// </summary>
		/// <param name="accounts">Список договоров</param>
		/// <returns>Информация по загруженным документам</returns>
		public async Task<AccountsForCheck[]> GetAccountsInfoForCheckDocumentsAsync(string[] accounts) =>
			await Channel.GetAccountsInfoForCheckDocumentsAsync(accounts);

		/// <summary>
		/// Получить список файлов по номеру договора 1С, клиенту из договора и коду документа
		/// </summary>
		/// <param name="adLogin">Логин пользователя, кто запрашивает документы</param>
		/// <param name="account1CCode">Код договора 1С</param>
		/// <param name="user">Клиент</param>
		/// <param name="documentID">Код документа</param>
		/// <returns>Список файлов по договору клиента и номеру документа</returns>
		public RegistrarDocument GetDocumentsByAccountAndClients1CAndDocumentID(string adLogin, string account1CCode, Client user, int documentID) =>
			Channel.GetDocumentsByAccountAndClients1CAndDocumentID(adLogin, account1CCode, user, documentID);

		/// <summary>
		/// Получить список файлов по номеру договора 1С, клиенту из договора и коду документа асинхронно.
		/// </summary>
		/// <param name="adLogin">Логин пользователя, кто запрашивает документы</param>
		/// <param name="account1CCode">Код договора 1С</param>
		/// <param name="user">Клиент</param>
		/// <param name="documentID">Код документа</param>
		/// <returns>Список файлов по договору клиента и номеру документа</returns>
		public async Task<RegistrarDocument> GetDocumentsByAccountAndClients1CAndDocumentIDAsync(string adLogin,
			string account1CCode, Client user, int documentID) =>
			await Channel.GetDocumentsByAccountAndClients1CAndDocumentIDAsync(adLogin, account1CCode, user,
				documentID);

		/// <summary>
		/// Получить список фотографий клиента
		/// </summary>
		/// <param name="client1CCode">Код клиента 1С</param>
		/// <param name="excludeAccount">Исключить фотографию из выбранного договора</param>
		/// <param name="adLogin">Пользователь, запрашивающий список</param>
		/// <returns>Список - ID файла и дата загрузки</returns>
		public Dictionary<int, DateTime> GetPhotoList(string client1CCode, string excludeAccount, string adLogin) =>
			Channel.GetPhotoList(client1CCode, excludeAccount, adLogin);

		/// <summary>
		/// Получить список фотографий клиента асинхронно.
		/// </summary>
		/// <param name="client1CCode">Код клиента 1С</param>
		/// <param name="excludeAccount">Исключить фотографию из выбранного договора</param>
		/// <param name="adLogin">Пользователь, запрашивающий список</param>
		/// <returns>Список - ID файла и дата загрузки</returns>
		public async Task<Dictionary<int, DateTime>> GetPhotoListAsync(string client1CCode, string excludeAccount,
			string adLogin) =>
			await Channel.GetPhotoListAsync(client1CCode, excludeAccount, adLogin);
	}
}
