using WPM_API.Data.DataContext.Entities;
using WPM_API.Models.AssetMgmt;
using System;
using System.Collections.Generic;

namespace WPM_API.Models
{
    public class ClientRefViewModel
    {
        public string Id { get; set; }
    }
    public class ClientViewModel
    {
        public string Id { get; set; }
        public string Uuid { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string AssetId { get; set; }
        public string Bios { get; set; }
        public string Vendor { get; set; }
        public List<string> MacAddresses { get; set; }
        public string WdsIp { get; set; }
        public string Unattend { get; set; }
        public string Subnet { get; set; }
        public List<OptionAssignRefViewModel> AssignedOptions { get; set; }
        public BaseNameAndIDModel Base { get; set; }
        public string BaseId { get; set; }
        public DateTime LastInventoryUpdate { get; set; }
        public string SerialNumber { get; set; }
        public string Type { get; set; }
        public string HyperVisor { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public string AssetModelId { get; set; }
        public string CustomerId { get; set; }
        public bool IsOnline { get; set; }
        public DateTime LastOnlineStatusUpdate { get; set; }
        public string Timezone { get; set; }
    }
    public class ClientAddViewModelWeb
    {
        public string Id { get; set; }
        public string Uuid { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string BaseId { get; set; }
        public string Subnet { get; set; }
        public string CustomerName { get; set; }
        public bool IsOnline { get; set; }
        public List<string> MacAddresses { get; set; }
        public string SerialNumber { get; set; }
        public string Timezone { get; set; }
    }

    public class ClientsAndVMs
    {
        public List<ClientViewModel> Clients { get; set; }
        public List<VirtualMachineViewModel> VMs { get; set; }
    }

    public class ClientParameterViewModel
    {
        public ClientParameterViewModel() { }
        public string Id { get; set; }
        public string PropertyName { get; set; }
        public string ParameterName { get; set; }
        public string Value { get; set; }
        public ClientViewModel Client { get; set; }
        public string ClientId { get; set; }
        public bool IsEditable { get; set; }
    }

    public class InventoryDataViewModel
    {
        public List<PreinstalledSoftwareViewModel> PreinstalledSoftwares { get; set; }
        public List<InventoryViewModel> Inventory { get; set; }
    }

    public class PreinstalledSoftwareViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public string Size { get; set; }
        public string InstalledAt { get; set; }
        public ClientViewModel Client { get; set; }
    }

    public class InventoryViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public string ClientId { get; set; }
        public ClientViewModel Client { get; set; }
        public string Type { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class ClientDetailInformation
    {
        public string Id { get; set; }
        public string SerialNumber { get; set; }
        public string UUID { get; set; }
        public string MACAddress { get; set; }
        public string Name { get; set; }
    }
}
