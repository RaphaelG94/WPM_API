using WPM_API.Data.Infrastructure;
using WPM_API.Code.Infrastructure.LogOn;

namespace WPM_API.Code.Infrastructure.Menu.Models
{
    public class MenuBuilderArgs
    {
        public MenuMvcArgs MenuMvcArgs { get; }
        public UnitOfWork UnitOfWork { get; }
        public ILoggedUserAccessor LoggedUser { get; }

        public MenuBuilderArgs(MenuMvcArgs menuMvcArgs, UnitOfWork unitOfWork, ILoggedUserAccessor loggedUser)
        {
            MenuMvcArgs = menuMvcArgs;
            UnitOfWork = unitOfWork;
            LoggedUser = loggedUser;
        }
    }
}
