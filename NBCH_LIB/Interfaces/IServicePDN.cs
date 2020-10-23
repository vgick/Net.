using System;
using System.Threading;
using System.Threading.Tasks;
using NBCH_LIB.Models.PDN;

namespace NBCH_LIB.Interfaces {
	public interface IServicePDN {
		/// <summary>
		/// Данные ПДН.
		/// </summary>
		/// <param name="accounts">Список договоров 1С</param>
		/// <returns>ПДН договоров</returns>
		PdnResult[] GetPDNPercents(string[] accounts);

		/// <summary>
		/// Данные ПДН асинхронно.
		/// </summary>
		/// <param name="accounts">Список договоров 1С</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns>ПДН договоров</returns>
		Task<PdnResult[]> GetPDNPercentsAsync(string[] accounts, CancellationToken cancellationToken);

		/// <summary>
		/// Вернуть все записи в которых ПДН больше 50%.
		/// </summary>
		/// <returns>Номера договоров</returns>
		string[] GetFullRecordOver50P();

		/// <summary>
		/// Вернуть все записи в которых ПДН больше 50% асинхронно.
		/// </summary>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns>Номера договоров</returns>
		Task<string[]> GetFullRecordOver50PAsync(CancellationToken cancellationToken);

		/// <summary>
		/// Рассчитать ПДН по номеру клиента 1С.
		/// </summary>
		/// <param name="account1CCode">Код договора 1С</param>
		/// <param name="accountDate">Дата договора 1С</param>
		/// <param name="client1CCode">Код клиента 1С</param>
		/// <returns>ПДН и дата анкеты, на основании которой был рассчитан ПДН</returns>
		PDNInfoList CalculatePDN(string account1CCode, DateTime accountDate, string client1CCode);

		/// <summary>
		/// Рассчитать ПДН по номеру клиента 1С асинхронно.
		/// </summary>
		/// <param name="account1CCode">Код договора 1С</param>
		/// <param name="accountDate">Дата договора 1С</param>
		/// <param name="client1CCode">Код клиента 1С</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns>ПДН и дата анкеты, на основании которой был рассчитан ПДН</returns>
		Task<PDNInfoList> CalculatePDNAsync(string account1CCode, DateTime accountDate, string client1CCode,
			CancellationToken cancellationToken);

		/// <summary>
		/// Получить сохраненный ПДН по номеру договору 1С.
		/// </summary>
		/// <param name="account1CCode">Номер договора из 1С</param>
		/// <returns>ПДН и дата анкеты, на основании которой был рассчитан ПДН</returns>
		PDNInfoList GetSavedPDN(string account1CCode);

		/// <summary>
		/// Получить сохраненный ПДН по номеру договору 1С асинхронно.
		/// </summary>
		/// <param name="account1CCode">Номер договора из 1С</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns>ПДН и дата анкеты, на основании которой был рассчитан ПДН</returns>
		Task<PDNInfoList> GetSavedPDNAsync(string account1CCode, CancellationToken cancellationToken);

		/// <summary>
		/// Сохранить ПДН.
		/// </summary>
		/// <param name="pdnInfoList">Данные ПДН</param>
		void SavePDN(PDNInfoList pdnInfoList);

		/// <summary>
		/// Сохранить ПДН асинхронно.
		/// </summary>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <param name="pdnInfoList">Данные ПДН</param>
		Task SavePDNAsync(PDNInfoList pdnInfoList, CancellationToken cancellationToken);

		/// <summary>
		/// Получить список договоров с ошибками при расчете ПДН.
		/// </summary>
		/// <returns>Список договоров с ошибками в расчете ПДН</returns>
		PDNErrorAccountInfo[] GetAccountsWithPDNError();

		/// <summary>
		/// Получить список договоров с ошибками при расчете ПДН асинхронно.
		/// </summary>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns>Список договоров с ошибками в расчете ПДН</returns>
		Task<PDNErrorAccountInfo[]> GetAccountsWithPDNErrorAsync(CancellationToken cancellationToken);
	}
}
