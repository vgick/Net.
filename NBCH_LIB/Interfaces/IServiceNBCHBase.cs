
using System.ServiceModel;
using NBCH_LIB.Models;
using NBCH_LIB.SOAP.SOAPNBCH;

namespace NBCH_LIB.Interfaces {
	[ServiceContract]
	public interface IServiceNBCHBase {
		/// <summary>
		/// Получить КИ по ID.
		/// </summary>
		/// <param name="creditHistoryID"></param>
		/// <returns>Кредитная история</returns>
		[OperationContract]
		Report GetSavedReport(int creditHistoryID);

		/// <summary>
		/// Получить список кредитных историй по коду клиента.
		/// </summary>
		/// <param name="client1CCode">Код клиента 1С</param>
		/// <returns>Список кредитных анкет</returns>
		[OperationContract]
		CreditHistoryInfo[] GetCreditHistoryList(string client1CCode);

		/// <summary>
		/// Найти клиентов по совпадению ФИО.
		/// </summary>
		/// <param name="fio"></param>
		/// <returns></returns>
		[OperationContract]
		SearchClientList[] SearchClient(string fio);

		/// <summary>
		/// Получить список сохраненных КИ клиента по коду анкеты.
		/// </summary>
		/// <param name="creditHistoryID">Код анкеты в базе</param>
		/// <returns>Список сохраненных КИ</returns>
		[OperationContract]
		CreditHistoryInfo[] GetCreditHistoryListByCreditHistoryID(int creditHistoryID);

		/// <summary>
		/// Вернуть номер последней, сохраненной анкеты.
		/// </summary>
		/// <param name="client1CCode">Код клиента 1С</param>
		/// <returns>Номер последней анкеты</returns>
		[OperationContract]
		int GetClientCreditHistoryID(string client1CCode);
	}
}