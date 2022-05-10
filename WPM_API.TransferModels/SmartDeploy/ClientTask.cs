using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace  WPM_API.TransferModels.SmartDeploy
{
    public class ClientTask
    {
        public string task_executionfilename;
        public object clienttask_id;
        public string status_status;

        public string task_uid { get; set; }
        public string task_commandline { get; set; }
        public string task_useshellexecute { get; set; }
        public List<string> task_filenames { get; set; }
        public bool checkVersionNr { get; set; }
        public string versionNr { get; set; } 
        public string exePath { get; set; }
        public string executionContext { get; set; }
        public string visibility { get; set; }
        public bool restartRequired { get; set; }
        public string Name { get; set; }
        public string Checksum { get; set; }
    }

    public class ClientTaskWithDetection
    {
        public string task_executionfilename;
        public object clienttask_id;
        public string status_status;

        public string task_uid { get; set; }
        public string task_commandline { get; set; }
        public string task_useshellexecute { get; set; }
        public List<string> task_filenames { get; set; }
        public bool checkVersionNr { get; set; }
        public string versionNr { get; set; }
        public string exePath { get; set; }
        public string executionContext { get; set; }
        public string visibility { get; set; }
        public bool restartRequired { get; set; }
        public string Name { get; set; }
        public string Checksum { get; set; }
        public RuleViewModel DetectionRule { get; set; }
    }
}
