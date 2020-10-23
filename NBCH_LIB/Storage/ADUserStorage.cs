using System.Threading.Tasks;
using NBCH_LIB.Interfaces;
using NBCH_LIB.Models;

// todo: удалить класс
namespace NBCH_LIB.Storage {
	/// <summary>
	/// Хранилище пользователей AD.
	/// </summary>
	public class ADUserStorage : IADUser {
		/// <summary>
		/// Хранилище пользователей AD.
		/// </summary>
		private IADUser ADUser {get; set;} = default;

		/// <summary>
		/// Конструктор.
		/// </summary>
		/// <param name="adUser">Хранилище пользователей AD</param>
		public ADUserStorage(IADUser adUser) => ADUser = adUser;

		/// <summary>
		/// Добавить пользователя AD в базу.
		/// </summary>
		/// <param name="name">Имя пользователя в AD</param>
		/// <returns>Пользователь AD.</returns>
		public ADUser AddADUser(string name) => ADUser.AddADUser(name);

		/// <summary>
		/// Добавить пользователя AD в базу.
		/// </summary>
		/// <param name="name">Имя пользователя в AD</param>
		/// <returns>Пользователь AD.</returns>
		public async Task<ADUser> AddADUserAsync(string name) => await Task.Run(() => ADUser.AddADUser(name));

		/// <summary>
		/// Удалить пользователя по ID.
		/// </summary>
		/// <param name="id">ID пользователя</param>
		public void DeleteADUser(int id) => ADUser.DeleteADUser(id);

		/// <summary>
		/// Удалить пользователя по ID асинхронно.
		/// </summary>
		/// <param name="id">ID пользователя</param>
		public async Task DeleteADUserAync(int id) => await Task.Run(() => ADUser.DeleteADUser(id));

		/// <summary>
		/// Получить информацию пользователя AD по ID.
		/// </summary>
		/// <param name="id">ID пользователя</param>
		/// <returns>Пользователь AD</returns>
		public ADUser GetADUserByID(int id) => ADUser.GetADUserByID(id);

		/// <summary>
		/// Получить информацию пользователя AD по ID асинхронно.
		/// </summary>
		/// <param name="id">ID пользователя</param>
		/// <returns>Пользователь AD</returns>
		public async Task<ADUser> GetADUserByIDAsync(int id) => await Task.Run(() => ADUser.GetADUserByID(id));

		/// <summary>
		/// Получить информацию пользователя AD по имени.
		/// </summary>
		/// <param name="name"></param>
		/// <returns>Пользователь AD</returns>
		public ADUser[] GetADUsersName(string name) => ADUser.GetADUsersName(name);

		/// <summary>
		/// Получить информацию пользователя AD по имени асинхронно.
		/// </summary>
		/// <param name="name">Имя пользователя</param>
		/// <returns>Пользователь AD</returns>
		public async Task<ADUser[]> GetADUsersNameAsync(string name) => await Task.Run(() => ADUser.GetADUsersName(name));

		/// <summary>
		/// Получить список всех пользователей AD, которые имеют доступ к системе.
		/// </summary>
		/// <returns>Список пользователей AD</returns>
		public ADUser[] GetADUsers() => ADUser.GetADUsers();

		/// <summary>
		/// Получить асинхронно список всех пользователей AD, которые имеют доступ к системе.
		/// </summary>
		/// <returns>Список пользователей AD</returns>
		public async Task<ADUser[]> GetADUsersAsync() => await Task.Run(() => ADUser.GetADUsers());

		/// <summary>
		/// Получить список пользователей AD, которые имеют доступ к системе.
		/// </summary>
		/// <param name="pageSize">Количество пользователей на страницу</param>
		/// <param name="pageNumber">Номер страницы (0 - первая страница)</param>
		/// <returns></returns>
		public ADUser[] GetADUsersByPage(int pageSize, int pageNumber) => ADUser.GetADUsersByPage(pageSize, pageNumber);

		/// <summary>
		/// Получить асинхронно список пользователей AD, которые имеют доступ к системе.
		/// </summary>
		/// <param name="pageSize">Количество пользователей на страницу</param>
		/// <param name="pageNumber">Номер страницы (0 - первая страница)</param>
		/// <returns></returns>
		public async Task<ADUser[]> GetADUsersByPageAsync(int pageSize, int pageNumber) => await Task.Run(() => ADUser.GetADUsersByPage(pageSize, pageNumber));

		/// <summary>
		/// Удалить пользователя по ID.
		/// </summary>
		/// <param name="id"></param>
		public async Task DeleteADUserAsync(int id) => await Task.Run( () => ADUser.DeleteADUser(id));

		/// <summary>
		/// Обновить данные пользователя.
		/// </summary>
		/// <param name="adUser">Данные пользователя для обновления</param>
		public void UpdateADUser(ADUser adUser) => ADUser.UpdateADUser(adUser);

		/// <summary>
		/// Обновить данные пользователя асинхронно.
		/// </summary>
		/// <param name="adUser">Данные пользователя для обновления</param>
		public async Task UpdateADUserAsync(ADUser adUser) => await Task.Run( () => ADUser.UpdateADUser(adUser));
	}
}
