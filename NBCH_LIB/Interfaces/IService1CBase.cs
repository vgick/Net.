using System.ServiceModel;
using NBCH_LIB.SOAP.SOAP1C.GetClientData;

namespace NBCH_LIB.Interfaces {
	[ServiceContract]
	public interface IService1CBase {
		/// <summary>
		/// Обновить информацию по договору 1С.
		/// </summary>
		/// <param name="document1C">Договора 1С</param>
		[OperationContract]
		void UpdateAccountAndClientInfo(CreditDocumentNResult document1C);

		/// <summary>
		/// Загрузить ПДН по договору.
		/// </summary>
		/// <param name="document1C">Договор</param>
		/// <param name="pdnValue">Значение процента ПДН</param>
		[OperationContract]
		void LoadPDNFromFile(CreditDocumentNResult document1C, double pdnValue);
	}
}