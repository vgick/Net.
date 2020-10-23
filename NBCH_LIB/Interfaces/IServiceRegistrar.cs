using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NBCH_LIB.Models;
using NBCH_LIB.Models.Registrar;

namespace NBCH_LIB.Interfaces {
	/// <summary>
	/// Интерфейс по работе с документами
	/// </summary>
	public interface IServiceRegistrar {
		/// <summary>
		/// Получить список всех групп документов.
		/// </summary>
		/// <returns>Список групп</returns>
		string[] GetDocumentGroups();

		/// <summary>
		/// Получить список всех групп документов асинхронно.
		/// </summary>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns>Список групп</returns>
		Task<string[]> GetDocumentGroupsAsync(CancellationToken cancellationToken);

		/// <summary>
		/// Добавить группу документов.
		/// </summary>
		/// <param name="groupName">Имя группы</param>
		void AddDocumentGroup(string groupName);

		/// <summary>
		/// Добавить группу документов асинхронно.
		/// </summary>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <param name="groupName">Имя группы</param>
		Task AddDocumentGroupAsync(string groupName, CancellationToken cancellationToken);

		/// <summary>
		/// Получить список документов по имени группы документов.
		/// </summary>
		/// <param name="documentsGroup">Группа документов</param>
		/// <returns>Список типов файлов в группе</returns>
		FileDescription[] GetFilesDescriptionsByDocumentGroupName(string documentsGroup);

		/// <summary>
		/// Получить список документов по имени группы документов асинхронно.
		/// </summary>
		/// <param name="documentsGroup">Группа документов</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns>Список типов файлов в группе</returns>
		Task<FileDescription[]> GetFilesDescriptionsByDocumentGroupNameAsync(string documentsGroup,
			CancellationToken cancellationToken);

		/// <summary>
		/// Добавить описание типа файла.
		/// </summary>
		/// <param name="documentsGroup">Группа файлов</param>
		/// <param name="fileDescription">Описание файла</param>
		/// <returns>Новая запись</returns>
		FileDescription AddFileDescription(string documentsGroup, FileDescription fileDescription);

		/// <summary>
		/// Добавить описание типа файла асинхронно.
		/// </summary>
		/// <param name="documentsGroup">Группа файлов</param>
		/// <param name="fileDescription">Описание файла</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns>Новая запись</returns>
		Task<FileDescription> AddFileDescriptionAsync(string documentsGroup, FileDescription fileDescription,
			CancellationToken cancellationToken);

		/// <summary>
		/// Обновить описание файла.
		/// </summary>
		/// <param name="fileDescription">Описание файла</param>
		void UpdateFileDescription(FileDescription fileDescription);

		/// <summary>
		/// Обновить описание файла асинхронно.
		/// </summary>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <param name="fileDescription">Описание файла</param>
		Task UpdateFileDescriptionAsync(FileDescription fileDescription, CancellationToken cancellationToken);

		/// <summary>
		/// Получить список документов по номеру договора 1С и списку клиентов из договора.
		/// </summary>
		/// <param name="adLogin">Логин пользователя, кто запрашивает документы</param>
		/// <param name="account1CCode">Код договора 1С</param>
		/// <param name="users">Клиенты</param>
		/// <returns>Список документов по договору клиента</returns>
		Dictionary<Client, RegistrarDocument[]> GetDocumentsByAccountAndClients1C(string adLogin, string account1CCode,
			Client[] users);

		/// <summary>
		/// Получить список документов по номеру договора 1С и списку клиентов из договора асинхронно.
		/// </summary>
		/// <param name="adLogin">Логин пользователя, кто запрашивает документы</param>
		/// <param name="account1CCode">Код договора 1С</param>
		/// <param name="users">Клиенты</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns>Список документов по договору клиента</returns>
		Task<Dictionary<Client, RegistrarDocument[]>> GetDocumentsByAccountAndClients1CAsync(string adLogin,
			string account1CCode, Client[] users, CancellationToken cancellationToken);

		/// <summary>
		/// Получить список файлов по номеру договора 1С, клиенту из договора и коду документа.
		/// </summary>
		/// <param name="adLogin">Логин пользователя, кто запрашивает документы</param>
		/// <param name="account1CCode">Код договора 1С</param>
		/// <param name="user">Клиент</param>
		/// <param name="documentID">Код документа</param>
		/// <returns>Список файлов по договору клиента и номеру документа</returns>
		RegistrarDocument GetDocumentsByAccountAndClients1CAndDocumentID(string adLogin, string account1CCode,
			Client user, int documentID);

		/// <summary>
		/// Получить список файлов по номеру договора 1С, клиенту из договора и коду документа асинхронно.
		/// </summary>
		/// <param name="adLogin">Логин пользователя, кто запрашивает документы</param>
		/// <param name="account1CCode">Код договора 1С</param>
		/// <param name="user">Клиент</param>
		/// <param name="documentID">Код документа</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns>Список файлов по договору клиента и номеру документа</returns>
		Task<RegistrarDocument> GetDocumentsByAccountAndClients1CAndDocumentIDAsync(string adLogin, string account1CCode,
			Client user, int documentID, CancellationToken cancellationToken);

		/// <summary>
		/// Загрузить в архив на сервер файлы.
		/// </summary>
		/// <param name="adLogin">Логин AD пользователя, осуществляющего загрузку</param>
		/// <param name="account1CCode">Код клиента 1С, к которому привязаны документы</param>
		/// <param name="client1CCode">Код договора, к которому привязаны документы</param>
		/// <param name="idFileDescription">ID описание загружаемого файла.</param>
		/// <param name="files">Загружаемые файлы (имя, содержимое)</param>
		/// <param name="clientTimeZone">Часовой пояс на клиентской машине</param>
		void UploadRegistrarFiles(string adLogin, string account1CCode, string client1CCode, int idFileDescription, Dictionary<string, byte[]> files, int clientTimeZone);

		/// <summary>
		/// Загрузить в архив на сервер файлы асинхронно.
		/// </summary>
		/// <param name="adLogin">Логин AD пользователя, осуществляющего загрузку</param>
		/// <param name="account1CCode">Код клиента 1С, к которому привязаны документы</param>
		/// <param name="client1CCode">Код договора, к которому привязаны документы</param>
		/// <param name="idFileDescription">ID описание загружаемого файла.</param>
		/// <param name="files">Загружаемые файлы (имя, содержимое)</param>
		/// <param name="clientTimeZone">Часовой пояс на клиентской машине</param>
		/// <param name="cancellationToken">Токен отмены</param>
		Task UploadRegistrarFilesAsync(string adLogin, string account1CCode, string client1CCode, int idFileDescription,
			Dictionary<string, byte[]> files, int clientTimeZone, CancellationToken cancellationToken);

		/// <summary>
		/// Получить файл из базы по его ID.
		/// </summary>
		/// <param name="adLogin">AD логин пользователя, запрашивающего файл</param>
		/// <param name="idFile">ID файла</param>
		/// <returns>Имя и содержимое файла</returns>
		RegistrarFileData GetRegistrarFile(string adLogin, int idFile);

		/// <summary>
		/// Получить файл из базы по его ID асинхронно.
		/// </summary>
		/// <param name="adLogin">AD логин пользователя, запрашивающего файл</param>
		/// <param name="idFile">ID файла</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns>Имя и содержимое файла</returns>
		Task<RegistrarFileData> GetRegistrarFileAsync(string adLogin, int idFile, CancellationToken cancellationToken);

		/// <summary>
		/// Пометить файл как удаленный.
		/// </summary>
		/// <param name="adLogin"> AD логин пользователя</param>
		/// <param name="idFile">ID пользователя</param>
		void MarkFileAsDeleted(string adLogin, int idFile);

		/// <summary>
		/// Пометить файл как удаленный асинхронно.
		/// </summary>
		/// <param name="adLogin"> AD логин пользователя</param>
		/// <param name="idFile">ID пользователя</param>
		/// <param name="cancellationToken">Токен отмены</param>
		Task MarkFileAsDeletedAsync(string adLogin, int idFile, CancellationToken cancellationToken);

		/// <summary>
		/// Получить точки продаж в разрезе организаций.
		/// Можно передавать дату по-умолчанию.
		/// </summary>
		/// <param name="dateFrom">Дата с которой делается выборка по организациям</param>
		/// <param name="dateTo">Дата по которую делается выборка по организациям</param>
		/// <param name="organizations">Организации</param>
		/// <returns>Точки продаж в разрезе организаций</returns>
		SellPoint[] GetSellPoints(DateTime dateFrom, DateTime dateTo, string[] organizations);

		/// <summary>
		/// Получить точки продаж в разрезе организаций асинхронно.
		/// Можно передавать дату по-умолчанию.
		/// </summary>
		/// <param name="dateFrom">Дата с которой делается выборка по организациям</param>
		/// <param name="dateTo">Дата по которую делается выборка по организациям</param>
		/// <param name="organizations">Организации</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns>Точки продаж в разрезе организаций</returns>
		Task<SellPoint[]> GetSellPointsAsync(DateTime dateFrom, DateTime dateTo, string[] organizations,
			CancellationToken cancellationToken);

		/// <summary>
		/// Получить информацию по загруженным документам.
		/// </summary>
		/// <param name="accounts">Список договоров</param>
		/// <returns>Информация по загруженным документам</returns>
		AccountsForCheck[] GetAccountsInfoForCheckDocuments(string[] accounts);

		/// <summary>
		/// Получить информацию по загруженным документам асинхронно.
		/// </summary>
		/// <param name="accounts">Список договоров</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns>Информация по загруженным документам</returns>
		Task<AccountsForCheck[]> GetAccountsInfoForCheckDocumentsAsync(string[] accounts, CancellationToken cancellationToken);

		/// <summary>
		/// Получить список фотографий клиента.
		/// </summary>
		/// <param name="client1CCode">Код клиента 1С</param>
		/// <param name="excludeAccount">Исключить фотографию из выбранного договора</param>
		/// <param name="adLogin">Пользователь, запрашивающий список</param>
		/// <returns>Список - ID файла и дата загрузки</returns>
		Dictionary<int, DateTime> GetPhotoList(string client1CCode, string excludeAccount, string adLogin);

		/// <summary>
		/// Получить список фотографий клиента асинхронно.
		/// </summary>
		/// <param name="client1CCode">Код клиента 1С</param>
		/// <param name="excludeAccount">Исключить фотографию из выбранного договора</param>
		/// <param name="adLogin">Пользователь, запрашивающий список</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns>Список - ID файла и дата загрузки</returns>
		Task<Dictionary<int, DateTime>> GetPhotoListAsync(string client1CCode, string excludeAccount, string adLogin,
			CancellationToken cancellationToken);
	}
}
