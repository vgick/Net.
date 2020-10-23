using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NBCH_ASP.Infrastructure.WebAPI {
    public static class PhotoApi {
        /// <summary>
        /// Проверить входные параметры метода Post.
        /// </summary>
        /// <param name="func">Функция результат</param>
        /// <param name="client1CCode">Номер договора 1С</param>
        /// <returns>Результат проверки</returns>
        public static ObjectResult GetCheckParams(Func<int, object, ObjectResult> func, string client1CCode) {
            if (client1CCode == default) {
                return func(StatusCodes.Status400BadRequest, "Не заданы код клиента 1С.");
            }

            return default;
        }
    }
}