using System.Collections.Generic;
using WPM_API.Common;

namespace WPM_API.Code.Infrastructure.Menu.Models
{
    public class MenuTree
    {
        public List<MenuItem> Items { get; set; }

        public MenuTree()
        {
            Items = new List<MenuItem>();
        }
    }

    public class MenuBranch
    {
        public IEnumerable<MenuItem> menuItems { get; set; }
        public int level { get; set; }
        public Enums.MenuItemTypes? currentMenuItem { get; set; }
    }
}