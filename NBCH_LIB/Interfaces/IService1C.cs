using System.Threading;
using System.Threading.Tasks;
using NBCH_LIB.SOAP.SOAP1C.GetClientData;

namespace NBCH_LIB.Interfaces {
	public interface IService1C: IService1CBase {

		/// <summary>
		/// Обновить информацию по договору 1С асинхронно.
		/// </summary>
		/// <param name="document1C">Договора 1С</param>
		/// <param name="cancellationToken">Токен отмены</param>
		Task UpdateAccountAndClientInfoAsync(CreditDocumentNResult document1C, CancellationToken cancellationToken);

		/// <summary>
		/// Загрузить ПДН по договору асинхронно.
		/// </summary>
		/// <param name="document1C">Договор</param>
		/// <param name="pdnValue">Значение процента ПДН</param>
		/// <param name="cancellationToken">Токен отмены</param>
		Task LoadPDNFromFileAsync(CreditDocumentNResult document1C, double pdnValue, CancellationToken cancellationToken);
	}
}
