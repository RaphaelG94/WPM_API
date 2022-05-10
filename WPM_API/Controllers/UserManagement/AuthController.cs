using System;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using WPM_API.Models;
using WPM_API.Code.Infrastructure.LogOn;
using WPM_API.Models.Api.Token;
using WPM_API.Common.Utils;
using WPM_API.Common.Logs;
using WPM_API.Data.DataContext.Entities;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using Microsoft.AspNetCore.Http;
using WPM_API.Common;
using System.Security.Claims;
using System.Collections;
using System.Collections.Generic;

namespace WPM_API.Controllers
{
    [Route("auth")]
    public class AuthController : BasisController
    {
        // private readonly JwtIssuerOptions _jwtOptions;
        private readonly ILogonManager _logonManager;

        public AuthController(ILogonManager logonManager)
        {
            _logonManager = logonManager;
        }

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
            account.UserName = account.UserName.Replace("�", "Ue");
            account.UserName = account.UserName.Replace("�", "ue");
            account.UserName = account.UserName.Replace("�", "Oe");
            account.UserName = account.UserName.Replace("�", "oe");
            account.UserName = account.UserName.Replace("�", "Ae");
            account.UserName = account.UserName.Replace("�", "ae");
            if (account == null || !PasswordHash.ValidatePassword(credentialsViewModel.Password, account.Password))
            {
                LogHolder.MainLog.Info($"Authentication not successful (email unknown or password wrong).");
                return Unauthorized();
            }
            if (!account.Active)
            {
                LogHolder.MainLog.Info($"Authentication not successful (User " + account.Id + " is not Active).");
                return Unauthorized();
            }
            DateTime? expires = DateTime.UtcNow.AddHours(2);
            var token = _logonManager.GenerateToken(new LoggedClaims(account), expires);
            Response.Cookies.Append("aackjwt", token, new CookieOptions() { HttpOnly = true, Secure = true, SameSite = SameSiteMode.Strict });
            var result = new TokenRetrieveModel { IsAuthenticated = true, Token = token, TokenExpiresAt = expires };
            return new OkObjectResult(JsonConvert.SerializeObject(result, _serializerSettings));
        }

        [HttpGet]
        [Route("addClaimsMSAL")]
        [Authorize]
        public IActionResult AddClaims()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                IEnumerable<Claim> claims = identity.Claims;
            }
            return Ok();
        }

        [HttpGet]
        [Route("resetPassword/{email}")]
        [AllowAnonymous]
        public IActionResult ResetPassword ([FromRoute] string email)
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
            SmtpClient client = new SmtpClient(_sendMailCreds.Host, _sendMailCreds.Port);
            NetworkCredential data = new NetworkCredential(_sendMailCreds.Email, _sendMailCreds.Password);
            client.Credentials = data;
            MailAddress from = new MailAddress(_sendMailCreds.Email, _sendMailCreds.DisplayName);
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