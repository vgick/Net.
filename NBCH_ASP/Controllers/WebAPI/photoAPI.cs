using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NBCH_ASP.Infrastructure;
using NBCH_LIB.Interfaces;
using NBCH_ASP.Models.WebAPI.PhotoApi;
using static NBCH_ASP.Infrastructure.WebAPI.PhotoApi;
using Microsoft.AspNetCore.Authorization;

namespace NBCH_ASP.Controllers.WebAPI {
	#if !(DEBUG)
	[Authorize(Roles = @"role,roleadmin")]
	#endif
	[Route("api/[controller]")]
	[ApiController]
	public class PhotoApi : ControllerBase {
		/// <summary>
		/// Сервис логирования
		/// </summary>
		private readonly ILogger<PhotoApi> _Logger;

		/// <summary>
		/// Сервис по работе с архивом.
		/// </summary>
		private readonly IServiceRegistrar _ServiceRegistrar;

		/// <summary>
		/// Конструктор.
		/// </summary>
		/// <param name="serviceRegistrar">Сервис для работы с хранилищем</param>
		/// <param name="logger">Логгер</param>
		public PhotoApi(IServiceRegistrar serviceRegistrar, ILogger<PhotoApi> logger) {
			_ServiceRegistrar	= serviceRegistrar;
			_Logger				= logger;
		}

		/// <summary>
		/// Получить список фотографий клиента.
		/// </summary>
		/// <param name="client1CCode">Код клиента 1С</param>
		/// <param name="excludeAccount">Исключить фотографии, прикрепленные к данному договору</param>
		/// <returns>Список(ID файла, дата загрузки на сервер)</returns>
		[HttpGet("{client1CCode}/{excludeAccount}")]
		public async Task<IActionResult> Get(string client1CCode, string excludeAccount = default) {
			ObjectResult checkResult	=  GetCheckParams(StatusCode, client1CCode);
			if (checkResult != default) return checkResult;
			
			UploadedPhoto[] result	= default;
			try {
				result	= (await _ServiceRegistrar.GetPhotoListAsync(client1CCode, excludeAccount, HelperASP.Login(User), CancellationToken.None)).
					Select(i => new UploadedPhoto() { ID = i.Key, UploadDate = i.Value.ToString() }).
					ToArray();
			}
			catch (Exception exception) {
				_Logger.LogError(
					"Ошибка получения фотографий клиента с кодом: {client1CCode}, excludeAccount: {excludeAccount}, пользователь {login}, ошибка: {exceptionMessage}.",
					client1CCode, excludeAccount, HelperASP.Login(User), exception.Message);
				return StatusCode(StatusCodes.Status500InternalServerError, exception.Message);
			}

			return Ok(result);
		}
	}
}
