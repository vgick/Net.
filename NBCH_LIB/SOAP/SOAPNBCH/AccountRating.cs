using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;

namespace NBCH_LIB.SOAP.SOAPNBCH {
	public class AccountReply {
		public string accountRating {get; set;}

		/// <summary>
		/// PDF (Status Date).
		/// </summary>
		public string accountRatingDate {get; set;}

		/// <summary>
		/// Статус договора.
		/// </summary>
		public string accountRatingText { get; set;}

		/// <summary>
		/// Статус договора ВЛФ.
		/// </summary>
		/// <param name="reportDate">Дата отчета</param>
		/// <returns>Статус ВЛФ</returns>
		public string accountRatingTextVLF(DateTime reportDate) {
			if (AccountRating == AccountRatinge.Active &&
				(String.IsNullOrEmpty(amtPastDue) || amtPastDue == "0")
				&& ((SOAPNBCH.StringToDateTime(paymentDueDate) == default ? DateTime.Now : SOAPNBCH.StringToDateTime(paymentDueDate)) < reportDate)
				) return Helper.GetDescription(AccountRatingeVLF.AccountClosedVLF);

			return accountRatingText;
		}

		public string acctNum { get; set; }

		public string acctType {get; set;}

		/// <summary>
		/// Describe acctType (text).
		/// </summary>
		public string acctTypeText {get; set;}

		/// <summary>
		/// Задолж-сть.
		/// </summary>
		public string amtOutstanding {get; set;}

		/// <summary>
		/// Просрочено.
		/// </summary>
		public string amtPastDue {get; set;}

		/// <summary>
		/// Следующий платеж.
		/// </summary>
		public string termsAmt {get; set;}

		public string bankGuaranteeAmt {get; set;}

		public string bankGuaranteeIndicatorCode {get; set;}

		public string bankGuaranteeIndicatorCodeText {get; set;}

		public string bankGuaranteeTerm {get; set;}

		public string bankGuaranteeVolumeCode {get; set;}

		public string bankGuaranteeVolumeCodeText {get; set;}

		/// <summary>
		/// Дата закрытия счета.
		/// </summary>
		public string closedDt {get; set;}

		public string collateral2Text {get; set;}

		public string collateralCode {get; set;}

		public string collateralValue {get; set;}

		public string collateralDate {get; set;}

		public string collateralExpirationDate {get; set;}

		/// <summary>
		/// Размер лимита по карте / сумма кредита.
		/// </summary>
		public string creditLimit {get; set;}

		/// <summary>
		/// ПСК.
		/// </summary>
		public string creditTotalAmt {get; set;}

		/// <summary>
		/// Всего выплачено.
		/// </summary>
		public string curBalanceAmt {get; set;}

		/// <summary>
		/// Сроч.зад.осн.долг (СРЗ).
		/// </summary>
		public string principalOutstanding {get; set;}

		/// <summary>
		/// Валюта договора.
		/// </summary>
		public string currencyCode {get; set;}

		/// <summary>
		/// Валюта договора.
		/// </summary>
		public string CurrencyCode { get => currencyCode == "RUB" || currencyCode == "" ? "руб" : currencyCode; }

		public string disputedDate {get; set;}

		public string disputedRemarks {get; set;}

		public string disputedStatus {get; set;}

		public string disputedStatusText {get; set;}

		public string fileSinceDt {get; set;}

		public string freezeFlag {get; set;}

		public string graceEndDt {get; set;}

		/// <summary>
		/// Сумма поручительства.
		/// </summary>
		public string guaranteeAmt {get; set;}

		public string guaranteeTerm {get; set;}

		public string guaranteeVolumeCode {get; set;}

		public string guaranteeVolumeCodeText {get; set;}

		public string guarantorIndicatorCode {get; set;}

		public string guarantorIndicatorCodeText {get; set;}

		public string interestPaymentDueDate {get; set;}

		public string interestPaymentFrequencyCode {get; set;}

		public string interestPaymentFrequencyText {get; set;}

		/// <summary>
		/// Сроч.задолж. по %%.
		/// </summary>
		public string intOutstanding {get; set;}

		public string intPastDue {get; set;}

		public string lastPaymtDt {get; set;}

		/// <summary>
		/// Дата последнего обновления.
		/// </summary>
		public string lastUpdatedDt {get; set;}

		/// <summary>
		/// Дата последнего обновления.
		/// </summary>
		[XmlIgnore]
		public DateTime LastUpdatedDt {
			get => lastUpdatedDt == default ? default : SOAPNBCH.StringToDateTime(lastUpdatedDt);
			set => lastUpdatedDt = value == default ? default : SOAPNBCH.DateTimeToString(value);
		}

		public string monthsReviewed {get; set;}

		public string numDays30 {get; set;}

		public string numDays60 {get; set;}

		public string numDays90 {get; set;}

		/// <summary>
		/// Дата открытия счета.
		/// </summary>
		public string openedDt {get; set;}

		/// <summary>
		/// Начало истории платежей.
		/// </summary>
		[XmlIgnore]
		public DateTime OpenedDt {
			get => openedDt == default ? default : SOAPNBCH.StringToDateTime(openedDt);
			set => openedDt = value == default ? default : SOAPNBCH.DateTimeToString(value);
		}

		/// <summary>
		/// Иные сроч.задолж-сти.
		/// </summary>
		public string otherAmtOutstanding {get; set;}

		public string otherAmtPastDue {get; set;}

		/// <summary>
		/// Вид счета (личный, поручитель...).
		/// </summary>
		[XmlElement("ownerIndic")]
		public string ownerIndic {get; set;}

		/// <summary>
		/// Вид счета (личный, поручитель...) в текстовом представлении.
		/// </summary>
		[XmlElement("ownerIndicText")]
		public string OwnerIndicText {get; set;}

		/// <summary>
		/// Дата финального платежа.
		/// </summary>
		public string paymentDueDate {get; set;}

		/// <summary>
		/// История платежей (180 месяцев - 15 лет).
		/// </summary>
		[XmlElement("paymtPat")]
		public string PaymentPattern {get; set;}

		/// <summary>
		/// Начало истории платежей.
		/// </summary>
		[XmlElement("paymtPatStartDt")]
		public string PaymtPatStartDt { get; set;}

		/// <summary>
		/// Начало истории платежей.
		/// </summary>
		[XmlIgnore]
		public DateTime PaymtPatStartDateTime {
			get => PaymtPatStartDt == default ? default : SOAPNBCH.StringToDateTime(PaymtPatStartDt);
			set => PaymtPatStartDt = value == default ? default : SOAPNBCH.DateTimeToString(value);
		}

		/// <summary>
		/// Получить историю качества платежей по счету в разрезе месяцев.
		/// При появлении нового статуса, который не указан в перечислении AccountStatus,
		/// выбирается значение AccountStatus.NewStatus
		/// </summary>
		/// <returns>Список месяцев и состояние счета в месяц</returns>
		public SortedDictionary<DateTime, AccountStatus> GetAccountHistory(){
			if (PaymentPattern == default || PaymtPatStartDt == default) return default;

			SortedDictionary<DateTime, AccountStatus> statusHistory	= new SortedDictionary<DateTime, AccountStatus>(Comparer<DateTime>.Create((x, y) => y.CompareTo(x)));
			DateTime firstDate	= PaymtPatStartDateTime;

			foreach (char statusOnMonth in PaymentPattern) {
				AccountStatus status	= AccountStatus.Null;
				if (Enum.IsDefined(typeof(AccountStatus), (int)statusOnMonth))
					status = (AccountStatus)((int)statusOnMonth);
				// todo: добавить картинку и действие на новый статус
				else status = AccountStatus.NewStatus;

				statusHistory.Add(firstDate, status);
				firstDate	= firstDate.AddMonths(-1);
			}

			return statusHistory;
		}

		/// <summary>
		/// PDF (Status Date).
		/// </summary>
		[XmlIgnore]
		public DateTime AccountRatingDate {
			get => accountRatingDate == default ? default : SOAPNBCH.StringToDateTime(accountRatingDate);
			set => accountRatingDate = value == default ? default : SOAPNBCH.DateTimeToString(value);
		}
		[XmlIgnore]
		public AccountRatinge AccountRating {
			get => ((AccountRatinge)int.Parse(accountRating ?? ((int)AccountRatinge.Null).ToString()));
			set {
				string newValue	= ((int)value).ToString();
				accountRating = newValue == AccountRatinge.Null.ToString() ? default : newValue;
			}
		}

		/// <summary>
		/// Статус по договору, скорректированный под внутренюю логику.
		/// </summary>
		public AccountRatingeVLF AccountRatingVLF(DateTime reportDate) {
			if (AccountRating == AccountRatinge.Active &&
				(String.IsNullOrEmpty(amtPastDue) || amtPastDue == "0")
				&& ((SOAPNBCH.StringToDateTime(paymentDueDate) == default ? DateTime.Now : SOAPNBCH.StringToDateTime(paymentDueDate)) < reportDate)
				) return AccountRatingeVLF.AccountClosedVLF;

			return ((AccountRatingeVLF)int.Parse(accountRating ?? ((int)AccountRatingeVLF.Null).ToString()));
		}

		/// <summary>
		/// Статус договора.
		/// </summary>
		[XmlIgnore]
		public AcctTypee AcctType {
			get => ((AcctTypee)int.Parse(acctType ?? ((int)AcctTypee.Null).ToString()));
			set {
				string newValue	= ((int)value).ToString();
				acctType = newValue == AcctTypee.Null.ToString() ? default : newValue;
			}
		}

		[XmlIgnore]
		public OwnerIndice OwnerIndic {
			get => ((OwnerIndice)int.Parse(ownerIndic ?? ((int)OwnerIndice.Null).ToString()));
			set {
				string newValue = ((int)value).ToString();
				ownerIndic = newValue == OwnerIndice.Null.ToString() ? default : newValue;
			}
		}

		/// <summary>
		/// Статус договора.
		/// </summary>
		public enum AccountStatus {
			[Description("Не выбрано")]
			Null			= -1,
			[Description("НОВЫЙ СТАТУС. Необходимо внести изменения в перечисление \"AccountStatus\"")]
			NewStatus		= 0,
			[Description("Новый, оценка невозможна")]
			New				= (int)'0',
			[Description("Оплата без просрочек")]
			Payed			= (int)'1',
			[Description("Просроченно 1-7 дней")]
			Delay_1_7		= (int)'B',
			[Description("Просроченно 1-29 дней")]
			Delay_1_29 = (int)'A',
			[Description("Просроченно 8-29 дней")]
			Delay_8_29		= (int)'C',
			[Description("Просроченно 30-59 дней")]
			Delay_30_59		= (int)'2',
			[Description("Просроченно 60-89 дней")]
			Delay_60_89		= (int)'3',
			[Description("Просроченно 9-119 дней")]
			Delay_90_119	= (int)'4',
			[Description("Просроченно более 120 дней")]
			DelayOver120	= (int)'5',
			[Description("Изменения/дополнения к договору")]
			ChangesAmendmentsToAgreement = (int)'7',
			[Description("Погашение за счет обеспечения")]
			PaidByCollateral = (int)'8',
			[Description("Безнадёжный долг/ передано на взыскание")]
			BadDebtCollectionSkip = (int)'9',
			[Description("Льготный период")]
			GracePeriod = (int)'Z',
			[Description("Нет данных")]
			DataIsNotAvailable	= (int)'X',
		}

		/// <summary>
		/// Принадлежность к счету.
		/// </summary>
		public enum OwnerIndice {
			[Description("Не выбрано")]
			Null = -1,
			[Description("Личный")]
			Individual	= 1,
			[Description("Кредитная карта")]
			SupplementaryCard	= 2,
			[Description("Кредитная карта или доверенность")]
			AuthorizedUser		= 3,
			[Description("Совместный")]
			Joint = 4,
			[Description("Поручитель")]
			Guarantor = 5,
			[Description("Главный")]
			Principal = 6,
			[Description("Бизнес")]
			Business = 9

		}

		/// <summary>
		/// Тип кредтного счета.
		/// </summary>
		public enum AcctTypee {
			[Description("Не выбрано")]
			Null = -1,
			[Description("Автокредит")]
			AutoLoan	= 1,
			[Description("Лизинг")]
			Leasing = 4,
			[Description("Ипотека")]
			Mortgage = 6,
			[Description("Кредитная карта")]
			CreditCard	= 7,
			[Description("Потребительский кредит")]
			ConsumerCredit	= 9,
			[Description("На развитие бизнеса")]
			ForDevelopmentOfBusiness	= 10,
			[Description("На увеличения оборотных средств")]
			ForEnlargingCirculatingAssets	= 11,
			[Description("На приобретения оборудования")]
			ForEquipmentPurchase	= 12,
			[Description("На строительства объектов недвижимости")]
			ForBuildingRealEstate	= 13,
			[Description("Для покупки ценных бумаг")]
			ForSecuritiesPurchase		= 14,
			[Description("Межбанковский кредит")]
			InterBankCredit	= 15,
			[Description("Микрокредит")]
			Microcredit = 16,
			[Description("Дебетовая карта с овердрафтом")]
			OverdraftDebitCard	= 17,
			[Description("Овердрафт")]
			Overdraft = 18
		}

		/// <summary>
		/// Статус договора НБКИ.
		/// </summary>
		public enum AccountRatinge {
			[Description("Не выбрано")]
			Null = -1,
			[Description("Активный")]
			Active = 0,
			[Description("Залог")]
			PaidByCollateral = 12,
			[Description("Счет закрыт")]
			AccountClosed	= 13,
			[Description("Счет закрыт - переведен в другую организацию")]
			AccountClosedTransferedToAnotherOrganization	= 14,
			[Description("По умолчанию или просрочено")]
			Dispute	= 21,
			[Description("По умолчанию или просрочено")]
			InDefaultOrPastDue	= 52,
			[Description("Мошенничество")]
			Fraud = 61,
			[Description("Передача данных остановлена")]
			DataSubmissionStopped	= 70,
			[Description("Обязательный платеж")]
			CompulsoryPayment	= 85,
			[Description("Списано")]
			WriteOff	= 90,
			[Description("Банкротство, освобождение от требований")]
			BankruptcyExemptionFromRequirements	= 95,
			[Description("Bankruptcy revival")]
			BankruptcyRevival	= 96
		}

		/// <summary>
		/// Статус договора ВЛФ.
		/// </summary>
		public enum AccountRatingeVLF {
			[Description("Не выбрано")]
			Null = -1,
			[Description("Активный")]
			Active = 0,
			[Description("Залог")]
			PaidByCollateral = 12,
			[Description("Счет закрыт")]
			AccountClosed = 13,
			[Description("Счет условно закрыт")]
			AccountClosedVLF = -13,
			[Description("Счет закрыт - переведен в другую организацию")]
			AccountClosedTransferedToAnotherOrganization = 14,
			[Description("По умолчанию или просрочено")]
			Dispute = 21,
			[Description("По умолчанию или просрочено")]
			InDefaultOrPastDue = 52,
			[Description("Мошенничество")]
			Fraud = 61,
			[Description("Передача данных остановлена")]
			DataSubmissionStopped = 70,
			[Description("Обязательный платеж")]
			CompulsoryPayment = 85,
			[Description("Списано")]
			WriteOff = 90,
			[Description("Банкротство, освобождение от требований")]
			BankruptcyExemptionFromRequirements = 95,
			[Description("Bankruptcy revival")]
			BankruptcyRevival = 96
		}

	}
}
