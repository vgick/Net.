using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading;
using System.Threading.Tasks;
using NBCH_LIB.Interfaces;
using NBCH_LIB.Interfaces.WCF;
using NBCH_LIB.Models;
using NBCH_LIB.SOAP.SOAPNBCH;

namespace NBCH_LIB.WCFProxy {
	/// <summary>
	/// Прокси класс для работы с WCF службой анкет НБКИ
	/// </summary>
	public class ServiceNBCHProxy : ClientBase<IServiceNBCHWCF>, IServiceNBCHWCF {
		#region Конструкторы
		public ServiceNBCHProxy() { }
		public ServiceNBCHProxy(string endpointName) : base(endpointName) { }
		public ServiceNBCHProxy(Binding binding, EndpointAddress remoteAddress) : base(binding, remoteAddress) { }
		#endregion
		/// <summary>
		/// Получить КИ по ID.
		/// </summary>
		/// <param name="creditHistoryID"></param>
		/// <returns>Кредитная история</returns>
		public Report GetSavedReport(int creditHistoryID) => Channel.GetSavedReport(creditHistoryID);

		/// <summary>
		/// Получить КИ по ID асинхронно.
		/// </summary>
		/// <param name="creditHistoryID"></param>
		/// <returns>Кредитная история</returns>
		public async Task<Report> GetSavedReportAsync(int creditHistoryID) =>
			await Channel.GetSavedReportAsync(creditHistoryID);

		/// <summary>
		/// Получить список кредитных историй.
		/// </summary>
		/// <param name="client1CCode">Код клиента 1С</param>
		/// <returns>Список кредитных анкет</returns>
		public CreditHistoryInfo[] GetCreditHistoryList(string client1CCode) => Channel.GetCreditHistoryList(client1CCode);

		/// <summary>
		/// Получить список кредитных историй асинхронно.
		/// </summary>
		/// <param name="client1CCode">Код клиента 1С</param>
		/// <returns>Список кредитных анкет</returns>
		public async Task<CreditHistoryInfo[]> GetCreditHistoryListAsync(string client1CCode) =>
			await Channel.GetCreditHistoryListAsync(client1CCode);

		/// <summary>
		/// Поиск клиента в базе.
		/// </summary>
		/// <param name="fio">часть имени клиента</param>
		/// <returns>Совпадения</returns>
		public SearchClientList[] SearchClient(string fio) => Channel.SearchClient(fio);

		/// <summary>
		/// Поиск клиента в базе асинхронно.
		/// </summary>
		/// <param name="fio">часть имени клиента</param>
		/// <returns>Совпадения</returns>
		public async Task<SearchClientList[]> SearchClientAsync(string fio) => await Channel.SearchClientAsync(fio);

		/// <summary>
		/// Получить список сохраненных КИ клиента по коду анкеты.
		/// </summary>
		/// <param name="creditHistoryID">Код анкеты в базе</param>
		/// <returns>Список сохраненных КИ</returns>
		public CreditHistoryInfo[] GetCreditHistoryListByCreditHistoryID(int creditHistoryID) =>
			Channel.GetCreditHistoryListByCreditHistoryID(creditHistoryID);

		/// <summary>
		/// Получить список сохраненных КИ клиента по коду анкеты асинхронно.
		/// </summary>
		/// <param name="creditHistoryID">Код анкеты в базе</param>
		/// <returns>Список сохраненных КИ</returns>
		public async Task<CreditHistoryInfo[]> GetCreditHistoryListByCreditHistoryIDAsync(int creditHistoryID) =>
			await Channel.GetCreditHistoryListByCreditHistoryIDAsync(creditHistoryID);

		/// <summary>
		/// Вернуть номер последней, сохраненной анкеты.
		/// </summary>
		/// <param name="client1CCode"></param>
		/// <returns>ID кредитной истории</returns>
		public int GetClientCreditHistoryID(string client1CCode) => Channel.GetClientCreditHistoryID(client1CCode);

		/// <summary>
		/// Вернуть номер последней, сохраненной анкеты асинхронно.
		/// </summary>
		/// <param name="client1CCode"></param>
		/// <returns>ID кредитной истории</returns>
		public async Task<int> GetClientCreditHistoryIDAsync(string client1CCode) =>
			await Channel.GetClientCreditHistoryIDAsync(client1CCode);

		/// <summary>
		/// Сохранить кредитную историю асинхронно.
		/// </summary>
		/// <param name="account1CCode">Номер кредитного договора в 1С</param>
		/// <param name="client1CCode">Код клиента 1С</param>
		/// <param name="signedXml">Подписанные НБКИ данные</param>
		/// <param name="unSignedXml">Анкета НБКИ без подписи</param>
		/// <param name="clientTimeZone">Часовой пояс на клиенте</param>
		/// <param name="error">Ошибка из сервиса НБКИ</param>
		public async Task WriteCreditHistoryAsync(string account1CCode, string client1CCode, byte[] signedXml,
			byte[] unSignedXml, int clientTimeZone, CTErr error) =>
			await Channel.WriteCreditHistoryAsync(account1CCode, client1CCode, signedXml, unSignedXml,
				clientTimeZone, error);
	}
}
