using System;
using WPM_API.Data.DataContext.Entities;
using WPM_API.Data.Infrastructure;
using System.Linq;
using System.Collections.Generic;
using WPM_API.Data.DataRepository.Users;

namespace  WPM_API.Data.DataRepository
{
    public class CustomerRepository : RepositoryEntityDeletableBase<Customer>
    {
        public CustomerRepository(DataContextProvider context) : base(context)
        {
        }

        public override void MarkForDelete(Customer entity, string userId)
        {
            List<User> users = Context.GetRepository<UserRepository>().GetAll().Where(x => x.CustomerId == entity.Id).ToList();
            foreach (User user in users)
            {
                Context.GetRepository<UserRepository>().MarkForDelete(user, userId);
            }
            List<Client> clients = Context.GetRepository<ClientRepository>().GetAll().Where(x => x.CustomerId == entity.Id).ToList();
            foreach (Client client in clients)
            {
                Context.GetRepository<ClientRepository>().MarkForDelete(client, userId);
            }
            base.MarkForDelete(entity, userId);
        }

        /*
        public AzureCredentials GetAzureCredentials(string customerId)
        {
            return Context.Set<AzureCredentials>().FirstOrDefault(m => m.CustomerId == customerId);
        }
        */
    }
}