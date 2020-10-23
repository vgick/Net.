using System;
using System.ComponentModel;

namespace NBCH_LIB.SOAP.SOAPNBCH {
	/// <summary>
	/// Данные о согласии клиента на запрос КИ.
	/// </summary>
	public class ConsentReq {
		/// <summary>
		/// Согласие клиента на предоставление КИ.
		/// </summary>
		public string consentFlag {get; set;}

		/// <summary>
		/// Дата согласия клиента на предоставление КИ.
		/// </summary>
		public string consentDate {get; set;}

		/// <summary>
		/// Дата истечения согласия на запрос КИ.
		/// </summary>
		public string consentExpireDate {get; set;}

		/// <summary>
		/// Цель согласия.
		/// </summary>
		public string consentPurpose {get; set;}

		/// <summary>
		/// Цель согласия (прочее).
		/// </summary>
		public string otherConsentPurpose {get; set;}

		/// <summary>
		/// Кто запрашивает согласие.
		/// </summary>
		public string reportUser {get; set;}

		/// <summary>
		/// Согласие, что ознакомлены с ответственностью.
		/// </summary>
		public string liability {get; set;}

		public ConsentPurposee ConsentPurpose {
			get => (ConsentPurposee)Enum.Parse(typeof(ConsentPurposee), consentPurpose);
			set => consentPurpose = ((int)value).ToString();
		}

		/// <summary>
		/// Цель запроса.
		/// </summary>
		public enum ConsentPurposee {
			[Description("Заключение и исполнение договора")]
			AgreementConclusionAndFulfilment	= 1,
			[Description("Проверка безопасности")]
			SecurityCheck	= 2,
			[Description("Прием на работу")]
			JobHire			= 3,
			[Description("Иное")]
			Other			= 4

		}
	}
}
