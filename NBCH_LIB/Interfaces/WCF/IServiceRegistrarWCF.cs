using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;
using NBCH_LIB.Models;
using NBCH_LIB.Models.Registrar;

namespace NBCH_LIB.Interfaces.WCF {
	/// <summary>
	/// Интерфейс по работе с документами
	/// </summary>
	[ServiceContract]
	public interface IServiceRegistrarWCF : IWCFContract {
		/// <summary>
		/// Получить список всех групп документов.
		/// </summary>
		/// <returns>Список групп</returns>
		[OperationContract]
		string[] GetDocumentGroups();

		/// <summary>
		/// Получить список всех групп документов асинхронно.
		/// </summary>
		/// <returns>Список групп</returns>
		[OperationContract(Name = "GetDocumentGroupsAsync")]
		Task<string[]> GetDocumentGroupsAsync();

		/// <summary>
		/// Добавить группу документов.
		/// </summary>
		/// <param name="groupName">Имя группы</param>
		[OperationContract]
		void AddDocumentGroup(string groupName);

		/// <summary>
		/// Добавить группу документов асинхронно.
		/// </summary>
		/// <param name="groupName">Имя группы</param>
		[OperationContract(Name = "AddDocumentGroupAsync")]
		Task AddDocumentGroupAsync(string groupName);

		/// <summary>
		/// Получить список документов по имени группы документов.
		/// </summary>
		/// <param name="documentsGroup">Группа документов</param>
		/// <returns>Список типов файлов в группе</returns>
		[OperationContract]
		FileDescription[] GetFilesDescriptionsByDocumentGroupName(string documentsGroup);

		/// <summary>
		/// Получить список документов по имени группы документов асинхронно.
		/// </summary>
		/// <param name="documentsGroup">Группа документов</param>
		/// <returns>Список типов файлов в группе</returns>
		[OperationContract(Name = "GetFilesDescriptionsByDocumentGroupNameAsync")]
		Task<FileDescription[]> GetFilesDescriptionsByDocumentGroupNameAsync(string documentsGroup);

		/// <summary>
		/// Добавить описание типа файла.
		/// </summary>
		/// <param name="documentsGroup">Группа файлов</param>
		/// <param name="fileDescription">Описание файла</param>
		/// <returns>Новая запись</returns>
		[OperationContract]
		FileDescription AddFileDescription(string documentsGroup, FileDescription fileDescription);

		/// <summary>
		/// Добавить описание типа файла асинхронно.
		/// </summary>
		/// <param name="documentsGroup">Группа файлов</param>
		/// <param name="fileDescription">Описание файла</param>
		/// <returns>Новая запись</returns>
		[OperationContract(Name = "AddFileDescriptionAsync")]
		Task<FileDescription> AddFileDescriptionAsync(string documentsGroup, FileDescription fileDescription);

		/// <summary>
		/// Обновить описание файла.
		/// </summary>
		/// <param name="fileDescription">Описание файла</param>
		[OperationContract]
		void UpdateFileDescription(FileDescription fileDescription);

		/// <summary>
		/// Обновить описание файла асинхронно.
		/// </summary>
		/// <param name="fileDescription">Описание файла</param>
		[OperationContract(Name = "UpdateFileDescriptionAsync")]
		Task UpdateFileDescriptionAsync(FileDescription fileDescription);

		/// <summary>
		/// Получить список документов по номеру договора 1С и списку клиентов из договора.
		/// </summary>
		/// <param name="adLogin">Логин пользователя, кто запрашивает документы</param>
		/// <param name="account1CCode">Код договора 1С</param>
		/// <param name="users">Клиенты</param>
		/// <returns>Список документов по договору клиента</returns>
		[OperationContract]
		Dictionary<Client, RegistrarDocument[]> GetDocumentsByAccountAndClients1C(string adLogin, string account1CCode,
			Client[] users);

		/// <summary>
		/// Получить список документов по номеру договора 1С и списку клиентов из договора асинхронно.
		/// </summary>
		/// <param name="adLogin">Логин пользователя, кто запрашивает документы</param>
		/// <param name="account1CCode">Код договора 1С</param>
		/// <param name="users">Клиенты</param>
		/// <returns>Список документов по договору клиента</returns>
		[OperationContract(Name = "GetDocumentsByAccountAndClients1CAsync")]
		Task<Dictionary<Client, RegistrarDocument[]>> GetDocumentsByAccountAndClients1CAsync(string adLogin,
			string account1CCode, Client[] users);

		/// <summary>
		/// Получить список файлов по номеру договора 1С, клиенту из договора и коду документа.
		/// </summary>
		/// <param name="adLogin">Логин пользователя, кто запрашивает документы</param>
		/// <param name="account1CCode">Код договора 1С</param>
		/// <param name="user">Клиент</param>
		/// <param name="documentID">Код документа</param>
		/// <returns>Список файлов по договору клиента и номеру документа</returns>
		[OperationContract]
		RegistrarDocument GetDocumentsByAccountAndClients1CAndDocumentID(string adLogin, string account1CCode,
			Client user, int documentID);

		/// <summary>
		/// Получить список файлов по номеру договора 1С, клиенту из договора и коду документа асинхронно.
		/// </summary>
		/// <param name="adLogin">Логин пользователя, кто запрашивает документы</param>
		/// <param name="account1CCode">Код договора 1С</param>
		/// <param name="user">Клиент</param>
		/// <param name="documentID">Код документа</param>
		/// <returns>Список файлов по договору клиента и номеру документа</returns>
		[OperationContract(Name = "GetDocumentsByAccountAndClients1CAndDocumentIDAsync")]
		Task<RegistrarDocument> GetDocumentsByAccountAndClients1CAndDocumentIDAsync(string adLogin, string account1CCode,
			Client user, int documentID);

		/// <summary>
		/// Загрузить в архив на сервер файлы.
		/// </summary>
		/// <param name="adLogin">Логин AD пользователя, осуществляющего загрузку</param>
		/// <param name="account1CCode">Код клиента 1С, к которому привязаны документы</param>
		/// <param name="client1CCode">Код договора, к которому привязаны документы</param>
		/// <param name="idFileDescription">ID описание загружаемого файла.</param>
		/// <param name="files">Загружаемые файлы (имя, содержимое)</param>
		/// <param name="clientTimeZone">Часовой пояс на клиентской машине</param>
		[OperationContract]
		void UploadRegistrarFiles(string adLogin, string account1CCode, string client1CCode, int idFileDescription,
			Dictionary<string, byte[]> files, int clientTimeZone);

		/// <summary>
		/// Загрузить в архив на сервер файлы асинхронно.
		/// </summary>
		/// <param name="adLogin">Логин AD пользователя, осуществляющего загрузку</param>
		/// <param name="account1CCode">Код клиента 1С, к которому привязаны документы</param>
		/// <param name="client1CCode">Код договора, к которому привязаны документы</param>
		/// <param name="idFileDescription">ID описание загружаемого файла.</param>
		/// <param name="files">Загружаемые файлы (имя, содержимое)</param>
		/// <param name="clientTimeZone">Часовой пояс на клиентской машине</param>
		[OperationContract(Name = "UploadRegistrarFilesAsync")]
		Task UploadRegistrarFilesAsync(string adLogin, string account1CCode, string client1CCode, int idFileDescription,
			Dictionary<string, byte[]> files, int clientTimeZone);

		/// <summary>
		/// Получить файл из базы по его ID.
		/// </summary>
		/// <param name="adLogin">AD логин пользователя, запрашивающего файл</param>
		/// <param name="idFile">ID файла</param>
		/// <returns>Имя и содержимое файла</returns>
		[OperationContract]
		RegistrarFileData GetRegistrarFile(string adLogin, int idFile);

		/// <summary>
		/// Получить файл из базы по его ID асинхронно.
		/// </summary>
		/// <param name="adLogin">AD логин пользователя, запрашивающего файл</param>
		/// <param name="idFile">ID файла</param>
		/// <returns>Имя и содержимое файла</returns>
		[OperationContract(Name = "GetRegistrarFileAsync")]
		Task<RegistrarFileData> GetRegistrarFileAsync(string adLogin, int idFile);

		/// <summary>
		/// Пометить файл как удаленный.
		/// </summary>
		/// <param name="adLogin"> AD логин пользователя</param>
		/// <param name="idFile">ID пользователя</param>
		[OperationContract]
		void MarkFileAsDeleted(string adLogin, int idFile);

		/// <summary>
		/// Пометить файл как удаленный асинхронно.
		/// </summary>
		/// <param name="adLogin"> AD логин пользователя</param>
		/// <param name="idFile">ID пользователя</param>
		[OperationContract(Name = "MarkFileAsDeletedAsync")]
		Task MarkFileAsDeletedAsync(string adLogin, int idFile);

		/// <summary>
		/// Получить точки продаж в разрезе организаций.
		/// Можно передавать дату по-умолчанию.
		/// </summary>
		/// <param name="dateFrom">Дата с которой делается выборка по организациям</param>
		/// <param name="dateTo">Дата по которую делается выборка по организациям</param>
		/// <param name="organizations">Организации</param>
		/// <returns>Точки продаж в разрезе организаций</returns>
		[OperationContract]
		SellPoint[] GetSellPoints(DateTime dateFrom, DateTime dateTo, string[] organizations);

		/// <summary>
		/// Получить точки продаж в разрезе организаций асинхронно.
		/// Можно передавать дату по-умолчанию.
		/// </summary>
		/// <param name="dateFrom">Дата с которой делается выборка по организациям</param>
		/// <param name="dateTo">Дата по которую делается выборка по организациям</param>
		/// <param name="organizations">Организации</param>
		/// <returns>Точки продаж в разрезе организаций</returns>
		[OperationContract(Name = "GetSellPointsAsync")]
		Task<SellPoint[]> GetSellPointsAsync(DateTime dateFrom, DateTime dateTo, string[] organizations);

		/// <summary>
		/// Получить информацию по загруженным документам.
		/// </summary>
		/// <param name="accounts">Список договоров</param>
		/// <returns>Информация по загруженным документам</returns>
		[OperationContract]
		AccountsForCheck[] GetAccountsInfoForCheckDocuments(string[] accounts);

		/// <summary>
		/// Получить информацию по загруженным документам асинхронно.
		/// </summary>
		/// <param name="accounts">Список договоров</param>
		/// <returns>Информация по загруженным документам</returns>
		[OperationContract(Name = "GetAccountsInfoForCheckDocumentsAsync")]
		Task<AccountsForCheck[]> GetAccountsInfoForCheckDocumentsAsync(string[] accounts);

		/// <summary>
		/// Получить список фотографий клиента.
		/// </summary>
		/// <param name="client1CCode">Код клиента 1С</param>
		/// <param name="excludeAccount">Исключить фотографию из выбранного договора</param>
		/// <param name="adLogin">Пользователь, запрашивающий список</param>
		/// <returns>Список - ID файла и дата загрузки</returns>
		[OperationContract]
		Dictionary<int, DateTime> GetPhotoList(string client1CCode, string excludeAccount, string adLogin);

		/// <summary>
		/// Получить список фотографий клиента асинхронно.
		/// </summary>
		/// <param name="client1CCode">Код клиента 1С</param>
		/// <param name="excludeAccount">Исключить фотографию из выбранного договора</param>
		/// <param name="adLogin">Пользователь, запрашивающий список</param>
		/// <returns>Список - ID файла и дата загрузки</returns>
		[OperationContract(Name = "GetPhotoListAsync")]
		Task<Dictionary<int, DateTime>> GetPhotoListAsync(string client1CCode, string excludeAccount, string adLogin);
	}
}
