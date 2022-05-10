using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace  WPM_API.Azure.Models
{
    public class VirtualMachineRefModel
    {
        public string SubscriptionId { get; set; }
        public string VmId { get; set; }
    }

    public class VirtualMachineModel
    {
        public string CustomerId { get; set; }
        public string SubscriptionId { get; set; }
        public string ResourceGroupName { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Location { get; set; }
        public string StorageAccountName { get; set; }
        public string OperatingSystem { get; set; }
        public VirtualMachineNetwork Network { get; set; }
        public AdminCredentials Admin { get; set; }
        public Disk SystemDisk { get; set; }
        public Disk AdditionalDisk { get; set; }
        public string LocalIp { get; set; }
        public string SubscriptionName { get; set; }
    }

    public class VirtualMachineAddModel
    {
        public string CustomerId { get; set; }
        public string SubscriptionId { get; set; }
        public string ResourceGroupName { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string StorageAccountId { get; set; }
        public string Location { get; set; }
        public string OperatingSystem { get; set; }
        public VirtualMachineNetworkAdd Network { get; set; }
        public AdminCredentials Admin { get; set; }
        public DiskAdd SystemDisk { get; set; }
        public DiskAdd AdditionalDisk { get; set; }
    }

    public class VirtualMachineEditModel
    {
        public string CustomerId { get; set; }
        public string SubscriptionId { get; set; }
        public string ResourceGroupName { get; set; }
        public string Id { get; set; }
        public string Type { get; set; }
        public DiskAdd AdditionalDisk { get; set; }
    }

    public class Disk
    {
        public Disk() { }
        public Disk(string name, int? size)
        {
            Name = name;
            SizeInGb = size;
        }
        public string Name { get; set; }
        public int? SizeInGb { get; set; }
    }

    public class DiskAdd
    {
        public DiskAdd() { }
        public DiskAdd(string name, int size)
        {
            Name = name;
            SizeInGb = size;
        }
        public string Name { get; set; }
        public int SizeInGb { get; set; }
    }

    public class VirtualMachineNetwork
    {
        public VirtualMachineNetwork() { }
        public VirtualMachineNetwork(string virtualNetworkName, string subnetName)
        {
            VirtualNetworkName = virtualNetworkName;
            SubnetName = subnetName;
        }
        public string VirtualNetworkName { get; set; }
        public string SubnetName { get; set; }
    }

    public class VirtualMachineNetworkAdd
    {
        public VirtualMachineNetworkAdd() { }
        public VirtualMachineNetworkAdd(string virtualNetworkId, string subnetName)
        {
            VirtualNetworkId = virtualNetworkId;
            SubnetName = subnetName;
        }
        public string VirtualNetworkId { get; set; }
        public string SubnetName { get; set; }
    }

    public class AdminCredentials
    {
        public AdminCredentials() { }
        public AdminCredentials(string username, string password)
        {
            Username = username;
            Password = password;
        }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
