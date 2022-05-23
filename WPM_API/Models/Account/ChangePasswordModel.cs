using System.ComponentModel.DataAnnotations;
using WPM_API.Code.Infrastructure.LogOn;
using WPM_API.Common;
using WPM_API.Common.Extensions;
using WPM_API.Common.Utils;
using WPM_API.Data.Infrastructure;

namespace WPM_API.Models.Account
{
    public class ChangePasswordModel : ValidatableModelBase
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = Constants.MinPasswordLength)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        protected override IEnumerable<ValidationResult> Validate(UnitOfWork unitOfWork, ILoggedUserAccessor loggedUser, ValidationContext validationContext)
        {
            var user = unitOfWork.Users.GetWithRolesOrNull(loggedUser.Id);
            if (!PasswordHash.ValidatePassword(OldPassword, user.Password))
            {
                yield return new ValidationResult("The current password is incorrect", new[] { this.GetPropertyName(m => m.OldPassword) });
            }
        }
    }
}
