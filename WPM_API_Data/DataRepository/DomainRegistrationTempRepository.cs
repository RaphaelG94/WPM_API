using WPM_API.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using WPM_API.Data.DataContext.Entities;

namespace  WPM_API.Data.DataRepository
{
    public class DomainRegistrationTempRepository : RepositoryEntityBase<DomainRegistrationTemp>
    {
        public DomainRegistrationTempRepository(DataContextProvider context) : base(context)
        {
        }
    }
}
