
using System;

namespace NBCH_LIB.Logger {
	/// <summary>
	/// Сообщение с параметрами для логгера.
	/// </summary>
	public readonly struct LogShortMessage {
		/// <summary>
		/// Конструктор.
		/// </summary>
		/// <param name="message">Форматированное сообщение</param>
		/// <param name="params">Параметры для строки сообщения</param>
		public LogShortMessage(string message, params object[] @params) {
			Message = message;
			Params = @params;	
		}
		/// <summary>
		/// Форматированное сообщение.
		/// </summary>
		public string Message { get; }

		/// <summary>
		/// Параметры для форматированной строки логгера.
		/// </summary>
		public object[] Params { get; }

		/// <summary>
		/// Сравнение значения.
		/// </summary>
		/// <param name="obj">Объект для сравнения</param>
		/// <returns>Результат сравнения на равенство</returns>
		public override bool Equals(object obj) {
			if (!(obj is LogShortMessage))  return false;

			return Message == ((LogShortMessage) obj).Message && Params == ((LogShortMessage) obj).Params;
		}
	}
}