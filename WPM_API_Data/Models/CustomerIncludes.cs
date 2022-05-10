using System;
using System.Collections.Generic;
using System.Text;

namespace  WPM_API.Data.Models
{
    public static class CustomerIncludes
    {
        public const string Systemhouse = "Systemhouse";
        public const string MainCompany = "MainCompany";
        public const string MainCompanyExpert = "MainCompany.Expert";
        public const string Headquarter = "MainCompany.Headquarter";
        public const string Parameters = "Parameters";
        public const string IconLeft = "IconLeft";
        public const string IconRight = "IconRight";
        public const string Banner = "Banner";
        public const string StorageEntryPoints = "StorageEntryPoints";
        public const string CustomerSoftwareStreams = "CustomerSoftwareStreams";
        public const string CustomerSoftwareStreamsStreamMembers = "CustomerSoftwareStreams.StreamMembers";
        public const string CustomerImageStreams = "CustomerImageStreams";

        public static string[] GetAllIncludes()
        {
            string[] includes =
            {
                CustomerIncludes.Systemhouse, CustomerIncludes.MainCompany,
                CustomerIncludes.MainCompanyExpert, CustomerIncludes.MainCompanyExpert, CustomerIncludes.Parameters,
                CustomerIncludes.IconRight, CustomerIncludes.IconLeft, CustomerIncludes.Banner, CustomerIncludes.StorageEntryPoints, 
                CustomerIncludes.CustomerSoftwareStreams, CustomerIncludes.CustomerImageStreams
            };
            return includes;
        }
    }
}
