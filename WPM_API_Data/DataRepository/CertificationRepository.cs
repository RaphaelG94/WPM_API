using System;
using System.Collections.Generic;
using System.Text;
using WPM_API.Data.DataContext.Entities;
using WPM_API.Data.Infrastructure;

namespace  WPM_API.Data.DataRepository
{
    public class CertificationRepository : RepositoryEntityBase<Certification>
    {
        public CertificationRepository(DataContextProvider context) : base(context)
        {
        }
    }
}
