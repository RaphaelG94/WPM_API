using System.Collections.Generic;
using System.Linq;
using WPM_API.Common;
using WPM_API.Data.DataContext.Entities;
using WPM_API.Data.Infrastructure;

namespace  WPM_API.Data.DataRepository.Users
{
    public class RoleRepository : RepositoryEntityBase<Role>
    {
        public RoleRepository(DataContextProvider context) : base(context)
        {
        }

        public List<Role> GetAllRoles()
        {
            return Context.Set<Role>().ToList();
        }

        public Role GetCustomerRole()
        {
            return Context.Set<Role>().Single(m => m.Name == Constants.Roles.Customer);
        }

        public Role GetSystemhouseRole()
        {
            return Context.Set<Role>().Single(m => m.Name == Constants.Roles.Systemhouse);
        }

        public Role GetAdminRole()
        {
            return Context.Set<Role>().Single(m => m.Name == Constants.Roles.Admin);
        }
    }
}
