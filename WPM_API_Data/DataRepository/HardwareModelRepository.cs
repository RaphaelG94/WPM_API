using WPM_API.Data.DataContext;
using WPM_API.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace  WPM_API.Data.DataRepository
{
    public class HardwareModelRepository : RepositoryEntityDeletableBase<HardwareModel>
    {
        public HardwareModelRepository(DataContextProvider context) : base(context)
        {
        }
    }
}
