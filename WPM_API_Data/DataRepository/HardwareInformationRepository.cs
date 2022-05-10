using WPM_API.Data.DataContext.Entities;
using WPM_API.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace  WPM_API.Data.DataRepository
{
    public class InventoryRepository : RepositoryEntityDeletableBase<Inventory>
    {
        public InventoryRepository(DataContextProvider context) : base(context)
        {
        }
    }
}
