using System;
using System.Xml.Serialization;

namespace NBCH_LIB.SOAP.SOAPNBCH {
	public class Req {
		/// <summary>
		/// Адрес регистрации и прописки.
		/// </summary>
		[XmlElement]
		public AddressReq[] AddressReq {get; set;}

		/// <summary>
		/// Документы клиента.
		/// </summary>
		[XmlElement]
		public IdReq[] IdReq {get; set;}

		/// <summary>
		/// Цель запроса.
		/// </summary>
		public InquiryReq InquiryReq {get; set;} = new InquiryReq();

		/// <summary>
		/// Данные клиента.
		/// </summary>
		public PersonReq PersonReq {get; set;} = new PersonReq();

		/// <summary>
		/// Данные запрашивающей организации.
		/// </summary>
		public RequestorReq RequestorReq {get; set;} = new RequestorReq();

		public RefReq RefReq {get; set;} = new RefReq();

		/// <summary>.
		/// Тип кредитной истории (полная расширенная, упрощенная)
		/// </summary>
		public string IOType {get => "B2B";}

		/// <summary>
		/// Формат ответа.
		/// </summary>
		public string OutputFormat {get => "XML";}

		/// <summary>
		/// Язык на котором будет предоставлена анкета.
		/// </summary>
		public string lang {get; set;} = "ru";

		/// <summary>
		/// Язык на котором будет предоставлена анкета.
		/// </summary>
		[XmlIgnore]
		public Languagee Language {
			get{
				if (!Enum.IsDefined(typeof(Languagee), lang ?? "")) return Languagee.ru;
				return (Languagee)Enum.Parse(typeof(Languagee), lang);
			}
			set => lang	= value.ToString();
		}

		/// <summary>
		/// Версия пакета обмена данными.
		/// </summary>
		public string version {get; set;} = ProductRequest.RequestVersion;

		public enum Languagee {
			ru,
			en
		}

	}
}
