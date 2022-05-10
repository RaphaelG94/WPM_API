using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WPM_API.Data.Infrastructure;
using WPM_API.Code.Infrastructure;
using WPM_API.Code.Infrastructure.LogOn;
using Microsoft.Extensions.DependencyInjection;

namespace WPM_API.Models
{
    public abstract class ValidatableModelBase: IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Validate(validationContext.GetService<IUnitOfWorkFactory>().UnitOfWork, AppDependencyResolver.Current.GetLoggedUser(), validationContext);
        }

        protected abstract IEnumerable<ValidationResult> Validate(UnitOfWork unitOfWork, ILoggedUserAccessor loggedUser, ValidationContext validationContext);
    }
}
