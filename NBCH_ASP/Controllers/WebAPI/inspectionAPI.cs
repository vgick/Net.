using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NBCH_ASP.Infrastructure;
using NBCH_LIB.Interfaces;
using static NBCH_LIB.Helper;

namespace NBCH_ASP.Controllers.WebAPI {
	#if !(DEBUG)
	[Authorize(Roles = @"role,roleadmin")]
	#endif
	[Route("api/[controller]")]
	[ApiController]
	public class InspectionApi : Controller {
		/// <summary>
		/// Логгер.
		/// </summary>
		private readonly ILogger<InspectionApi> _Logger;

		/// <summary>
		/// Сервис для логирования работы проверяющих сотрудников.
		/// </summary>
		private readonly IServiceInspecting _InspectingService;

		/// <summary>
		/// Конструктор.
		/// </summary>
		/// <param name="logger">Логгер</param>
		/// <param name="inspectingService">Сервис для логирования работы проверяющих сотрудников</param>
		public InspectionApi(ILogger<InspectionApi> logger, IServiceInspecting inspectingService) {
			_Logger				= logger;
			_InspectingService	= inspectingService;
		}


		/// <summary>
		/// Привязать договор к проверяющему сотруднику.
		/// </summary>
		/// <param name="account1CCode">Номер договора</param>
		/// <param name="clientTimeZone">Часовой пояс клиента</param>
		/// <returns></returns>
		[HttpPost("{account1CCode}")]
		public async Task<IActionResult> Index(string account1CCode, [FromForm] int clientTimeZone) {

			try {
				await _InspectingService.SetInspectionAsync(
					account1CCode,
					HelperASP.Login(User),
					DateTime.Now.AddHours(clientTimeZone - ServerTimeZone),
					clientTimeZone,
					CancellationToken.None);
			}
			catch (Exception exception) {
				_Logger.LogError(exception,
					"Ошибка не удалось привязать проверяющего сотрудника к договору. Договор {account1CCode}," +
					" пользователь {login}, ошибка: {exceptionMessage}.",
					account1CCode, HelperASP.Login(User), exception.Message);
				
				return StatusCode(StatusCodes.Status500InternalServerError, exception.Message);
			}

			return Ok();
			
		}
	}
}