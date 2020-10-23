using System.ServiceModel;
using NBCH_LIB.Interfaces.WCF;
using NBCH_LIB.Models;

namespace NBCH_LIB.Interfaces {
	/// <summary>
	/// Контракт для работы с WCF сервисом пользователей AD
	/// </summary>
	[ServiceContract]
	public interface IADUser : IWCFContract {
		/// <summary>
		/// Добавить пользователя AD в базу
		/// </summary>
		/// <param name="name">Имя пользователя в AD</param>
		/// <returns>Код новой записи. Если ошибка, то Null</returns>
		[OperationContract]
		ADUser AddADUser(string name);
		
		/// <summary>
		/// Получить информацию пользователя AD по ID
		/// </summary>
		/// <param name="id">ID пользователя</param>
		/// <returns>Пользователь AD</returns>
		[OperationContract]
		ADUser GetADUserByID(int id);

		/// <summary>
		/// Получить информацию пользователя AD по имени
		/// </summary>
		/// <param name="name">Имя пользователя</param>
		/// <returns>Пользователь AD</returns>
		[OperationContract]
		ADUser[] GetADUsersName(string name);

		/// <summary>
		/// Получить список всех пользователей AD, которые имеют доступ к системе.
		/// </summary>
		/// <returns>Список пользователей AD</returns>
		[OperationContract]
		ADUser[] GetADUsers();

		/// <summary>
		/// Получить список пользователей AD, которые имеют доступ к системе.
		/// </summary>
		/// <param name="pageSize">Количество пользователей на страницу</param>
		/// <param name="pageNumber">Номер страницы (0 - первая страница)</param>
		/// <returns>Список пользователей AD</returns>
		[OperationContract]
		ADUser[] GetADUsersByPage(int pageSize, int pageNumber);

		/// <summary>
		/// Обновить данные пользователя
		/// </summary>
		/// <param name="adUser">Данные пользователя для обновления</param>
		[OperationContract]
		void UpdateADUser(ADUser adUser);

		/// <summary>
		/// Удалить пользователя по ID
		/// </summary>
		/// <param name="id"></param>
		[OperationContract]
		void DeleteADUser(int id);
	}
}
