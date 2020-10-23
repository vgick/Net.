
namespace NBCH_LIB.Logger {
	public static partial class ExceptionLog {
		/// <summary>
		/// Структура с альтернативными значениями.
		/// </summary>
		public struct ExLogValue {
			/// <summary>
			/// Значение для исключения.
			/// </summary>
			public object ExValue { get; set; }
			/// <summary>
			/// Значение для лога.
			/// </summary>
			public object LogValue { get; set; }
		}
	}
}