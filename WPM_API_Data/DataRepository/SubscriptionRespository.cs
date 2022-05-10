using WPM_API.Data.DataContext.Entities;
using WPM_API.Data.Infrastructure;

namespace  WPM_API.Data.DataRepository
{
    public class SubscriptionRepository : RepositoryEntityDeletableBase<Subscription>
    {
        /// <summary>
        /// Konstruktor
        /// </summary>
        public SubscriptionRepository(DataContextProvider context) : base(context)
        {
        }
    }
}