using WPM_API.Data.DataContext.Entities;
using WPM_API.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using File = WPM_API.Data.DataContext.Entities.File;

namespace  WPM_API.Data.DataRepository
{
    public class FileRepository : RepositoryEntityBase<File>
    {
        public FileRepository(DataContextProvider context) : base(context)
        {
        }

        public File GetByGuid(string guid)
        {
            return Context.Set<File>().FirstOrDefault(m => m.Guid.Equals(guid));
        }
    }
}
