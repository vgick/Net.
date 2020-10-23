using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading;
using System.Threading.Tasks;
using NBCH_LIB.Interfaces;
using NBCH_LIB.SOAP.SOAPNBCH;

namespace NBCH_LIB.WCFProxy {
	/// <summary>
	/// Прокси класс для работы с WCF службой анкет НБКИ
	/// </summary>
	public class ServiceNBCHsoapProxy : ClientBase<IServiceNBCHsoap>, IServiceNBCHsoap {
		#region Конструкторы
		public ServiceNBCHsoapProxy() { }
		public ServiceNBCHsoapProxy(string endpointName) : base(endpointName) { }
		public ServiceNBCHsoapProxy(Binding binding, EndpointAddress remoteAddress) : base(binding, remoteAddress) { }
		#endregion

		/// <summary>
		/// Получить КИ клиента из НБКИ.
		/// </summary>
		/// <param name="url">Урл веб службы</param>
		/// <param name="request">Данные клиента</param>
		/// <param name="account1CCode">Номер договора в 1С</param>
		/// <param name="client1CCode">Код клиента в 1С</param>
		/// <param name="clientTimeZone">Часовой пояс, откуда пришел запрос</param>
		/// <returns>КИ из НБКИ</returns>
		public Report GetReport(string url, ProductRequest request, string account1CCode, string client1CCode, int clientTimeZone) =>
			Channel.GetReport(url, request, account1CCode, client1CCode, clientTimeZone);

		/// <summary>
		/// Получить КИ клиента из НБКИ асинхронно.
		/// </summary>
		/// <param name="url">Урл веб службы</param>
		/// <param name="request">Данные клиента</param>
		/// <param name="account1CCode">Номер договора в 1С</param>
		/// <param name="client1CCode">Код клиента в 1С</param>
		/// <param name="clientTimeZone">Часовой пояс, откуда пришел запрос</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns>КИ из НБКИ</returns>
		public async Task<Report> GetReportAsync(string url, ProductRequest request, string account1CCode, string client1CCode,
			int clientTimeZone, CancellationToken cancellationToken) =>
			
			await Channel.GetReportAsync(url, request, account1CCode, client1CCode, clientTimeZone, cancellationToken);
	}
}
