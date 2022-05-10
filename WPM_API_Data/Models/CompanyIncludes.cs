using System;
using System.Collections.Generic;
using System.Text;

namespace  WPM_API.Data.Models
{
    public static class CompanyIncludes
    {
        public static string Expert = "Expert";
        public static string Headquarter = "Headquarter";

        public static string[] GetAllIncludes()
        {
            string[] includes =
            {
                CompanyIncludes.Expert, CompanyIncludes.Headquarter
            };
            return includes;
        }
    }
}
