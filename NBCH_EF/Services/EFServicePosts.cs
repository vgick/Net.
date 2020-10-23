using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NBCH_EF.Helpers;
using NBCH_EF.Tables;
using NBCH_LIB;
using NBCH_LIB.ADServiceProxy;
using NBCH_LIB.Interfaces;
using NBCH_LIB.Interfaces.WCF;
using NBCH_LIB.Logger;
using NBCH_LIB.Models.Posts;
using static NBCH_LIB.Logger.ExceptionLog;
using static NBCH_EF.MKKContext;

namespace NBCH_EF.Services {
	/// <summary>
	/// Сервис по работе с постами.
	/// </summary>
	public class EFServicePosts : IServicePosts, IServicePostsWCF {
		/// <summary>
		/// Статический конструктор.
		/// </summary>
		static EFServicePosts() {
			_Logger = MKKContext.LoggerFactory.CreateLogger<EFServicePosts>();
		}

		/// <summary>
		/// Логгер.
		/// </summary>
		private static readonly ILogger<EFServicePosts> _Logger;

		/// <summary>
		/// Роль на добавление постов.
		/// </summary>
		private const string _AddPostADRole = "vlf-registrar-chat-write";

		/// <summary>
		/// Просмотр всех сообщений.
		/// </summary>
		private const string _ReadAllPostADRole = "vlf-registrar-chat-read-all";

		/// <summary>
		/// Просмотр сообщений (своих).
		/// </summary>
		private const string _ReadPostADRole = "vlf-registrar-chat-read";

		/// <summary>
		/// Получить все сообщения по договору.
		/// </summary>
		/// <param name="adLogin">Логин пользователя AD</param>
		/// <param name="account1CCode">Номер договора 1С</param>
		/// <returns>Все сообщения по договору</returns>
		public Post[] GetPosts(string adLogin, string account1CCode) =>
			GetPostsAsync(adLogin, account1CCode, CancellationToken.None).ResultAndThrowException();

		/// <summary>
		/// Получить все сообщения по договору асинхронно.
		/// </summary>
		/// <param name="adLogin">Логин пользователя AD</param>
		/// <param name="account1CCode">Номер договора 1С</param>
		/// <returns>Все сообщения по договору</returns>
		public async Task<Post[]> GetPostsAsync(string adLogin, string account1CCode) =>
			await GetPostsAsync(adLogin, account1CCode, CancellationToken.None);

		/// <summary>
		/// Есть права на просмотр всех сообщений.
		/// </summary>
		/// <param name="adLogin">Логин пользователя</param>
		/// <returns>Признак наличия прав на просмотр всех сообщений</returns>
		private static bool CanReadAllPost(string adLogin) {
			string[] roles = Singleton<ProxyADRoles>.Values[adLogin];
			return roles.Contains(_ReadAllPostADRole);
		}

		/// <summary>
		/// Проверить входные параметры метода GetPosts.
		/// </summary>
		/// <param name="adLogin">AD логин пользователя</param>
		/// <param name="account1CCode">Номер договора</param>
		private static void GetPostsCheckParams(string adLogin, string account1CCode) {
			if (!CanReadPost(adLogin))
				LogAndThrowException<ArgumentException, EFServicePosts>(_Logger,
					"",
					"У пользователя {adLogin} нет прав на чтение сообщений./* Метод {methodName}.*/",
					adLogin, "GetPostsCheckParams");

			if (string.IsNullOrEmpty(account1CCode))
				LogAndThrowException<ArgumentNullException, EFServicePosts>(_Logger,
					nameof(account1CCode),
					"Не задан договор 1С./* Метод {methodName} значение account1CCode: {account1CCode}.*/",
					"GetPostsCheckParams", account1CCode);

			if (string.IsNullOrEmpty(adLogin))
				LogAndThrowException<ArgumentNullException, EFServicePosts>(_Logger,
					nameof(adLogin),
					"Не задан пользователь AD./* Метод {methodName} adLogin: {adLogin}.*/",
					"GetPostsCheckParams", adLogin);
		}

		/// <summary>
		/// Проверить наличие роли у пользователя.
		/// </summary>
		/// <param name="adLogin">Логин пользователя</param>
		/// <param name="role">Роль, наличие которой необходимо проверить</param>
		/// <returns></returns>
		private static bool RoleContains(string adLogin, string role) {
			string[] roles;
			try { roles = Singleton<ProxyADRoles>.Values[adLogin]; }
			catch (Exception exception) {
				LoggedMessage<EFServicePosts> logMessage = new LoggedMessage<EFServicePosts>(
					_Logger,
					"Не удалось получить список ролей./* Метод {methodName} adLogin: {adLogin}.*/",
					"RoleContains", adLogin);
				logMessage.LogMessage($"{Environment.NewLine}" + "Описание ошибки: {exception}", exception);
				throw;
			}

			return roles.Contains(role);
		}

		/// <summary>
		/// Есть права на просмотр сообщений.
		/// </summary>
		/// <param name="adLogin">Логин пользователя</param>
		/// <returns>Признак наличия прав на просмотр сообщений</returns>
		private static bool CanReadPost(string adLogin) {
			return RoleContains(adLogin, _ReadPostADRole);
		}

		/// <summary>
		/// Добавить сообщение.
		/// </summary>
		/// <param name="adLogin">Логин пользователя AD</param>
		/// <param name="account1CCode">Номер договора 1С</param>
		/// <param name="post">Пост для добавления</param>
		/// <param name="clientTimeZone">Часовой пояс клиента</param>
		public void AddPost(string adLogin, string account1CCode, Post post, int clientTimeZone) =>
			AddPostAsync(adLogin, account1CCode, post, clientTimeZone, CancellationToken.None).WaitAndThrowException();

		/// <summary>
		/// Проверить входные параметры.
		/// </summary>
		/// <param name="adLogin">Логин пользователя</param>
		/// <param name="account1CCode">Договор 1С</param>
		/// <param name="post">Сообщения для публикования</param>
		/// <param name="clientTimeZone">Часовой пояс клиента</param>
		private static void AddPostCheckParams(string adLogin, string account1CCode, Post post, int clientTimeZone) {
			if (!CanAddPost(adLogin))
				LogAndThrowException<ArgumentException, EFServicePosts>(_Logger,
					"",
					"У пользователя {adLogin} нет прав на добавление сообщений./* Метод {methodName}.*/",
					adLogin, "AddPostCheckParams");

			if (clientTimeZone == default)
				LogAndThrowException<ArgumentNullException, EFServicePosts>(_Logger,
					nameof(clientTimeZone),
					"Не задан часовой пояс клиента./* Метод {methodName} значение clientTimeZone: {clientTimeZone}.*/",
					"AddPostCheckParams", clientTimeZone);

			if (string.IsNullOrEmpty(account1CCode))
				LogAndThrowException<ArgumentNullException, EFServicePosts>(_Logger,
					nameof(account1CCode),
					"Не задан договор 1С./* Метод {methodName} значение account1CCode: {account1CCode}.*/",
					"AddPostCheckParams", account1CCode);

			if (string.IsNullOrEmpty(adLogin))
				LogAndThrowException<ArgumentNullException, EFServicePosts>(_Logger,
					nameof(adLogin),
					"Не задан пользователь AD./* Метод {methodName}  adLogin: {adLogin}.*/",
					"AddPostCheckParams", adLogin);

			if (post == default || string.IsNullOrEmpty(post.Message))
				LogAndThrowException<ArgumentNullException, EFServicePosts>(_Logger,
					nameof(post),
					"Сообщение для публикации пустое./* Метод {methodName}  post: {post}.*/",
					"AddPostCheckParams", new ExLogValue {LogValue = post, ExValue = "null"});
		}

		/// <summary>
		/// Есть права на добавление сообщений.
		/// </summary>
		/// <param name="adLogin">Логин пользователя</param>
		/// <returns>Признак наличия прав на добавление сообщений</returns>
		private static bool CanAddPost(string adLogin) {
			return RoleContains(adLogin, _AddPostADRole);
		}

		/// <summary>
		/// Получить все сообщения по договору асинхронно.
		/// </summary>
		/// <param name="adLogin">Логин пользователя AD</param>
		/// <param name="account1CCode">Номер договора 1С</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns>Все сообщения по договору</returns>
		public async Task<Post[]> GetPostsAsync(string adLogin, string account1CCode, CancellationToken cancellationToken) {
			GetPostsCheckParams(adLogin, account1CCode);

			ADLoginsDB author = Singleton<ProxyADLoginsDB>.Values[adLogin];
			Account1C account = await FindAccountAndLogErrorAsync<EFServicePosts>(account1CCode, cancellationToken,
				i => i.Client);

			if (account == default) return new Post[0];

			Task<List<PostsDB>> postsInAccountTask	= GetPostsInAccountAsync(account, cancellationToken);
			Task<List<PostsDB>> postsByClientTask	= GetPostsByClientAsync(account, cancellationToken);

			List<PostsDB> posts = (await postsByClientTask).Where(i => i.Author.ID.Equals(author.ID) || CanReadAllPost(adLogin)).ToList();
			posts.AddRange((await postsInAccountTask).Where(i => i.Author.ID.Equals(author.ID) || CanReadAllPost(adLogin)).ToArray());

			Post[] result	= posts.Select(i => (Post)i).ToArray();

			return result;
		}

		/// <summary>
		/// Получить список сообщений по выбранному договору асинхронно.
		/// </summary>
		/// <param name="account">Договор</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns>Задача с сообщениями</returns>
		private static async Task<List<PostsDB>> GetPostsInAccountAsync(Account1C account,
			CancellationToken cancellationToken) {
			
			using (IDBSource dbSource = new MKKContext()) {
				return await dbSource.PostsDBs.
					AsNoTracking().
					Include(i => i.Account).
					Include(i => i.Author).
					Where(i => i.Account.Account1CCode.Equals(account.Account1CCode)).
					Select(i => i).
					ToListAndLogErrorAsync<PostsDB, EFServicePosts>(cancellationToken);
			}
		}

		/// <summary>
		/// Получить список сообщений по клиенту, кроме одного, асинхронно.
		/// </summary>
		/// <param name="account">Договор, до которого необходимо получить все сообщения</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns>Задача с сообщениями</returns>
		private static async Task<List<PostsDB>> GetPostsByClientAsync(Account1C account,
			CancellationToken cancellationToken) {
			
			using (IDBSource dbSource = new NBCH_EF.MKKContext()) {
				return await dbSource.PostsDBs.
					AsNoTracking().
					Include(i => i.Account).
					Include(i => i.Account.Client).
					Include(i => i.Author).
					Where(
						i => i.Account.Client.ID.Equals(account.Client.ID) &&
						i.Account.DateTime <= account.DateTime &&
						!i.Account.Account1CCode.Equals(account.Account1CCode)
					).ToListAndLogErrorAsync<PostsDB, EFServicePosts>(cancellationToken);
			}
		}

		/// <summary>
		/// Добавить сообщение асинхронно.
		/// </summary>
		/// <param name="adLogin">AD логин пользователя</param>
		/// <param name="account1CCode">Код договора 1С</param>
		/// <param name="post">Сообщение для добавления</param>
		/// <param name="clientTimeZone">Часовой пояс клиента</param>
		/// <returns></returns>
		public async Task AddPostAsync(string adLogin, string account1CCode, Post post, int clientTimeZone) =>
			await AddPostAsync(adLogin, account1CCode, post, clientTimeZone, CancellationToken.None);

		/// <summary>
		/// Добавить сообщение асинхронно.
		/// </summary>
		/// <param name="adLogin">AD логин пользователя</param>
		/// <param name="account1CCode">Код договора 1С</param>
		/// <param name="post">Сообщение для добавления</param>
		/// <param name="clientTimeZone">Часовой пояс клиента</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns></returns>
		public async Task AddPostAsync(string adLogin, string account1CCode, Post post, int clientTimeZone,
			CancellationToken cancellationToken) {
			AddPostCheckParams(adLogin, account1CCode, post, clientTimeZone);

			using (IDBSource dbSource = new MKKContext()) {
				PostsDB postDB	= await PostsDB.CreatePostsDB(post, clientTimeZone, adLogin, CancellationToken.None);

				await dbSource.PostsDBs.AddAsync(postDB, cancellationToken);

				AttachAndLogError<Account1C, EFServicePosts>(dbSource, postDB.Account);
				AttachAndLogError<ADLoginsDB, EFServicePosts>(dbSource, postDB.Author);

				await dbSource.SaveChangesAndLogErrorAsync<EFServicePosts>(new LogShortMessage(
					message: "/*Метод {methodName}, adLogin {adLogin}, account1CCode {account1CCode}, post {post}*/",
					"AddPostAsync", adLogin, account1CCode, post),
					cancellationToken
				);
			}
		}
	}
}
