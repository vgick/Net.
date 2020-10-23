/// <summary>
/// Наследование от классов в объектах БД, приводит к дополнительным
/// полям, а TPC в Core не поддерживается.
/// </summary>

namespace NBCH_EF.Tables.Interface {
	/// <summary>
	/// Интерфейс таблицы БД с ID.
	/// </summary>
	internal interface IDBTableID : IDBTable {
		/// <summary>
		/// Ключевое поле.
		/// </summary>
		int ID { get; set; }
	}
}
