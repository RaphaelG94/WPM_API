using System;

namespace  WPM_API.Data.DataContext.Interfaces
{
    public interface ICreatable
    {
        string CreatedByUserId { get; set; }
        DateTime CreatedDate { get; set; }
    }
}
