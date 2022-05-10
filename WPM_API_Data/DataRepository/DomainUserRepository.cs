using WPM_API.Data.DataContext.Entities;
using WPM_API.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


namespace  WPM_API.Data.DataRepository
{
    public class DomainUserRepository : RepositoryEntityDeletableBase<DomainUser>
    {
        public DomainUserRepository(DataContextProvider context) : base(context)
        {
        }
    }
}
