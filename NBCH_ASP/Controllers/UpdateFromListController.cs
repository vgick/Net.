using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NBCH_ASP.Infrastructure.DataFromConfigurationFile.ISecrets;
using NBCH_ASP.Infrastructure.NBCH;
using NBCH_ASP.Models;
using NBCH_ASP.Models.NBCH.NBCHRequest;
using NBCH_LIB.Interfaces;
using NBCH_LIB.Models.PDN;
using NBCH_LIB.SOAP.SOAPNBCH;

namespace NBCH_ASP.Controllers
{
	/// <summary>
	/// Служебный контроллер.
	/// </summary>
	[Authorize(Roles = @"admin")]
	public class UpdateFromListController : Controller
	{
		/// <summary>
		/// Сервис 1С.
		/// </summary>
		private readonly IService1СSoap _Service1C;

		/// <summary>
		/// Сервис для работы с данными НБКИ.
		/// </summary>
		private readonly IServicePDN _ServiceServicePDN;

		/// <summary>
		/// Настройки подключения к 1С.
		/// </summary>
		private readonly ISecret1C _Secret1Cs;

		/// <summary>
		/// Настройки программы.
		/// </summary>
		private readonly IConfiguration _Configuration;

		/// <summary>
		/// Конструктор.
		/// </summary>
		/// <param name="service1C">Сервис для работы с 1С</param>
		/// <param name="serviceServicePDN">Сервис для работы с данными НБКИ</param>
		/// <param name="secret1C">Настройки подключения к 1С</param>
		/// <param name="configuration">Настройки программы</param>
		public UpdateFromListController(IService1СSoap service1C, IServicePDN serviceServicePDN, ISecret1C secret1C, IConfiguration configuration) {
            _Service1C		= service1C;
			_ServiceServicePDN		= serviceServicePDN;
			_Secret1Cs		= secret1C;
			_Configuration	= configuration;
        }

		/// <summary>
		/// Загрузить список договоров для работы с ними.
		/// </summary>
		/// <returns></returns>
        [HttpGet]
        public IActionResult Index() {
            string[] file   = System.IO.File.ReadAllLines("AccountList.txt");
            return View(file);
        }

		/// <summary>
		/// Обновить договора.
		/// </summary>
		/// <param name="file">Файл со списком договоров</param>
		/// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Index(string[] file){

			CancellationToken cancellationToken	= CancellationToken.None;
			List<string> message	= new List<string>();
			ModelState.Clear();

            foreach (string account1C in file) {
				IndexModel credit	= await NBCHRequest.GetDataFrom1CAsync(_Service1C, account1C, _Secret1Cs, "Дальний восток", _Configuration);

				if (!String.IsNullOrEmpty(credit.Account1CCode) && !String.IsNullOrEmpty(credit.Client1CCode))
					try {
						PDNInfoList pdnInfoListN = await _ServiceServicePDN.GetSavedPDNAsync(account1C, cancellationToken);
						if (pdnInfoListN == default) {
							PDNInfoList pdnInfoList = await _ServiceServicePDN.CalculatePDNAsync(credit.Account1CCode, credit.Account1CDate, credit.Client1CCode, cancellationToken);
							if (pdnInfoList.ReportDate >= credit.Account1CDate.AddDays(-SOAPNBCH.NBCHAnketaExpiredDay)) {
								await _ServiceServicePDN.SavePDNAsync(pdnInfoList, cancellationToken);
								message.Add($"Договор {credit.Account1CCode}, данные обновлены");
							}
							else {
								message.Add($"Договор {credit.Account1CCode}, для расчета ПДН, требуется обновить анкету НБКИ");
							}
						};
					}
					catch (FaultException ex) {
						message.Add($"Договор {credit.Account1CCode}, {ex.Message}");
					}
					catch (PDNAnketaNotFoundException) {
						message.Add($"Договор {credit.Account1CCode}, необходимо запросить анкету НБКИ");
					}
					catch (EndpointNotFoundException) {
						message.Add($"Договор {credit.Account1CCode}, не удалось подключиться к службе NBCH (расчет ПДН)");
					}
					catch (Exception ex) {
						message.Add($"Договор {credit.Account1CCode}, {ex.Message.ToString()}");
					}
			}


			return View(message.ToArray());
        }


		[HttpGet]
		public IActionResult UpdateAccounts() {
			string[] file = System.IO.File.ReadAllLines("AccountList.txt");
			return View("Index", file);
		}

		[HttpPost]
		public async Task<IActionResult> UpdateAccounts(string[] file) {
			string[] file1 = await System.IO.File.ReadAllLinesAsync("AccountList.txt");

			List<string> message = new List<string>();
			ModelState.Clear();

			file1.AsParallel().ForAll(async i => await NBCHRequest.GetDataFrom1CAsync(_Service1C, i, _Secret1Cs, "Дальний восток", _Configuration));

			return View("Index", message.ToArray());
		}





	}
}