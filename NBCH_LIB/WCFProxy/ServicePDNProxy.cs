using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading;
using System.Threading.Tasks;
using NBCH_LIB.Interfaces;
using NBCH_LIB.Interfaces.WCF;
using NBCH_LIB.Models.PDN;

namespace NBCH_LIB.WCFProxy {
	/// <summary>
	/// Прокси класс для работы с ПДН.
	/// </summary>
	public class ServicePDNProxy : ClientBase<IServicePDNWCF>, IServicePDNWCF {
		#region Конструкторы
		public ServicePDNProxy() { }
		public ServicePDNProxy(string endpointName) : base(endpointName) { }
		public ServicePDNProxy(Binding binding, EndpointAddress remoteAddress) : base(binding, remoteAddress) { }
		#endregion

		/// <summary>
		/// Вернуть все записи в которых ПДН больше 50%.
		/// </summary>
		public string[] GetFullRecordOver50P() => Channel.GetFullRecordOver50P();

		/// <summary>
		/// Вернуть все записи в которых ПДН больше 50% асинхронно.
		/// </summary>
		public async Task<string[]> GetFullRecordOver50PAsync() =>
			await Channel.GetFullRecordOver50PAsync();

		/// <summary>
		/// Рассчитать ПДН по номеру клиента 1С.
		/// </summary>
		/// <param name="account1CCode">Код договора 1С</param>
		/// <param name="accountDate">Дата договора 1С</param>
		/// <param name="client1CCode">Код клиента 1С</param>
		/// <returns>ПДН и дата анкеты, на основании которой был рассчитан ПДН</returns>
		public PDNInfoList CalculatePDN(string account1CCode, DateTime accountDate, string client1CCode) =>
			Channel.CalculatePDN(account1CCode, accountDate, client1CCode);

		/// <summary>
		/// Рассчитать ПДН по номеру клиента 1С асинхронно.
		/// </summary>
		/// <param name="account1CCode">Код договора 1С</param>
		/// <param name="accountDate">Дата договора 1С</param>
		/// <param name="client1CCode">Код клиента 1С</param>
		/// <returns>ПДН и дата анкеты, на основании которой был рассчитан ПДН</returns>
		public async Task<PDNInfoList> CalculatePDNAsync(string account1CCode, DateTime accountDate, string client1CCode) =>
			await Channel.CalculatePDNAsync(account1CCode, accountDate, client1CCode);
			
		/// <summary>
		/// Данные ПДН.
		/// </summary>
		/// <param name="accounts">Список договоров 1С</param>
		/// <returns>ПДН договоров</returns>
		public PdnResult[] GetPDNPercents(string[] accounts) => Channel.GetPDNPercents(accounts);

		/// <summary>
		/// Данные ПДН асинхронно.
		/// </summary>
		/// <param name="accounts">Список договоров 1С</param>
		/// <returns>ПДН договоров</returns>
		public async Task<PdnResult[]> GetPDNPercentsAsync(string[] accounts) => await Channel.GetPDNPercentsAsync(accounts);

		/// <summary>
		/// Получить сохраненный ПДН по номеру договору 1С.
		/// </summary>
		/// <param name="account1CCode">Номер договора из 1С</param>
		/// <returns>ПДН и дата анкеты, на основании которой был рассчитан ПДН</returns>
		public PDNInfoList GetSavedPDN(string account1CCode) => Channel.GetSavedPDN(account1CCode);

		/// <summary>
		/// Получить сохраненный ПДН по номеру договору 1С асинхронно.
		/// </summary>
		/// <param name="account1CCode">Номер договора из 1С</param>
		/// <returns>ПДН и дата анкеты, на основании которой был рассчитан ПДН</returns>
		public async Task<PDNInfoList> GetSavedPDNAsync(string account1CCode) =>
			await Channel.GetSavedPDNAsync(account1CCode);

		/// <summary>
		/// Сохранить ПДН.
		/// </summary>
		/// <param name="pdnInfoList">Данные ПДН</param>
		public void SavePDN(PDNInfoList pdnInfoList) => Channel.SavePDN(pdnInfoList);

		/// <summary>
		/// Сохранить ПДН асинхронно.
		/// </summary>
		/// <param name="pdnInfoList">Данные ПДН</param>
		public async Task SavePDNAsync(PDNInfoList pdnInfoList) =>
			await Channel.SavePDNAsync(pdnInfoList);

		/// <summary>
		/// Получить список договоров с ошибками при расчете ПДН.
		/// </summary>
		/// <returns>Список договоров с ошибками в расчете ПДН</returns>
		public PDNErrorAccountInfo[] GetAccountsWithPDNError() => Channel.GetAccountsWithPDNError();

		/// <summary>
		/// Получить список договоров с ошибками при расчете ПДН асинхронно.
		/// </summary>
		/// <returns>Список договоров с ошибками в расчете ПДН</returns>
		public async Task<PDNErrorAccountInfo[]> GetAccountsWithPDNErrorAsync() =>
			await Channel.GetAccountsWithPDNErrorAsync();
	}
}
