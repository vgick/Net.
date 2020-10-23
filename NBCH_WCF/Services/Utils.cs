using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NBCH_LIB.Logger;
using NBCH_LIB.Models.PDN;

namespace NBCH_WCF.Services {
	public static class Utils {
		/// <summary>
		/// Вызвать метод, при возникновении ошибки залогировать её и отправить на клиент описание исключения
		/// </summary>
		/// <typeparam name="TResult">Тип возвращаемого параметра</typeparam>
		/// <typeparam name="TLoggerClass">Класс логгера</typeparam>
		/// <param name="func">Выполняемая функция</param>
		/// <param name="logShortMessage">Дополнительное сообщение для логгера. Может быть пустым</param>
		/// <returns>Результат выполнения функции</returns>
		public static TResult ExecuteWithTryCatch<TResult, TLoggerClass>(Func<TResult> func, LogShortMessage logShortMessage = default)
			where TLoggerClass : class {
			
			TResult result = default;
			try {
				result = func();
			}
			catch (PDNAnketaNotFoundException ex) {
				ThrowFaultException(ex);
			}
			catch (Exception exception) {
				LogException<TLoggerClass>(exception, logShortMessage);
				ThrowFaultException(exception);
			}

			return result;
		}

		/// <summary>
		/// Вызвать метод, при возникновении ошибки залогировать её и отправить на клиент описание исключения
		/// </summary>
		/// <typeparam name="TLoggerClass">Класс логгера</typeparam>
		/// <param name="action">Обрабатываемый метод</param>
		/// <param name="logShortMessage">Дополнительное сообщение для логгера. Может быть пустым</param>
		/// <returns>Результат выполнения функции</returns>
		public static void ExecuteWithTryCatch<TLoggerClass>(Action action, LogShortMessage logShortMessage = default) where TLoggerClass : class {
			try {
				action();
			}
			catch (PDNAnketaNotFoundException ex) {
				ThrowFaultException(ex);
			}
			catch (Exception exception) {
				LogException<TLoggerClass>(exception, logShortMessage);
				ThrowFaultException(exception);
			}
		}

		
		/// <summary>
		/// Записать исключение в лог.
		/// </summary>
		/// <param name="exception">Логируемое исключение</param>
		/// <param name="logShortMessage">Дополнительное сообщение для логгера. Может быть пустым</param>
		/// <typeparam name="TLoggerClass">Класс логгера</typeparam>
		private static void LogException<TLoggerClass>(Exception exception, LogShortMessage logShortMessage = default)
			where TLoggerClass : class {
			ILogger<TLoggerClass> logger = ServiceMethod.LoggerFactory.CreateLogger<TLoggerClass>();
			
			List<object> @params	= new List<object>();
			string message 			= "Ошибка выполнения. Exception: {Exception}.";
			
			if (!logShortMessage.Equals(default(LogShortMessage))) {
				message	= logShortMessage.Message + Environment.NewLine + 
				          "Ошибка выполнения. Exception: {Exception}.";
				
				if ((logShortMessage.Params?.Length ?? 0) > 0)
					@params.AddRange(logShortMessage.Params);
			}
			@params.Add(exception);
			
			logger.LogError(message, @params.ToArray());
		}

		/// <summary>
		/// Вызвать метод асинхронно, при возникновении ошибки залогировать её и отправить на клиент описание исключения.
		/// </summary>
		/// <typeparam name="TLoggerClass">Класс логгера</typeparam>
		/// <param name="actionAsync">Обрабатываемый метод</param>
		/// <param name="logShortMessage">Дополнительное сообщение для логгера. Может быть пустым</param>
		/// <returns>Результат выполнения функции</returns>
		public static async Task ExecuteWithTryCatchAsync<TLoggerClass>(Func<Task> actionAsync, LogShortMessage logShortMessage = default)
			where TLoggerClass : class {
			try {
				await actionAsync();
			}
			catch (PDNAnketaNotFoundException exception) {
				ThrowFaultException(exception);
			}
			catch (Exception exception) {
				LogException<TLoggerClass>(exception, logShortMessage);
				ThrowFaultException(exception);
			}
		}

		/// <summary>
		/// Вызвать метод асинхронно с возвращаемым параметром, при возникновении ошибки залогировать её и отправить
		/// на клиент описание исключения.
		/// </summary>
		/// <param name="actionAsync">Обрабатываемый метод</param>
		/// <param name="logShortMessage">Дополнительное сообщение для логгера. Может быть пустым</param>
		/// <typeparam name="TLoggerClass">Класс логгера</typeparam>
		/// <typeparam name="TResult">Тип возвращаемого значения</typeparam>
		/// <returns>Результат выполнения функции</returns>
		public static async Task<TResult> ExecuteWithTryCatchAsync<TResult, TLoggerClass>(
			Func<Task<TResult>> actionAsync, LogShortMessage logShortMessage = default)
		
			where TLoggerClass : class {
			try {
				return await actionAsync();
			}
			catch (PDNAnketaNotFoundException exception) {
				ThrowFaultException(exception);
			}
			catch (Exception exception) {
				LogException<TLoggerClass>(exception, logShortMessage);
				ThrowFaultException(exception);
			}

			return default;
		}

		/// <summary>
		/// Упаковать и пробросить исключение на клиент.
		/// </summary>
		/// <param name="exception">Исключение, которое необходимо пробросить на клиента</param>
		private static void ThrowFaultException(Exception exception) {
			ExceptionDetail detail	= new ExceptionDetail(exception);
			throw new FaultException<ExceptionDetail>(detail, exception.Message);
		}
	}
}
