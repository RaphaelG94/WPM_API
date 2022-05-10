using WPM_API.Data.DataContext.Entities;
using WPM_API.Data.Extensions;
using WPM_API.Data.Infrastructure;
using System.Collections.Generic;
using System.Linq;

namespace  WPM_API.Data.DataRepository
{
    public class ScriptRepository : RepositoryEntityDeletableBase<Script>
    {
        /// <summary>
        /// Konstruktor
        /// </summary>
        public ScriptRepository(DataContextProvider context) : base(context)
        {
        }

        public List<ScriptVersion> GetVersions(string scriptId)
        {
            return EntitySet.FirstOrDefault(x => x.Id == scriptId)?.Versions;
        }
    }
}