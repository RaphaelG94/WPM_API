﻿using System;
using System.Collections.Generic;
using System.Text;
using WPM_API.Data.Infrastructure;
using WPM_API.Data.DataContext.Entities;

namespace  WPM_API.Data.DataRepository
{
    public class WorkflowRepository : RepositoryEntityBase<Workflow>
    {
        public WorkflowRepository(DataContextProvider context) : base(context)
        {
        }
    }
}
