/// <summary>
/// Наследование от классов в объектах БД, приводит к дополнительным
/// полям, а TPC в Core не поддерживается.
/// </summary>

namespace NBCH_EF.Tables.Interface {
	/// <summary>
	/// Интерфейс таблицы БД с ID, Name, Code1C.
	/// </summary>
	internal interface IDBTableIDNameW1CCode : IDBTableName {
		/// <summary>
		/// Код 1С.
		/// </summary>
		string Code1C {get; set;}
	}
}
