using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NBCH_ASP.Infrastructure.WebAPI {
    public class RegistrarFileApi {
        /// <summary>
        /// Проверить входные параметры метода Get.
        /// </summary>
        /// <param name="func">Функция результат</param>
        /// <param name="idFile">ID файла</param>
        /// <returns></returns>
        internal static ObjectResult GetCheckParams(Func<int, object, ObjectResult> func, int idFile) {
            if (idFile == default) {
                return func(StatusCodes.Status400BadRequest, "Не задан ID файла.");
            }

            return default;
        }

        /// <summary>
        /// Проверить входные параметры метода Post.
        /// </summary>
        /// <param name="func">Функция результат</param>
        /// <param name="files"></param>
        /// <param name="idFileDescription"></param>
        /// <param name="client1CCode"></param>
        /// <param name="account1CCode">Номер договора 1С</param>
        /// <param name="clientTimeZone">Часовой пояс</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal static ObjectResult PostCheckParams(Func<int, object, ObjectResult> func, IFormFileCollection files,
            int idFileDescription, string client1CCode, string account1CCode, int clientTimeZone) {

            if (files == default || files.Count == 0) {
                return func(StatusCodes.Status400BadRequest, "Нет файлов для загрузки на сервер.");
            }

            if (idFileDescription == default) {
                return func(StatusCodes.Status400BadRequest, "Не задан вид загружаемого файла.");
            }

            if (clientTimeZone == default) {
                return func(StatusCodes.Status400BadRequest, "Не задан часовой пояс.");
            }

            if (string.IsNullOrEmpty(client1CCode)) {
                return func(StatusCodes.Status400BadRequest, "Не задан код клиента.");
            }

            if (string.IsNullOrEmpty(account1CCode)) {
                return func(StatusCodes.Status400BadRequest, "Не задан номер договора 1С.");
            }

            return default;
        }
    }
}