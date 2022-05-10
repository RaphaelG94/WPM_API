using System;
using System.Collections.Generic;
using System.Text;

namespace  WPM_API.Data.Models
{
    public static class DomainIncludes
    {
        public const string OrganizationalUnits = "OrganizationalUnits";
        public const string DomainUsers = "DomainUsers";
        public const string Gpo = "Gpo";
        public const string Wallpaper = "Gpo.Wallpaper";
        public const string Lockscreen = "Gpo.Lockscreen";
        public const string DomainUserCSV = "DomainUserCSV";
        public const string DNS = "DNS";
         
        public static string[] GetAllIncludes()
        {
            string[] includes =
            {
                OrganizationalUnits, DomainUsers, Gpo, DomainUserCSV, DNS, Wallpaper, Lockscreen
            };
            return includes;
        }
    }
}
