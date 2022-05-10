using System;
using WPM_API.Data.DataContext;
using WPM_API.Data.Infrastructure;
using WPM_API.Code.Infrastructure.LogOn;
using Microsoft.Extensions.DependencyInjection;

namespace WPM_API.Code.Infrastructure
{
    public class AppDependencyResolver
    {
        private static AppDependencyResolver _resolver;

        public static AppDependencyResolver Current
        {
            get
            {
                if (_resolver == null)
                    throw new Exception("AppDependencyResolver not initialized. You should initialize it in Startup class");
                return _resolver;
            }
        }

        public static void Init(IServiceProvider services)
        {
            _resolver = new AppDependencyResolver(services);
        }

        private readonly IServiceProvider _serviceProvider;

        private AppDependencyResolver(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public UnitOfWork CreateUoWinCurrentThread()
        {
            var scopeResolver = _serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
            return UnitOfWork.CreateInScope(scopeResolver.ServiceProvider.GetRequiredService<DBData>(), scopeResolver);
        }

        public ILoggedUserAccessor GetLoggedUser()
        {
            return _serviceProvider.GetRequiredService<ILoggedUserAccessor>();
        }
    }

}
