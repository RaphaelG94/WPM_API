using WPM_API.Data.DataContext.Entities;
using WPM_API.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace  WPM_API.Data.DataRepository
{
    public class ImagesSystemhouseRepository : RepositoryEntityDeletableBase<ImagesSystemhouse>
    {
        public ImagesSystemhouseRepository(DataContextProvider context) : base(context)
        {
        }
    }
}
