using WPM_API.Data.Infrastructure;
using WPM_API.Data.DataContext.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace  WPM_API.Data.DataRepository
{
    public class VirtualMachinesRepository : RepositoryEntityDeletableBase<VirtualMachine>
    {
        public VirtualMachinesRepository(DataContextProvider context) : base(context)
        {
        }
    }
}
