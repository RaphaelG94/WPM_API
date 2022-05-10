using WPM_API.Azure.Models;
using Microsoft.Azure.Management.Network;
using Microsoft.Azure.Management.Network.Models;
using Microsoft.Azure.Management.ResourceManager.Fluent.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace WPM_API.Azure.Core
{
    public class VPNService
    {

        private AzureCredentials _credentials;

        public VPNService(AzureCredentials _credentials)
        {
            this._credentials = _credentials;
        }

        public async Task<VPN> GetVPNAsync(string subscriptionId, string resourceGroupName, string id)
        {
            using (var networkClient = new NetworkManagementClient(_credentials))
            {
                networkClient.SubscriptionId = subscriptionId;
                var virtualNetworks = networkClient.VirtualNetworks.ListAsync(resourceGroupName);
                var publicIps = networkClient.PublicIPAddresses.ListAsync(resourceGroupName);
                var connection = networkClient.VirtualNetworkGatewayConnections.List(resourceGroupName).Where(x => x.Id == id).First();
                VPN vpn = new VPN();
                if (connection != null)
                {
                    var virtualGateway = networkClient.VirtualNetworkGateways.List(resourceGroupName).Where(x => x.Id == connection.VirtualNetworkGateway1.Id).First();
                    var localGateway = networkClient.LocalNetworkGateways.List(resourceGroupName).Where(x => x.Id == connection.LocalNetworkGateway2.Id).First();


                    string subnetid = virtualGateway.IpConfigurations.First().Subnet.Id;
                    var list = (await virtualNetworks).ToList();
                    Subnet subnet = list.SelectMany(x => x.Subnets).First(x => x.Id == subnetid);
                    VirtualNetwork vn = list.First(x => x.Subnets.Contains(subnet));

                    vpn.Name = virtualGateway.Name;
                    vpn.Id = virtualGateway.Id;
                    vpn.ResourceGroup = resourceGroupName;

                    vpn.Virtual.VirtualNetwork = vn.Name;
                    vpn.Virtual.PublicIp = (await publicIps).ToList().First(x => x.Id == virtualGateway.IpConfigurations.First().PublicIPAddress.Id).IpAddress;

                    vpn.Local.PublicIp = localGateway.GatewayIpAddress;
                    vpn.Local.AddressRange = localGateway.LocalNetworkAddressSpace.AddressPrefixes[0];
                }
                return vpn;
            }
        }

        public async Task<List<VPN>> GetVPNsAsync(string subscriptionId, string resourceGroupName)
        {
            using (var networkClient = new NetworkManagementClient(_credentials))
            {
                networkClient.SubscriptionId = subscriptionId;
                List<VPN> result = new List<VPN>();
                var virtualNetworks = networkClient.VirtualNetworks.ListAsync(resourceGroupName);
                var publicIps = networkClient.PublicIPAddresses.ListAsync(resourceGroupName);
                var connections = networkClient.VirtualNetworkGatewayConnections.List(resourceGroupName);

                foreach (VirtualNetworkGatewayConnection connection in connections)
                {
                    var virtualGateway = networkClient.VirtualNetworkGateways.List(resourceGroupName).Where(x => x.Id == connection.VirtualNetworkGateway1.Id).First();
                    var localGateway = networkClient.LocalNetworkGateways.List(resourceGroupName).Where(x => x.Id == connection.LocalNetworkGateway2.Id).First();

                    VPN vpn = new VPN();
                    string subnetid = virtualGateway.IpConfigurations.First().Subnet.Id;
                    var list = (await virtualNetworks).ToList();
                    Subnet subnet = list.SelectMany(x => x.Subnets).First(x => x.Id == subnetid);
                    VirtualNetwork vn = list.First(x => x.Subnets.Contains(subnet));

                    vpn.Name = virtualGateway.Name;
                    vpn.Id = virtualGateway.Id;
                    vpn.ResourceGroup = resourceGroupName;

                    vpn.Virtual.VirtualNetwork = vn.Name;
                    vpn.Virtual.PublicIp = (await publicIps).ToList().First(x => x.Id == virtualGateway.IpConfigurations.First().PublicIPAddress.Id).IpAddress;

                    vpn.Local.PublicIp = localGateway.GatewayIpAddress;
                    vpn.Local.AddressRange = localGateway.LocalNetworkAddressSpace.AddressPrefixes[0];

                    result.Add(vpn);
                }
                return result;
            }
        }

        public async Task<VPN> AddVPN(string subscriptionId, string resourceGroupName, VpnAdd vpn)
        {
            using (var networkClient = new NetworkManagementClient(_credentials))
            {
                networkClient.SubscriptionId = subscriptionId;
                var vn = networkClient.VirtualNetworks.List(resourceGroupName).First(x => x.Id == vpn.VirtualNetworkId);
                string location = vn.Location;

                LocalNetworkGateway localGW = new LocalNetworkGateway();
                localGW.LocalNetworkAddressSpace = new AddressSpace(new List<string> { vpn.Local.AddressRange });
                localGW.GatewayIpAddress = vpn.Local.PublicIp;
                localGW.Location = location;
                var localGateway = networkClient.LocalNetworkGateways.BeginCreateOrUpdateAsync(resourceGroupName, "localGW", localGW);

                PublicIPAddress gwpip = new PublicIPAddress();
                gwpip.PublicIPAllocationMethod = "Dynamic";
                gwpip.Location = location;
                var publicIp = networkClient.PublicIPAddresses.CreateOrUpdateAsync(resourceGroupName, "gwpip", gwpip);

                VirtualNetworkGatewayIPConfiguration vnetipconfig = new VirtualNetworkGatewayIPConfiguration();
                // Get gatewaysubnet
                vnetipconfig.Name = "vnetgwipconfig";
                vnetipconfig.Subnet = new SubResource(vn.Subnets.First(x => x.Name == "gatewaysubnet").Id);
                vnetipconfig.PublicIPAddress = new SubResource((await publicIp).Id);
                vnetipconfig.PrivateIPAllocationMethod = "Dynamic";

                VirtualNetworkGateway vnetgw = new VirtualNetworkGateway();
                vnetgw.IpConfigurations = new List<VirtualNetworkGatewayIPConfiguration> { vnetipconfig };
                vnetgw.Location = location;
                vnetgw.GatewayType = "Vpn";
                vnetgw.VpnType = "RouteBased";
                vnetgw.EnableBgp = false;
                var VNGateway = networkClient.VirtualNetworkGateways.CreateOrUpdate(resourceGroupName, vpn.Name, vnetgw);

                VirtualNetworkGatewayConnection connection = new VirtualNetworkGatewayConnection();
                connection.VirtualNetworkGateway1 = VNGateway;
                connection.LocalNetworkGateway2 = await localGateway;
                connection.ConnectionType = "IPsec";
                connection.SharedKey = GenerateSharedKey(32);
                connection.Location = location;

                var VNGConnection = networkClient.VirtualNetworkGatewayConnections.CreateOrUpdate(resourceGroupName, "vnetgw-localgw", connection);

                VPN result = new VPN();
                result.Id = VNGateway.Id;
                result.Local.AddressRange = connection.LocalNetworkGateway2.LocalNetworkAddressSpace.AddressPrefixes[0];
                result.Local.PublicIp = connection.LocalNetworkGateway2.GatewayIpAddress;
                result.Name = VNGateway.Name;
                result.ResourceGroup = resourceGroupName;
                result.Virtual.VirtualNetwork = vn.AddressSpace.AddressPrefixes.First();
                result.Virtual.PublicIp = networkClient.PublicIPAddresses.Get(resourceGroupName, (await publicIp).Name).IpAddress;
                result.SharedKey = connection.SharedKey;

                return result;
            }
        }

        public async Task<VPN> EditVPN(string subscriptionId, string resourceGroupName, VpnEdit vpn)
        {
            using (var networkClient = new NetworkManagementClient(_credentials))
            {
                networkClient.SubscriptionId = subscriptionId;
                var virtualNetworks = networkClient.VirtualNetworks.ListAsync(resourceGroupName);
                var virtualNetworkGateway = networkClient.VirtualNetworkGateways.List(resourceGroupName).Where(x => x.Name == vpn.Name).FirstOrDefault();

                var list = (await virtualNetworks).ToList();
                Subnet subnet = list.SelectMany(x => x.Subnets).First(x => x.Id == virtualNetworkGateway.IpConfigurations[0].Subnet.Id);
                VirtualNetwork vn = list.First(x => x.Subnets.Contains(subnet));

                var publicIpName = virtualNetworkGateway.IpConfigurations[0].Name;
                var connections = networkClient.VirtualNetworkGatewayConnections.List(resourceGroupName);
                var connection = connections.Where(x => x.VirtualNetworkGateway1.Id == virtualNetworkGateway.Id).FirstOrDefault();
                var localNetworkGateway = networkClient.LocalNetworkGateways.List(resourceGroupName).Where(x => x.Id == connection.LocalNetworkGateway2.Id).FirstOrDefault();
                var publicIps = networkClient.PublicIPAddresses.ListAsync(resourceGroupName);

                LocalNetworkGateway localGW = networkClient.LocalNetworkGateways.Get(resourceGroupName, localNetworkGateway.Name);
                localGW.LocalNetworkAddressSpace = new AddressSpace(new List<string> { vpn.Local.AddressRange });
                localGW.GatewayIpAddress = vpn.Local.PublicIp;
                var localGateway = networkClient.LocalNetworkGateways.BeginCreateOrUpdateAsync(resourceGroupName, "localGW", localGW);

                VPN result = new VPN();
                result.Id = virtualNetworkGateway.Id;
                result.Name = virtualNetworkGateway.Name;
                result.ResourceGroup = resourceGroupName;
                result.Virtual.VirtualNetwork = vn.Name;
                result.Virtual.PublicIp = (await publicIps).ToList().First(x => x.Id == virtualNetworkGateway.IpConfigurations.First().PublicIPAddress.Id).IpAddress;
                result.Local.AddressRange = (await localGateway).LocalNetworkAddressSpace.AddressPrefixes[0];
                result.Local.PublicIp = (await localGateway).GatewayIpAddress;

                return result;
            }
        }

        public void DeleteVPN(string subscriptionId, string resourceGroupName, string vpnId)
        {
            using (var networkClient = new NetworkManagementClient(_credentials))
            {
                networkClient.SubscriptionId = subscriptionId;

                var virtualNetworkGateway = networkClient.VirtualNetworkGateways.List(resourceGroupName).Where(x => x.Id == vpnId).FirstOrDefault();
                var publicIpName = virtualNetworkGateway.IpConfigurations[0].Name;
                var connection = networkClient.VirtualNetworkGatewayConnections.List(resourceGroupName).Where(x => x.VirtualNetworkGateway1.Id == vpnId).FirstOrDefault();
                var localNetworkGateway = networkClient.LocalNetworkGateways.List(resourceGroupName).Where(x => x.Id == connection.LocalNetworkGateway2.Id).FirstOrDefault();

                if (connection != null)
                {
                    networkClient.VirtualNetworkGatewayConnections.Delete(resourceGroupName, connection.Name);
                }
                if (localNetworkGateway != null)
                {
                    networkClient.LocalNetworkGateways.BeginDeleteAsync(resourceGroupName, localNetworkGateway.Name);
                }
                if (virtualNetworkGateway != null)
                {
                    networkClient.VirtualNetworkGateways.Delete(resourceGroupName, virtualNetworkGateway.Name);
                }
                if (publicIpName != null)
                {
                    networkClient.PublicIPAddresses.BeginDeleteAsync(resourceGroupName, publicIpName);
                }
            }
        }

        /// <summary>
        /// Generates a random string having the given length 
        /// with the provided amount of non alphanumeric characters
        /// </summary>
        /// <param name="length"></param>
        /// <param name="numberOfNonAlphanumericCharacters"></param>
        /// <returns>The key string.</returns>
        private static string GenerateSharedKey(int length)
        {
            Random rand = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[rand.Next(s.Length)]).ToArray());
    }
    }
}
