using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NBCH_LIB.SOAP.SOAPNBCH;

namespace NBCH_ASP.Infrastructure {
	public static class CreditAccount {
		/// <summary>
		/// Цвет фона и шрифта с привязкой к статусу договора.
		/// </summary>
		/// <param name="accountRating">Статус договора</param>
		/// <param name="ownerIndice">Основной заемщик или поручитель</param>
		/// <returns></returns>
		public static string GetCardHeadStyle(AccountReply.AccountRatingeVLF accountRating, AccountReply.OwnerIndice ownerIndice) =>
			accountRating switch {
				AccountReply.AccountRatingeVLF.BankruptcyRevival		=> " bg-danger ",
				AccountReply.AccountRatingeVLF.BankruptcyExemptionFromRequirements			=> " bg-danger ",
				AccountReply.AccountRatingeVLF.WriteOff				=> " bg-danger ",
				AccountReply.AccountRatingeVLF.DataSubmissionStopped	=> " bg-warning ",
				AccountReply.AccountRatingeVLF.Fraud					=> " bg-danger ",
				AccountReply.AccountRatingeVLF.InDefaultOrPastDue	=> " bg-warning ",
				AccountReply.AccountRatingeVLF.Dispute				=> " bg-warning ",
				AccountReply.AccountRatingeVLF.Active				=> ownerIndice != AccountReply.OwnerIndice.Guarantor ? " bg-info text-white " : " bg-secondary text-white ",
				AccountReply.AccountRatingeVLF.PaidByCollateral		=> " bg-info text-white ",
				AccountReply.AccountRatingeVLF.AccountClosed			=> " bg-light ",
				AccountReply.AccountRatingeVLF.AccountClosedVLF		=> " bg-light ",
				AccountReply.AccountRatingeVLF.AccountClosedTransferedToAnotherOrganization	=> " bg-light ",
				AccountReply.AccountRatingeVLF.CompulsoryPayment		=> " bg-dark text-white ",
				_ => " bg-dark text-white",
			};

		/// <summary>
		/// Возвращает сумму договора.
		/// </summary>
		/// <param name="accountReply"></param>
		/// <returns></returns>
		public static string GetAccountAmount(AccountReply accountReply) {
			int accountAmount;
			string currency		= accountReply.CurrencyCode;
			try {
				accountAmount = accountReply.OwnerIndic == AccountReply.OwnerIndice.Guarantor ? int.Parse(accountReply.guaranteeAmt ?? "0") : int.Parse(accountReply.creditLimit ?? "0");
			}
			catch (Exception) {
				return $@"Ошибка. {accountReply.OwnerIndic.ToString()}, guaranteeAmt:{accountReply.guaranteeAmt ?? "0"}, creditLimit: {accountReply.creditLimit ?? "0"}";
			}

			return $"{accountAmount:#,0} {currency}";
		}

		/// <summary>
		/// Возвращает число в строковом виде, с пробелами.
		/// </summary>
		/// <param name="value">строка числа</param>
		/// <param name="decimalValue">Число с дробной частью</param>
		/// <returns>строка числа с пробелами</returns>
		public static string NumberStringWithSpace(string value, bool decimalValue = false) {
			if (!decimal.TryParse(value, out decimal doubleValue))
				return "0";

			if (decimalValue)
				return $"{doubleValue:##,##0.000}";

			return $"{doubleValue:#,0}";
		}
	}
}
