using WPM_API.Models;

namespace WPM_API.Models
{
    public class VirtualMachineViewModel
    {
        public string AzureId { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string Type { get; set; }
        public string OperatingSystem { get; set; }
        public string AdminUserName { get; set; }
        public DiskViewModel SystemDisk { get; set; }
        public DiskViewModel AdditionalDisk { get; set; }
        public string LocalIp { get; set; }
        public string BaseName { get; set; }
        public string Status { get; set; }
        public string SubscriptionName { get; set; }
        public string SubscriptionId { get; set; }
    }
    public class VirtualMachineAddViewModel
    {
        public VirtualMachineAddViewModel()
        {
            Admin = new AdminCredentials();
        }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Location { get; set; }
        public string Subnet{ get; set; }
//        public string SubnetId { get; set; }
        public string OperatingSystem { get; set; }
        public AdminCredentials Admin { get; set; }
        public DiskViewModel SystemDisk { get; set; }
        public DiskViewModel AdditionalDisk { get; set; }
        public string BaseId { get; set; }
        public string CustomerId { get; set; }
        public string CurrentCustomerId { get; set; }
    }

    public class DiskViewModel
    {
        public DiskViewModel() { }
        public DiskViewModel(string name, int? size)
        {
            Name = name;
            SizeInGb = size;
        }
        public string Name { get; set; }
        public int? SizeInGb { get; set; }
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