using WPM_API.Data.DataContext.Entities;
using WPM_API.Data.Infrastructure;
using System.Linq;

namespace  WPM_API.Data.DataRepository
{
    public class CategoryRepository : RepositoryEntityBase<Category>
    {
        public CategoryRepository(DataContextProvider context) : base(context)
        {
        }
        public Category GetByName(string name, CategoryType type)
        {
            return Context.Set<Category>().FirstOrDefault(m => m.Name.ToLower().Equals(name) && m.Type.Equals(type));
        }
    }
}
