using System;
using System.Xml.Serialization;

namespace NBCH_LIB.SOAP.SOAP1C.GetAccountsList {
	/// <summary>
	/// Информация по договорам из 1С в SOAP ответе.
	/// </summary>
	public class AccountLegend : ICloneable {
		/// <summary>
		/// Номер договора.
		/// </summary>
		[XmlElement("doc_namber")]
		public string doc_number { get; set; }

		/// <summary>
		/// Дата договора.
		/// </summary>
		public string doc_date { get; set; }

		/// <summary>
		/// Вид счета (Партнерский...)
		/// </summary>
		public string doc_type { get; set; }

		/// <summary>
		/// Код организации.
		/// </summary>
		public string organization_code { get; set; }

		/// <summary>
		/// Наименование организации.
		/// </summary>
		public string organization_name { get; set; }

		/// <summary>
		/// Город где была заключена сделка.
		/// </summary>
		public string city { get; set; }

		/// <summary>
		/// ФИО клиента.
		/// </summary>
		public string fio { get; set; }

		/// <summary>
		/// Текущий статус договора.
		/// </summary>
		public string doc_status { get; set; }

		/// <summary>
		/// Дата выставления статуса.
		/// </summary>
		public string date_status { get; set; }

		/// <summary>
		/// Дата выставления статуса - действующий.
		/// </summary>
		public string date_status_acting { get; set; }

		/// <summary>
		/// Дата закрытия договора.
		/// </summary>
		public string date_status_closed { get; set; }

		/// <summary>
		/// Клонирование объекта.
		/// </summary>
		/// <returns></returns>
		public object Clone() {
			return this.MemberwiseClone();
		}
	}
}