using System;
using System.Collections.Generic;
using System.Text;

namespace  WPM_API.Data.Models
{
    public static class PersonIncludes
    {
        public const string Company = "Company";

        public static string[] GetAllIncludes()
        {
            string[] includes =
            {
                PersonIncludes.Company
            };
            return includes;
        }
    }
}
