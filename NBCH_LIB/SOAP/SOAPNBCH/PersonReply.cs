using System.Xml.Serialization;

namespace NBCH_LIB.SOAP.SOAPNBCH {
	public class PersonReply {
		public string serialNum { get; set; }

		public string fileSinceDt { get; set; }

		/// <summary>
		/// Фамилия.
		/// </summary>
		[XmlElement("name1")]
		public string Surname { get; set; }

		/// <summary>
		/// Имя.
		/// </summary>
		[XmlElement("first")]
		public string FirstName { get; set; }

		/// <summary>
		/// Отчество.
		/// </summary>
		[XmlElement("paternal")]
		public string MiddleName { get; set; }

		/// <summary>
		/// Дата Рождения.
		/// </summary>
		[XmlElement("birthDt")]
		public string BirthDate { get; set; }

		public string deathFlag { get; set; }

		/// <summary>
		/// Место рождения.
		/// </summary>
		[XmlElement("placeOfBirth")]
		public string BirthPlace { get; set; }

		public string lastUpdatedDt { get; set; }

		public string freezeFlag { get; set; }

		public string suppressFlag { get; set; }

		public static explicit operator PersonReq(PersonReply personReply){
			PersonReq personReq	= new PersonReq(){
				BirthDate	= personReply.BirthDate,
				BirthPlace	= personReply.BirthPlace,
				FirstName	= personReply.FirstName,
				SecondName	= personReply.MiddleName,
				LastName		= personReply.Surname
			};

			return personReq;
		}
	}
}
