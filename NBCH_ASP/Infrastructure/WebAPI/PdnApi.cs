using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NBCH_ASP.Infrastructure.WebAPI {
    public static class PdnApi {
        /// <summary>
        /// Проверить входные параметры метода Post.
        /// </summary>
        /// <param name="func">Функция результат</param>
        /// <param name="accounts">Договора</param>
        /// <returns>Результат проверки</returns>
        public static ObjectResult PostCheckParams(Func<int, object, ObjectResult> func, string[] accounts) {
            if (accounts == default) {
                return func(StatusCodes.Status400BadRequest, "Не заданы статусы договоров для отбора.");
            }

            return default;
        }
    }
}