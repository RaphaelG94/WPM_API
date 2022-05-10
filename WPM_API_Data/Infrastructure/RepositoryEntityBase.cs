using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using WPM_API.Data.Exceptions;
using WPM_API.Data.Extensions;
using Microsoft.EntityFrameworkCore;
using WPM_API.Data.DataContext.Entities;
using WPM_API.Data.DataContext.Interfaces;

namespace  WPM_API.Data.Infrastructure
{
    public abstract class RepositoryEntityBase<T> : RepositoryBase
        where T : class, IEntity, new()
    {
        protected DbSet<T> EntitySet { get; private set; }

        protected RepositoryEntityBase(DataContextProvider context)
            : base(context)
        {
            EntitySet = Context.Set<T>();
        }

        public T GetOrNull(string id, params string[] includes)
        {
            IQueryable<T> set = EntitySet.IncludeMultiple(includes);
            if(set.Count(x =>x.Id == id) > 0)
            {
                return set.Where(x => x.Id == id).First();
            }
            return null;
        }

        public T Get(string id, params string[] includes)
        {
            var record = GetOrNull(id, includes);
            if (record == null)
                throw new RecordNotFoundException(typeof(T), id);
            return record;
        }

        public virtual IQueryable<T> GetAll(params string[] includes)
        {
            return EntitySet.IncludeMultiple(includes).GetDefaultOrder();
        }

        public virtual T CreateEmpty()
        {
            return base.CreateEmpty<T>();
        }

        public virtual void MarkForInsert(T entity)
        {
            base.MarkForInsert(entity);
        }

        public virtual void MarkForUpdate(T entity)
        {
            base.MarkForUpdate(entity);
        }

        public virtual void MarkForDelete(T entity)
        {
            base.MarkForDelete(entity);
        }

        public virtual void MarkForDelete(IEnumerable<T> entities)
        {
            base.MarkForDelete(entities);
        }

        public virtual void Delete(T entity)
        {
            base.Delete(entity);
        }

        public TResult GetCustomOrDefault<TResult>(string id, Expression<Func<T, TResult>> selector)
        {
            return GetCustomInner(id, selector, true);
        }

        public TResult GetCustom<TResult>(string id, Expression<Func<T, TResult>> selector)
        {
            return GetCustomInner(id, selector);
        }

        protected TResult GetCustomInner<TResult>(string id, Expression<Func<T, TResult>> selector, bool returnDefault = false, IQueryable<T> useAsBaseQuery = null)
        {
            var keys = Context.GetEntityKeys<T>().ToArray();
            if (keys.Length != 1)
                throw new Exception("GetCustomInner works only with Entity which has one primary key column.");

            var baseQuery = useAsBaseQuery ?? EntitySet;

            //var query = baseQuery.Where(keys[0]+"=@0", id).Select(selector);

            var param = Expression.Parameter(typeof(T), "p");
            var exp = Expression.Lambda<Func<T, bool>>(
                Expression.Equal(
                    Expression.Property(param, keys[0]),
                    ExpressionClosureFactory.GetField(id)
                ),
                param
            );
            var query = baseQuery.Where(exp).Select(selector);

            var resultList = query.ToArray();
            if (returnDefault && resultList.Length == 0)
                return default(TResult);

            if (resultList.Length == 0)
                throw new RecordNotFoundException(typeof(T), id);

            return resultList[0];
        }
    }
}
