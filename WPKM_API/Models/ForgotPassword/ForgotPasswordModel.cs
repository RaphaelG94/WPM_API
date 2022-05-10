using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WPM_API.Data.Infrastructure;
using WPM_API.Code.Infrastructure.LogOn;

namespace WPM_API.Models.ForgotPassword
{
    public class ForgotPasswordModel : ValidatableModelBase
    {
        [EmailAddress, Required]
        public string Email { get; set; }

        protected override IEnumerable<ValidationResult> Validate(UnitOfWork unitOfWork, ILoggedUserAccessor loggedUser, ValidationContext validationContext)
        {
            var user = unitOfWork.Users.GetByEmailOrNull(Email);
            if (user == null)
            {
                yield return new ValidationResult("The email address could not be found.", new[] { nameof(Email) });
            }
        }
    }
}
