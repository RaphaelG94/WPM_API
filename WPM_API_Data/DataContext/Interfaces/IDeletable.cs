using System;

namespace  WPM_API.Data.DataContext.Interfaces
{
    public interface IDeletable
    {
        string CreatedByUserId { get; set; }
        DateTime CreatedDate { get; set; }
        string UpdatedByUserId { get; set; }
        DateTime UpdatedDate { get; set; }
        string DeletedByUserId { get; set; }
        DateTime? DeletedDate { get; set; }
    }
}
