using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace NBCH_LIB.Logger {
	public static partial class ExceptionLog {
		/// <summary>
		/// Логировать сообщение об ошибке и бросить его дальше.
		/// </summary>
		/// <typeparam name="TException">Тип пробрасываемого исключения</typeparam>
		/// <typeparam name="TLoggerClass">Класс логируемого объекта</typeparam>
		/// <param name="logger">Логгер</param>
		/// <param name="paramName">Имя параметр для ArgumentNullException</param>
		/// <param name="patternString">Шаблон сообщения об ошибке</param>
		/// <param name="values">Значения для шаблона</param>
		public static void LogAndThrowException<TException, TLoggerClass>(ILogger<TLoggerClass> logger, string paramName, string patternString,
			params object[] values) where TLoggerClass : class where TException : Exception, new() {

			ExceptionLogMessageValues exceptionLogMessageValues	= ExceptionAndErrorMessageFromPattern(patternString, values);

			logger.LogError(exceptionLogMessageValues.LogMessage, exceptionLogMessageValues.LogValues.ToArray());

			switch (new TException()) {
				case ArgumentNullException argumentNullException:
					throw new ArgumentNullException(paramName, exceptionLogMessageValues.ExceptionMessage);
				case ArgumentOutOfRangeException argumentOutOfRangeException:
					throw new ArgumentOutOfRangeException(paramName, exceptionLogMessageValues.ExceptionMessage);
				case ArgumentException argumentException:
					throw new ArgumentException(exceptionLogMessageValues.ExceptionMessage);
				default:
					throw (TException)Activator.CreateInstance(typeof(TException), exceptionLogMessageValues.ExceptionMessage);
			}
		}

		/// <summary>
		/// Подготовить строку для исключения и логирования по шаблону
		/// </summary>
		/// <param name="patternString">Строка с шаблоном</param>
		/// <param name="values">Значения для шаблона. Может использоваться структура ExLogValue для
		/// разных значений в исключении и логе (для структурных значений)</param>
		/// <returns>Строки для исключения и лога</returns>
		public static ExceptionLogMessageValues ExceptionAndErrorMessageFromPattern(string patternString, params object[] values) {
			ExceptionLogMessageValues exceptionLogMessageValues = new ExceptionLogMessageValues() {
				ExceptionMessage	= patternString,
				LogMessage			= patternString,
				LogValues			= new List<object>()
			};

			foreach (var value in values) {
				if (value is ExLogValue val) {
					exceptionLogMessageValues.ExceptionMessage	= ReplaceSubstring(exceptionLogMessageValues.ExceptionMessage, val.ExValue);
					exceptionLogMessageValues.LogValues.Add(val.LogValue ?? "null");
				}
				else {
					exceptionLogMessageValues.ExceptionMessage	= ReplaceSubstring(exceptionLogMessageValues.ExceptionMessage, value);
					exceptionLogMessageValues.LogValues.Add(value ?? "null");
				}
			}

			exceptionLogMessageValues.LogMessage		= RemoveDevTag(exceptionLogMessageValues.LogMessage, true);
			exceptionLogMessageValues.ExceptionMessage	= RemoveDevTag(exceptionLogMessageValues.ExceptionMessage, false);

			return exceptionLogMessageValues;
		}

		public static string RemoveDevTag(string message, bool logMessage) {
			if (logMessage) {
				int startDevSection = message.IndexOf(ExceptionLogMessageValues.BeginDevSection, StringComparison.Ordinal);
				if (startDevSection >= 0) {
					message = message.Remove(startDevSection, ExceptionLogMessageValues.BeginDevSection.Length);

					int endDevSection =
						message.IndexOf(ExceptionLogMessageValues.EndDevSection, StringComparison.Ordinal);
					message = message.Remove(endDevSection, ExceptionLogMessageValues.EndDevSection.Length);
				}
			}
			else {
				int startDevSection	= message.IndexOf(ExceptionLogMessageValues.BeginDevSection, StringComparison.Ordinal);
				if (startDevSection >= 0) {
					int endDevSection =
						message.IndexOf(ExceptionLogMessageValues.EndDevSection, StringComparison.Ordinal);
					message = message.Substring(0, startDevSection) +
					          message.Substring(endDevSection + ExceptionLogMessageValues.EndDevSection.Length);
				}
			}

			return message;
		}


		private static string ReplaceSubstring(string sourceString, object value) {
			int startIndex	= sourceString.IndexOf(ExceptionLogMessageValues.BeginPatternString, StringComparison.Ordinal);
			int endIndex	= sourceString.IndexOf(ExceptionLogMessageValues.EndPatternString, StringComparison.Ordinal);

			string result = sourceString.Substring(0, startIndex)
			                + value
			                + sourceString.Substring(endIndex + ExceptionLogMessageValues.EndPatternString.Length);

			return result;
		}
	}
}
