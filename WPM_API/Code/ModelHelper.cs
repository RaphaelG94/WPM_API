using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using WPM_API.Common.Utils;

namespace WPM_API.Code
{
    public static class ModelHelper
    {
        public static string GetDisplayName<TModel>(Expression<Func<TModel, object>> expression)
        {
            var property = PropertyUtil.GetProperty(expression);
            string propertyDisplayName = null;

            var attr = (DisplayAttribute)property.GetCustomAttributes(typeof(DisplayAttribute), true).SingleOrDefault();
            if (attr == null)
            {
                var attrDisplayName = (DisplayNameAttribute)property.GetCustomAttributes(typeof(DisplayNameAttribute), true).SingleOrDefault();
                if (attrDisplayName != null)
                {
                    propertyDisplayName = attrDisplayName.DisplayName;
                }
            }
            else
            {
                propertyDisplayName = attr.Name;
            }
            return propertyDisplayName ?? property.Name;
        }        
    }
}
