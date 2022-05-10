using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace  WPM_API.TransferModels.SmartDeploy
{
    public class RuleViewModel : RuleAddViewModel
    {
        public string Id { get; set; }
    }

    public class RuleAddViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public List<string> Architecture { get; set; }
        public Boolean Successon { get; set; }
        public FileRefViewModel Data { get; set; }
        public string Path { get; set; }
        public Boolean CheckVersionNr { get; set; }
        public string VersionNr { get; set; }
        public string OsType { get; set; }
        public List<string> OsVersionNames { get; set; }
        public List<string> Win10Versions { get; set; }
        public List<string> Win11Versions { get; set; }
    }

    public class RulesViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Architecture { get; set; }
        public string Path { get; set; }
        public bool Successon { get; set; }
        public TypeViewModel Type { get; set; }
        public FileRefViewModel Data { get; set; }
        public Boolean CheckVersionNr { get; set; }
        public string VersionNr { get; set; }
        public string OsType { get; set; }
        public List<string> OsVersionNames { get; set; }
        public List<string> Win10Versions { get; set; }
        public List<string> Win11Versions { get; set; }
    }

    public class TypeViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class SoftwareRulesViewModel
    {
        public RuleViewModel RuleDetection { get; set; }
        public RuleViewModel RuleApplicability { get; set; }
    }
}
