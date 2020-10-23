using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NBCH_ASP.Infrastructure;
using NBCH_LIB.Interfaces;
using NBCH_LIB.Models.Posts;
using Microsoft.AspNetCore.Authorization;
using static NBCH_ASP.Infrastructure.WebAPI.PostsApi;

namespace NBCH_ASP.Controllers.WebAPI {
	#if !(DEBUG)
	[Authorize(Roles = @"role,roleadmin")]
	#endif
	[Route("api/[controller]")]
	[ApiController]
	public class PostsApi : ControllerBase
	{
		/// <summary>
		/// Сервис по работе с сообщениями
		/// </summary>
		private readonly IServicePosts _ServicePosts;

		/// <summary>
		/// Сервис логирования
		/// </summary>
		private readonly ILogger<PostsApi> _Logger;

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="servicePosts">Сервис по работе с сообщениями</param>
		/// <param name="logger">Сервис логирования</param>
		public PostsApi(IServicePosts servicePosts, ILogger<PostsApi> logger) {
			_ServicePosts	= servicePosts;
			_Logger	= logger;
		}

		/// <summary>
		/// Получить все сообщения по договору
		/// </summary>
		/// <param name="account1CCode">Номер договора</param>
		/// <returns></returns>
		// GET api/<ServicePosts>/5
		[HttpGet("{account1CCode}")]
		public async Task<IActionResult> Get(string account1CCode) {
			ObjectResult checkResult	=  GetCheckParams(StatusCode, account1CCode);
			if (checkResult != default) return checkResult;

			Post[] posts;

			try {
				posts = await _ServicePosts.GetPostsAsync(HelperASP.Login(User), account1CCode, CancellationToken.None);
			}
			catch (Exception exception) {
				_Logger.LogError(
					exception,
					"Ошибка получения списка сообщений по договору: {account1CCode}. Пользователь: {login}, ошибка: {exceptionMessage}.",
					account1CCode, HelperASP.Login(User), exception.Message);
				return StatusCode(StatusCodes.Status500InternalServerError, exception.Message);
			}

			return Ok(posts);
		}

		/// <summary>
		/// Сохранить новое сообщение
		/// </summary>
		/// <param name="account1CCode">Номер договора</param>
		/// <param name="date">Дата сообщения</param>
		/// <param name="clientTimeZone">Часовой пояс клиента</param>
		/// <param name="message">Текст сообщения</param>
		/// <returns></returns>
		// Post api/<ServicePosts>/5
		[HttpPost("{account1CCode}")]
		public async Task<IActionResult> Post(string account1CCode, [FromForm]DateTime date , [FromForm] int clientTimeZone,
			[FromForm] string message) {
			
			ObjectResult checkResult	=  PostCheckParams(StatusCode, account1CCode, date, clientTimeZone, message);
			if (checkResult != default) return checkResult;

			Post post = new Post() {
				Account1CCode	= account1CCode,
				Date			= date,
				Message			= message
			};

			try {
				await _ServicePosts.AddPostAsync(HelperASP.Login(User), account1CCode, post, clientTimeZone, CancellationToken.None);
			}
			catch (Exception exception) {
				_Logger.LogError(exception,
					"Ошибка добавления сообщения в договор {account1CCode}. Часовой пояс клиента {clientTimeZone}," +
					" сообщение {message}, пользователь {login}, ошибка: {exceptionMessage}.",
					account1CCode, clientTimeZone, message, HelperASP.Login(User), exception.Message);
				return StatusCode(StatusCodes.Status500InternalServerError, exception.Message);
			}

			return Ok();
		}
	}
}
