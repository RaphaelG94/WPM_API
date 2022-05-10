using WPM_API.Data.DataContext.Entities;
using WPM_API.Data.Infrastructure;
using System.Linq;

namespace  WPM_API.Data.DataRepository
{
    public class ClientPropertyRepository : RepositoryEntityBase<ClientProperty>
    {
        public ClientPropertyRepository(DataContextProvider context) : base(context)
        {

        }

        public ClientProperty GetByName(string Name, string category)
        {
            return Context.Set<ClientProperty>().FirstOrDefault(m => m.PropertyName.Equals(Name) && m.Category.Name.Equals(category));
        }
    }
}
