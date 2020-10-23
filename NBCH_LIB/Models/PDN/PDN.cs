using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using NBCH_LIB.SOAP.SOAPNBCH;
using static NBCH_LIB.SOAP.SOAPNBCH.AccountReply;

namespace NBCH_LIB.Models.PDN {
	/// <summary>
	/// ПДН по счету
	/// </summary>
	[DataContract]
	public class PDN {
		/// <summary>
		/// На сколько дней вперед смотреть, от дня заведения договора, в поисках анкеты НБКИ.
		/// </summary>
		public static readonly int MaxDayAfter	= 5;


		/// <summary>
		/// Максимально допустимое значение ПСК на дату.
		/// </summary>
		private const double _MaxPSK_2020_05_01 = 366;


		/// <summary>
		/// Минимально допустимое значение ПСК на дату.
		/// </summary>
		private const double _MinPSK_2020_05_01 = 0.001;


		/// <summary>
		/// Максимальное кол-во месяцев на дату, в течении которых допускается отсутствие обновлений, и договор считается активным для расчета ПДН.
		/// Если обновлений по договору нет более этого кол-ва месяцев, то счет считается непригодным для расчета ПДН.
		/// </summary>
		private static readonly int MaxActiveMonth_2020_05_01	= 3;

		/// <summary>
		/// Максимальное значение ПСК.
		/// </summary>
		/// <param name="dateTime">Дата на которое берется значение</param>
		/// <returns>Максимальное значение ПСК на дату</returns>
		public static double MaxPSK(DateTime dateTime){
			if (dateTime > new DateTime(2020, 05, 01)) return _MaxPSK_2020_05_01;

			return _MaxPSK_2020_05_01;
		}

		/// <summary>
		/// Минимальное значение ПСК.
		/// </summary>
		/// <param name="dateTime">Дата на которое берется значение</param>
		/// <returns>Минимальное значение ПСК на дату</returns>
		public static double MinPSK(DateTime dateTime) {
			if (dateTime > new DateTime(2020, 05, 01)) return _MinPSK_2020_05_01;

			return _MinPSK_2020_05_01;
		}

		/// <summary>
		/// Если данные по договору выгружаются болше этого кол-ва дней, то договор не участвует в расчете долговой нагрузки.
		/// </summary>
		/// <param name="dateTime">Дата на которое берется значение</param>
		/// <returns>Кол-во месяцев</returns>
		public static int MaxActiveMonth(DateTime dateTime) {
			if (dateTime > new DateTime(2020, 05, 01)) return MaxActiveMonth_2020_05_01;

			return MaxActiveMonth_2020_05_01;
		}

		/// <summary>
		/// Статус договора.
		/// </summary>
		[DataMember]
		public AccountRatingeVLF AccountRatingeVLF {get; set;}

		/// <summary>
		/// Проверить на корректность заполнения.
		/// </summary>
		/// <param name="dateTime">Дата расчета</param>
		/// <returns>Ошибки в данных</returns>
		public virtual string[] CheckPDNError(DateTime dateTime) {return default;}

		/// <summary>
		/// Фабрика PDN объектов.
		/// </summary>
		/// <param name="accountReply">Счет</param>
		/// <param name="calculateDate">Дата расчета</param>
		/// <returns>данные для PDN</returns>
		public static PDN CreatePDNObgect(AccountReply accountReply, DateTime calculateDate) {
			PDN	pdn	= default;

			if (PDNCard.IsCardActiveAccount(accountReply, calculateDate)) pdn = PDNCard.GetPDNCard(accountReply, calculateDate);
			if (PDNNonCard.IsNonCardActiveAccount(accountReply, calculateDate)) pdn = PDNNonCard.GetPDNNonCard(accountReply, calculateDate);

			if (pdn != default) pdn.Errors = pdn.CheckPDNError(calculateDate);

			return pdn;
		}

		/// <summary>
		/// Фабрика PDN объектов.
		/// </summary>
		/// <param name="accountReplys">Счет</param>
		/// <param name="calculateDate">Дата расчета</param>
		/// <returns>данные для PDN</returns>
		public static PDN[] CreatePDNObgect(AccountReply[] accountReplys, DateTime calculateDate) {
			if (accountReplys == default) return default;
			List<PDN> pdns	= new List<PDN>();
			foreach (AccountReply accountReply in accountReplys) {
				pdns.Add(PDN.CreatePDNObgect(accountReply, calculateDate));
			}

			return pdns.Count > 0 ? pdns.ToArray() : default;
		}

		/// <summary>
		/// Ошибка в расчетах.
		/// </summary>
		[DataMember]
		public string[] Errors { get; set; }

		/// <summary>
		/// Создать объект ПДН.
		/// </summary>
		/// <param name="accountReply">Счет</param>
		/// <param name="calculateDate">Дата расчета</param>
		/// <returns></returns>
		public PDN CreatePDN(AccountReply accountReply, DateTime calculateDate) {
			PDN pdn		= PDN.CreatePDNObgect(accountReply, calculateDate);
			pdn.Errors	= pdn?.CheckPDNError(calculateDate);

			return pdn;
		}

		/// <summary>
		/// Рассчитать ПДН.
		/// </summary>
		/// <returns>Значение ПДН</returns>
		public virtual double CalculatePayment(DateTime reportDate = default, bool IgnoreError = false) {return default;}
	}
}
