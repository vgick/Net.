using System;
using System.Xml.Serialization;
using NBCH_LIB.Models;
using NBCH_LIB.SOAP.SOAPNBCH;

namespace NBCH_LIB.SOAP.SOAP1C.GetClientData {
	/// <summary>
	/// Информация о заемщике
	/// </summary>
	public class ClientProfile {
		/// <summary>
		/// Количество адресов клиента. Всегда должно быть 2 (адрес регистрации и адрес проживания).
		/// </summary>
		private const int _AddressCount	= 2;

		/// <summary>
		/// Фамилия клиента.
		/// </summary>
		[XmlElement("s_name")]
		public string LastName { get; set; }

		/// <summary>
		/// Имя клиента.
		/// </summary>
		[XmlElement("f_name")]
		public string FirstName { get; set; }

		/// <summary>
		/// Отчество клиента.
		/// </summary>
		[XmlElement("o_name")]
		public string SecondName { get; set; }

		/// <summary>
		/// Дата рождения клиента (19710624).
		/// </summary>
		[XmlElement("birth_date")]

		public string BirthDate { get; set; }

		/// <summary>
		/// Место рождения клиента.
		/// </summary>
		[XmlElement("birth_place")]
		public string BirthPlace { get; set; }

		/// <summary>
		/// Серия паспорта.
		/// </summary>
		[XmlElement("passport_series")]
		public string PassportSeries { get; set; }

		/// <summary>
		/// Номер паспорта.
		/// </summary>
		[XmlElement("passport_number")]
		public string PassportNumber { get; set; }

		/// <summary>
		/// Дата выдачи паспорта (20160715).
		/// </summary>
		[XmlElement("passport_given_when")]
		public string PassportIssueDate { get; set; }

		/// <summary>
		/// Место выдачи паспорта.
		/// </summary>
		[XmlElement("passport_given_whom")]
		public string PassportIssuePlace { get; set; }

		/// <summary>
		/// Город проживания по регистрации.
		/// </summary>
		[XmlElement("reg_city")]
		public string RegistrationCity { get; set; }

		/// <summary>
		/// Улица проживания по регистрации.
		/// </summary>
		[XmlElement("reg_street")]
		public string RegistrationStreet { get; set; }

		/// <summary>
		/// Номер дома проживания по регистрации.
		/// </summary>
		[XmlElement("reg_house")]
		public string RegistrationHouseNumber { get; set; }

		/// <summary>
		/// Блок дома проживания по регистрации.
		/// </summary>
		[XmlElement("reg_corp")]
		public string RegistrationBlock { get; set; }

		/// <summary>
		/// Номер квартиры проживания по регистрации.
		/// </summary>
		[XmlElement("reg_flat")]
		public string RegistrationApartmentNumber { get; set; }

		/// <summary>
		/// Город фактического проживания.
		/// </summary>
		[XmlElement("lives_city")]
		public string ResidenceCity { get; set; }

		/// <summary>
		/// Улица фактического проживания.
		/// </summary>
		[XmlElement("lives_street")]
		public string ResidenceStreet { get; set; }

		/// <summary>
		/// Номер дома фактического проживания.
		/// </summary>
		[XmlElement("lives_house")]
		public string ResidenceHouseNumber  { get; set; }

		/// <summary>
		/// Блок дома фактического проживания.
		/// </summary>
		[XmlElement("lives_corp")]
		public string ResidenceDuildingBlock { get; set; }

		/// <summary>
		/// Номер квартиры, фактического проэивания.
		/// </summary>
		[XmlElement("lives_flat")]
		public string ResidenceApartmentNumber { get; set; }

		/// <summary>
		/// Номер телефона, сотовой связи.
		/// </summary>
		[XmlElement("phone_mobile")]
		public string MobilePhone { get; set; }

		/// <summary>
		/// Номер домашнего телефона.
		/// </summary>
		[XmlElement("phone_home")]
		public string HomePhone { get; set; }

		/// <summary>
		/// ИНН клиента.
		/// </summary>
		[XmlElement("INN")]
		public string INN { get; set; }

		/// <summary>
		/// Место работы.
		/// </summary>
		[XmlElement("work_place")]
		public string WorkPlace { get; set; }

		/// <summary>
		/// Адрес работы.
		/// </summary>
		[XmlElement("work_address")]
		public string WorkAddress { get; set; }

		/// <summary>
		/// Должность.
		/// </summary>
		[XmlElement("position")]
		public string Position { get; set; }

		/// <summary>
		/// Месяцев работы на текущем месте работы.
		/// </summary>
		[XmlElement("work_experience")]
		public string WorkExperienceMonth { get; set; }

		/// <summary>
		/// Руководитель.
		/// </summary>
		[XmlElement("the_chief")]
		public string Chief { get; set; }

		/// <summary>
		/// Номер телефона руководителя.
		/// </summary>
		[XmlElement("phone_chief")]
		public string ChiefPhone { get; set; }

		/// <summary>
		/// Код клиента.
		/// </summary>
		[XmlElement("code")]
		public string ID1C { get; set; }

		/// <summary>
		/// Основной заработок.
		/// </summary>
		[XmlElement("profit")]
		public string Salary { get; set; }

		/// <summary>
		/// Дополнительный заработок.
		/// </summary>
		[XmlElement("add_profit")]
		public string Additionincome { get; set; }

		/// <summary>
		/// Преобразовать информацию об адресах клиента из 1С в адреса НБКИ.
		/// </summary>
		/// <param name="clientProfile">Адреса прописки и проживания для НБКИ</param>
		public static explicit operator AddressReq[](ClientProfile clientProfile){
			AddressReq registration	= new AddressReq(){
				AddressType	= ((int)AddressType.Registration).ToString(),
				City			= clientProfile.RegistrationCity,
				Street			= clientProfile.RegistrationStreet,
				HouseNumber		= clientProfile.RegistrationHouseNumber,
				ApartmentNumber	= clientProfile.RegistrationApartmentNumber
			};

			AddressReq residence	= new AddressReq(){
				AddressType	= ((int)AddressType.Residence).ToString(),
				City			= clientProfile.ResidenceCity,
				Street			= clientProfile.ResidenceStreet,
				HouseNumber		= clientProfile.ResidenceHouseNumber,
				ApartmentNumber	= clientProfile.ResidenceApartmentNumber
			};

			return new AddressReq[] {registration, residence};
		}

		/// <summary>
		/// Преобразовать информацию о паспорте клиента из 1С в документ НБКИ.
		/// </summary>
		/// <param name="clientProfile">Документ НБКИ (паспорт)</param>
		public static explicit operator IdReq(ClientProfile clientProfile){
			IdReq idReq	= new IdReq{
				DocumentType	= ((int)DocumentType.RussianPassport).ToString(),
				DocumentSeries	= clientProfile.PassportSeries,
				DocumentNumber	= clientProfile.PassportNumber,
				IssueDate		= SOAPNBCH.SOAPNBCH.Date1CToDateNBCH(clientProfile.PassportIssueDate),
				IssueAuthority	= clientProfile.PassportIssuePlace
			};

			return idReq;
		}

		/// <summary>
		/// Приведение ClientProfile к PersonReq.
		/// </summary>
		/// <param name="clientProfile"PersonReq></param>
		public static explicit operator PersonReq(ClientProfile clientProfile){
			PersonReq personReq	= new PersonReq(){
				LastName	= clientProfile.LastName,
				FirstName	= clientProfile.FirstName,
				SecondName	= clientProfile.SecondName,
				BirthDate	= SOAPNBCH.SOAPNBCH.Date1CToDateNBCH(clientProfile.BirthDate),
				BirthPlace	= clientProfile.BirthPlace
			};

			return personReq;
		}

		/// <summary>
		/// Приведение ClientProfile к Client.
		/// </summary>
		/// <param name="clientProfile">ClientProfile</param>
		public static implicit operator Client(ClientProfile clientProfile) {
			if (clientProfile == default) throw new ArgumentNullException($"Привведение типа 'ClientProfile' к 'Client'.{Environment.NewLine}  Объект с данными клиента 'ClientProfile' не задан");

			Client client	= new Client() {
				Code1C		= clientProfile.ID1C,
				FirstName	= clientProfile.FirstName,
				SecondName	= clientProfile.SecondName,
				LastName	= clientProfile.LastName,
				BirthDate	= SOAP1C.StringToDateTime(clientProfile.BirthDate),
			};

			return client;
		}
	}
}
