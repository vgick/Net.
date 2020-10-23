using System;
using System.ServiceModel;
using System.Threading.Tasks;
using NBCH_LIB.Models.PDN;

namespace NBCH_LIB.Interfaces.WCF {
	[ServiceContract]
	public interface IServicePDNWCF: IWCFContract {
		/// <summary>
		/// Данные ПДН.
		/// </summary>
		/// <param name="accounts">Список договоров 1С</param>
		/// <returns>ПДН договоров</returns>
		[OperationContract]
		PdnResult[] GetPDNPercents(string[] accounts);

		/// <summary>
		/// Данные ПДН асинхронно.
		/// </summary>
		/// <param name="accounts">Список договоров 1С</param>
		/// <returns>ПДН договоров</returns>
		[OperationContract(Name = "GetPDNPercentsAsync")]
		Task<PdnResult[]> GetPDNPercentsAsync(string[] accounts);

		/// <summary>
		/// Вернуть все записи в которых ПДН больше 50%.
		/// </summary>
		/// <returns>Номера договоров</returns>
		[OperationContract]
		string[] GetFullRecordOver50P();

		/// <summary>
		/// Вернуть все записи в которых ПДН больше 50% асинхронно.
		/// </summary>
		/// <returns>Номера договоров</returns>
		[OperationContract(Name = "GetFullRecordOver50PAsync")]
		Task<string[]> GetFullRecordOver50PAsync();

		/// <summary>
		/// Рассчитать ПДН по номеру клиента 1С.
		/// </summary>
		/// <param name="account1CCode">Код договора 1С</param>
		/// <param name="accountDate">Дата договора 1С</param>
		/// <param name="client1CCode">Код клиента 1С</param>
		/// <returns>ПДН и дата анкеты, на основании которой был рассчитан ПДН</returns>
		[OperationContract]
		PDNInfoList CalculatePDN(string account1CCode, DateTime accountDate, string client1CCode);

		/// <summary>
		/// Рассчитать ПДН по номеру клиента 1С асинхронно.
		/// </summary>
		/// <param name="account1CCode">Код договора 1С</param>
		/// <param name="accountDate">Дата договора 1С</param>
		/// <param name="client1CCode">Код клиента 1С</param>
		/// <returns>ПДН и дата анкеты, на основании которой был рассчитан ПДН</returns>
		[OperationContract(Name = "CalculatePDNAsync")]
		Task<PDNInfoList> CalculatePDNAsync(string account1CCode, DateTime accountDate, string client1CCode);

		/// <summary>
		/// Получить сохраненный ПДН по номеру договору 1С.
		/// </summary>
		/// <param name="account1CCode">Номер договора из 1С</param>
		/// <returns>ПДН и дата анкеты, на основании которой был рассчитан ПДН</returns>
		[OperationContract]
		PDNInfoList GetSavedPDN(string account1CCode);

		/// <summary>
		/// Получить сохраненный ПДН по номеру договору 1С асинхронно.
		/// </summary>
		/// <param name="account1CCode">Номер договора из 1С</param>
		/// <returns>ПДН и дата анкеты, на основании которой был рассчитан ПДН</returns>
		[OperationContract(Name = "GetSavedPDNAsync")]
		Task<PDNInfoList> GetSavedPDNAsync(string account1CCode);

		/// <summary>
		/// Сохранить ПДН.
		/// </summary>
		/// <param name="pdnInfoList">Данные ПДН</param>
		[OperationContract]
		void SavePDN(PDNInfoList pdnInfoList);

		/// <summary>
		/// Сохранить ПДН асинхронно.
		/// </summary>
		/// <param name="pdnInfoList">Данные ПДН</param>
		[OperationContract(Name = "SavePDNAsync")]
		Task SavePDNAsync(PDNInfoList pdnInfoList);

		/// <summary>
		/// Получить список договоров с ошибками при расчете ПДН.
		/// </summary>
		/// <returns>Список договоров с ошибками в расчете ПДН</returns>
		[OperationContract]
		PDNErrorAccountInfo[] GetAccountsWithPDNError();

		/// <summary>
		/// Получить список договоров с ошибками при расчете ПДН асинхронно.
		/// </summary>
		/// <returns>Список договоров с ошибками в расчете ПДН</returns>
		[OperationContract(Name = "GetAccountsWithPDNErrorAsync")]
		Task<PDNErrorAccountInfo[]> GetAccountsWithPDNErrorAsync();
	}
}
