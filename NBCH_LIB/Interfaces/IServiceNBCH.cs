using System.Threading;
using System.Threading.Tasks;
using NBCH_LIB.Models;
using NBCH_LIB.SOAP.SOAPNBCH;

namespace NBCH_LIB.Interfaces {
	public interface IServiceNBCH: IServiceNBCHBase {
		/// <summary>
		/// Получить КИ по ID асинхронно.
		/// </summary>
		/// <param name="creditHistoryID"></param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns>Кредитная история</returns>
		Task<Report> GetSavedReportAsync(int creditHistoryID, CancellationToken cancellationToken);

		/// <summary>
		/// Получить список кредитных историй по коду клиента асинхронно.
		/// </summary>
		/// <param name="client1CCode">Код клиента 1С</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns>Список кредитных анкет</returns>
		Task<CreditHistoryInfo[]> GetCreditHistoryListAsync(string client1CCode, CancellationToken cancellationToken);

		/// <summary>
		/// Найти клиентов по совпадению ФИО асинхронно.
		/// </summary>
		/// <param name="fio"></param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns></returns>
		Task<SearchClientList[]> SearchClientAsync(string fio, CancellationToken cancellationToken);

		/// <summary>
		/// Получить список сохраненных КИ клиента по коду анкеты асинхронно.
		/// </summary>
		/// <param name="creditHistoryID">Код анкеты в базе</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns>Список сохраненных КИ</returns>
		Task<CreditHistoryInfo[]> GetCreditHistoryListByCreditHistoryIDAsync(int creditHistoryID,
			CancellationToken cancellationToken);

		/// <summary>
		/// Вернуть номер последней, сохраненной анкеты асинхронно.
		/// </summary>
		/// <param name="client1CCode">Код клиента 1С</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns>Номер последней анкеты</returns>
		Task<int> GetClientCreditHistoryIDAsync(string client1CCode, CancellationToken cancellationToken);

		/// <summary>
		/// Сохранить кредитную историю асинхронно.
		/// </summary>
		/// <param name="account1CCode">Номер кредитного договора в 1С</param>
		/// <param name="client1CCode">Код клиента 1С</param>
		/// <param name="signedXml">Подписанные НБКИ данные</param>
		/// <param name="unSignedXml">Анкета НБКИ без подписи</param>
		/// <param name="clientTimeZone">Часовой пояс на клиенте</param>
		/// <param name="error">Ошибка из сервиса НБКИ</param>
		/// <param name="cancellationToken">Токен отмены</param>
		Task WriteCreditHistoryAsync(string account1CCode, string client1CCode, byte[] signedXml, byte[] unSignedXml,
			int clientTimeZone, CTErr error, CancellationToken cancellationToken);
	}
}
