using System;
using System.Collections.Generic;

namespace WPM_API.Models
{
    public class DomainRefViewModel
    {
        public string Id { get; set; }
        public DomainNameViewModel Domain { get; set; }
        public string Status { get; set; }
    }

    public class DomainViewModel : DomainAddViewModel
    {
        public string Id { get; set; }
        public string Status { get; set; }
        public DateTime CreationDate { get; set; }
        public string CreationUser { get; set; }
    }

    public class DomainAddViewModel
    {
        public DomainNameViewModel Domain { get; set; }
        public OUAddViewModel OrganizationalUnits { get; set; }
        public List<DomainUserViewModel> DomainUsers { get; set; }
        public GroupPolicyObjectViewModel Gpo { get; set; }
        public List<ParameterViewModel> Parameters { get; set; }
        public FileRefModel DomainUserCSV { get; set; }
        public FileRefModel Office365ConfigurationXML { get; set; }
        public DNSAddViewModel DNS { get; set; }
    }

    public class DomainNameViewModel
    {
        public string Name { get; set; }
        public string Tld { get; set; }
    }

    public class DomainUserViewModel
    {
        public string Name { get; set; }
        public string UserGivenName { get; set; }
        public string UserLastName { get; set; }
        public string SamAccountName { get; set; }
        public string UserPrincipalName { get; set; }
        public string MemberOf { get; set; }
        public string Description { get; set; }
        public string Displayname { get; set; }
        public string Workphone { get; set; }
        public string Email { get; set; }
    }

    public class GroupPolicyObjectViewModel
    {
        public string Id { get; set; }
        public bool BsiCertified { get; set; }
        public FileRefModel Wallpaper { get; set; }
        public FileRefModel Lockscreen { get; set; }
        public string Settings { get; set; }
    }

}