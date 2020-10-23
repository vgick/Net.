using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using NBCH_LIB.SOAP.SOAPNBCH;

namespace NBCH_ASP.Models.NBCH.NBCHRequest {
	/// <summary>
	/// Данные для представления Index контроллера NBCHRequest
	/// </summary>
	public class IndexModel {
		/// <summary>
		/// Код договора в 1С
		/// </summary>
		public string Account1CCode {get; set;}

		/// <summary>
		/// Дата договора.
		/// </summary>
		public DateTime Account1CDate {get; set;}

		/// <summary>
		/// Название выбранного региона, для поиска серверов веб служб 1С.
		/// </summary>
		public SelectList RegionsWebServiceListName { get; set; } = default;

		/// <summary>
		/// Код клиента из 1С.
		/// </summary>
		public string Client1CCode {get; set;}

		/// <summary>
		/// Персональные данные клиента.
		/// </summary>
		public ClientPersonalInfo ClientPersonalInfo {get; set;} = new ClientPersonalInfo();

		/// <summary>
		/// Данные по сумме кредита из 1С.
		/// </summary>
		public InquiryReq InquiryReq {get; set;} = new InquiryReq();

		/// <summary>
		/// Счета клиента.
		/// </summary>
		public AccountReply[] AccountReply {get; set;} = new AccountReply[0];

		/// <summary>
		/// Часовой пояс на клиенте.
		/// </summary>
		public int ClientTimeZone {get; set;}

		/// <summary>
		/// Общая информация по счету.
		/// </summary>
		public Calc Calc{get; set;}

		/// <summary>
		/// Ошибка при выполнении запроса.
		/// </summary>
		public String ErrorMessage {get; set;}

		/// <summary>
		/// Привести тип NBCHRequestIndex к ProductRequest.
		/// </summary>
		/// <param name="indexModel"></param>
		public static explicit operator ProductRequest(IndexModel indexModel) {
			if (indexModel == default) return default;

			ProductRequest productRequest = new ProductRequest();

			Req req			= productRequest.Prequest?.Req ?? new Req();
			req.AddressReq	= indexModel.ClientPersonalInfo.AddressReq ?? new AddressReq[0];
			req.IdReq		= new IdReq[] { (IdReq)indexModel.ClientPersonalInfo.IdReq ?? new IdReq()};
			req.PersonReq	= indexModel.ClientPersonalInfo.PersonReq ?? new PersonReq();
			req.InquiryReq	= indexModel.InquiryReq ?? new InquiryReq();

			return productRequest;
		}

		/// <summary>
		/// Вариант запроса формы.
		/// </summary>
		public enum SubmitType {
			/// <summary>
			/// Не задано.
			/// </summary>
			Null,
			/// <summary>
			/// Получить данные для запроса из 1С.
			/// </summary>
			GetFrom1C,
			/// <summary>
			/// Получить кредитную анкету из НБКИ.
			/// </summary>
			GetCH
		}
	}
}
