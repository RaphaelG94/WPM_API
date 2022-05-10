using WPM_API.Data.DataContext.Entities;
using WPM_API.Data.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace  WPM_API.Data.DataRepository
{
    
    public class ClientClientPropertyRepository : RepositoryEntityDeletableBase<ClientClientProperty>
    {
        public ClientClientPropertyRepository(DataContextProvider context) : base(context)
        {

        }
        /*
        public ClientClientProperty GetByName(string PropertyName, string Category, string ClientId)
        {
            return Context.Set<ClientClientProperty>().FirstOrDefault(m => m.Property.Name.Equals(PropertyName) && m.Property.Category.Name.Equals(Category) && m.ClientId.Equals(ClientId));
        }
        public List<ClientClientProperty> GetAllUnexecutedCommands(string ClientId)
        {
            return Context.Set<ClientClientProperty>().Include("Property").Where(m => m.ClientId.Equals(ClientId) && m.Property.Command != null && m.Value == null).ToList();

        }
        */
    }
}
