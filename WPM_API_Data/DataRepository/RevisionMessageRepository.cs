using WPM_API.Data.DataContext.Entities.SmartDeploy;
using WPM_API.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace  WPM_API.Data.DataRepository
{
    public class RevisionMessageRepository : RepositoryEntityDeletableBase<RevisionMessage>
    {
        public RevisionMessageRepository(DataContextProvider context) : base(context)
        {
        }
    }
}
