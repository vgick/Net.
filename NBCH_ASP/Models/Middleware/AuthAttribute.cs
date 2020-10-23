using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace NBCH_ASP.Models.Middleware {
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public sealed class AuthAttribute : Attribute {

        public AuthRole[] Role {get; private set;}
        
        public AuthAttribute(params AuthRole[] role) {
            Role    = role;
        }

        /// <summary>
        /// Проверить пользователя - есть ли у него необходимая роль приложения,
        /// связанная с AD
        /// </summary>
        /// <param name="httpContext">HttpContext</param>
        /// <param name="role">Список ролей, которые необходимо проверить</param>
        /// <param name="config">Конфигурационный файл приложения</param>
        /// <returns>Есть или нет у пользователя группа приложения, связанная с AD</returns>
        public static bool IsInGroup(HttpContext httpContext, AuthAttribute.AuthRole[] role, IConfiguration config) {
            foreach (AuthRole item in role) {
                IEnumerable<string> groupNames = from id in ((WindowsIdentity)httpContext.User.Identity).Groups
                    select id.Translate(typeof(NTAccount)).Value;

                string roleForCheck = config.GetSection(item.ToString()).Value;
                if (httpContext.User.IsInRole(roleForCheck)) return true;
            }

            return false;
        }

        /// <summary>
        /// Группы пользователей приложения
        /// </summary>
        public enum AuthRole {Admin, User}
	}
}
