using System;
using System.Collections.Generic;
using System.Linq;
using NBCH_LIB.Interfaces;
using NBCH_LIB.Models;

namespace NBCH_ASP.LocalStorage {
	/// <summary>
	/// Класс для тестирования, реализующий интерфейс IADUser
	/// </summary>
	public class LocalADUsers : IADUser {
		/// <summary>
		/// Хранилище списка пользователей системы ADUser
		/// </summary>
		private List<ADUser> ADUsers {get; set;}	= new List<ADUser>();

		/// <summary>
		/// Статический конструктор для заполнения первоначальными данными
		/// </summary>
		public LocalADUsers() {
			ADUser adUser0	= new ADUser() { ADName = "User0", ID = 0};
			adUser0.Regions	= new Region[] {LocalRegions.Regions[0], LocalRegions.Regions[1]};
			ADUsers.Add(adUser0);

			ADUsers.Add(new ADUser() {ADName = "User1", ID = 1});
			ADUsers.Add(new ADUser() {ADName = "User2", ID = 2});
		}

		/// <summary>
		/// Добавить пользователя AD в базу
		/// </summary>
		/// <param name="name">Имя пользователя в AD</param>
		/// <returns>Код новой записи. Если ошибка, то Null</returns>
		public ADUser AddADUser(string name) {
			ADUser user = new ADUser {ADName = name, ID = ADUsers.Max(i => i.ID)};

			ADUsers.Add(user);

			return user;
		}

		/// <summary>
		/// Удалить пользователя по ID
		/// </summary>
		/// <param name="id">ID пользователя</param>
		public void DeleteADUser(int id) {
			ADUser user = GetADUserByID(id);

			if (user == null) throw new ArgumentOutOfRangeException();
			ADUsers.Remove(user);
		}

		/// <summary>
		/// Получить информацию пользователя AD по ID
		/// </summary>
		/// <param name="id">ID пользователя</param>
		/// <returns>Пользователь AD. Null если пользователь не найден</returns>
		public ADUser GetADUserByID(int id) => ADUsers.FirstOrDefault(i => i.ID == id);

		/// <summary>
		/// Получить список всех пользователей AD, которые имеют доступ к системе.
		/// </summary>
		/// <returns>Список пользователей AD</returns>
		public ADUser[] GetADUsers() => ADUsers.ToArray();

		/// <summary>
		/// Получить список пользователей AD, которые имеют доступ к системе.
		/// </summary>
		/// <param name="pageSize">Количество пользователей на страницу</param>
		/// <param name="pageNumber">Номер страницы (0 - первая страница)</param>
		/// <returns>Список пользователей AD</returns>
		public ADUser[] GetADUsersByPage(int pageSize, int pageNumber) => ADUsers.Skip(pageNumber * pageSize).Take(pageSize).ToArray();

		/// <summary>
		/// Получить информацию пользователя AD по имени
		/// </summary>
		/// <param name="name">Имя пользователя</param>
		/// <returns>Пользователь AD. Null если пользователь не найден</returns>
		public ADUser[] GetADUsersName(string name) => ADUsers.Where(i => i.ADName.Contains(name)).ToArray();

		/// <summary>
		/// Обновить данные пользователя
		/// </summary>
		/// <param name="adUser">Данные пользователя для обновления</param>
		public void UpdateADUser(ADUser adUser) {
			ADUser stored	= ADUsers.Find((i) => i.ID.Equals(adUser.ID));
			if (stored.Equals(adUser))
				// Проверить, что данные обновляются???
				throw new NotImplementedException();
		}
	}
}
