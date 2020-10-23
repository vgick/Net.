using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace NBCH_LIB.Models {
	/// <summary>
	/// Описание клиента 1С
	/// </summary>
	[DataContract]
	public class Client {
		private static Client _NullClient {get; set;}
		/// <summary>
		/// Код клиента из 1С.
		/// </summary>
		[DataMember]
		[Required]
		public string Code1C {get; set;}

		/// <summary>
		/// Фамилия клиента.
		/// </summary>
		[DataMember]
		[Required]
		public string LastName {get; set;}

		/// <summary>
		/// Имя клиента.
		/// </summary>
		[DataMember]
		[Required]
		public string FirstName {get; set;}

		/// <summary>
		/// Отчество клиента.
		/// </summary>
		[DataMember]
		public string SecondName {get; set;}

		/// <summary>
		/// Дата рождения клиента.
		/// </summary>
		[DataMember]
		[Required]
		public DateTime BirthDate {get; set;}

		/// <summary>
		/// Принадлежность к счету: владелец, поручитель.
		/// </summary>
		[Required]
		public EAffiliationOfAccount AffiliationOfAccount {get; set;}

		/// <summary>
		/// Поручитель в договоре.
		/// </summary>
		public string GuarantorOnAccount { get; set; }

		/// <summary>
		/// ФИО клиента одной строкой.
		/// </summary>
		public string FIO {
			get { return $"{LastName?.Trim() ?? ""} {FirstName?.Trim() ?? ""} {SecondName?.Trim() ?? ""}".Replace("  ", " ").Replace("  ", " "); }
		}

		/// <summary>
		/// Дефолтный, пустой объект.
		/// </summary>
		/// <returns>Пустой обхект Client</returns>
		public static Client NullClient {
			get {
				if (_NullClient == default)
					_NullClient	= new Client() {
						LastName	= "Клиент",
						FirstName	= "не",
						SecondName	= "задан",
						Code1C		= default,
						BirthDate	= default
					};

				return _NullClient;
			}
		}

		/// <summary>
		/// Принадлежность к счету: владелец, поручитель.
		/// </summary>
		public enum EAffiliationOfAccount {
			[Description("Основной заемщик")]
			Owner,
			[Description("Поручитель")]
			Guarantor
		}
	}
}
