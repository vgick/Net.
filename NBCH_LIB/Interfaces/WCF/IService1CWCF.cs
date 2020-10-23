using System.ServiceModel;
using System.Threading.Tasks;
using NBCH_LIB.SOAP.SOAP1C.GetClientData;

namespace NBCH_LIB.Interfaces.WCF {
	[ServiceContract]
	public interface IService1CWCF : IWCFContract, IService1CBase {
		/// <summary>
		/// Обновить информацию по договору 1С асинхронно.
		/// </summary>
		/// <param name="document1C">Договора 1С</param>
		[OperationContract(Name = "UpdateAccountAndClientInfoAsync")]
		Task UpdateAccountAndClientInfoAsync(CreditDocumentNResult document1C);

		/// <summary>
		/// Загрузить ПДН по договору асинхронно.
		/// </summary>
		/// <param name="document1C">Договор</param>
		/// <param name="pdnValue">Значение процента ПДН</param>
		[OperationContract(Name = "LoadPDNFromFileAsync")]
		Task LoadPDNFromFileAsync(CreditDocumentNResult document1C, double pdnValue);
	}
}
