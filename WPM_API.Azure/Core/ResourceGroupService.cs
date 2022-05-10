using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Rest;
using Microsoft.Azure.Management.ResourceManager.Models;
using Microsoft.Azure.Management.ResourceManager;
using Microsoft.Rest.Azure;

namespace  WPM_API.Azure.Core
{
    public class ResourceGroupService
    {
        private ServiceClientCredentials _credentials;

        public ResourceGroupService(ServiceClientCredentials _credentials)
        {
            this._credentials = _credentials;
        }

        public List<ResourceGroup> GetRessourceGroups(string subscriptionId)
        {
            using (var ressourceClient = new ResourceManagementClient(_credentials))
            {
                ressourceClient.SubscriptionId = subscriptionId;
                var groups = ressourceClient.ResourceGroups.List();
                return groups.ToList();
            }
        }

        public bool GetRessourceGroupByName(string name, string subscriptionId)
        {
            using (var resourceClient = new ResourceManagementClient(_credentials))
            {
                resourceClient.SubscriptionId = subscriptionId;
                var resourceGrp = resourceClient.ResourceGroups.CheckExistence(name);
                return resourceGrp;
            }
        }

        public async Task<ResourceGroup> GetResourceGroup(string name, string subscriptionId)
        {
            using (var resourceClient = new ResourceManagementClient(_credentials))
            {
                resourceClient.SubscriptionId = subscriptionId;
                var resourceGrp = await resourceClient.ResourceGroups.GetAsync(name);
                return resourceGrp;
            }
        }

        public async Task<ResourceGroup> AddResourceGroup(string subscriptionId, ResourceGroup resourceGroup)
        {
            using (var ressourceClient = new ResourceManagementClient(_credentials))
            {
                ressourceClient.SubscriptionId = subscriptionId;

                var parameters = new ResourceGroup()
                {
                    Location = resourceGroup.Location,
                };

                return await ressourceClient.ResourceGroups.CreateOrUpdateAsync(resourceGroup.Name, parameters);
            }
        }

        public async Task<AzureOperationResponse> DeleteRessourceGroupAsync(string subscriptionId, string resourceGroupName)
        {
            using (var ressourceClient = new ResourceManagementClient(_credentials))
            {
                ressourceClient.SubscriptionId = subscriptionId;
                return await ressourceClient.ResourceGroups.DeleteWithHttpMessagesAsync(resourceGroupName);
            }
        }
    }
}
