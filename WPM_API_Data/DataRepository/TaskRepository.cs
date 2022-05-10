using WPM_API.Data.DataContext.Entities;
using WPM_API.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using Task = WPM_API.Data.DataContext.Entities.Task;

namespace  WPM_API.Data.DataRepository
{
    public class TaskRepository : RepositoryEntityDeletableBase<Task>
    {
        public TaskRepository(DataContextProvider context) : base(context)
        {
        }
    }
}
