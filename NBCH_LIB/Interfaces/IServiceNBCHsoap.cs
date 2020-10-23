using System.Threading;
using System.Threading.Tasks;
using NBCH_LIB.SOAP.SOAPNBCH;

namespace NBCH_LIB.Interfaces {
	public interface IServiceNBCHsoap {
		/// <summary>
		/// Получить КИ клиента из НБКИ.
		/// </summary>
		/// <param name="url">Урл веб службы</param>
		/// <param name="request">Данные клиента</param>
		/// <param name="account1CCode">Номер договора в 1С</param>
		/// <param name="client1CCode">Код клиента в 1С</param>
		/// <param name="clientTimeZone">Часовой пояс, откуда пришел запрос</param>
		/// <returns>КИ из НБКИ</returns>
		Report GetReport(string url, ProductRequest request, string account1CCode, string client1CCode, int clientTimeZone);

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
		Task<Report> GetReportAsync(string url, ProductRequest request, string account1CCode, string client1CCode,
			int clientTimeZone, CancellationToken cancellationToken);
	}
}
