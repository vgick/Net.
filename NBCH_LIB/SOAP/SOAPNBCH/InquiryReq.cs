using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace NBCH_LIB.SOAP.SOAPNBCH {
	public class InquiryReq {
		/// <summary>
		/// Информация по согласию КИ клиента.
		/// </summary>
		public ConsentReq ConsentReq { get; set; }

		/// <summary>
		/// Цель запроса КИ (InquiryPurpose - 2 символа вида "07").
		/// </summary>
		public string inqPurpose { get; set; }

		/// <summary>
		/// Сумма договора.
		/// </summary>
		public string inqAmount { get; set; }

		/// <summary>
		/// Валюта договора.
		/// </summary>
		public string currencyCode { get; set; }

		/// <summary>
		/// Валюта договора, заполняет строку currencyCode..
		/// </summary>
		[XmlIgnore]
		public Currency CurrencyCode {
			get => (Currency)Enum.Parse(typeof(Currency), currencyCode);
			set => currencyCode = InquiryReq.Currency.RUB.ToString();
		}

		/// <summary>
		/// Цель запроса КИ. Заполняет inqPurpose.
		/// </summary>
		[XmlIgnore]
		public InquiryPurpose InqPurpose {
			get => ((InquiryPurpose)int.Parse(inqPurpose ?? ((int)InquiryPurpose.Null).ToString()));
			set {
				string newValue = ((int)value).ToString("00");
				inqPurpose	= newValue == InquiryPurpose.Null.ToString() ? null : newValue;
			}
		}

		/// <summary>
		/// Валюта запроса, 3 символа.
		/// </summary>
		public enum Currency {
			[Description("Рубли")]
			RUB,
			[Description("Доллары США")]
			USD
		}
		/// <summary>
		/// Цель щапроса КИ.
		/// </summary>
		public enum InquiryPurpose {
			[Description("Not set")]
			Null = -1,
			[Description("Автокредит")]
			AutoLoan			= 1,
			[Description("Лизинг")]
			Leasing				= 4,
			[Description("Ипотека")]
			Mortgage			= 6,
			[Description("Кредитная карта")]
			CreditCard			= 7,
			[Description("Потребительский кредит")]
			ConsumerCredit		= 9,
			[Description("Для развития бизнеса")]
			ForDevelopmentOfBusiness		= 10,
			[Description("Для увеличения оборотных средств")]
			ForEnlargingCirculatingAssets	= 11,
			[Description("Для приобретения оборудования")]
			ForEquipmentPurchase			= 12,
			[Description("Для строительства недвижимости")]
			ForBuildingRealEstate			= 13,
			[Description("Покупка ценных бумаг")]
			ForSecuritiesPurchase			= 14,
			[Description("Межбанковский кредит")]
			InterBankCredit		= 15,
			[Description("Микрокредит")]
			Microcredit			= 16,
			[Description("Дебетовая карта с овердрафтом")]
			OverdraftDebitCard	= 17,
			[Description("Овердрафт")]
			Overdraft = 15,
			[Description("Просмотр счета")]
			AccountReview = 50,
			[Description("Неизвестно")]
			Unknown = 99
		}
	}
}
