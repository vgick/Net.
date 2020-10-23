using System;
using System.Xml.Serialization;

namespace NBCH_LIB.SOAP.SOAPNBCH {
	/// <summary>
	/// Описание клиента.
	/// </summary>
	public class PersonReq {
		/// <summary>
		/// Фамилия.
		/// </summary>
		[XmlElement("name1")]
		public string LastName { get; set; }

		/// <summary>
		/// Имя.
		/// </summary>
		[XmlElement("first")]
		public string FirstName { get; set; }

		/// <summary>
		/// Отчество.
		/// </summary>
		[XmlElement("paternal")]
		public string SecondName { get; set; }

		/// <summary>
		/// Дата Рождения.
		/// </summary>
		[XmlElement("birthDt")]
		public string BirthDate { get; set; }

		/// <summary>
		/// Место рождения.
		/// </summary>
		[XmlElement("placeOfBirth")]
		public string BirthPlace { get; set; }

		/// <summary>
		/// Дата рождения в формате DateTime (заполняет birthDt).
		/// </summary>
		[XmlIgnore]
		public DateTime BirthDateTime {
			get => SOAPNBCH.StringToDateTime(BirthDate);
			set => BirthDate = SOAPNBCH.DateTimeToString(value);
		}
	}
}
