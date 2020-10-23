using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NBCH_EF.Tables.Interface;
using NBCH_LIB.Models.PDN;

namespace NBCH_EF.Tables {
	/// <summary>
	/// Данные для расчета ПДН.
	/// </summary>
	[Table("PDNDatas")]
	internal class PDNData : IDBTableID {
		/// <summary>
		/// Ключ.
		/// </summary>
		[Column("ID")]
		public int ID { get; set; }

		/// <summary>
		/// Счет в 1С.
		/// </summary>
		[Required]
		[Column("Account1C_Account1CCode")]
		public string Account1CAccount1CCode { get; set; }
		public Account1C Account1C { get; set; }

		/// <summary>
		/// Вариант расчета ПДН.
		/// </summary>
		[Column("PDNCalculateType")]
		public PDNCalculateType PDNCalculateType { get; set; }

		/// <summary>
		/// Кредитный лимит (СРЗ).
		/// </summary>
		public double CreditLimit { get; set; }

		/// <summary>
		/// Задолженность.
		/// </summary>
		public double AmtOutstanding { get; set; }

		/// <summary>
		/// Просрочено.
		/// </summary>
		public double AmtPastDue { get; set; }

		/// <summary>
		/// ПСК.
		/// </summary>
		[Column("PSK")]
		public double PSK { get; set; }

		/// <summary>
		/// Дата открытия.
		/// </summary>
		[Column(TypeName = "datetime")]
		public DateTime? OpenDate { get; set; }

		/// <summary>.
		/// Дата финального платежа
		/// </summary>
		[Column(TypeName = "datetime")]
		public DateTime? PaymentDueDate { get; set; }

		/// <summary>
		/// По поле CreditLimitSrz, хранится не СРЗ, задолженность.
		/// </summary>
		public bool NoSrz {get; set;}

		/// <summary>
		/// Платеж.
		/// </summary>
		public double Payment {get; set;}

		/// <summary>
		/// Дата договора НБКИ.
		/// </summary>
		[Column(TypeName = "datetime")]
		public DateTime? AccountDate { get; set; }

		/// <summary>
		/// Ошибка в расчетах.
		/// </summary>
		[StringLength(512)]
		public string Error { get; set; }

		/// <summary>
		/// Приведение к типа PDNCard.
		/// </summary>
		/// <param name="pdnData"></param>
		public static implicit operator PDNCard (PDNData pdnData){
			return new PDNCard() {
				AmtOutstanding	= pdnData.AmtOutstanding,
				AmtPastDue		= pdnData.AmtPastDue,
				CreditLimit		= pdnData.CreditLimit,
				Errors			= pdnData.Error?.Split('\n'),
			};
		}

		/// <summary>
		/// Приведение из типа PDNCard.
		/// </summary>
		/// <param name="pdnCard"></param>
		public static implicit operator PDNData(PDNCard pdnCard) {
			return new PDNData() {
				AmtOutstanding		= pdnCard.AmtOutstanding,
				AmtPastDue			= pdnCard.AmtPastDue,
				CreditLimit			= pdnCard.CreditLimit,
				Error				= pdnCard.Errors != default ? String.Join("\n", pdnCard.Errors ?? new [] {"null"}) : default,
				PDNCalculateType	= PDNCalculateType.Card,
			};
		}

		/// <summary>
		/// Приведение к типа PDNNonCard.
		/// </summary>
		/// <param name="pdnData"></param>
		public static implicit operator PDNNonCard(PDNData pdnData) {
			return new PDNNonCard() {
				AmtPastDue		= pdnData.AmtPastDue,
				CreditLimitSrz	= pdnData.CreditLimit,
				NoSrz			= pdnData.NoSrz,
				OpenDate		= pdnData.OpenDate ?? default,
				PaymentDueDate	= pdnData.PaymentDueDate ?? default,
				PSK				= pdnData.PSK,
				Errors			= pdnData.Error?.Split('\n'),
				AccountDate		= pdnData.AccountDate ?? default
			};
		}

		/// <summary>
		/// Приведение из типа PDNNonCard.
		/// </summary>
		/// <param name="pdnData"></param>
		public static implicit operator PDNData(PDNNonCard pdnData) {
			return new PDNData() {
				AmtPastDue			= pdnData.AmtPastDue,
				CreditLimit			= pdnData.CreditLimitSrz,
				NoSrz				= pdnData.NoSrz,
				OpenDate			= pdnData.OpenDate,
				PaymentDueDate		= pdnData.PaymentDueDate,
				PSK					= Math.Round(pdnData.PSK, 3),
				Error				= pdnData.Errors != default ? String.Join("\n", pdnData.Errors ?? new [] {"null"}) : default,
				PDNCalculateType	= PDNCalculateType.NonCard,
				AccountDate			= pdnData.AccountDate
			};
		}
	}
}
