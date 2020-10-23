using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NBCH_ASP.Infrastructure.WebAPI {
    public static class PostsApi {
        /// <summary>
        /// Проверить входные параметры метода Get.
        /// </summary>
        /// <param name="func">Функция результат</param>
        /// <param name="account1CCode">Номер договора 1С</param>
        /// <returns></returns>
        internal static ObjectResult GetCheckParams(Func<int, object, ObjectResult> func, string account1CCode) {
            if (string.IsNullOrEmpty(account1CCode)) {
                return func(StatusCodes.Status400BadRequest, "Не задан номер договора 1С.");
            }

            return default;
        }
        
        /// <summary>
        /// Проверить входные параметры метода Post.
        /// </summary>
        /// <param name="func">Функция результат</param>
        /// <param name="account1CCode">Номер договора 1С</param>
        /// <param name="date">Дата</param>
        /// <param name="clientTimeZone">Часовой пояс</param>
        /// <param name="message">Сообщение</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal static ObjectResult PostCheckParams(Func<int, object, ObjectResult> func, string account1CCode,
            DateTime date, int clientTimeZone, string message) {

            if (string.IsNullOrEmpty(account1CCode)) {
                return func(StatusCodes.Status400BadRequest, "Не заданы номер договора 1С.");
            }

            if (date == default) {
                return func(StatusCodes.Status400BadRequest, "Не задана дата.");
            }

            if (clientTimeZone == default) {
                return func(StatusCodes.Status400BadRequest, "Не задан часовой пояс.");
            }

            if (string.IsNullOrEmpty(message)) {
                return func(StatusCodes.Status400BadRequest, "Сообщение пустое.");
            }

            return default;
        }
    }
}