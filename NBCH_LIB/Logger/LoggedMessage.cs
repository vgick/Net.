using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace NBCH_LIB.Logger {
	public class LoggedMessage<TClass> {
		/// <summary>
		/// Логгер.
		/// </summary>
		private readonly ILogger<TClass> _Logger;

		/// <summary>
		/// Отформатированная строка сообщения.
		/// </summary>
		public string Message { get; set; }

		/// <summary>
		/// Значения для отформатированной строки лога.
		/// </summary>
		private readonly object[] _Values;
		public LoggedMessage(ILogger<TClass> logger, string message, params object[] values) {
			_Logger		= logger;
			Message		= message;
			_Values		= values;
		}

		/// <summary>
		/// Логировать сообщение.
		/// </summary>
		public void LogMessage() {
			_Logger.LogError(Message, _Values);
		}

		/// <summary>
		/// Логировать сообщение с исключением.
		/// </summary>
		/// <param name="message">Отформатированная строка сообщения</param>
		/// <param name="exception">Исключение</param>
		public void LogMessage(string message, Exception exception) {
			string logMessage		= ExceptionLog.RemoveDevTag(Message + Environment.NewLine + message, true);
			List<object> logValues	= new List<object>();
			logValues.AddRange(_Values);
			logValues.Add(exception.Message);

			_Logger.LogError(exception, logMessage, logValues.ToArray());
		}
	}
}
