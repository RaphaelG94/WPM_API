using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using WPM_API.Code.Infrastructure;
using WPM_API.Code.Infrastructure.LogOn;
using WPM_API.Common;
using WPM_API.Common.Utils;
using WPM_API.Data.DataContext.Entities;
using WPM_API.Models;
using WPM_API.Models.Api.Token;
using WPM_API.Options;

namespace WPM_API.Controllers
{
    [Route("auth")]
    public class AuthController : BasisController
    {
        public AuthController(AppSettings appSettings, ConnectionStrings connectionStrings, OrderEmailOptions orderEmailOptions, AgentEmailOptions agentEmailOptions, SendMailCreds sendMailCreds, SiteOptions siteOptions, ILogonManager logonManager) : base(appSettings, connectionStrings, orderEmailOptions, agentEmailOptions, sendMailCreds, siteOptions, logonManager)
        {
        }

        // private readonly JwtIssuerOptions _jwtOptions;


        /// <summary>
        /// Authenticate against the API to retrieve an auth token.
        /// </summary>
        /// <param name="credentialsViewModel">The Login</param>
        /// <returns>AuthenticationResponse</returns>
        [HttpPost]
        [AllowAnonymous]
        public IActionResult Login([FromBody] CredentialsViewModel credentialsViewModel)
        {
            var account = UnitOfWork.Users.GetAccountByLoginOrNull(credentialsViewModel.Email);

            if (account == null || !PasswordHash.ValidatePassword(credentialsViewModel.Password, account.Password))
            {
                // LogHolder.MainLog.Info($"Authentication not successful (email unknown or password wrong).");
                return Unauthorized();
            }
            else
            {
                account.UserName = account.UserName.Replace("Ü", "Ue");
                account.UserName = account.UserName.Replace("ü", "ue");
                account.UserName = account.UserName.Replace("Ö", "Oe");
                account.UserName = account.UserName.Replace("ö", "oe");
                account.UserName = account.UserName.Replace("Ä", "Ae");
                account.UserName = account.UserName.Replace("ä", "ae");
            }
            if (!account.Active)
            {
                // LogHolder.MainLog.Info($"Authentication not successful (User " + account.Id + " is not Active).");
                return Unauthorized();
            }
            DateTime? expires = DateTime.UtcNow.AddHours(2);
            var token = logonManager.GenerateToken(new LoggedClaims(account), expires);
            Response.Cookies.Append("aackjwt", token, new CookieOptions() { HttpOnly = true, Secure = true, SameSite = SameSiteMode.Strict });
            var result = new TokenRetrieveModel { IsAuthenticated = true, Token = token, TokenExpiresAt = expires };
            return new OkObjectResult(JsonConvert.SerializeObject(result, serializerSettings));
        }

        [HttpGet]
        [Route("getClaimsForUi")]
        [Authorize(Policy = Constants.Policies.Admin)]
        public IActionResult GetClaimsForUIPermissions()
        {
            List<Claim> claims = new List<Claim>();
            foreach (ClaimsIdentity userIdentity in HttpContext.User.Identities)
            {
                claims.AddRange(userIdentity.Claims.ToList());
            }
            return Ok();
        }

        [HttpGet]
        [Route("resetPassword/{email}")]
        [AllowAnonymous]
        public IActionResult ResetPassword([FromRoute] string email)
        {
            User user = UnitOfWork.Users.GetAll().Where(x => x.Email == email).FirstOrDefault();
            if (user == null)
            {
                return BadRequest("ERROR: The password could not be reset. The user does not exist");
            }

            string newPassword = GenerateRandomPW();
            user.Password = PasswordHash.HashPassword(newPassword);
            UnitOfWork.Users.MarkForUpdateAnonymous(user);
            UnitOfWork.SaveChanges();

            // Inform user via mail
            SmtpClient client = new SmtpClient(sendMailCreds.Host, sendMailCreds.Port);
            NetworkCredential data = new NetworkCredential(sendMailCreds.Email, sendMailCreds.Password);
            client.Credentials = data;
            MailAddress from = new MailAddress(sendMailCreds.Email, sendMailCreds.DisplayName);
            MailAddress to = new MailAddress(email);
            MailMessage message = new MailMessage(from, to);
            message.Body = new StringBuilder("Dear " + user.UserName + ",<br /><br />you requested a password reset.<br /> Your new password is: <h4>"
                + newPassword + "</h4><br />Please change your password after the first login immediately.<br /><br />Sincerely,<br /><br />the Bitstream Team").ToString();
            message.BodyEncoding = System.Text.Encoding.UTF8;
            message.Subject = "Reset Password Request";
            message.SubjectEncoding = System.Text.Encoding.UTF8;
            message.IsBodyHtml = true;
            client.EnableSsl = true;
            client.Send(message);
            message.Dispose();
            return Ok();
        }

        [HttpPut]
        [Authorize(Constants.Policies.Customer)]
        public IActionResult RemoveCookie()
        {
            if (HttpContext.Request.Cookies["aackjwt"] != null)
            {
                Response.Cookies.Delete("aackjwt");
                return Ok();
            }
            return Unauthorized();
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("ping")]
        public IActionResult Ping()
        {
            return Ok();
        }

        private string GenerateRandomPW()
        {
            int length = 8;
            string validChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*?";
            Random random = new Random();
            char[] chars = new char[length];

            // Generate PW
            for (int i = 0; i < length; i++)
            {
                chars[i] = validChars[random.Next(0, validChars.Length)];
            }
            return new string(chars);
        }
    }
}