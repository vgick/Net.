using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NBCH_ASP.Infrastructure;
using NBCH_LIB.Interfaces;

namespace NBCH_ASP.Controllers.WebAPI {
	/// <summary>
	/// Общий доступ.
	/// </summary>
	[Route("api/[controller]")]
	[ApiController]
	public class PdnFullApi : ControllerBase
	{
		/// <summary>
		/// Логгер. 
		/// </summary>
		private readonly ILogger<PdnFullApi> _Logger;

		/// <summary>
		/// Сервис для работы с ПДН.
		/// </summary>
		private readonly IServicePDN _ServicePDN;

		/// <summary>
		/// Конструктор.
		/// </summary>
		/// <param name="servicePDN">Сервис ПДН</param>
		/// <param name="logger">Логгер</param>
		public PdnFullApi(IServicePDN servicePDN, ILogger<PdnFullApi> logger) {
			_ServicePDN	= servicePDN;
			_Logger		= logger;
		}
		
		// GET: api/<pdnFullAPI>
		[HttpGet]
		// Task<IEnumerable<string>>
		public async Task<IActionResult> Get() {
			try { return Ok(await _ServicePDN.GetFullRecordOver50PAsync(CancellationToken.None)); }
			catch (Exception exception) {
				_Logger.LogError(
					"Ошибка получения ПДН. Пользователь {login}, ошибка: {exceptionMessage}.",
					HelperASP.Login(User), exception.Message);
				return StatusCode(StatusCodes.Status500InternalServerError, exception.Message);
			}
		}
	}
}
