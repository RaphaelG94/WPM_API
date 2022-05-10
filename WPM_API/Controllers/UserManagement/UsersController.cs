using WPM_API.Common;
using WPM_API.Common.Utils;
using WPM_API.Data.DataContext.Entities;
using WPM_API.Data.DataContext.Projections.Users;
using WPM_API.Data.Models;
using WPM_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using static WPM_API.Common.Constants;
using ROLES = WPM_API.Models.Role;

namespace WPM_API.Controllers
{
    [Route("users")]
    public class UsersController : BasisController
    {

        /// <summary>
        /// Retrieve all users.
        /// Visibitily depends on authenticated user role
        /// 1) Admins see all users
        /// 2) Systemhouse managers see users of the same systemhouse and all their customers
        /// 3) Customer manager see coworkers of their company.
        /// </summary>
        /// <returns>[User]</returns>
        [HttpGet]
        [Authorize(Policy = Constants.Roles.Customer)]
        public IActionResult GetUsers()
        {
            List<User> users = new List<User>();
            AccountProjection user = GetCurrentUser();

            if (CurrentUserIsInRole(Constants.Roles.Admin))
            {
                // Admin: Darf alle User sehen
                users = UnitOfWork.Users.GetAll(UserIncludes.Systemhouse, UserIncludes.Customer, UserIncludes.Roles).ToList();
            }
            else if (CurrentUserIsInRole(Constants.Roles.Systemhouse))
            {
                // Systemhouse_Manager: Darf alle User sehen die dem Systemhouse unterstellt sind
                users = UnitOfWork.Users.GetAll(UserIncludes.Systemhouse, UserIncludes.Customer, UserIncludes.Roles).Where(x => x.SystemhouseId.Equals(user.SystemhouseId)).ToList();
            }
            else if (CurrentUserIsInRole(Constants.Roles.Customer))
            {
                // Customer_Manager: Darf alle User sehen die dem Customer unterstellt sind
                users = UnitOfWork.Users.GetAll(UserIncludes.Systemhouse, UserIncludes.Customer, UserIncludes.Roles).Where(x => x.CustomerId.Equals(user.CustomerId)).ToList();
            }

            var json = JsonConvert.SerializeObject(Mapper.Map<List<User>, List<UserViewModel>>(users), _serializerSettings);
            return new OkObjectResult(json);
        }

        /// <summary>
        /// Create new User.
        /// </summary>
        /// <param name="userAdd">New User</param>
        /// <returns>User</returns>
        [HttpPost]
        [Authorize(Policy = Constants.Roles.Customer)]
        public IActionResult AddUser([FromBody] AddUserViewModel userAdd)
        {
            // Nur ein Admin darf Admin anlegen
            if (userAdd.Admin && !CurrentUserIsInRole(Constants.Roles.Admin))
            {
                return BadRequest("Only adminstrator can add new administrator.");
            }
            // Wenn Admin, dann kann nur SystemhouseManager sein
            if (userAdd.Admin && userAdd.Role != ROLES.systemhouse)
            {
                return BadRequest("User could not be created. An admin has to be a systemhouse-manager.");
            }
            // Wenn Customer muss Systemhouse bef�llt sein.
            if (userAdd.Role == ROLES.customer && (string.IsNullOrEmpty(userAdd.Systemhouse) || string.IsNullOrEmpty(userAdd.Customer)))
            {
                return BadRequest("User could not be created. A customer-manager needs a customer and a systemhouse.");
            }
            // Wenn Systemhouse, kein Customer
            if (userAdd.Role == ROLES.systemhouse && (string.IsNullOrEmpty(userAdd.Systemhouse) || !string.IsNullOrEmpty(userAdd.Customer)))
            {
                return BadRequest("User could not be created. A systemhouse-manager needs a customer and must not have a systemhouse.");
            }
            // E-Mail has to be unique
            if (!isEmailUnique(userAdd.Email))
            {
                return BadRequest("User could not be created. Email has to be unique.");
            }

            User user = UnitOfWork.Users.CreateEmpty(GetCurrentUser().Id);
            user.UserName = userAdd.Name;
            user.Email = userAdd.Email;
            user.Login = userAdd.Email;
            user.Deletable = true;
            user.Active = userAdd.Active;
            user.Password = PasswordHash.HashPassword(userAdd.Password);

            if (!string.IsNullOrEmpty(userAdd.Customer))
            {
                // Customer darf nur weiter Customer seines Bereichs hinzuf�gen
                if (CurrentUserIsInRole(Constants.Roles.Customer) && (GetCurrentUser()).CustomerId != userAdd.Customer)
                {
                    return BadRequest("Customer-managers can add user only from the same customer.");
                }
                user.CustomerId = userAdd.Customer;
            }
            if (!string.IsNullOrEmpty(userAdd.Systemhouse))
            {
                // Systemhouse darf nur weiter Systemhouse seines Bereichs hinzuf�gen
                if (!CurrentUserIsInRole(Constants.Roles.Admin) && (CurrentUserIsInRole(Constants.Roles.Customer) || CurrentUserIsInRole(Constants.Roles.Systemhouse) && (GetCurrentUser()).SystemhouseId != userAdd.Systemhouse))
                {
                    return BadRequest("Only Systemhouse-managers can add user only from the same systemhouse.");
                }
                user.SystemhouseId = userAdd.Systemhouse;
            }

            user.UpdatedByUserId = LoggedUser.Id;
            user.UpdatedDate = DateTime.Now;

            Data.DataContext.Entities.Role role = null;
            try
            {
                role = UnitOfWork.Users.Roles.GetAllRoles().Find(x => x.Name == userAdd.Role.ToString());
            }
            catch (Exception) { }
            UserRole systemhouseUserRole = new UserRole() { Role = role, User = user };
            user.UserRoles.Add(systemhouseUserRole);

            if (userAdd.Admin)
            {
                Data.DataContext.Entities.Role adminRole = null;
                try
                {
                    adminRole = UnitOfWork.Users.Roles.GetAllRoles().Find(x => x.Name == Roles.Admin);
                }
                catch (Exception) { }
                UserRole adminUserRole = new UserRole() { Role = adminRole, User = user };
                user.UserRoles.Add(adminUserRole);
            }

            try
            {
                UnitOfWork.SaveChanges();
            }
            catch (Exception ex)
            {
                return BadRequest("User could not be created. " + ex.Message);
            }
            // Load with added Systemhouse/Customer
            user = UnitOfWork.Users.Get(user.Id, "Systemhouse", "Customer");

            // Customer was created and is returned.
            var json = JsonConvert.SerializeObject(Mapper.Map<User, UserViewModel>(user), _serializerSettings);
            return new OkObjectResult(json);
        }

        /// <summary>
        /// Change an existing user.
        /// </summary>
        /// <param name="userId">Id of the User</param>
        /// <param name="userEdit">Modified User</param>
        /// <returns>User</returns>
        [HttpPut]
        [Authorize(Policy = Constants.Roles.Systemhouse)]
        [Route("{userId}")]
        public IActionResult UpdateUser([FromRoute] string userId, [FromBody] UserEditViewModel userEdit)
        {
            User toEdit = UnitOfWork.Users.Get(userEdit.Id, UserIncludes.GetAllIncludes());
            UserViewModel userViewModel = Mapper.Map<User, UserViewModel>(toEdit);
            string[] roles = userEdit.Role.Split(",");
            foreach (string r in roles)
            {
                r.Trim();
            }
            // Nur ein Admin darf Admin anlegen
            if (userEdit.Admin && !CurrentUserIsInRole(Constants.Roles.Admin))
            {
                return BadRequest("Only adminstrator can edit administrator.");
            }
            if (userEdit.Admin && !CurrentUserIsInRole(Constants.Roles.Systemhouse))
            {
                return BadRequest("User could not be updated. An admin has to be a systemhouse-manager.");
            }
            // Wenn Customer muss Systemhouse befüllt sein.
            if (roles.Contains("customer") && (string.IsNullOrEmpty(userEdit.Systemhouse) || string.IsNullOrEmpty(userEdit.Customer)))
            {
                return BadRequest("User could not be updated. A customer-manager needs a customer and a systemhouse.");
            }
            // Wenn Systemhouse, kein Customer
            if (roles.Contains("systemhouse") && (string.IsNullOrEmpty(userEdit.Systemhouse) || !string.IsNullOrEmpty(userEdit.Customer)))
            {
                return BadRequest("User could not be updated. A systemhouse-manager needs a customer and must not have a systemhouse.");
            }
            // E-Mail has to be unique if it was edited
            if (!isEmailUnique(userEdit.Email) && userEdit.Email != toEdit.Email)
            {
                return BadRequest("User could not be updated. Email has to be unique.");
            }
            // Wenn Customer: You are not allowed to modify users.
            if (CurrentUserIsInRole(Roles.Customer) && !CurrentUserIsInRole(Roles.Admin))
            {
                return Unauthorized();
            }
            toEdit.UserName = userEdit.Name;
            toEdit.Email = userEdit.Email;
            toEdit.Active = userEdit.Active;
            toEdit.Login = userEdit.Email;
            toEdit.UserRoles = new List<UserRole>();

            if (!string.IsNullOrEmpty(userEdit.Customer))
            {
                // Customer darf nur weiter Customer seines Bereichs hinzuf�gen
                if (CurrentUserIsInRole(Constants.Roles.Customer) && (GetCurrentUser()).CustomerId != userEdit.Customer)
                {
                    return BadRequest("Customer-managers can only edit users from the same customer.");
                }
                toEdit.CustomerId = userEdit.Customer;
            }
            if (!string.IsNullOrEmpty(userEdit.Systemhouse))
            {
                // Systemhouse darf nur weiter Systemhouse seines Bereichs hinzuf�gen
                if (!CurrentUserIsInRole(Constants.Roles.Admin) && (CurrentUserIsInRole(Constants.Roles.Customer) || CurrentUserIsInRole(Constants.Roles.Systemhouse) && (GetCurrentUser()).SystemhouseId != userEdit.Systemhouse))
                {
                    return BadRequest("Only Systemhouse-managers can only edit users from the same systemhouse.");
                }
                toEdit.SystemhouseId = userEdit.Systemhouse;
            }

            toEdit.UpdatedByUserId = LoggedUser.Id;
            toEdit.UpdatedDate = DateTime.Now;
            Data.DataContext.Entities.Role role = null;
            try
            {
                role = UnitOfWork.Users.Roles.GetAllRoles().Find(x => x.Name == userEdit.Role.ToString());
            }
            catch (Exception) { }
            UserRole systemhouseUserRole = new UserRole() { Role = role, User = toEdit };
            toEdit.UserRoles.Add(systemhouseUserRole);
            if (systemhouseUserRole.Role.Name == "systemhouse")
            {
                toEdit.Customer = null;
            }
            if (userEdit.Admin)
            {
                Data.DataContext.Entities.Role adminRole = null;
                try
                {
                    adminRole = UnitOfWork.Users.Roles.GetAllRoles().Find(x => x.Name == Roles.Admin);
                }
                catch (Exception) { }
                UserRole adminUserRole = new UserRole() { Role = adminRole, User = toEdit };
                toEdit.UserRoles.Add(adminUserRole);
            }
            UnitOfWork.Users.MarkForUpdate(toEdit, GetCurrentUser().Id);
            UnitOfWork.SaveChanges();
            toEdit = UnitOfWork.Users.Get(userId, UserIncludes.GetAllIncludes());
            var currentUser = GetCurrentUser();

            // Customer was created and is returned.
            var json = JsonConvert.SerializeObject(Mapper.Map<User, UserViewModel>(toEdit), _serializerSettings);
            return new OkObjectResult(json);
        }
        
        /// <summary>
        /// Delete an existing user.
        /// </summary>
        /// <param name="userId">Id of the User</param>
        /// <returns></returns>
        [HttpDelete]
        [Authorize(Policy = Constants.Roles.Systemhouse)]
        [Route("{userId}")]
        public IActionResult DeleteUser([FromRoute] string userId)
        {
            User user = UnitOfWork.Users.GetAll("Customer", "Systemhouse", "UserRoles", "UserRoles.Role").Where(x => x.Id.Equals(userId)).FirstOrDefault();
            UserViewModel userViewModel = Mapper.Map<User, UserViewModel>(user);
            if (user.Deletable == false)
            {
                throw new InvalidOperationException("User is undeletable.");
            }
            // Admin darf nur von Admins gel�scht werden
            if (userViewModel.Admin && !CurrentUserIsInRole(Roles.Admin))
            {
                return BadRequest("Only adminstrators can delete administrator.");
            }

            // Systemhouse darf nicht von anderen Systemh�usern oder Customern gel�scht werden
            if (CurrentUserIsInRole(Roles.Systemhouse) && !CurrentUserIsInRole(Roles.Admin) && (userViewModel.Systemhouse != null))
            {
                if (GetCurrentUser().SystemhouseId != userViewModel.Systemhouse.Id)
                {
                    return BadRequest("You can only delete assigned systemhouses or customer");
                }
            }
            // Wenn Customer: You are not allowed to delete users.
            if (CurrentUserIsInRole(Roles.Customer) && !CurrentUserIsInRole(Roles.Admin))
            {
                return Unauthorized();
            }

            AccountProjection CurrentUser = GetCurrentUser();
            // Nutzer (außer Admins) können sich  nicht selbst löschen
            if ((CurrentUser.Id == userId) && !CurrentUserIsInRole(Roles.Admin))
            {
                return BadRequest("Users can not delete themselves.");
            }

            UnitOfWork.Users.MarkForDelete(user, GetCurrentUser().Id);
            UnitOfWork.SaveChanges();

            // Customer was created and is returned.
            var json = JsonConvert.SerializeObject(Mapper.Map<User, UserViewModel>(user), _serializerSettings);
            return new OkObjectResult(json);
        }

        /// <summary>
        /// Retrieve one user via his email address.
        /// Visibitily depends on authenticated user role
        /// Used for validation purposes in order to have unique email addresses.
        /// </summary>
        /// <param name="emailAddress">Email address to be validated</param>
        /// <returns>bool</returns>
        [HttpGet]
        [Authorize(Policy = Constants.Roles.Customer)]
        [Route("{emailAddress}")]
        public bool EmailExists([FromRoute] string EmailAddress)
        {
            //List<User> users = new List<User>();

            //// Email address has to be unique globally
            //users = UnitOfWork.Users.GetAll(UserIncludes.Systemhouse, UserIncludes.Customer, UserIncludes.Roles).ToList();
            //bool emailExists = users.Any(x => x.Email == EmailAddress);
            return isEmailUnique(EmailAddress);
        }

        [HttpPost]
        [Route("bugReport")]
        public IActionResult ReportBug([FromBody] BugReportViewModel data)
        {
            User user = UnitOfWork.Users.GetOrNull(data.UserId, "Systemhouse", "Customer");
            if (user == null)
            {
                return BadRequest("ERROR: The user does not exist");
            }

            SmtpClient mailClient = new SmtpClient(_sendMailCreds.Host, _sendMailCreds.Port);
            NetworkCredential creds = new NetworkCredential(_sendMailCreds.Email, _sendMailCreds.Password);
            mailClient.Credentials = creds;
            MailAddress from = new MailAddress(_sendMailCreds.Email, _sendMailCreds.DisplayName);
            MailAddress to = new MailAddress(_agentEmailOptions.Email);
            MailMessage message = new MailMessage(from, to);
            message.Subject = "BUG REPORT: " + data.Subject;
            message.BodyEncoding = System.Text.Encoding.UTF8;
            message.SubjectEncoding = System.Text.Encoding.UTF8;
            string additionalInfo = String.Empty;
            if (user.Systemhouse != null)
            {
                additionalInfo += "Systemhouse: " + user.Systemhouse.Name;
            }
            if (user.Customer != null)
            {
                additionalInfo += " / Customer: " + user.Customer.Name;
            }
            if (additionalInfo == String.Empty)
            {
                additionalInfo = "No systemhouse / No customer";
            }
            message.Body = new StringBuilder("A new bug report from " + user.UserName + " (" + additionalInfo + "):<br /><br />" + data.Message).ToString();
            message.IsBodyHtml = true;
            mailClient.EnableSsl = true;
            mailClient.Send(message);
            message.Dispose();
            return Ok();
        }

        private bool isEmailUnique(string EmailAddress)
        {
            List<User> users = new List<User>();

            // Email address has to be unique globally
            //users = UnitOfWork.Users.GetAll(UserIncludes.Systemhouse, UserIncludes.Customer, UserIncludes.Roles).ToList();
            //bool emailExists = users.Any(x => x.Email == EmailAddress);
            User user = UnitOfWork.Users.GetByEmailOrNull(EmailAddress);
            bool emailExists = user != null;
            // negate to represent if an email is unique
            return !emailExists;
        }
    }
}
