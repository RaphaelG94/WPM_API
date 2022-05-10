using System;
using System.Collections.Generic;

namespace WPM_API.Models
{
    public class BaseRefViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
    }

    public class BaseViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public SubscriptionViewModel Subscription { get; set; }
        public ResourceGroupViewModel ResourceGroup { get; set; }
        public StorageAccountViewModel StorageAccount { get; set; }
        public VirtualNetworkViewModel VirtualNetwork { get; set; }
        public VpnViewModel Vpn { get; set; }
        public List<VirtualMachineViewModel> VirtualMachines { get; set; }
        public DateTime CreationDate { get; set; }
        public string CreationUser { get; set; }
        public List<AdvancedPropertyViewModel> AdvancedProperties { get; set; }
        public BaseStatusViewModel BaseStatus { get; set; }
    }

    public class BaseAddViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string SubscriptionId { get; set; }
        public ResourceGroupViewModel ResourceGroup { get; set; }
        public StorageAccountViewModel StorageAccount { get; set; }
        public VirtualNetworkAddViewModel VirtualNetwork { get; set; }
        public VpnAddViewModel Vpn { get; set; }
        // public List<VirtualNetworkAddViewModel> VirtualMachines { get; set; }
        public VirtualMachineAddViewModel VirtualMachine { get; set; }
    }

    public class AdvancedPropertyViewModel
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public bool isEditable { get; set; }
        public CategoryViewModel Category { get; set; }
    }
    public class AdvancedPropertyAddViewModel
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }

    public class BaseNameAndIDModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class ParameterNames
    {
        public List<string> ParamNames { get; set; }
    }

    public class ParameterModel
    {
        public ParameterModel(string name, string value) { Name = name; Value = value; }
        public string Name { get; set; }
        public string Value { get; set; }
    }

    public class ParameterKeyValueModel
    {
        public List<ParameterModel> Parameters { get; set; }
    }

    public class BaseStatusViewModel
    {
        public string Id { get; set; }
        public string Status { get; set; }
        public string ResourceGroupStatus { get; set; }
        public string VirtualNetworkStatus { get; set; }
        public string StorageAccountStatus { get; set; }
        public string VPNStatus { get; set; }
        public string ErrorMessage { get; set; }
    }
}
