using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPM_API.Common;
using WPM_API.Common.Utils;
using WPM_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WPM_API.Controllers.UserManagement
{
    [Authorize(Policy = Constants.Roles.Customer)]
    [Route("settings")]
    public class SettingsController : BasisController
    {
        [HttpPut]
        public IActionResult UpdateSettings([FromBody] UserSettingViewModel userSettings)
        {
            using (var unitOfWork = CreateUnitOfWork())
            {
                var user = unitOfWork.Users.Get(GetCurrentUser().Id);
                user.Email = userSettings.Email;
                user.UserName = userSettings.Name;
                user.Password = PasswordHash.HashPassword(userSettings.Password);
                unitOfWork.Users.MarkForUpdate(user, GetCurrentUser().Id);
                unitOfWork.SaveChanges();
            }

            // Settings were changed.
            return new OkResult();
        }
    }
}