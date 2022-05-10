using WPM_API.Data.DataContext.Entities;
using WPM_API.Data.Extensions;
using WPM_API.Data.Infrastructure;
using System.Collections.Generic;
using System.Linq;

namespace  WPM_API.Data.DataRepository
{
    /// <summary>
    /// Utility repository for retrieving attached files of a scriptversion.
    /// Not used for retrieving ScriptVersions in general.
    /// </summary>
    public class ScriptVersionRepository : RepositoryEntityBase<ScriptVersion>
    {
        public ScriptVersionRepository(DataContextProvider context) : base(context)
        {
        }
    }
}