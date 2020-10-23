using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.ServiceModel;
using System.Threading;
using Microsoft.Extensions.Logging;
using NBCH_ASP.Infrastructure;
using NBCH_ASP.Infrastructure.DataFromConfigurationFile.ISecrets;
using NBCH_ASP.Infrastructure.NBCH;
using NBCH_ASP.Models.Registrar;
using NBCH_ASP.Models.Registrar.RegistrarDocuments;
using NBCH_LIB;
using NBCH_LIB.Interfaces;
using NBCH_LIB.Models;
using NBCH_LIB.Models.Registrar;
using NBCH_LIB.SOAP.SOAP1C;

namespace NBCH_ASP.Controllers.Registrar {
	[Authorize(Roles = @"admin")]
	public class RegistrarDocumentsController : Controller {
		/// <summary>
		/// Логгер.
		/// </summary>
		private ILogger<RegistrarDocumentsController> _Logger;
		
		/// <summary>
		/// Данные для подключения к сервису 1С.
		/// </summary>
		private readonly ISecret1C _Secret1Cs;

		/// <summary>
		/// Сервис для работы с документами.
		/// </summary>
		private readonly IServiceRegistrar _ServiceRegistrar;

		/// <summary>
		/// Сервис для работы с 1С.
		/// </summary>
		private readonly IService1СSoap _Service1C;

		/// <summary>
		/// Конструктор.
		/// </summary>
		/// <param name="service1C">Сервис 1С</param>
		/// <param name="serviceRegistrar">Сервис по работе с документами</param>
		/// <param name="secret1Cs">Данные для подключения</param>
		/// <param name="logger">Логгер</param>
		public RegistrarDocumentsController(IService1СSoap service1C, IServiceRegistrar serviceRegistrar,
			ISecret1C secret1Cs, ILogger<RegistrarDocumentsController> logger) {
			
			_ServiceRegistrar	= serviceRegistrar;
			_Service1C	= service1C;
			_Secret1Cs	= secret1Cs;
			_Logger		= logger;
		}

		/// <summary>
		/// Форма для ввода данных.
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public IActionResult Index() {
			RegistrarDocumentsIndex registrarDocumentsIndex		= new RegistrarDocumentsIndex() {
				RegionsWebServiceListName	= Secret1C.GetRegions(_Secret1Cs, Request.Cookies[Startup.WebService1CRegion] ?? _Secret1Cs.Servers.Keys.First())
			};

			SetOrganizationsList();

			return View(registrarDocumentsIndex);
		}

		/// <summary>
		/// Список доступных организаций.
		/// </summary>
		private void SetOrganizationsList() {
			Organization.Organizations[] orgs = Organization.OrganizationsByLogin(User.Identity.Name);
			Dictionary<Organization.Organizations, bool> orgsSelected = new Dictionary<Organization.Organizations, bool>();
			foreach (Organization.Organizations organization in orgs) {
				orgsSelected.Add(organization, true);
			}

			ViewData["orgs"] = orgsSelected;
		}

		/// <summary>
		/// Отобразить документы из архива.
		/// </summary>
		/// <param name="account1CCode">Номер договора</param>
		/// <param name="regionWebServiceListName">Регион пользователя</param>
		/// <param name="orgs">Организации</param>
		/// <returns></returns>
		[HttpPost]
		public async Task<IActionResult> Index(string account1CCode, string regionWebServiceListName, string[] orgs) {
			(Client[] Clients, string[] Errors) creditDocument	= await RegistrarDocuments.GetClientsFrom1CAccountAsync(_Service1C, _Secret1Cs, regionWebServiceListName, account1CCode);

			RegistrarDocumentsIndex model = new RegistrarDocumentsIndex() {
				Clients			= creditDocument.Clients,
				Errors			= creditDocument.Errors,
				Account1CCode	= account1CCode,
				RegionsWebServiceListName	= Secret1C.GetRegions(_Secret1Cs, Request.Cookies[Startup.WebService1CRegion] ?? _Secret1Cs.Servers.Keys.First())
			};

			ViewData["orgs"]	= Organization.OrganizationsByLogin(User.Identity.Name, orgs);
			Response.Cookies.Append(Startup.WebService1CRegion, regionWebServiceListName);

			return View(model);
		}

		/// <summary>
		/// Сохранить файл на сервере.
		/// </summary>
		/// <param name="files">Файл</param>
		/// <param name="idFileDescription">ID документа</param>
		/// <param name="client1CCode">Код клиента</param>
		/// <param name="account1CCode">Номер договора</param>
		/// <param name="clientTimeZone">Часовой пояс клиента</param>
		/// <returns></returns>
		[HttpPost]
		public async Task<IActionResult> UploadFiles(IFormFileCollection files, int idFileDescription, string client1CCode, string account1CCode, int clientTimeZone) {
			if (clientTimeZone == default) return StatusCode(400, "Не задан часовой пояс клиента");

			Dictionary<string, byte[]> filesToSend	= new Dictionary<string, byte[]>();

			if ((files?.Count ?? 0)> 0) {
				foreach (IFormFile file in files) {
					string fileExtension = Path.GetExtension(file.FileName)?.ToUpper();
					if (
						fileExtension != ".PDF"  &&
						fileExtension != ".JPEG" &&
						fileExtension != ".JPG"  &&
						fileExtension != ".GIF"  &&
						fileExtension != ".PNG"  &&
						fileExtension != ".TIF")
					{return StatusCode(400, "Допускается загружать файлы только с расширением 'PDF, JPEG, JPG, GIF, PNG, TIF'");}

					await using var memoryStream = new MemoryStream();
						if (file.Length <= 0) continue;
						await file.CopyToAsync(memoryStream);
							byte[] fileBytes	= memoryStream.ToArray();
							string fileName		= Path.GetFileName(file.FileName);
							filesToSend.Add(fileName ?? string.Empty, fileBytes);
				}

				if (idFileDescription != default && client1CCode != default && account1CCode != default && filesToSend.Count > 0)
					try {
						await _ServiceRegistrar.UploadRegistrarFilesAsync(User.Identity.Name, account1CCode,
							client1CCode, idFileDescription, filesToSend, clientTimeZone, CancellationToken.None);
					}
					catch (Exception exception) {
						_Logger.LogError(
							exception,
							"Не удалось сохранить файл на сервер. Пользователь: {login}, idFileDescription: {idFileDescription}, client1CCode: {client1CCode}," +
							" account1CCode: {account1CCode}, clientTimeZone: {clientTimeZone}, filesCount {filesCount}, ошибка: {exceptionMessage}",
							HelperASP.Login(User), idFileDescription, client1CCode, account1CCode, clientTimeZone, files.Count,  exception.Message);
						return StatusCode(StatusCodes.Status500InternalServerError, exception.Message);
					}
			}

			return Ok();
		}

		/// <summary>
		/// Скачать файл из хранилища.
		/// </summary>
		/// <param name="idFile">ID файла</param>
		/// <returns></returns>
		public async Task<FileResult> DownloadFile(int idFile) {
			if (idFile != default) {

				RegistrarFileData registrarFileData	= new RegistrarFileData();
				try {
					registrarFileData =
						await _ServiceRegistrar.GetRegistrarFileAsync(User.Identity.Name, idFile, CancellationToken.None);
				}
				catch (Exception exception) {
					_Logger.LogError(
						exception,
						"Не удалось получить файл с сервера. Пользователь: {login}, idFile: {idFile}, ошибка: {exceptionMessage}",
						HelperASP.Login(User), idFile, exception.Message);
				}

				byte[] mas		= registrarFileData.Data;
				string fileType;
				string fileName	= registrarFileData.FileName;

				switch (Path.GetExtension(registrarFileData.FileName)?.ToUpper()) {
					case ".PDF":
						fileType = "application/pdf";
						break;
					case ".JPEG":
					case ".JPG":
						fileType = "image/jpeg";
						break;
					case ".GIF":
						fileType = "image/gif";
						break;
					case ".PNG":
						fileType = "image/png";
						break;
					case ".TIF":
						fileType = "image/tiff";
						break;
					default:
						fileType = "application/pdf";
						break;
				}

				return File(mas, fileType, fileName);
			}

			return null;
		}

		/// <summary>
		/// Получить список файлов по документу.
		/// </summary>
		/// <param name="account1CCode">Номер договора</param>
		/// <param name="client1CCode">Код клиента</param>
		/// <param name="affiliationOfAccount">Принадлежность клиента в договору (основной заемщик/поручитель)</param>
		/// <returns></returns>
		public IActionResult UpdateRegistrar(string account1CCode, string client1CCode, Client.EAffiliationOfAccount affiliationOfAccount) {
			return ViewComponent("RegistrarClientDocuments", new {
				Account1CCode	= account1CCode,
				Client			= new Client() {
					Code1C					= client1CCode,
					AffiliationOfAccount	= affiliationOfAccount
				}
			});
		}

		/// <summary>
		/// Удалить файл.
		/// </summary>
		/// <param name="idFile"></param>
		/// <returns></returns>
		public async Task<IActionResult> DeleteFile(int idFile) {
			if (idFile == default) return StatusCode(400, "Не задан ID файла");

			try {
				await _ServiceRegistrar.MarkFileAsDeletedAsync(User.Identity.Name, idFile, CancellationToken.None);
			}
			catch (Exception exception) {
				_Logger.LogError(
					exception,
					"Файл на сервере не удален. Пользователь: {login}, idFile: {idFile}, ошибка: {exceptionMessage}",
					HelperASP.Login(User), idFile, exception.Message);
				return StatusCode(StatusCodes.Status500InternalServerError, exception.Message);
			}

			return Ok();
		}


		/// <summary>
		/// Получить представление по договорам в статусе на проверке.
		/// </summary>
		/// <param name="region">Регион (для выбора приоритетного сервера)</param>
		/// <param name="orgs">Список организаций для фильтра</param>
		/// <returns>Список счетов</returns>
		public IActionResult GetAccountTable(string region, string[] orgs) {
			Dictionary<Organization.Organizations, bool> organizations	= Organization.OrganizationsByLogin(User.Identity.Name, orgs);
			SOAP1C.AccountStatus[] accst = new SOAP1C.AccountStatus[] { SOAP1C.AccountStatus.Verification, SOAP1C.AccountStatus.CheckSB, SOAP1C.AccountStatus.New, SOAP1C.AccountStatus.OnClientAssign};

			return ViewComponent("AccountTable", new { region, orgs = organizations, accountStatus = accst });
		}
	}
}
