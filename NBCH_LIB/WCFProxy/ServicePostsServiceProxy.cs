using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading;
using System.Threading.Tasks;
using NBCH_LIB.Interfaces;
using NBCH_LIB.Interfaces.WCF;
using NBCH_LIB.Models.Posts;

namespace NBCH_LIB.WCFProxy {
	/// <summary>
	/// Прокси класс для работы с сообщениями.
	/// </summary>
	public class ServicePostsServiceProxy : ClientBase<IServicePostsWCF>, IServicePostsWCF {
		#region Конструкторы
		public ServicePostsServiceProxy() { }
		public ServicePostsServiceProxy(string endpointName) : base(endpointName) { }
		public ServicePostsServiceProxy(Binding binding, EndpointAddress remoteAddress) : base(binding, remoteAddress) { }
		#endregion

		/// <summary>
		/// Добавить сообщение.
		/// </summary>
		/// <param name="adLogin">Логин пользователя AD</param>
		/// <param name="account1CCode">Номер договора 1С</param>
		/// <param name="post">Пост для добавления</param>
		/// <param name="clientTimeZone">Часовой пояс клиента</param>
		public void AddPost(string adLogin, string account1CCode, Post post, int clientTimeZone) => Channel.AddPost(adLogin, account1CCode, post, clientTimeZone);

		/// <summary>
		/// Добавить сообщение асинхронно.
		/// </summary>
		/// <param name="adLogin">Логин пользователя AD</param>
		/// <param name="account1CCode">Номер договора 1С</param>
		/// <param name="post">Пост для добавления</param>
		/// <param name="clientTimeZone">Часовой пояс клиента</param>
		public async Task AddPostAsync(string adLogin, string account1CCode, Post post, int clientTimeZone) =>
			await Channel.AddPostAsync(adLogin, account1CCode, post, clientTimeZone);

		/// <summary>
		/// Получить все сообщения по договору.
		/// </summary>
		/// <param name="adLogin">Логин пользователя AD</param>
		/// <param name="account1CCode">Номер договора 1С</param>
		/// <returns>Все сообщения по договору</returns>
		public Post[] GetPosts(string adLogin, string account1CCode) => Channel.GetPosts(adLogin, account1CCode);

		/// <summary>
		/// Получить все сообщения по договору асинхронно.
		/// </summary>
		/// <param name="adLogin">Логин пользователя AD</param>
		/// <param name="account1CCode">Номер договора 1С</param>
		/// <returns>Все сообщения по договору</returns>
		public async Task<Post[]> GetPostsAsync(string adLogin, string account1CCode) =>
			await Channel.GetPostsAsync(adLogin, account1CCode);
	}
}
