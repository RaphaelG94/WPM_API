﻿using WPM_API.Common;

namespace  WPM_API.Data.Models
{
    public class PagingSortingInfo
    {
        public const string SortDescendingFlag = "descending";

        public int Page { get; set; }
        public int? PageSize { get; set; }
        public int? TotalItemCount { get; set; }
        public string SortMember { get; set; }
        public bool SortDescending { get; set; }

        public int PageSizeReal
        {
            get { return PageSize ?? Constants.PageSizeDefault; }
        }

        public string Sort
        {
            get { return SortDescending ? string.Format("{0} {1}", SortMember, SortDescendingFlag) : SortMember; }
        }

        public static PagingSortingInfo GetDefault(string sortMember)
        {
            return new PagingSortingInfo()
            {
                Page = 1,
                PageSize = Constants.PageSizeDefault,
                SortMember = sortMember,
                SortDescending = false
            };
        }
    }
}
