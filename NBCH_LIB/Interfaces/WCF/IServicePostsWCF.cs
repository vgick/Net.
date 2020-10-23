using System.ServiceModel;
using System.Threading.Tasks;
using NBCH_LIB.Models.Posts;

namespace NBCH_LIB.Interfaces.WCF {
	/// <summary>
	/// Работа с сообщениями
	/// </summary>
	[ServiceContract]
	public interface IServicePostsWCF : IWCFContract {
		/// <summary>
		/// Получить все сообщения по договору.
		/// </summary>
		/// <param name="adLogin">Логин пользователя AD</param>
		/// <param name="account1CCode">Номер договора 1С</param>
		/// <returns>Все сообщения по договору</returns>
		[OperationContract]
		Post[] GetPosts(string adLogin, string account1CCode);

		/// <summary>
		/// Получить все сообщения по договору асинхронно.
		/// </summary>
		/// <param name="adLogin">Логин пользователя AD</param>
		/// <param name="account1CCode">Номер договора 1С</param>
		/// <returns>Все сообщения по договору</returns>
		[OperationContract(Name = "GetPostsAsync")]
		Task<Post[]> GetPostsAsync(string adLogin, string account1CCode);

		/// <summary>
		/// Добавить сообщение.
		/// </summary>
		/// <param name="adLogin">Логин пользователя AD</param>
		/// <param name="account1CCode">Номер договора 1С</param>
		/// <param name="post">Пост для добавления</param>
		/// <param name="clientTimeZone">Часовой пояс клиента</param>
		[OperationContract]
		void AddPost(string adLogin, string account1CCode, Post post, int clientTimeZone);

		/// <summary>
		/// Добавить сообщение асинхронно.
		/// </summary>
		/// <param name="adLogin">Логин пользователя AD</param>
		/// <param name="account1CCode">Номер договора 1С</param>
		/// <param name="post">Пост для добавления</param>
		/// <param name="clientTimeZone">Часовой пояс клиента</param>
		[OperationContract(Name = "AddPostAsync")]
		Task AddPostAsync(string adLogin, string account1CCode, Post post, int clientTimeZone);
	}
}
