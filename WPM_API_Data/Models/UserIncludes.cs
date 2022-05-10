using System;
using System.Collections.Generic;
using System.Text;

namespace  WPM_API.Data.Models
{
    public static class UserIncludes
    {
        public const string Systemhouse = "Systemhouse";
        public const string Customer = "Customer";
        public const string UserRoles = "UserRoles";
        public const string Roles = "UserRoles.Role";
        public static string[] GetAllIncludes()
        {
            string[] includes =
            {
                UserIncludes.Systemhouse, UserIncludes.Customer, UserIncludes.Roles, UserIncludes.UserRoles
            };
            return includes;
        }
    }
}
