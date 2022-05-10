using System.Linq;

namespace  WPM_API.Data.Models
{

    public static class SoftwareIncludes
    {
        public const string RuleDetection = "RuleDetection";
        public const string RuleApplicability = "RuleApplicability";
        public const string RuleDetectionType = "RuleDetection.Type";
        public const string RuleApplicabilityType = "RuleApplicability.Type";
        public const string RuleDetectionFile = "RuleDetection.Data";
        public const string RuleApplicabilityFile = "RuleApplicability.Data";
        public const string TaskInstall = "TaskInstall";
        public const string TaskUninstall = "TaskUninstall";
        public const string TaskUpdate = "TaskUpdate";
        public const string TaskInstallFiles = "TaskInstall.Files";
        public const string TaskUninstallFiles = "TaskUninstall.Files";
        public const string TaskUpdateFiles = "TaskUpdate.Files";
        public const string TaskInstallExecutionFile = "TaskInstall.ExecutionFile";
        public const string TaskUninstallExecutionFile = "TaskUninstall.ExecutionFile";
        public const string TaskUpdateExecutionFile = "TaskUpdate.ExecutionFile";
        public const string RuleApplicabilityArchitecture = "RuleApplicability.Architecture";
        public const string RuleApplicabilityOsVersionNames = "RuleApplicability.OsVersionNames";
        public const string RuleDetectionArchitecture = "RuleDetection.Architecture";
        public const string RuleApplicabilityWin10Versions = "RuleApplicability.Win10Versions";
        public const string RuleApplicabilityWin11Versions = "RuleApplicability.Win11Versions";
        public const string RuleApplicabilityOsType = "RuleApplicability.OsType";
        public const string RuleDetectionOsType = "RuleDetection.OsType";
        public const string Systemhouses = "Systemhouses";
        public const string Customers = "Customers";
        public const string Clients = "Clients";


        public static string[] GetAllIncludes()
        {
            return GetAllRules().Union(GetAllTasks()).ToArray();
        }

        public static string[] GetAllRules()
        {
            string[] includes =
            {
                RuleDetection,
                RuleApplicability, 
                RuleDetectionType, 
                RuleApplicabilityType, 
                RuleDetectionFile, 
                RuleApplicabilityFile, 
                TaskInstallExecutionFile,
                TaskUninstallExecutionFile,
                TaskUpdateExecutionFile,
                RuleApplicabilityArchitecture,
                RuleDetectionArchitecture,
                RuleApplicabilityOsVersionNames,
                RuleApplicabilityWin10Versions,
                RuleApplicabilityWin11Versions,
                Systemhouses,
                Customers,
                Clients
            };
            return includes;
        }

        public static string[] GetAllTasks()
        {
            string[] includes =
            {
                TaskInstall, TaskUninstall, TaskUpdate, TaskInstallFiles,  TaskUninstallFiles, TaskUpdateFiles
            };
            return includes;
        }
    }

}
