
namespace NBCH_LIB.SOAP.SOAP1C {
	/// <summary>
	/// Результат запроса
	/// </summary>
	public class SOAPResponse {
		/// <summary>
		/// Raw ответ SOAP сервиса
		/// </summary>
		public byte[] Response { get; set; }
		/// <summary>
		/// Ошибки при выполнении запроса
		/// </summary>
		public string[] Errors { get; set; }
	}
}
