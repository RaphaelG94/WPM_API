using WPM_API.Data.DataContext.Entities;
using WPM_API.Data.Infrastructure;

namespace  WPM_API.Data.DataRepository
{
    public class BaseRepository : RepositoryEntityDeletableBase<Base>
    {
        /// <summary>
        /// Konstruktor
        /// </summary>
        public BaseRepository(DataContextProvider context) : base(context)
        {
        }
        
    }
}