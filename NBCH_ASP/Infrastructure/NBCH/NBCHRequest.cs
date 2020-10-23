using System;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NBCH_ASP.Infrastructure.DataFromConfigurationFile.ISecrets;
using NBCH_ASP.Models.NBCH.NBCHRequest;
using NBCH_LIB.Interfaces;
using NBCH_LIB.SOAP.SOAP1C.GetClientData;
using NBCH_LIB.SOAP.SOAPNBCH;

namespace NBCH_ASP.Infrastructure.NBCH {
	public static class NBCHRequest {
		/// <summary>
		/// Получить данные из 1С и подготовить для вывода на форму асинхронно.
		/// </summary>
		/// <param name="service1C">Сервис 1С</param>
		/// <param name="account1CCode">Номер договора</param>
		/// <param name="secret1C">Данные для подключения</param>
		/// <param name="region">Регион</param>
		/// <param name="configuration">Настройки подключения</param>
		/// <returns>Модель данных для отображения</returns>
		public static async Task<IndexModel> GetDataFrom1CAsync(IService1СSoap service1C, string account1CCode,
			ISecret1C secret1C, string region, IConfiguration configuration) {
			
			IndexModel result				= new IndexModel();
			CreditDocumentNResult account	= await service1C.GetCreditDocumentAsync(
				secret1C.Servers[region],
				secret1C.Login,
				secret1C.Password,
				account1CCode,
				CancellationToken.None);

			if (account.CreditDocument == default) throw new Exception($"{Environment.NewLine}{account.Errors}");
			if (account.CreditDocument.Client == default)
				throw new Exception($"{Environment.NewLine}Договор с номером \"{account1CCode}\" в базе 1С не найден");

			CreditDocument creditDocument			= account.CreditDocument;

			result.ClientPersonalInfo.AddressReq	= (AddressReq[])creditDocument.Client;
			result.ClientPersonalInfo.IdReq			= (IdReq)creditDocument.Client;
			result.ClientPersonalInfo.PersonReq		= (PersonReq)creditDocument.Client;
			result.InquiryReq						= (InquiryReq)creditDocument;
			result.Client1CCode						= creditDocument.Client.ID1C;
			result.Account1CCode					= creditDocument.Code1C;
			result.Account1CDate					= SOAPNBCH.StringToDateTime(creditDocument.DateOfCredit);

			return result;
		}

		/// <summary>
		/// Получить КИ из НБКИ.
		/// </summary>
		/// <param name="serviceNBCH">Сервис НБКИ</param>
		/// <param name="data">Данные для запроса КИ клиента</param>
		/// <param name="secretNBCH">Настройки для подключения</param>
		/// <param name="logger">Логгер</param>
		/// <typeparam name="TLoggerClass">Класс логгера</typeparam>
		/// <returns>Анкета НБКИ</returns>
		internal static async Task<Report> GetCreditHistoryAsync<TLoggerClass>(IServiceNBCHsoap serviceNBCH, IndexModel data,
			ISecretNBCH secretNBCH, ILogger<TLoggerClass> logger) where TLoggerClass : class {
			
			ProductRequest productRequest				= (ProductRequest)data;
			productRequest.Prequest.Req.RequestorReq	= new RequestorReq() {
				MemberCode	= secretNBCH.MemberCode,
				Password	= secretNBCH.Password,
				UserID		= secretNBCH.UserId
			};

			Report report	= await serviceNBCH.GetReportAsync(
				SOAPNBCH.ProductServiceURL,
				productRequest,
				data.Account1CCode,
				data.Client1CCode,
				data.ClientTimeZone,
				CancellationToken.None);
			
			return report;
		}
	}
}
