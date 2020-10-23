using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NBCH_ASP.Infrastructure;
using NBCH_LIB.Interfaces;
using NBCH_LIB.Models.Registrar;
using static NBCH_ASP.Infrastructure.WebAPI.RegistrarFileApi;

namespace NBCH_ASP.Controllers.WebAPI {
	#if !(DEBUG)
		[PrincipalPermission(SecurityAction.Demand, Role = @"admin")]
	#endif
	[Route("api/[controller]")]
	[ApiController]
	public class RegistrarFileApi : ControllerBase {
		private readonly ILogger<RegistrarFileApi> _Logger;

		/// <summary>
		/// Сервис для работы с документами (файлами).
		/// </summary>
		private readonly IServiceRegistrar _ServiceRegistrar;

		/// <summary>
		/// Сервис по работе с архивом документов.
		/// </summary>
		/// <param name="serviceRegistrar">Сервис по работе с архивом</param>
		/// <param name="logger">Логгер</param>
		public RegistrarFileApi(IServiceRegistrar serviceRegistrar, ILogger<RegistrarFileApi> logger) {
			_ServiceRegistrar	= serviceRegistrar;
			_Logger				= logger;
		}

		/// <summary>
		/// Получить файл по id.
		/// </summary>
		/// <param name="idFile">id файла</param>
		/// <returns>Файл</returns>
		// GET api/<registrarFileAPI>/5
		[HttpGet("{idFile}")]
		public async Task<IActionResult> Get(int idFile) {
			ObjectResult checkResult	=  GetCheckParams(StatusCode, idFile);
			if (checkResult != default) return checkResult;

			RegistrarFileData registrarFileData	= default;

			try {
				registrarFileData	=
					await _ServiceRegistrar.GetRegistrarFileAsync(HelperASP.Login(User), idFile, CancellationToken.None);
			}
			catch (Exception exception) {
				_Logger.LogError(
					exception,
					"Ошибка получения файла. ID файла: {idFile}. Пользователь: {login}, ошибка: {exceptionMessage}",
					idFile, HelperASP.Login(User), exception.Message);
				return StatusCode(StatusCodes.Status500InternalServerError, exception.Message);
			}

			byte[] mas = registrarFileData.Data;
			string fileType;
			string fileName = registrarFileData.FileName;

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

		/// <summary>
		/// Загрузить на сервер файл.
		/// </summary>
		/// <param name="files">Содержимое файла</param>
		/// <param name="idFileDescription">id документа</param>
		/// <param name="client1CCode">Код клиента</param>
		/// <param name="account1CCode">Номер договора</param>
		/// <param name="clientTimeZone">Часовой пояс</param>
		/// <returns>Результат выполнения операции</returns>
		// POST api/<registrarFileAPI>
		[HttpPost]
		public async Task<IActionResult> Post([FromForm] IFormFileCollection files, [FromForm] int idFileDescription,
			[FromForm] string client1CCode, [FromForm] string account1CCode, [FromForm] int clientTimeZone) {
			
			ObjectResult checkResult	=  PostCheckParams(StatusCode, files, idFileDescription, client1CCode,
				account1CCode, clientTimeZone);
			if (checkResult != default) return checkResult;

			Dictionary<string, byte[]> filesToSend = new Dictionary<string, byte[]>();

			if (files.Count > 0) {
				foreach (IFormFile file in files) {
					string fileExtension = Path.GetExtension(file.FileName)?.ToUpper();
					if (
						fileExtension != ".PDF" &&
						fileExtension != ".JPEG" &&
						fileExtension != ".JPG" &&
						fileExtension != ".GIF" &&
						fileExtension != ".PNG" &&
						fileExtension != ".TIF")
						return StatusCode(StatusCodes.Status400BadRequest, "Допускается загружать файлы только с расширением 'PDF, JPEG, JPG, GIF, PNG, TIF.'");

					await using (var memoryStream = new MemoryStream()) {
						if (file.Length <= 0) continue;

						await file.CopyToAsync(memoryStream);
						byte[] fileBytes	= memoryStream.ToArray();
						string fileName		= Path.GetFileName(file.FileName);
						filesToSend.Add(fileName ?? string.Empty, fileBytes);
					}
				}

				try {
					await _ServiceRegistrar.UploadRegistrarFilesAsync(
						HelperASP.Login(User),
						account1CCode,
						client1CCode,
						idFileDescription,
						filesToSend,
						clientTimeZone,
						CancellationToken.None
					);
				}
				catch (Exception exception) {
					_Logger.LogError(
						exception, 
						"Не удалось сохранить файлы на сервер. Пользователь: {login}," +
						" Account1CCode: {account1CCode}, Client1CCode: {client1CCode}," +
						" IdFileDescription: {idFileDescription} , Кол-во файлов {FilesCount}," +
						" Часовой пояс: {clientTimeZone}, Ошибка: {exceptionMessage}",
						HelperASP.Login(User), account1CCode, client1CCode, idFileDescription,
						filesToSend.Count, clientTimeZone, exception.Message);
					return StatusCode(StatusCodes.Status500InternalServerError, exception.Message);
				}

			}

			return Ok();
		}

		/// <summary>
		/// Удалить (установить пометку об удалении) файл.
		/// </summary>
		/// <param name="idFile">id файла</param>
		/// <returns>Результат выполнения операции</returns>
		// DELETE api/<registrarFileAPI>/5
		[HttpDelete("{idFile}")]
		public async Task<IActionResult> Delete(int idFile) {
			if (idFile == default) return StatusCode(400, "Не задан ID файла.");

			try {
				await _ServiceRegistrar.MarkFileAsDeletedAsync(HelperASP.Login(User), idFile, CancellationToken.None);
			}
			catch (Exception ex) {
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}

			return Ok();
		}
	}
}
