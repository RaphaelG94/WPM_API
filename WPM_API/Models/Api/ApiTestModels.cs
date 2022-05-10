using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WPM_API.Data.Infrastructure;
using WPM_API.Code.Infrastructure;
using WPM_API.Code.Infrastructure.LogOn;
using Microsoft.AspNetCore.Http;

namespace WPM_API.Models.Api
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class TestValidationArgs:ValidatableModelBase
    {
        [NotDefaultValueRequired]
        public int UserId { get; set; }

        protected override IEnumerable<ValidationResult> Validate(UnitOfWork unitOfWork, ILoggedUserAccessor loggedUser, ValidationContext validationContext)
        {
            if (UserId < 0)
            {
                yield return new ValidationResult($"UserId cannot be lower 0. UserId - {UserId}", new[] { nameof(UserId) });
            }
        }
    }

    public class TestPostArgs
    {
        public int UserId { get; set; }
        public TestUserType UserType { get; set; }
        public int? CompanyId { get; set; }
        public List<UserModel> ExternalUsers { get; set; }
    }

    public class TestPostFileArgs
    {
        public int UserId { get; set; }
        public int? CompanyId { get; set; }
        public IFormFile CompanyFile { get; set; }
    }

    public enum TestUserType
    {
        Test1Type = 1,
        Test2Type = 2,
        Test3Type = 3
    }
}
