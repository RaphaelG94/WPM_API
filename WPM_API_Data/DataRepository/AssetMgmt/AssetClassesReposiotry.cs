using WPM_API.Data.DataContext;
using WPM_API.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace  WPM_API.Data.DataRepository
{
    public class AssetClassesReposiotry : RepositoryEntityBase<AssetClass>
    {
        public AssetClassesReposiotry(DataContextProvider context) : base(context)
        {
        }
    }
}
