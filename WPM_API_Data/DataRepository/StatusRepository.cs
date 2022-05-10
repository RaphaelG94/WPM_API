using WPM_API.Data.DataContext.Entities;
using WPM_API.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace  WPM_API.Data.DataRepository
{
    public class StatusRepository : RepositoryEntityBase<Status>
    {
        public StatusRepository(DataContextProvider context) : base(context)
        {
        }
    }
}
