using WPM_API.Data.DataContext.Entities;
using WPM_API.Data.Extensions;
using WPM_API.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace  WPM_API.Data.DataRepository
{
    public class ClientRepository : RepositoryEntityDeletableBase<Client>
    {
        public ClientRepository(DataContextProvider context) : base(context)
        {
        }

        public Client GetByUuid(string clientUuid, params string[] includes)
        {
            return EntitySetNotDeleted.IncludeMultiple(
                    "Customer",
                    "Customer.Banner",
                    "Customer.IconRight",
                    "Customer.IconLeft",
                    "Tasks",
                    "Tasks.Task",
                    "Tasks.Task.ExecutionFile",
                    "AssignedSoftware",
                    "AssignedSoftware.CustomerSoftware",
                    "AssignedSoftware.CustomerSoftware.RuleApplicability.Type",
                    "AssignedSoftware.CustomerSoftware.RuleApplicability.Data",
                    "AssignedSoftware.CustomerSoftware.RuleDetection.Type",
                    "AssignedSoftware.CustomerSoftware.RuleDetection.Data",
                    "AssignedSoftware.CustomerSoftware.TaskInstall.Files",
                    "AssignedSoftware.CustomerSoftware.TaskUninstall.Files",
                    "AssignedSoftware.CustomerSoftware.TaskUpdate.Files")
                .IncludeMultiple(includes)
                .FirstOrDefault(m => m.UUID.Equals(clientUuid));
        }

        public ClientTask GetClientTask(string clientTaskId)
        {
            return Context.Set<ClientTask>().FirstOrDefault(m => m.Id.Equals(clientTaskId));
        }

        public void AddTaskById(string taskId, string clientId, string type, string userId = null)
        {
            //List<Status> ls = new List<Status>();
            //Status s = new Status() { Name = , Message = DateTime.Now.ToString(), CreatedDate = DateTime.Now, CreatedByUserId = userId };
            // ls.Add(s);
            ClientTask newClientTask = new ClientTask()
            {
                TaskId = taskId,
                ClientId = clientId,
                Status = "assigned",
                Type = type
            };
            Context.Set<ClientTask>().Add(newClientTask);
        }
    }
}
