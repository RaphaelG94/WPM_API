using WPM_API.Data.DataContext.Entities.AssetMgmt;
using WPM_API.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace  WPM_API.Data.DataRepository
{
    public class AssetMgmtRepository : RepositoryEntityDeletableBase<AssetModel>
    {
        public AssetMgmtRepository(DataContextProvider context) : base(context)
        {
        }
    }
}
