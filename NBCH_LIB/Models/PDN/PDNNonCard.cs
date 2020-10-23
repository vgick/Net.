using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using NBCH_LIB.SOAP.SOAPNBCH;

namespace NBCH_LIB.Models.PDN {
	/// <summary>
	/// ПДН по НЕ карточному счету
	/// </summary>
	[DataContract]
	public class PDNNonCard : PDN {
		/// <summary>
		/// Дата договора.
		/// </summary>
		[DataMember]
		public DateTime AccountDate { get; set; }

		/// <summary>
		/// Кредитный лимит/ Срз.
		/// </summary>
		[DataMember]
		public double CreditLimitSrz {get; set;}

		/// <summary>
		/// ПСК.
		/// </summary>
		[DataMember]
		public double PSK {get; set;}

		/// <summary>
		/// Дата открытия.
		/// </summary>
		[DataMember]
		public DateTime OpenDate {get; set;}

		/// <summary>
		/// Дата финального платежа.
		/// </summary>
		[DataMember]
		public DateTime PaymentDueDate {get; set;}

		/// <summary>
		/// Просрочено.
		/// </summary>
		[DataMember]
		public double AmtPastDue {get; set;}

		/// <summary>
		/// По поле CreditLimitSrz, хранится не СРЗ, задолженность.
		/// </summary>
		[DataMember]
		public bool NoSrz {get; set;}

		/// <summary>
		/// Это активный НЕ карточный счет.
		/// </summary>
		/// <returns>Активный НЕ карточный счет или нет</returns>
		public static bool IsNonCardActiveAccount(AccountReply accountReply, DateTime calculateDate) {
			return (accountReply.OwnerIndic != AccountReply.OwnerIndice.Guarantor
				&& accountReply.AcctType != AccountReply.AcctTypee.CreditCard
				&& accountReply.AcctType != AccountReply.AcctTypee.Overdraft
				&& accountReply.AcctType != AccountReply.AcctTypee.OverdraftDebitCard
				&& (accountReply.AccountRatingVLF(calculateDate) == AccountReply.AccountRatingeVLF.Active || accountReply.AccountRatingVLF(calculateDate) == AccountReply.AccountRatingeVLF.InDefaultOrPastDue)
				&& accountReply.LastUpdatedDt > calculateDate.AddMonths(-MaxActiveMonth(calculateDate)));
		}

		/// <summary>
		/// Получить данные для расчета ПДН по кредитным картам.
		/// </summary>
		/// <param name="accountReply">Данные по карточному счета</param>
		/// <param name="calculateDate">Дата расчета</param>
		/// <returns>Данные для расчета ПДН по карточным счетам</returns>
		public static PDNNonCard GetPDNNonCard(AccountReply accountReply, DateTime calculateDate) {
			if (!IsNonCardActiveAccount(accountReply, calculateDate)) return default;

			PDNNonCard pdnCard = new PDNNonCard();

			pdnCard.AccountRatingeVLF	= accountReply.AccountRatingVLF(calculateDate);
			pdnCard.CreditLimitSrz		= GetCreditLimitSrz(accountReply);
			pdnCard.NoSrz				= isNoSrz(accountReply);
			pdnCard.AccountDate			= accountReply.OpenedDt;
			pdnCard.OpenDate			= pdnCard.NoSrz ? accountReply.OpenedDt : calculateDate;
			pdnCard.OpenDate			= new DateTime(pdnCard.OpenDate.Year, pdnCard.OpenDate.Month, pdnCard.OpenDate.Day);

			if (Double.TryParse(accountReply.creditTotalAmt, out Double psk)) pdnCard.PSK = psk;
			else pdnCard.PSK = 0;

			if (DateTime.TryParse(accountReply.paymentDueDate, out DateTime paymentDueDate)) pdnCard.PaymentDueDate = paymentDueDate;
			else pdnCard.PaymentDueDate = default;

			if (Double.TryParse(accountReply.amtPastDue, out Double amtPastDue)) pdnCard.AmtPastDue = amtPastDue;
			else pdnCard.AmtPastDue = 0;

			return pdnCard;
		}

		/// <summary>
		/// Узнать СРЗ по счету или же кредитный лимит, если нет СРЗ.
		/// </summary>
		/// <param name="accountReply">счет с СРЗ/задолженностью</param>
		/// <returns>СРЗ/Задолженность</returns>
		private static double GetCreditLimitSrz(AccountReply accountReply) {
			if (isNoSrz(accountReply)) {
				if (Double.TryParse((String.IsNullOrEmpty(accountReply.creditLimit) ? "0" : accountReply.creditLimit), out Double resultNoSrz))
					return resultNoSrz;
				return 0;
			}

			if (Double.TryParse((String.IsNullOrEmpty(accountReply.principalOutstanding) ? "0" : accountReply.principalOutstanding), out Double resultSrz))
				return resultSrz;

			return 0;
		}

		/// <summary>
		/// Узнать - в счете нет данных СРЗ.
		/// </summary>
		/// <param name="accountReply">счет</param>
		/// <returns>Есть или нет данных по СРЗ</returns>
		private static bool isNoSrz(AccountReply accountReply) {
			if (!Decimal.TryParse(accountReply.principalOutstanding, out decimal val) || val == 0) return true;

			return false;
		}

		/// <summary>
		/// Проверить данные на ошибки.
		/// </summary>
		/// <returns>Массив со списком ошибок</returns>
		public override string[] CheckPDNError(DateTime dateTime) {
			List<string> errors = new List<string>();

			if (AccountRatingeVLF != AccountReply.AccountRatingeVLF.Active && AccountRatingeVLF != AccountReply.AccountRatingeVLF.InDefaultOrPastDue) errors.Add($"Счет не активный. Состояние счета: {Helper.GetDescription(AccountRatingeVLF)}");
			if (CreditLimitSrz <= 0) errors.Add($"Кредитный лимит (СРЗ) не задан или меньше нуля {CreditLimitSrz}");
			if (PSK > MaxPSK(dateTime)) errors.Add($"ПСК больше допустимого значения. Текущее значение ПСК: {PSK}, максимальное значение ПСК: {MaxPSK(dateTime)}");
			if (PSK < MinPSK(dateTime)) errors.Add($"ПСК меньше допустимого значения. Текущее значение ПСК: {PSK}, минимальное значение ПСК: {MinPSK(dateTime)}");
			if (OpenDate > dateTime) errors.Add($"Дата открытия, больше даты расчета. Дата открытия: {OpenDate.ToShortDateString()}, дата расчета: {dateTime.ToShortDateString()}");
			if (OpenDate == default) errors.Add($"Не задана дата открытия");
			if (PaymentDueDate == default) errors.Add($"Не задана дата последнего платежа");
			if (AmtPastDue < 0) errors.Add($"Сумма просрочки не может быть меньше нуля. Сумма просрочки: {AmtPastDue}, дата расчета: {AmtPastDue}");

			return errors.Count > 0 ? errors.ToArray() : default;
		}

		/// <summary>
		/// Рассчитать платеж для ПДН.
		/// Формула взята у 1Сников и с небольшими изменениями адаптирована.
		/// </summary>
		/// <param name="reportDate">Дата расчета</param>
		/// <returns>Платеж для ПДН</returns>
		public override double CalculatePayment(DateTime reportDate = default, bool IgnoreError = false) {
			if (reportDate == default) throw new ArgumentNullException("Для расчета не карточных счетов, требуется указать дату на которую необхожимо сформировать отчет");

			if (!IgnoreError && CheckPDNError(reportDate) != default) return default;

			if (this.PaymentDueDate < this.OpenDate) {
				DateTime tmp		= this.OpenDate;
				this.OpenDate		= this.PaymentDueDate;
				this.PaymentDueDate	= this.OpenDate;
			}

			double days	= this.PaymentDueDate.Subtract(this.OpenDate).TotalDays;

			DateTime zeroTime = new DateTime(1, 1, 1);

			int pw			= default;
			double pskKor	= default;
			if (days <= 30) {
				pskKor	= this.PSK / 100 * (days/30) / 12;
				pw		= 1;
			}
			else {
				pskKor	= this.PSK / 100 / 12;

				if (this.PaymentDueDate > reportDate) {
					pw		= this.NoSrz ? (zeroTime + (this.PaymentDueDate - this.OpenDate)).Month + ((zeroTime + (this.PaymentDueDate - this.OpenDate)).Year - 1) * 12 :
						(zeroTime + (this.PaymentDueDate - reportDate)).Month + ((zeroTime + (this.PaymentDueDate - reportDate)).Year -1) * 12;
				}
				else {
					return this.AmtPastDue;
				}
			}

			return (1 - Math.Pow(1 + pskKor, -pw)) == 0 ? this.AmtPastDue : (pskKor * this.CreditLimitSrz) / (1 - Math.Pow(1 + pskKor, -pw)) + this.AmtPastDue;
		}

	}
}
