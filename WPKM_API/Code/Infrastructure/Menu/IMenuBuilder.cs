using WPM_API.Code.Infrastructure.Menu.Models;

namespace WPM_API.Code.Infrastructure.Menu
{
    public interface IMenuBuilder
    {
        MenuTree Build(bool hideItemsWithoutPermission = true, bool hideEmptyItems = true);
    }
}
