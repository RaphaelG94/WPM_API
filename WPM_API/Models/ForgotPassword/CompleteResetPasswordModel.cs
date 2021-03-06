using System.ComponentModel.DataAnnotations;
using WPM_API.Common;

namespace WPM_API.Models.ForgotPassword
{
    public class CompleteResetPasswordModel
    {
        public Guid RequestId { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = Constants.MinPasswordLength)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
