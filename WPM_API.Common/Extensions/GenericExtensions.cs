using System;
using System.Linq;
using System.Linq.Expressions;
using WPM_API.Common.Utils;

namespace WPM_API.Common.Extensions
{
    public static class GenericExtensions
    {
        public static string GetPropertyName<TObject>(this TObject type,
                                                       Expression<Func<TObject, object>> propertyRefExpr)
        {
            return PropertyUtil.GetName(propertyRefExpr);
        }

        public static bool In<T>(this T item, params T[] items)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            return items.Contains(item);
        }
    }
}
