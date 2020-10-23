using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NBCH_ASP.Infrastructure.DataFromConfigurationFile.ISecrets;
using NBCH_ASP.Models.WebAPI.AccountsListApi;
using NBCH_LIB;
using NBCH_LIB.SOAP.SOAP1C;
using NBCH_LIB.SOAP.SOAP1C.GetAccountsList;

namespace NBCH_ASP.Infrastructure.WebAPI {
    internal static class AccountsListApi {
        /// <summary>
        /// Получить статус договора в строковом виде из порядкового номера.
        /// </summary>
        /// <param name="accountStatuses">Номера статусов</param>
        /// <returns>Статусы в строковом представлении</returns>
        internal static string[] GetAccountStatusList(int[] accountStatuses) {
            return accountStatuses.Select(accountStatus => ((SOAP1C.AccountStatus) accountStatus).
                    GetDescription()).
                ToArray();
        }

        /// <summary>
        /// Проверить входные параметры метода Get. 
        /// </summary>
        /// <param name="func">Функция результат</param>
        /// <param name="accountStatus">Список статусов</param>
        /// <param name="region">Регион</param>
        /// <param name="secret1C">Настройки подключения к 1С</param>
        /// <returns>Результат проверки</returns>
        internal static ObjectResult GetCheckParams(Func<int, object, ObjectResult> func, int[] accountStatus,
            string region, ISecret1C secret1C) {
            
            if (accountStatus == default || accountStatus.Length == 0)
                return func(StatusCodes.Status400BadRequest, "Не заданы статусы договоров для отбора.");
            if (string.IsNullOrEmpty(region))
                return func(StatusCodes.Status400BadRequest, "Не задан код региона.");
            if (!secret1C.Servers.Keys.Contains(region))
                return func(StatusCodes.Status400BadRequest, "Неизвестный регион.");

            return default;
        }

        /// <summary>
        /// Оставить только доступные по организации договора.
        /// </summary>
        /// <param name="accountLegendNResult">Список договоров</param>
        /// <param name="user">Пользователь AD</param>
        internal static void FilterByOrganization(AccountLegendNResultApi accountLegendNResult, ClaimsPrincipal user) {
            Organization.Organizations[] organizations	= Organization.OrganizationsByLogin(HelperASP.Login(user));
            List<AccountLegendApi> lst					= new List<AccountLegendApi>();

            foreach (Organization.Organizations org in organizations) {
                lst.AddRange(accountLegendNResult.AccountLegendApi.Where(i => i.organization_name.Equals(org.GetDescription())));
            }

            accountLegendNResult.AccountLegendApi	= lst.ToArray();
        }

    }
}