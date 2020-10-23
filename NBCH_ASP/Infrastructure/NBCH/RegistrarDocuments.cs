using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using NBCH_ASP.Infrastructure.DataFromConfigurationFile.ISecrets;
using NBCH_LIB.Interfaces;
using NBCH_LIB.Models;
using NBCH_LIB.SOAP.SOAP1C.GetClientData;

namespace NBCH_ASP.Infrastructure.NBCH {
	/// <summary>
	/// Вспомогательный класс для контроллера RegistrarDocuments
	/// </summary>
	public static class RegistrarDocuments {
		/// <summary>
		/// Получить из 1С список клиентов по номеру договора асинхронно.
		/// </summary>
		/// <param name="service1C">Сервис 1С</param>
		/// <param name="secret1Cs">Данные для подключения к сервису 1С</param>
		/// <param name="region">Регион для определения приоритетного сервера</param>
		/// <param name="account1CCode">Номер договора 1С</param>
		/// <returns>Список клиентов</returns>
		public static async Task<(Client[] Clients, string[] Errors)> GetClientsFrom1CAccountAsync(IService1СSoap service1C, ISecret1C secret1Cs, string region, string account1CCode) {
			List<string> errors	= new List<string>();
			Client[] clients	= new Client[0];

			try {
				CreditDocumentNResult creditDocument =  
					await service1C.GetCreditDocumentAsync(
						secret1Cs.Servers[region],
						secret1Cs.Login,
						secret1Cs.Password,
						account1CCode,
						CancellationToken.None);

					if (creditDocument.Errors.Length > 0) errors.AddRange(creditDocument.Errors);
					clients = (Client[])creditDocument.CreditDocument;
			}
			catch (EndpointNotFoundException) {
				errors.Add("Не удалось подключиться к службе 1C (запрос данных кредитного договора)");
			}
			catch (Exception ex) {
				errors.Add(ex.Message);
			}

			return (clients, errors.ToArray());
		}
	}
}
