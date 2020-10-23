using System.Security.Permissions;
using System.Threading;
using System.Threading.Tasks;
using NBCH_LIB.Interfaces;
using NBCH_LIB.Interfaces.WCF;
using NBCH_LIB.Logger;
using NBCH_LIB.Models.Posts;
using static NBCH_WCF.Services.Utils;
using static NBCH_WCF.Services.ServiceMethod;

namespace NBCH_WCF.Services {
	/// <summary>
	/// Сервис по работе с постами.
	/// </summary>
	public class WCFServicePosts : IServicePostsWCF {
		/// <summary>
		/// Получить все сообщения по договору.
		/// </summary>
		/// <param name="adLogin">Логин пользователя AD</param>
		/// <param name="account1CCode">Номер договора 1С</param>
		/// <returns>Все сообщения по договору</returns>
		[PrincipalPermission(SecurityAction.Demand, Role = @"admin")]
		public Post[] GetPosts(string adLogin, string account1CCode) =>
			ExecuteWithTryCatch<Post[], WCFServicePosts>(() => ServicePosts.GetPosts(adLogin, account1CCode),
				new LogShortMessage("Ошибка вызова метода. Метод: {methodName}, adLogin: {adLogin}," +
				                    " account1CCode: {account1CCode},",
					"GetPosts", adLogin, account1CCode));

		/// <summary>
		/// Получить все сообщения по договору асинхронно.
		/// </summary>
		/// <param name="adLogin">Логин пользователя AD</param>
		/// <param name="account1CCode">Номер договора 1С</param>
		/// <returns>Все сообщения по договору</returns>
		[PrincipalPermission(SecurityAction.Demand, Role = @"admin")]
		public async Task<Post[]> GetPostsAsync(string adLogin, string account1CCode) =>
			await ExecuteWithTryCatchAsync<Post[], WCFServicePosts>(
				() => ServicePosts.GetPostsAsync(adLogin, account1CCode),
				new LogShortMessage("Ошибка вызова метода. Метод: {methodName}, adLogin: {adLogin}," +
				                    " account1CCode: {account1CCode},",
					"GetPostsAsync", adLogin, account1CCode));

		/// <summary>
		/// Добавить сообщение.
		/// </summary>
		/// <param name="adLogin">Логин пользователя AD</param>
		/// <param name="account1CCode">Номер договора 1С</param>
		/// <param name="post">Пост для добавления</param>
		/// <param name="clientTimeZone">Часовой пояс клиента</param>
		[PrincipalPermission(SecurityAction.Demand, Role = @"admin")]
		public void AddPost(string adLogin, string account1CCode, Post post, int clientTimeZone) =>
			ExecuteWithTryCatch<WCFServicePosts>(() => ServicePosts.AddPost(adLogin, account1CCode, post, clientTimeZone),
				new LogShortMessage("Ошибка вызова метода. Метод: {methodName}, adLogin: {adLogin}," +
				                    " account1CCode: {account1CCode}, post: {post}, clientTimeZone: {clientTimeZone}",
					"AddPost", adLogin, account1CCode, post, clientTimeZone));

		/// <summary>
		/// Добавить сообщение асинхронно.
		/// </summary>
		/// <param name="adLogin">Логин пользователя AD</param>
		/// <param name="account1CCode">Номер договора 1С</param>
		/// <param name="post">Пост для добавления</param>
		/// <param name="clientTimeZone">Часовой пояс клиента</param>
		[PrincipalPermission(SecurityAction.Demand, Role = @"admin")]
		public async Task AddPostAsync(string adLogin, string account1CCode, Post post, int clientTimeZone) =>
			await ExecuteWithTryCatchAsync<WCFServicePosts>(() =>
				ServicePosts.AddPostAsync(adLogin, account1CCode, post, clientTimeZone),
				new LogShortMessage("Ошибка вызова метода. Метод: {methodName}, adLogin: {adLogin}," +
				                    " account1CCode: {account1CCode}, post: {post}, clientTimeZone: {clientTimeZone}",
					"AddPostAsync", adLogin, account1CCode, post, clientTimeZone));
	}
}
