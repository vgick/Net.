using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Extensions.Logging;
using NBCH_EF.Tables.Interface;
using NBCH_LIB.SOAP.SOAP1C.GetClientData;
using NBCH_LIB.SOAP.SOAPNBCH;

namespace NBCH_EF.Tables {
	/// <summary>
	/// Информация по счету из 1С.
	/// </summary>
	[Table("Account1C")]
	internal class Account1C : IDBTable {
		/// <summary>
		/// Номер счета 1С.
		/// </summary>
		[Key]
		[Column("Account1CCode")]
		[MaxLength(128)]
		public string Account1CCode {get; set;}

		/// <summary>
		/// Дата договора 1С.
		/// </summary>
		[Required]
		[Column(TypeName = "datetime")]
		public DateTime DateTime {get; set;}

		/// <summary>
		/// Данные для расчета ПДН.
		/// </summary>
		public IEnumerable<PDNData> PDNData {get; set;}

		/// <summary>
		/// Платеж для ПДН, рассчитанный по данным НБКИ.
		/// </summary>
		public double Payments {get; set;}

		/// <summary>
		/// Ошибка при расчете ПДН.
		/// </summary>
		[Column("PDNError")]
		public bool PDNError {get; set;}

		/// <summary>
		/// Ручная корректировка ПДН.
		/// </summary>
		[Column("PDNManual")]
		public bool PDNManual {get; set;}

		/// <summary>
		/// Принять текущий расчет ПДН как действительный (принять возможные ошибки в расчете, как допустимые).
		/// </summary>
		[Column("PDNAccept")]
		public bool PDNAccept {get; set;}

		/// <summary>
		/// Анкета НБКИ, используемая для расчета ПДН.
		/// </summary>
		[Column("PDNCreditHistoryAnket")]
		public int PDNCreditHistoryAnket {get; set;}

		/// <summary>
		/// Город в котором заведен договор.
		/// </summary>
		[Column("City_ID")]
		// todo: сделать обязательным
		public int? CityID { get; set; }
		public City City { get; set; }

		/// <summary>
		/// Клиента на которого оформлен договор.
		/// </summary>
		[Column("Client_ID")]
		// todo: сделать обязательным
		public int? ClientID { get; set; }
		public ClientDB Client { get; set; }

		/// <summary>
		/// Организация заключившая сделку.
		/// </summary>
		[Required]
		[Column("Organization_ID")]
		public int OrganizationID { get; set; }
		public OrganizationDB Organization { get; set; }

		/// <summary>
		/// Точка заключения договора.
		/// </summary>
		[Required]
		[Column("SellPont_ID")]
		public int SellPontID { get; set; }
		public SellPontDB SellPont { get; set; }

		/// <summary>
		/// Дополнительное соглашение.
		/// </summary>
		public bool AdditionAgrement { get; set; }

		/// <summary>
		/// Канал перевода средств.
		/// </summary>
		[Column("TypeOfCharge_ID")]
		public int? TypeOfChargeID { get; set; }
		public TypeOfChargeDB TypeOfCharge { get; set; }

		/// <summary>
		/// Поручители.
		/// </summary>
		public List<GuarantorDB> GuarantorDBs { get; set; }

		/// <summary>
		/// Проверяющий.
		/// </summary>
		public List<AccountInspectingDB> AccountInspectingDbs { get; set; } = new List<AccountInspectingDB>();

		/// <summary>
		/// Сравнение объектов по параметрам 1С.
		/// </summary>
		/// <param name="account1C">Объект для сравнения</param>
		/// <returns>Результат сравнения</returns>
		public bool Equals1CParams(Account1C account1C) {
			if (account1C == default)
				return false;

			if (!Account1CCode.ToUpper().Equals(account1C.Account1CCode?.ToUpper()  ??"") || !(Client?.Equals1C(account1C.Client) ?? false) || !DateTime.Equals(account1C.DateTime))
				return false;

			return true;
		}

		/// <summary>
		/// Приведение CreditDocument к Account1C.
		/// </summary>
		/// <param name="creditDocument">Account1C</param>
		public static explicit operator Account1C(CreditDocument creditDocument) {
			Account1C account1C;
			try {
				account1C = new Account1C() {
					Account1CCode		= creditDocument.Code1C,
					DateTime			= SOAPNBCH.StringToDateTime(creditDocument.DateOfCredit),
					Client				= (ClientDB) creditDocument.Client,
					Organization		= new OrganizationDB() {Name = creditDocument.Organization},
					TypeOfCharge		= new TypeOfChargeDB() {Name = creditDocument.TypeOfCharge},
					// todo: Добавить код подразделения после обновления WEB сервиса 1С.
					SellPont			= new SellPontDB() {Name = creditDocument.PointOfSale},
					AdditionAgrement	= !string.IsNullOrEmpty(creditDocument?.AdditionAgreement)
				};
			}
			catch (Exception exception) {
				ILogger<Account1C> logger	= MKKContext.LoggerFactory.CreateLogger<Account1C>();
				logger.LogError("Не удалось привести CreditDocument {CreditDocument} к Account1C. Ошибка {exception}", creditDocument, exception);
				return default;
			}

			return account1C;
		}
	}
}
