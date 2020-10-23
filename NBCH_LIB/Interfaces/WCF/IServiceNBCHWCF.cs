using System.ServiceModel;
using System.Threading.Tasks;
using NBCH_LIB.Models;
using NBCH_LIB.SOAP.SOAPNBCH;

namespace NBCH_LIB.Interfaces.WCF {
	[ServiceContract]
	public interface IServiceNBCHWCF : IWCFContract, IServiceNBCHBase {
		/// <summary>
		/// Получить КИ по ID асинхронно.
		/// </summary>
		/// <param name="creditHistoryID"></param>
		/// <returns>Кредитная история</returns>
		[OperationContract(Name = "GetSavedReportAsync")]
		Task<Report> GetSavedReportAsync(int creditHistoryID);

		/// <summary>
		/// Получить список кредитных историй по коду клиента асинхронно.
		/// </summary>
		/// <param name="client1CCode">Код клиента 1С</param>
		/// <returns>Список кредитных анкет</returns>
		[OperationContract(Name = "GetCreditHistoryListAsync")]
		Task<CreditHistoryInfo[]> GetCreditHistoryListAsync(string client1CCode);

		/// <summary>
		/// Найти клиентов по совпадению ФИО асинхронно.
		/// </summary>
		/// <param name="fio"></param>
		/// <returns></returns>
		[OperationContract(Name = "SearchClientAsync")]
		Task<SearchClientList[]> SearchClientAsync(string fio);

		/// <summary>
		/// Получить список сохраненных КИ клиента по коду анкеты асинхронно.
		/// </summary>
		/// <param name="creditHistoryID">Код анкеты в базе</param>
		/// <returns>Список сохраненных КИ</returns>
		[OperationContract(Name = "GetCreditHistoryListByCreditHistoryIDAsync")]
		Task<CreditHistoryInfo[]> GetCreditHistoryListByCreditHistoryIDAsync(int creditHistoryID);

		/// <summary>
		/// Вернуть номер последней, сохраненной анкеты асинхронно.
		/// </summary>
		/// <param name="client1CCode">Код клиента 1С</param>
		/// <returns>Номер последней анкеты</returns>
		[OperationContract(Name = "GetClientCreditHistoryIDAsync")]
		Task<int> GetClientCreditHistoryIDAsync(string client1CCode);

		/// <summary>
		/// Сохранить кредитную историю асинхронно.
		/// </summary>
		/// <param name="account1CCode">Номер кредитного договора в 1С</param>
		/// <param name="client1CCode">Код клиента 1С</param>
		/// <param name="signedXml">Подписанные НБКИ данные</param>
		/// <param name="unSignedXml">Анкета НБКИ без подписи</param>
		/// <param name="clientTimeZone">Часовой пояс на клиенте</param>
		/// <param name="error">Ошибка из сервиса НБКИ</param>
		[OperationContract(Name = "WriteCreditHistoryAsync")]
		Task WriteCreditHistoryAsync(string account1CCode, string client1CCode, byte[] signedXml, byte[] unSignedXml,
			int clientTimeZone, CTErr error);
	}
}
