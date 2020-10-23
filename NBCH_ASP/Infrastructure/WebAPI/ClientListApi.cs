using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NBCH_ASP.Infrastructure.WebAPI {
    public static class ClientListApi {
        /// <summary>
        /// Проверить входные параметры Get.
        /// </summary>
        /// <param name="func">Функция результат</param>
        /// <param name="account">Номер договора 1С</param>
        /// <param name="region">Регион</param>
        /// <returns>Результат проверки</returns>
        public static ObjectResult GetCheckParams(Func<int, object, ObjectResult> func, string account, string region) {
            if (string.IsNullOrEmpty(region))
                return func(StatusCodes.Status400BadRequest, "Не задан код региона.");
            
            if (string.IsNullOrEmpty(region))
                return func(StatusCodes.Status400BadRequest, "Не задан Номер договора.");

            return default;
        }
        
    }
}