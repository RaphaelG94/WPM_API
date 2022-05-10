using System;
using System.Collections.Generic;
using System.Text;

namespace  WPM_API.Data.Models
{
    public class ClientParameterIncludes
    {
        public const string Client = "Client";

        public static string[] GetAllIncludes()
        {
            string[] includes =
            {
                ClientParameterIncludes.Client
            };
            return includes;
        }
    }
}
