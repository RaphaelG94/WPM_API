using AutoMapper;
using WPM_API.Data.Infrastructure;
using WPM_API.Code.Infrastructure.LogOn;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace WPM_API.Code.Infrastructure.BaseControllers
{
    public abstract class ControllerSiteBase: Controller
    {
        protected UnitOfWork UnitOfWork => HttpContext.RequestServices.GetRequiredService<IUnitOfWorkFactory>().UnitOfWork;
        protected ILoggedUserAccessor LoggedUser => HttpContext.RequestServices.GetRequiredService<ILoggedUserAccessor>();
        protected IMapper Mapper => HttpContext.RequestServices.GetRequiredService<IMapper>();
    }
}
