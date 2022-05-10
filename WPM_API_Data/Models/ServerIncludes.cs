using System;
using System.Collections.Generic;
using System.Text;

namespace  WPM_API.Data.Models
{
    public static class ServerIncludes
    {
        public const string RuleDetection = "RuleDetection";
        public const string RuleApplicability = "RuleApplicability";
        public const string TaskInstall = "TaskInstall";
        public const string TaskUninstall = "TaskUninstall";
        public const string TaskUpdate = "TaskUpdate";
        public const string Icon = "Icon";

        public static string[] GetTaskAndRuleIncludes()
        {
            string[] includes =
            {
                RuleApplicability, RuleDetection, TaskInstall, TaskUninstall, TaskUpdate
            };
            return includes;
        }
    }
}
