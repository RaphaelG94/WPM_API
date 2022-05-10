using WPM_API.Data.DataContext.Entities;
using WPM_API.Data.DataRepository.Users;
using WPM_API.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace  WPM_API.Data.DataRepository
{
    public class SystemhouseRepository : RepositoryEntityDeletableBase<Systemhouse>
    {
        /// <summary>
        /// Konstruktor
        /// </summary>
        public SystemhouseRepository(DataContextProvider context) : base(context)
        {
        }

        public override void MarkForDelete(Systemhouse entity, string userId)
        {
            if (!entity.Deletable)
            {
                throw new InvalidOperationException();
            }
            List<Customer> customers = Context.GetRepository<CustomerRepository>().GetAll().Where(x => x.SystemhouseId == entity.Id).ToList();
            foreach (Customer c in customers)
            {
                Context.GetRepository<CustomerRepository>().MarkForDelete(c, userId);
            }
            List<User> users = Context.GetRepository<UserRepository>().GetAll().Where(x => x.SystemhouseId == entity.Id).ToList();
            foreach (User user in users)
            {
                Context.GetRepository<UserRepository>().MarkForDelete(user, userId);
            }
            base.MarkForDelete(entity, userId);
        }
    }
}