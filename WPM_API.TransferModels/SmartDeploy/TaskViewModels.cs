using System;
using System.Collections.Generic;

namespace WPM_API.TransferModels.SmartDeploy
{
    public class TaskViewModel : TaskAddViewModel
    {
        public string Id { get; set; }
    }


    public class TaskEditViewModel : TaskAddViewModel
    {
    }

    public class TaskAddViewModel
    {
        public string Name { get; set; }
        public List<FileRef> Files { get; set; }
        public string Executable { get; set; }
        public string Commandline { get; set; }
        public Boolean UseShellExecute { get; set; }
        public string ExecutionContext { get; set; }
        public string Visibility { get; set; }
        public bool RestartRequired { get; set; }
        public string RunningContext { get; set; }
        public string InstallationType { get; set; }
    }
}
