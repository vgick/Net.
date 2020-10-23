using System.Threading;
using System.Threading.Tasks;
using NBCH_LIB.Models.Posts;

namespace NBCH_LIB.Interfaces {
	/// <summary>
	/// Работа с сообщениями
	/// </summary>
	public interface IServicePosts {
		/// <summary>
		/// Получить все сообщения по договору.
		/// </summary>
		/// <param name="adLogin">Логин пользователя AD</param>
		/// <param name="account1CCode">Номер договора 1С</param>
		/// <returns>Все сообщения по договору</returns>
		Post[] GetPosts(string adLogin, string account1CCode);

		/// <summary>
		/// Получить все сообщения по договору асинхронно.
		/// </summary>
		/// <param name="adLogin">Логин пользователя AD</param>
		/// <param name="account1CCode">Номер договора 1С</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns>Все сообщения по договору</returns>
		Task<Post[]> GetPostsAsync(string adLogin, string account1CCode, CancellationToken cancellationToken);

		/// <summary>
		/// Добавить сообщение.
		/// </summary>
		/// <param name="adLogin">Логин пользователя AD</param>
		/// <param name="account1CCode">Номер договора 1С</param>
		/// <param name="post">Пост для добавления</param>
		/// <param name="clientTimeZone">Часовой пояс клиента</param>
		void AddPost(string adLogin, string account1CCode, Post post, int clientTimeZone);

		/// <summary>
		/// Добавить сообщение асинхронно.
		/// </summary>
		/// <param name="adLogin">Логин пользователя AD</param>
		/// <param name="account1CCode">Номер договора 1С</param>
		/// <param name="post">Пост для добавления</param>
		/// <param name="clientTimeZone">Часовой пояс клиента</param>
		/// <param name="cancellationToken">Токен отмены</param>
		Task AddPostAsync(string adLogin, string account1CCode, Post post, int clientTimeZone, CancellationToken cancellationToken);
	}
}
