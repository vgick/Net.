using System.Xml.Serialization;

namespace NBCH_LIB.SOAP.SOAPNBCH {
	/// <summary>
	/// Описание адреса в ответе веб сервиса.
	/// </summary>
	public class AddressReply {
		[XmlElement("serialNum")]
		public string SerialNum { get; set; }

		[XmlElement("fileSinceDt")]		
		public string FileSinceDt { get; set; }

		/// <summary>
		/// Район.
		/// </summary>
		[XmlElement("district")]
		public string District { get; set; }

		/// <summary>
		/// Номер дома.
		/// </summary>
		[XmlElement("houseNumber")]
		public string HouseNumber { get; set; }

		/// <summary>
		/// Улица.
		/// </summary>
		[XmlElement("street")]
		public string Street { get; set; }

		/// <summary>
		/// Блок дома.
		/// </summary>
		[XmlElement("block")]
		public string Block { get; set; }

		/// <summary>
		/// Номер строения.
		/// </summary>
		[XmlElement("building")]
		public string Building { get; set; }

		/// <summary>
		/// Номер квартиры.
		/// </summary>
		[XmlElement("apartment")]
		public string Apartment { get; set; }

		/// <summary>
		/// Город.
		/// </summary>
		[XmlElement("city")]
		public string City { get; set; }

		/// <summary>
		/// Почтовый индекс.
		/// </summary>
		[XmlElement("postal")]
		public string Postal { get; set; }

		/// <summary>
		/// Код страны.
		/// </summary>
		[XmlElement("countryCode")]
		public string CountryCode { get; set; }

		/// <summary>
		/// Тип адреса (регистрации, фактического проживания...).
		/// </summary>
		[XmlElement("addressType")]
		public string AddressType { get; set; }

		/// <summary>
		/// Тип адреса (регистрации, фактического проживания...) В текстовом виде.
		/// </summary>
		[XmlElement("addressTypeText")]
		public string AddressTypeText { get; set; }

		/// <summary>
		/// Дата последнего обновления.
		/// </summary>
		[XmlElement("lastUpdatedDt")]
		public string LastUpdatedDt { get; set; }

		[XmlElement("freezeFlag")]
		public string FreezeFlag { get; set; }

		[XmlElement("suppressFlag")]
		public string SuppressFlag { get; set; }

	}
}