using System.Security.Permissions;
using System.Threading.Tasks;
using NBCH_LIB.Interfaces.WCF;
using NBCH_LIB.Logger;
using NBCH_LIB.SOAP.SOAP1C.GetClientData;
using static NBCH_WCF.Services.Utils;
using static NBCH_WCF.Services.ServiceMethod;

namespace NBCH_WCF.Services {
	/// <summary>
	/// Сервис, реализующего интерфейс IService1C для работы с данными из 1С.
	/// </summary>
	public class WCFService1C : IService1CWCF {
		/// <summary>
		/// Обновить информацию по договору 1С.
		/// </summary>
		/// <param name="document1C">Договора 1С</param>
		[PrincipalPermission(SecurityAction.Demand, Role = @"admin")]
		public void UpdateAccountAndClientInfo(CreditDocumentNResult document1C) =>
			ExecuteWithTryCatch<WCFService1C>( () => Service1C.UpdateAccountAndClientInfo(document1C),
				new LogShortMessage("Ошибка вызова метода. Метод {methodName}, document1C {document1C}.",
					"UpdateAccountAndClientInfo", document1C));

		/// <summary>
		/// Обновить информацию по договору 1С асинхронно.
		/// </summary>
		/// <param name="document1C">Договора 1С</param>
		[PrincipalPermission(SecurityAction.Demand, Role = @"admin")]
		public async Task UpdateAccountAndClientInfoAsync(CreditDocumentNResult document1C) =>
			await ExecuteWithTryCatchAsync<WCFService1C>( () =>
				Service1C.UpdateAccountAndClientInfoAsync(document1C),
				new LogShortMessage("Ошибка вызова метода. Метод {methodName}, document1C {document1C}.",
					"UpdateAccountAndClientInfo", document1C));

		/// <summary>
		/// Загрузить ПДН по договору.
		/// </summary>
		/// <param name="document1C">Договор</param>
		/// <param name="pdnValue">Значение процента ПДН</param>
		[PrincipalPermission(SecurityAction.Demand, Role = @"admin")]
		public void LoadPDNFromFile(CreditDocumentNResult document1C, double pdnValue) =>
			ExecuteWithTryCatch<WCFService1C>( () => Service1C.LoadPDNFromFile(document1C, pdnValue),
				new LogShortMessage("Ошибка вызова метода. Метод {methodName}, document1C {document1C}, pdnValue {pdnValue}.",
					"LoadPDNFromFile", document1C, pdnValue));

		/// <summary>
		/// Загрузить ПДН по договору асинхронно.
		/// </summary>
		/// <param name="document1C">Договор</param>
		/// <param name="pdnValue">Значение процента ПДН</param>
		[PrincipalPermission(SecurityAction.Demand, Role = @"admin")]
		public async Task LoadPDNFromFileAsync(CreditDocumentNResult document1C, double pdnValue) =>
			await ExecuteWithTryCatchAsync<WCFService1C>( () => Service1C.LoadPDNFromFileAsync(document1C, pdnValue),
				new LogShortMessage("Ошибка вызова метода. Метод {methodName}, document1C {document1C}, pdnValue {pdnValue}.",
					"LoadPDNFromFile", document1C, pdnValue));
	}
}
