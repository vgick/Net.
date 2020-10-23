using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using NBCH_LIB.Models;
using NBCH_LIB.SOAP.SOAPNBCH;
using static NBCH_LIB.Models.Client;

namespace NBCH_LIB.SOAP.SOAP1C.GetClientData {
	[XmlType(Namespace = "ns_credit", TypeName = "Deal")]
	public class CreditDocument {
		/// <summary>
		/// Срок в месяцах, на которое было выдано согласие
		/// </summary>
		private const int _DurationOfConsen	= 6;

		/// <summary>
		/// Код договора 1С
		/// </summary>
		[XmlElement("code")]
		public string Code1C {get; set;}

		/// <summary>
		/// Информация о заемщике
		/// </summary>
		[XmlElement("client")]
		public ClientProfile Client {get; set;}

		/// <summary>
		/// Информация о заемщике
		/// </summary>
		[XmlElement("guarantor")]
		public ClientProfile[] Guarantors { get; set; }

		/// <summary>
		/// Кредитный эксперт
		/// </summary>
		[XmlElement("author")]
		public string CreditInspector {get; set;}

		/// <summary>
		/// Точка заключения займа
		/// </summary>
		[XmlElement("point")]
		public string PointOfSale {get; set;}

		/// <summary>
		/// Кредитный продукт
		/// </summary>
		[XmlElement("credit_product")]
		public string CreditProduct {get; set;}

		/// <summary>
		/// Количество месяцев кредита
		/// </summary>
		[XmlElement("month_quantity")]
		public string CreditProductMonths {get; set;}

		/// <summary>
		/// Ежемесячный платеж
		/// </summary>
		[XmlElement("month_payment")]
		public string MonthlyPayment {get; set;}

		/// <summary>
		/// Сумма займа
		/// </summary>
		[XmlElement("sum")]
		public string CreditAmount {get; set;}

		/// <summary>
		/// Статус???
		/// </summary>
		[XmlElement("status")]
		public string Status {get; set;}

		/// <summary>
		/// Кредитный эксперт
		/// </summary>
		[XmlElement("inspector")]
		public string Inspector {get; set;}

		/// <summary>
		/// Запрос в НБКИ (???)
		/// </summary>
		[XmlElement("nbki_request")]
		public string nbki_request {get; set;}

		/// <summary>
		/// Оршанизация, выдавшая кредит
		/// </summary>
		[XmlElement("Organization")]
		public string Organization {get; set;}

		/// <summary>
		/// Дата договора
		/// </summary>
		[DataMember(Order = 13)]
		[XmlElement("DateOfCredit")]
		public string DateOfCredit {get; set;}

		/// <summary>
		/// Город
		/// </summary>
		[DataMember(Order = 14)]
		[XmlElement("city")]
		public string City { get; set; }

		/// <summary>
		/// Код города из 1С
		/// </summary>
		[DataMember(Order = 15)]
		[XmlElement("code_city")]
		public string City1CCode { get; set; }

		/// <summary>
		/// Способ расчета
		/// </summary>
		[DataMember(Order = 16)]
		[XmlElement("TypeOfCharge")]
		public string TypeOfCharge { get; set; }

		/// <summary>
		/// Сумма дополнительного соглашения.
		/// </summary>
		[DataMember(Order = 17)]
		[XmlElement("DS")]
		public string AdditionAgreement { get; set; }

		/// <summary>
		/// Преобразовать тип CreditDocument (информация по кредиту из 1С) в InquiryReq (поле в НБКИ - согласие на проверку и прочее)
		/// </summary>
		/// <param name="creditDocument"></param>
		public static explicit operator InquiryReq(CreditDocument creditDocument) {
			InquiryReq inquiryReq	= new InquiryReq() {
				CurrencyCode	= InquiryReq.Currency.RUB,
				inqAmount		= creditDocument.CreditAmount.Split('.').First().Split(',').First(),
				InqPurpose		= InquiryReq.InquiryPurpose.ConsumerCredit
			};

			ConsentReq consentReq	= new ConsentReq() {
				consentFlag	= "Y",  // Клиент дал согласие на запрос КИ
				consentDate			= SOAPNBCH.SOAPNBCH.DateTimeToString(SOAPNBCH.SOAPNBCH.StringToDateTime(creditDocument.DateOfCredit)),
				consentExpireDate	= SOAPNBCH.SOAPNBCH.DateTimeToString(SOAPNBCH.SOAPNBCH.StringToDateTime(creditDocument.DateOfCredit).AddMonths(_DurationOfConsen)),
				ConsentPurpose		= ConsentReq.ConsentPurposee.AgreementConclusionAndFulfilment,
				liability			= "Y",  // Согласие, что ознакомлены с ответственностью по получению КИ...
				reportUser			= creditDocument.Organization
			};

			inquiryReq.ConsentReq	= consentReq;

			return inquiryReq;
		}
		/// <summary>
		/// Приведение типа. Получить список из клиента и поручителей по договору
		/// </summary>
		/// <param name="value">Список из клиента и поручителей по договору</param>
		public static explicit operator Client[](CreditDocument value) {
			if (value == default) throw new ArgumentNullException($"Приведение типа 'CreditDocument' к 'Client[]'.{Environment.NewLine}  Объект с данными клиента 'CreditDocument' не задан");

			List<Client> clients				= new List<Client>();
			ClientProfile clientProfileOwner	= value.Client;

			if (clientProfileOwner != default) {
				Client owner				= clientProfileOwner;
				owner.AffiliationOfAccount	= EAffiliationOfAccount.Owner;
				clients.Add(owner);
			}

			ClientProfile[] clientProfileGuarantors	= value.Guarantors;
			if (clientProfileGuarantors == default) return clients.ToArray();

			foreach (var clientProfileGuarantor in clientProfileGuarantors) {
				Client guarantor				= clientProfileGuarantor;
				guarantor.AffiliationOfAccount	= EAffiliationOfAccount.Guarantor;
				guarantor.GuarantorOnAccount	= value.Code1C;
				clients.Add(guarantor);
			}

			return clients.ToArray();
		}
	}
}
