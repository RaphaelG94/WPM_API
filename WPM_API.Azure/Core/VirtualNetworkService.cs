using System.Collections.Generic;
using Microsoft.Azure.Management.Network;
using Microsoft.Azure.Management.Network.Models;
using System.Linq;
using System.Threading.Tasks;
using AZURE = Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Core;
using Microsoft.Azure.Management.ResourceManager.Fluent.Authentication;
using WPM_API.Azure.Models;
using System;

namespace WPM_API.Azure.Core
{
    public class VirtualNetworkService
    {
        private AzureCredentials _credentials;

        public VirtualNetworkService(AzureCredentials _credentials)
        {
            this._credentials = _credentials;
        }

        public async Task<List<VirtualNetwork>> GetVirtualNetworksAsync(string subscriptionId, string ressourceGroupName)
        {
            using (var networkClient = new NetworkManagementClient(_credentials))
            {
                networkClient.SubscriptionId = subscriptionId;

                var virtualNetworks = (await networkClient.VirtualNetworks.ListAsync(ressourceGroupName)).ToList();
                return virtualNetworks;
            }
        }

        public Task<Microsoft.Azure.Management.Network.Fluent.INetwork> AddOrModifyVirtualNetworkAsync(string subscriptionId, string resourceGroupName, VirtualNetworkAddOrEditViewModel virtualNetworkModel, string Location)
        {
            var azure = AZURE.Azure.Configure()
                .WithLogLevel(HttpLoggingDelegatingHandler.Level.Basic)
                .Authenticate(_credentials)
                .WithSubscription(subscriptionId);

            var network = azure.Networks.Define(virtualNetworkModel.Name)
                .WithRegion(Location)
                .WithExistingResourceGroup(resourceGroupName)
                .WithAddressSpace(virtualNetworkModel.AddressRange);

            foreach (SubnetViewModel s in virtualNetworkModel.Subnets)
            {
                network.DefineSubnet(s.Name)
                        .WithAddressPrefix(s.AddressRange)
                        .Attach();
            }

            try
            {
                string subnetmask = virtualNetworkModel.Subnets[0].AddressRange.Split('/')[1].Replace("0", "");
                string[] ipParts = virtualNetworkModel.Subnets[0].AddressRange.Split('/')[0].Split('.');
                if (ipParts.Count() == 4 && (subnetmask == "8" || subnetmask == "16" || subnetmask == "24"))
                {
                    string dnsIp = ipParts[0] + "." + ipParts[1] + "." + ipParts[2] + ".4";
                    //return network.WithDnsServer(dnsIp).CreateAsync();
                    return network.CreateAsync();
                }
            }
            catch (Exception)
            {
                throw new Exception("Subnet addressrange has false format.");
            }
            return network.CreateAsync();
        }

        public void DeleteVirtualNetwork(string subscriptionId, string virtualNetworkId)
        {
            var azure = AZURE.Azure.Configure()
                .WithLogLevel(HttpLoggingDelegatingHandler.Level.Basic)
                .Authenticate(_credentials)
                .WithSubscription(subscriptionId);

            azure.Networks.DeleteByIdAsync(virtualNetworkId);
        }

        public Task SetDnsServer(string subscriptionId, string virtualNetworkId, string dnsIp)
        {
            var azure = AZURE.Azure.Configure()
                .WithLogLevel(HttpLoggingDelegatingHandler.Level.Basic)
                .Authenticate(_credentials)
                .WithSubscription(subscriptionId);

            return azure.Networks.GetById(virtualNetworkId).Update().WithDnsServer(dnsIp).ApplyAsync();
        }
    }
}
