using System.Collections.Generic;
using System.Linq;
using Microsoft.Azure.Management.ResourceManager;
using Microsoft.Rest;
using Microsoft.Azure.Management.ResourceManager.Models;
using System.Threading.Tasks;

namespace  WPM_API.Azure.Core
{
    public class SubscriptionService
    {
        private ServiceClientCredentials _credentials;

        public SubscriptionService(ServiceClientCredentials _credentials)
        {
            this._credentials = _credentials;
        }

        public List<Subscription> GetSubscriptions(List<string> subscriptionIds)
        {
            List<Subscription> result = new List<Subscription>();
            using (var subscriptionClient = new SubscriptionClient(_credentials))
            {
                IQueryable<Subscription> azureSubscriptions = subscriptionClient.Subscriptions.List().AsQueryable();
                result.AddRange(azureSubscriptions.Where(x => subscriptionIds.Contains(x.SubscriptionId)).ToList());
            }
            return result;
        }

        public async Task<List<Subscription>> GetSubscriptions()
        {
            List<Subscription> result = new List<Subscription>();
            using (var subscriptionClient = new SubscriptionClient(_credentials))
            {
                result = (await subscriptionClient.Subscriptions.ListAsync()).ToList();
            }
            return result;
        }

        public async Task<Subscription> GetSubscription(string subscriptionId)
        {
            using (var subscriptionClient = new SubscriptionClient(_credentials))
            {
                IQueryable<Subscription> azureSubscriptions = (await subscriptionClient.Subscriptions.ListAsync()).AsQueryable();
                return azureSubscriptions.Where(x => subscriptionId == x.SubscriptionId).FirstOrDefault();
            }
        }

        public async Task<List<Location>> GetLocations()
        {
            List<Location> result = new List<Location>();
            List<Subscription> subscriptions = await GetSubscriptions();
            using (var subscriptionClient = new SubscriptionClient(_credentials))
            {
                foreach(Subscription subscription in subscriptions)
                {
                    result.AddRange((await subscriptionClient.Subscriptions.ListLocationsAsync(subscription.SubscriptionId)).ToList());
                }
            }
            return result;
        }
    }
}
