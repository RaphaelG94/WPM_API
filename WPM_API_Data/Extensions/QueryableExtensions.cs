using System;
using System.Linq;
using WPM_API.Data.DataContext.Interfaces;
using WPM_API.Data.Models;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace  WPM_API.Data.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> PagingSorting<T>(this IQueryable<T> source, PagingSortingInfo page)
        {
            IQueryable<T> qSource = source;
            if (source == null)
                throw new ArgumentNullException(nameof(source), "source is null.");
            if (page == null)
                return qSource;

            page.TotalItemCount = qSource.Count();

            if (!string.IsNullOrWhiteSpace(page.Sort))
            {
                qSource = qSource.OrderBy(page.Sort);
            }
            qSource = qSource.Skip((page.Page - 1) * page.PageSizeReal).Take(page.PageSizeReal);

            return qSource;
        }

        public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, string sortExpression)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source), "source is null.");

            if (string.IsNullOrEmpty(sortExpression))
                throw new ArgumentException("sortExpression is null or empty.", nameof(sortExpression));

            return DynamicQueryableExtensions.OrderBy(source, sortExpression.Replace("__", "."));
        }

        public static IQueryable<T> GetNotDeleted<T>(this IQueryable<T> q) where T : IDeletable, new() 
        {
            return q.Where(x=>x.DeletedDate == null);
        }
        public static IQueryable<T> IncludeMultiple<T>(this IQueryable<T> query,
            params string[] includes) where T : class
        {
            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }
            return query;
        }
        public static IQueryable<T> GetDefaultOrder<T>(this IQueryable<T> q)
        {
            Type orderedType = typeof(T);


            const string ordinalColumn = "Ordinal";
            bool existsOrdinal = orderedType.GetProperty(ordinalColumn) != null;

            const string nameColumn = "Name";
            bool existsName = orderedType.GetProperty(nameColumn) != null;

            if (existsName)
                return q.OrderBy($"{(existsOrdinal ? ordinalColumn + ", " : "")}{nameColumn}");


            const string titleColumn = "Title";
            bool existsTitle = orderedType.GetProperty(titleColumn) != null;

            if (existsTitle)
                return q.OrderBy($"{(existsOrdinal ? ordinalColumn + ", " : "")}{titleColumn}");


            if (existsOrdinal)
                return q.OrderBy(ordinalColumn);

            return q;
        }
    }
}
