using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WPM_API.Common
{
    public class Enums
    {
        public enum SchedulerActionTypes
        {
            ResetPasswordEmail = 1,

            BaseCreation = 2,

            ExampleAction = 1000
        }

        public enum MenuItemTypes
        {

        }

        public enum DbDataInitStrategyTypes
        {
            MigrateValidate,
            MigrateToLatest,
            None
        }
    }
}
