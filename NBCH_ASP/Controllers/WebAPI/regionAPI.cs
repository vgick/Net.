using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NBCH_ASP.Infrastructure;
using NBCH_ASP.Infrastructure.DataFromConfigurationFile.ISecrets;

namespace NBCH_ASP.Controllers.WebAPI {
	#if !(DEBUG)
	[Authorize(Roles = @"role,roleadmin")]
	#endif
	[Route("api/[controller]")]
	[ApiController]
	public class RegionApi : ControllerBase {
		/// <summary>
		/// Настройки для подключения со списком регионов. 
		/// </summary>
		private readonly ISecret1C _Secret1C;
		
		/// <summary>
		/// Сервис логирования.
		/// </summary>
		private readonly ILogger<RegionApi> _Logger;
		
		/// <summary>
		/// Конструктор.
		/// </summary>
		/// <param name="secret1C">Конфигурация для подключения к 1С со списком регионов</param>
		/// <param name="logger"></param>
		public RegionApi(ISecret1C secret1C, ILogger<RegionApi> logger) {
			_Logger		= logger;
			_Secret1C	= secret1C;
		}

		/// <summary>
		/// Список регионов
		/// </summary>
		/// <returns>Список регионов</returns>
		// GET: api/<regionAPI>
		[HttpGet]
		// IEnumerable<string>
		public IActionResult Get() {
			try { return Ok(_Secret1C.Servers.Keys.ToArray()); }
			catch (Exception exception) {
				_Logger.LogError(
					exception,
					"Ошибка получения списка регионов. Пользователь {login}, ошибка: {exceptionMessage}.",
					HelperASP.Login(User), exception.Message);
				return StatusCode(StatusCodes.Status500InternalServerError, exception.Message);
			}
			
			// new[] { "Дальний восток", "Крым" };
		}
	}
}
