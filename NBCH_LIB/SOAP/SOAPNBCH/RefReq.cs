using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace NBCH_LIB.SOAP.SOAPNBCH {
	public class RefReq {
		/// <summary>
		/// Тип отчета
		/// </summary>
		public string product {get; set;} = "CHST";

		/// <summary>
		/// Тип отчета
		/// </summary>
		[XmlIgnore]
		public ProductType Language {
			get {
				if (!Enum.IsDefined(typeof(ProductType), product ?? "")) return ProductType.CHST;
				return (ProductType)Enum.Parse(typeof(ProductType), product);
			}
			set => product = value.ToString();
		}

		public enum ProductType {
			[Description("Без информационной части")]
			CHST,
			[Description("С информационной частью")]
			CHIP,
			[Description("Только общая часть")]
			CIPO
		}
	}
}
