using WPM_API.Data.DataContext.Entities;
using WPM_API.Data.Infrastructure;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace  WPM_API.Data.DataRepository
{
    public class ClientParameterRepository : RepositoryEntityBase<ClientParameter>
    {
        public ClientParameterRepository(DataContextProvider context) : base(context)
        {
        }
        public ClientParameter getByClientId(string clientId,string parameterName)
        {
            return Context.Set<ClientParameter>().FirstOrDefault(x => x.ClientId.Equals(clientId) && x.ParameterName.ToLower().Equals(parameterName.ToLower()));
        }
    }
}
