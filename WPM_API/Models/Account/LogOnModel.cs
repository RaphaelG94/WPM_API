using System.ComponentModel.DataAnnotations;
using WPM_API.Code.Infrastructure.LogOn;
using WPM_API.Common.Utils;
using WPM_API.Data.DataContext.Projections.Users;
using WPM_API.Data.Infrastructure;

namespace WPM_API.Models.Account
{
    public class LogOnModel : ValidatableModelBase
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }

        public AccountProjection AccountAfterValidation { get; private set; }

        protected override IEnumerable<ValidationResult> Validate(UnitOfWork unitOfWork, ILoggedUserAccessor loggedUser, ValidationContext validationContext)
        {
            AccountAfterValidation = unitOfWork.Users.GetAccountByLoginOrNull(UserName);
            return ValidateUser(AccountAfterValidation, Password);
        }

        public static IEnumerable<ValidationResult> ValidateUser(AccountProjection user, string userPassword)
        {
            if (user == null || !PasswordHash.ValidatePassword(userPassword, user.Password))
            {
                yield return new ValidationResult("The user name or password provided is incorrect.");
            }
            else
            {
                if (!user.Roles.Any())
                    yield return new ValidationResult("User has no one role.");
            }
        }
    }
}
