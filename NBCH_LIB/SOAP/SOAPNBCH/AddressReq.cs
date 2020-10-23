using System.ComponentModel;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace NBCH_LIB.SOAP.SOAPNBCH {
	/// <summary>
	/// Описание адреса в запросе к веб сервису.
	/// </summary>
	[DataContract]
	public class AddressReq {
		/// <summary>
		/// Номер дома.
		/// </summary>
		[DataMember]
		[XmlElement("houseNumber")]
		public string HouseNumber { get; set; }

		/// <summary>
		/// Улица.
		/// </summary>
		[DataMember]
		[XmlElement("street")]
		public string Street { get; set; }

		/// <summary>
		/// Номер квартиры.
		/// </summary>
		[DataMember]
		[XmlElement("apartment")]
		public string ApartmentNumber { get; set; }

		/// <summary>
		/// Город.
		/// </summary>
		[DataMember]
		[XmlElement("city")]
		public string City { get; set; }

		/// <summary>
		/// Индекс.
		/// </summary>
		[DataMember]
		[XmlElement("postal")]
		public string Postal { get; set; }

		/// <summary>
		/// Тип адреса.
		/// </summary>
		[DataMember]
		[XmlElement("addressType")]
		public string AddressType { get; set; }

		/// <summary>
		/// Район.
		/// </summary>
		[DataMember]
		[XmlElement("district")]
		public string District {get; set;}

		/// <summary>
		/// Привести AddressReq к типу AddressReply.
		/// </summary>
		/// <param name="addressReq">AddressReply</param>
		public static explicit operator AddressReq(AddressReply addressReq){
			// todo: Заполнить не обязательные поля
			AddressReq addressReply	= new AddressReq() {
				AddressType		= addressReq.AddressType,
				ApartmentNumber	= addressReq.Apartment,
				HouseNumber		= addressReq.HouseNumber,
				City			= addressReq.City,
				Street			= addressReq.Street
			};

			return addressReply;
		}

	}

	public enum AddressType {
		[Description("Адрес прописки")]
		Registration	= 1,
		[Description("Адрес проживания")]
		Residence		= 2
	}

}
