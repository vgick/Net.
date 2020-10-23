using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using NBCH_LIB.SOAP.SOAPNBCH;

namespace NBCH_LIB.Models.PDN {
	/// <summary>
	/// ПДН по карточному счету.
	/// </summary>
	[DataContract]
	public class PDNCard : PDN {
		/// <summary>
		/// Кредитный лимит.
		/// </summary>
		[DataMember]
		public double CreditLimit {get; set;}

		/// <summary>
		/// Задолженность.
		/// </summary>
		[DataMember]
		public double AmtOutstanding {get; set;}

		/// <summary>
		/// Просрочено.
		/// </summary>
		[DataMember]
		public double AmtPastDue {get; set;}

		/// <summary>
		/// Сроч.задолж. по осн.долгу.
		/// </summary>
		[DataMember]
		public int PrincipalOutstanding {get; set;}

		/// <summary>
		/// Сроч.задолж. по %%.
		/// </summary>
		[DataMember]
		public int IntOutstanding {get; set;}

		/// <summary>
		/// Иные сроч.задолж-сти.
		/// </summary>
		[DataMember]
		public int OtherAmtOutstanding {get; set;}

		/// <summary>
		/// Это активный карточный счет.
		/// </summary>
		/// <returns>Активный карточный счет или нет</returns>
		public static bool IsCardActiveAccount(AccountReply accountReply, DateTime calculateDate) {
//			AmtOutstanding

			bool result	= (accountReply.OwnerIndic != AccountReply.OwnerIndice.Guarantor
				// Кредитки и овердрафты с суммой
				&& (accountReply.AcctType == AccountReply.AcctTypee.CreditCard
					|| ((accountReply.AcctType == AccountReply.AcctTypee.OverdraftDebitCard || accountReply.AcctType == AccountReply.AcctTypee.Overdraft)
						&& (Int32.TryParse(accountReply.principalOutstanding, out int PrincipalOutstanding) && PrincipalOutstanding > 0
							|| Int32.TryParse(accountReply.amtOutstanding, out int AmtOutstanding) && AmtOutstanding > 0)
						)
					)
				&& (accountReply.AccountRatingVLF(calculateDate) == AccountReply.AccountRatingeVLF.Active || accountReply.AccountRatingVLF(calculateDate) == AccountReply.AccountRatingeVLF.InDefaultOrPastDue)
				&& (Decimal.TryParse(accountReply.amtOutstanding, out Decimal amtOutstanding) && amtOutstanding > 0)
				&& accountReply.LastUpdatedDt > calculateDate.AddMonths(-MaxActiveMonth(calculateDate)));

			return result;
		}

		/// <summary>
		/// Получить данные для расчета ПДН по кредитным картам.
		/// </summary>
		/// <param name="accountReply">Данные по карточному счета</param>
		/// <param name="calculateDate">Дата расчета</param>
		/// <returns>Данные для расчета ПДН по карточным счетам</returns>
		public static PDNCard GetPDNCard(AccountReply accountReply, DateTime calculateDate) {
			if (!IsCardActiveAccount(accountReply, calculateDate)) return default;

			PDNCard pdnCard = new PDNCard();

			pdnCard.AccountRatingeVLF = accountReply.AccountRatingVLF(calculateDate);

			if (Double.TryParse(accountReply.amtOutstanding, out Double amtOutstanding)) pdnCard.AmtOutstanding = amtOutstanding;
			else pdnCard.AmtOutstanding = 0;

			if (Double.TryParse(accountReply.amtPastDue, out Double amtPastDue)) pdnCard.AmtPastDue = amtPastDue;
			else pdnCard.AmtPastDue = 0;

			if (Double.TryParse(accountReply.creditLimit, out Double creditLimit)) pdnCard.CreditLimit = creditLimit;
			else pdnCard.CreditLimit = 0;

			if (Int32.TryParse(accountReply.principalOutstanding, out Int32 principalOutstanding)) pdnCard.PrincipalOutstanding = principalOutstanding;
			else pdnCard.PrincipalOutstanding = 0;

			if (Int32.TryParse(accountReply.intOutstanding, out Int32 intOutstanding)) pdnCard.IntOutstanding = intOutstanding;
			else pdnCard.IntOutstanding = 0;

			if (Int32.TryParse(accountReply.otherAmtOutstanding, out Int32 otherAmtOutstanding)) pdnCard.OtherAmtOutstanding = otherAmtOutstanding;
			else pdnCard.OtherAmtOutstanding = 0;

			return pdnCard;
		}

		/// <summary>
		/// Проверить на корректность заполнения.
		/// </summary>
		/// <param name="dateTime">Дата расчета</param>
		/// <returns>Ошибки в данных</returns>
		public override string[] CheckPDNError(DateTime dateTime) {
			List<string> errors	= new List<string>();

			if (AccountRatingeVLF != AccountReply.AccountRatingeVLF.Active && AccountRatingeVLF != AccountReply.AccountRatingeVLF.InDefaultOrPastDue) errors.Add($"Счет не активный. Состояние счета: {Helper.GetDescription(AccountRatingeVLF)}");
			if (AmtOutstanding <= 0) errors.Add($"Задолженность не задана или меньше нуля {AmtOutstanding}");
			if (AmtPastDue < 0) errors.Add($"Сумма просрочки меньше нуля {AmtPastDue}");

			return errors.Count > 0 ? errors.ToArray() : default;
		}

		/// <summary>
		/// Рассчитать платеж ПДН.
		/// </summary>
		/// <returns>Значение платежа ПДН</returns>
		public override double CalculatePayment(DateTime reportDate = default, bool IgnoreError = false) {
			if (!IgnoreError && CheckPDNError(reportDate) != default) return default;

			int amtOutstanding_SRZ = PrincipalOutstanding > 0 ? PrincipalOutstanding + IntOutstanding + OtherAmtOutstanding : (int)AmtOutstanding;

			double payment0_0	= (CreditLimit + this.AmtPastDue) * 0.05;
			double payment0	= payment0_0 > this.AmtPastDue ? payment0_0 : this.AmtPastDue;

			double payment1 = amtOutstanding_SRZ * 0.1 + this.AmtPastDue;

			return payment0 > payment1 ? payment1 : payment0;
		}
	}
}
