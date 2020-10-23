
namespace NBCH_LIB.SOAP.SOAPNBCH {
	/// <summary>
	/// Класс для идентификации банка при подключении
	/// </summary>
	public class RequestorReq {
		/// <summary>
		/// Код банка (12 символов)
		/// </summary>
		public string MemberCode { get; set; }
		/// <summary>
		/// Пользователь
		/// </summary>
		public string UserID { get; set; }
		/// <summary>
		/// пароль
		/// </summary>
		public string Password { get; set; }
	}
}
