using WPM_API.Data.DataContext.Entities;
using WPM_API.Data.Infrastructure;

namespace  WPM_API.Data.DataRepository
{
    public class OrderRepository : RepositoryEntityBase<Order>
    {
        public OrderRepository(DataContextProvider context) : base(context)
        {
        }
    }
}
