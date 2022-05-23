using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WPM_API.Code.Infrastructure;
using WPM_API.Code.Infrastructure.LogOn;
using WPM_API.Common;
using WPM_API.Common.Utils;
using WPM_API.Models;
using WPM_API.Options;

namespace WPM_API.Controllers.UserManagement
{
    [Authorize(Policy = Constants.Roles.Customer)]
    [Route("settings")]
    public class SettingsController : BasisController
    {
        public SettingsController(AppSettings appSettings, ConnectionStrings connectionStrings, OrderEmailOptions orderEmailOptions, AgentEmailOptions agentEmailOptions, SendMailCreds sendMailCreds, SiteOptions siteOptions, ILogonManager logonManager) : base(appSettings, connectionStrings, orderEmailOptions, agentEmailOptions, sendMailCreds, siteOptions, logonManager)
        {
        }

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