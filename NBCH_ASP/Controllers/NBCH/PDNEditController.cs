using System;
using System.Linq;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NBCH_ASP.Infrastructure;
using NBCH_ASP.Models;
using NBCH_ASP.Models.NBCH.PDNEdit;
using NBCH_LIB.Interfaces;
using NBCH_LIB.Models.PDN;

namespace NBCH_ASP.Controllers.NBCH {
	[Authorize(Roles = @"role,roleadmin")]
	public class PDNEditController : Controller {
		/// <summary>
		/// Логгер.
		/// </summary>
		private ILogger<PDNEditController> _Logger;
		
		/// <summary>
		/// Сервис для работы с анкетами НБКИ.
		/// </summary>
		private readonly IServicePDN _ServiceServicePDN;

		/// <summary>
		/// Конструктор.
		/// </summary>
		/// <param name="serviceServicePDN"></param>
		/// <param name="service1C">Сервис 1С</param>
		/// <param name="logger">Логгер</param>
		public PDNEditController(IServicePDN serviceServicePDN, IService1СSoap service1C, ILogger<PDNEditController> logger) {
			_ServiceServicePDN	= serviceServicePDN;
			_Logger				= logger;
		}

		/// <summary>
		/// Список всех договоров с ошибкой в расчетах ПДН.
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public async Task<IActionResult> Index() {
			return View(await _ServiceServicePDN.GetAccountsWithPDNErrorAsync(CancellationToken.None));
		}

		/// <summary>
		/// Обновить список.
		/// </summary>
		/// <param name="account1CCode"></param>
		/// <returns></returns>
		[HttpPost]
		public IActionResult Index(string account1CCode) {
			return View();
		}

		/// <summary>
		/// Открыть форму редактирования отдельной строки расчета ПДН.
		/// </summary>
		/// <param name="account1CCode">Номер договора</param>
		/// <param name="reportDate">Дата договора (расчета)</param>
		/// <returns></returns>
		[HttpGet]
		public IActionResult Edit(string account1CCode, DateTime reportDate) {
			PDNEditEdit pdnEditIndex = new PDNEditEdit() {
				Account1CCode	= account1CCode,
				ReportDate		= reportDate
			};

			return View(pdnEditIndex);
		}

		/// <summary>
		/// Сохранить отредактированные данные расчета ПДН.
		/// </summary>
		/// <param name="reportDate">Дата расчета</param>
		/// <param name="account1CCode">Номер договора</param>
		/// <param name="pdnCards">Расчет ПДН карт</param>
		/// <param name="pdnNonCards">Расчет ПДН НЕ карт</param>
		/// <param name="creditHistoryID">Анкета на основании которой сделан расчет</param>
		/// <param name="pdnAccept">Принять данные как есть (считать их верными)</param>
		/// <returns></returns>
		[HttpPost]
		public async Task<IActionResult> Edit(DateTime reportDate, string account1CCode, PDNCard[] pdnCards,
			PDNNonCard[] pdnNonCards, int creditHistoryID, bool pdnAccept) {
			
			pdnCards.AsParallel().ForAll(i => i.Errors = i.CheckPDNError(reportDate));
			pdnNonCards.AsParallel().ForAll(i => i.Errors = i.CheckPDNError(reportDate));

			PDNInfoList pdnInfoList = new PDNInfoList {
				PDNCards		= pdnCards,
				PDNNonCards		= pdnNonCards,
				Manual			= true,
				Account1CID		= account1CCode,
				ReportDate		= reportDate,
				CreditHistoryID	= creditHistoryID,
				PDNAccept		= pdnAccept
			};

			try { await _ServiceServicePDN.SavePDNAsync(pdnInfoList, CancellationToken.None); }
			catch (Exception exception) {
				_Logger.LogError(
					exception,
					"Не удалось сохранить данные ПДН. Пользователь: {login},  данные: {pdnInfoList}, ошибка: {exceptionMessage}",
					HelperASP.Login(User), pdnInfoList, exception.Message);
			}
			

			PDNEditEdit pdnEditIndex = new PDNEditEdit {
				Account1CCode	= account1CCode,
				ReportDate		= reportDate
			};

			if (pdnAccept) return RedirectToAction("Index");

			return View(pdnEditIndex);
		}

	}
}
