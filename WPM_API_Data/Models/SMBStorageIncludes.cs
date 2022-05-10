using System;
using System.Collections.Generic;
using System.Text;

namespace  WPM_API.Data.Models
{
    public static class SMBStorageIncludes
    {
        public const string Client = "Client";

        public static string[] GetAllIncludes ()
        {
            return new string[]
            {
                SMBStorageIncludes.Client
            };
        }
    }
}
