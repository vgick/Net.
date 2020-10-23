/// <summary>
/// Наследование от классов в объектах БД, приводит к дополнительным
/// полям, а TPC в Core не поддерживается.
/// </summary>

namespace NBCH_EF.Tables.Interface {
	/// <summary>
	/// Интерфейс таблицы БД с ID, Name.
	/// </summary>
	internal interface IDBTableName : IDBTableID  {
		/// <summary>
		/// Наименование.
		/// </summary>
		string Name {get; set;}
	}
}
