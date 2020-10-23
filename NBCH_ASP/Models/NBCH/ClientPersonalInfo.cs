using NBCH_LIB.SOAP.SOAPNBCH;

namespace NBCH_ASP.Models.NBCH {
	/// <summary>
	/// Персональные данные клиента.
	/// </summary>
	public class ClientPersonalInfo {
		/// <summary>
		/// Информация по клиенту для НБКИ.
		/// </summary>
		public PersonReq PersonReq {get; set;}

		/// <summary>
		/// Документы клиента для НБКИ.
		/// </summary>
		public IdReq IdReq {get; set;}

		/// <summary>
		/// Адреса клиента для НБКИ.
		/// </summary>
		public AddressReq[] AddressReq {get; set;}
	}
}
