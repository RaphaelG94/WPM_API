using System;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using WPM_API.Common.Extensions;
using WPM_API.Common.Utils;

namespace WPM_API.Code.Mappers
{
    public static class MapperExtensions
    {
        public static IMappingExpression<TSource, TDest> Map<TSource, TDest, TMember>(this IMappingExpression<TSource, TDest> expression, Expression<Func<TDest, object>> propertyDest, Expression<Func<TSource, TMember>> propertySrc)
        {
            expression.ForMember(propertyDest, opt => opt.MapFrom(propertySrc));
            return expression;
        }

        public static IMappingExpression<TSource, TDest> Ignore<TSource, TDest>(this IMappingExpression<TSource, TDest> expression, Expression<Func<TDest, object>> property)
        {
            expression.ForMember(property, opt => opt.Ignore());
            return expression;
        }
    }
}