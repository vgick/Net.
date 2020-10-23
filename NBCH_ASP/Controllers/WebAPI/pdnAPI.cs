using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NBCH_ASP.Infrastructure;
using NBCH_LIB.Interfaces;
using NBCH_LIB.Models.PDN;
using static NBCH_ASP.Infrastructure.WebAPI.PdnApi;
	
namespace NBCH_ASP.Controllers.WebAPI {
	/// <summary>
	/// Общий доступ.
	/// </summary>
	[Route("api/[controller]")]
	[ApiController]
	public class PdnApi : ControllerBase
	{
		/// <summary>
		/// Сервис данных ПДН.
		/// </summary>
		private readonly IServicePDN _ServicePDN;

		/// <summary>
		/// Конструктор.
		/// </summary>
		/// <param name="servicePDN"></param>
		public PdnApi(IServicePDN servicePDN) {
			_ServicePDN	= servicePDN;
		}
		
		/// <summary>
		/// Логгер. 
		/// </summary>
		private readonly ILogger<PdnApi> _Logger;

		/// <summary>
		/// Конструктор.
		/// </summary>
		/// <param name="logger">Логгер</param>
		public PdnApi(ILogger<PdnApi> logger) {
			_Logger	= logger;
		}
		
		/// <summary>
		/// Получить ПДН.
		/// </summary>
		/// <param name="accounts">Договора по которым требуется получить ПДН</param>
		/// <returns>ПДН/договор</returns>
		// POST api/<pdnAPI>
		// PdnResult[]
		[HttpPost]
		public async Task<ActionResult> Post([FromBody] string[] accounts) {
			ObjectResult checkResult	= PostCheckParams(StatusCode, accounts);
			if (checkResult != default) return checkResult;
			
			PdnResult[] result;
			
			try { result	= await _ServicePDN.GetPDNPercentsAsync(accounts, CancellationToken.None); }
			catch (Exception exception) {
				_Logger.LogError(
					exception,
					"Ошибка получения ПДН по договорам. Договора: {accounts}, пользователь: {login}. ошибка: {exceptionMessage}.",
					string.Join(',', accounts), HelperASP.Login(User), exception.Message);
				return StatusCode(StatusCodes.Status500InternalServerError, exception.Message);
			}
			
			return Ok(result);
		}
	}
}
