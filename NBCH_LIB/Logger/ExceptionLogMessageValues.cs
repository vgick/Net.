using System.Collections.Generic;

namespace NBCH_LIB.Logger {
	/// <summary>
	/// Сообщение и значения для исключения и лога.
	/// </summary>
	public struct ExceptionLogMessageValues {
		/// <summary>
		/// Начало блока замены формирования текста Exception.
		/// </summary>
		public const string BeginPatternString = "{";

		/// <summary>
		/// Окончание блока замены формирования текста Exception.
		/// </summary>
		public const string EndPatternString = "}";

		/// <summary>
		/// Начало блока сообщения для разработчика. Удаляется из текста исключения.
		/// </summary>
		public const string BeginDevSection = @"/*";

		/// <summary>
		/// Окончание блока сообщения для разработчика. Удаляется из текста исключения.
		/// </summary>
		public const string EndDevSection = @"*/";

		/// <summary>
		/// Строка для исключения.
		/// </summary>
		public string ExceptionMessage { get; set; }

		/// <summary>
		/// Строка для лога.
		/// </summary>
		public string LogMessage { get; set; }

		/// <summary>
		/// Значения для лога.
		/// </summary>
		public List<object> LogValues { get; set; }
	}
}