using System;
using AutoMapper;
using WPM_API.Data.Infrastructure;
using WPM_API.Code.Infrastructure.LogOn;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace WPM_API.Code.Infrastructure
{
    public abstract class ViewComponentBase: ViewComponent
    {
        protected UnitOfWork UnitOfWork => RequestServices.GetRequiredService<IUnitOfWorkFactory>().UnitOfWork;
        protected ILoggedUserAccessor LoggedUser => RequestServices.GetRequiredService<ILoggedUserAccessor>();
        protected IMapper Mapper => RequestServices.GetRequiredService<IMapper>();
        

        private IServiceProvider RequestServices => ViewComponentContext.ViewContext.HttpContext.RequestServices;
    }
}
