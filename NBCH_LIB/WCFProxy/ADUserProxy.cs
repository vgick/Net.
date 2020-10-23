using System.ServiceModel;
using System.ServiceModel.Channels;
using NBCH_LIB.Interfaces;
using NBCH_LIB.Models;

namespace NBCH_LIB.WCFProxy {
	public class ADUserProxy : ClientBase<IADUser>, IADUser {
		#region Конструкторы
		public ADUserProxy() { }
		public ADUserProxy(string endpointName) : base(endpointName) { }
		public ADUserProxy(Binding binding, EndpointAddress remoteAddress) : base(binding, remoteAddress) { }
		#endregion
		/// <summary>
		/// Добавить пользователя AD в базу
		/// </summary>
		/// <param name="name">Имя пользователя в AD</param>
		/// <returns>Пользователь AD.</returns>
		public ADUser AddADUser(string name) => Channel.AddADUser(name);

		/// <summary>
		/// Удалить пользователя по ID
		/// </summary>
		/// <param name="id">ID пользователя</param>
		public void DeleteADUser(int id) => Channel.DeleteADUser(id);

		/// <summary>
		/// Получить информацию пользователя AD по ID
		/// </summary>
		/// <param name="id">ID пользователя</param>
		/// <returns>Пользователь AD. Null если пользователь не найден</returns>
		public ADUser GetADUserByID(int id) => Channel.GetADUserByID(id);

		/// <summary>
		/// Получить информацию пользователя AD по имени
		/// </summary>
		/// <param name="name">Имя пользователя</param>
		/// <returns>Пользователь AD. Null если пользователь не найден</returns>
		public ADUser[] GetADUsersName(string name) => Channel.GetADUsersName(name);

		/// <summary>
		/// Получить список всех пользователей AD, которые имеют доступ к системе.
		/// </summary>
		/// <returns>Список пользователей AD</returns>
		public ADUser[] GetADUsers() => Channel.GetADUsers();

		/// <summary>
		/// Получить список пользователей AD, которые имеют доступ к системе.
		/// </summary>
		/// <param name="pageSize">Количество пользователей на страницу</param>
		/// <param name="pageNumber">Номер страницы (0 - первая страница)</param>
		/// <returns></returns>
		public ADUser[] GetADUsersByPage(int pageSize, int pageNumber) => Channel.GetADUsersByPage(pageSize, pageNumber);

		/// <summary>
		/// Обновить данные пользователя
		/// </summary>
		/// <param name="adUser">Данные пользователя для обновления</param>
		public void UpdateADUser(ADUser adUser) => Channel.UpdateADUser(adUser);
	}
}
