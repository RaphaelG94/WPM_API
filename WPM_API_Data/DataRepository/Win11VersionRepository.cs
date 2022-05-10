using WPM_API.Data.DataContext.Entities;
using WPM_API.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace  WPM_API.Data.DataRepository
{
    public class Win11VersionRepository : RepositoryEntityDeletableBase<Win11Version>
    {
        public Win11VersionRepository(DataContextProvider context) : base(context)
        {
        }
    }
}
