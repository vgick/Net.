using System;

namespace NBCH_LIB.SOAP.SOAP1C {
	/// <summary>
	/// Ответ SOAP сервиса с информацией о возможных ошибках при выполнении.
	/// </summary>
	internal class ISOAPMethodResult<T> {
		
		/// <summary>
		/// Время обновления данных.
		/// </summary>
		public DateTime UpdateTime { get; set; }

		/// <summary>
		/// Ответ сервиса.
		/// </summary>
		public T Data { get; set; }

		/// <summary>
		/// Возможные ошибки при выполнении запроса.
		/// </summary>
		public string Error { get; set; }
	}
}
