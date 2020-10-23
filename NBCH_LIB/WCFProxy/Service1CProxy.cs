using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading;
using System.Threading.Tasks;
using NBCH_LIB.Interfaces;
using NBCH_LIB.Interfaces.WCF;
using NBCH_LIB.SOAP.SOAP1C.GetClientData;

namespace NBCH_LIB.WCFProxy {
	/// <summary>
	/// Прокси класс для работы с WCF службой 1С
	/// </summary>
	public class Service1CProxy : ClientBase<IService1CWCF>, IService1CWCF {
		#region Конструкторы
		public Service1CProxy() { }
		public Service1CProxy(string endpointName) : base(endpointName) { }
		public Service1CProxy(Binding binding, EndpointAddress remoteAddress) : base(binding, remoteAddress) { }
		#endregion

		/// <summary>
		/// Обновить информацию по договору 1С.
		/// </summary>
		/// <param name="document1C">Договора 1С</param>
		public void UpdateAccountAndClientInfo(CreditDocumentNResult document1C) =>
			Channel.UpdateAccountAndClientInfo(document1C);

		/// <summary>
		/// Обновить информацию по договору 1С асинхронно.
		/// </summary>
		/// <param name="document1C">Договора 1С</param>
		public async Task UpdateAccountAndClientInfoAsync(CreditDocumentNResult document1C) =>
			await Channel.UpdateAccountAndClientInfoAsync(document1C);

		/// <summary>
		/// Загрузить ПДН по договору.
		/// </summary>
		/// <param name="document1C">Договор</param>
		/// <param name="pdnValue">Значение процента ПДН</param>
		public void LoadPDNFromFile(CreditDocumentNResult document1C, double pdnValue) =>
			Channel.LoadPDNFromFile(document1C, pdnValue);

		/// <summary>
		/// Загрузить ПДН по договору асинхронно.
		/// </summary>
		/// <param name="document1C">Договор</param>
		/// <param name="pdnValue">Значение процента ПДН</param>
		public async Task LoadPDNFromFileAsync(CreditDocumentNResult document1C, double pdnValue) =>
			await Channel.LoadPDNFromFileAsync(document1C, pdnValue);
	}
}
