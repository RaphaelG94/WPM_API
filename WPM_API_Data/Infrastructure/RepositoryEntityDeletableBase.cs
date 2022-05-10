using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using WPM_API.Data.DataContext.Interfaces;
using WPM_API.Data.Exceptions;
using WPM_API.Data.Extensions;
using WPM_API.Data.DataContext.Entities;
using Microsoft.EntityFrameworkCore;

namespace  WPM_API.Data.Infrastructure
{
    public class RepositoryEntityDeletableBase<T> : RepositoryEntityBase<T>
        where T : class, IEntity, IDeletable, new()
    {
        protected IQueryable<T> EntitySetNotDeleted => EntitySet.GetNotDeleted();

        public RepositoryEntityDeletableBase(DataContextProvider context)
            : base(context)
        {
        }

        public override IQueryable<T> GetAll(params string[] includes)
        {
            return EntitySetNotDeleted.IncludeMultiple(includes).GetDefaultOrder();
        }

        public new T GetOrNull(string id, params string[] includes)
        {
            var result = base.GetOrNull(id, includes);
            if (result == null)
                return null;

            return result.DeletedDate != null ? null : result;
        }

        public new T Get(string id, params string[] includes)
        {
            var result = base.Get(id, includes);

            if (result.DeletedDate != null)
                throw new RecordNotFoundException(typeof(T), id, true);

            return result;
        }

        public T GetWithDeletedOrNull(string id)
        {
            return base.GetOrNull(id);
        }

        public T GetWithDeleted(string id)
        {
            return base.Get(id);
        }

        public new TResult GetCustomOrDefault<TResult>(string id, Expression<Func<T, TResult>> selector)
        {
            return GetCustomInner(id, selector, true, EntitySetNotDeleted);
        }

        public new TResult GetCustom<TResult>(string id, Expression<Func<T, TResult>> selector)
        {
            return GetCustomInner(id, selector, false, EntitySetNotDeleted);
        }

        public TResult GetCustomWithDeletedOrDefault<TResult>(string id, Expression<Func<T, TResult>> selector)
        {
            return GetCustomInner(id, selector, true);
        }

        public TResult GetCustomWithDeleted<TResult>(string id, Expression<Func<T, TResult>> selector)
        {
            return GetCustomInner(id, selector);
        }

        public virtual T CreateEmpty(string userId)
        {
            T empty = base.CreateEmpty<T>();
            empty.CreatedDate = DateTime.Now;
            empty.CreatedByUserId = userId;
            return empty;
        }

        public virtual void MarkForInsert(T entity, string userId)
        {
            entity.CreatedDate = DateTime.Now;
            entity.CreatedByUserId = userId;
            base.MarkForInsert(entity);
        }

        public virtual void MarkForUpdate(T entity, string userId)
        {
            entity.UpdatedDate = DateTime.Now;
            entity.UpdatedByUserId = userId;
            base.MarkForUpdate(entity);
        }

        public virtual void MarkForUpdateAnonymous(T entity)
        {
            entity.UpdatedDate = DateTime.Now;
            base.MarkForUpdate(entity);
        }

        public virtual void MarkForDelete(T entity, string userId)
        {
            entity.DeletedDate = DateTime.Now;
            entity.DeletedByUserId = userId;
        }

        public virtual void MarkForDelete(IEnumerable<T> entities, string userId)
        {
            foreach (T entity in entities)
            {
                entity.DeletedDate = DateTime.Now;
                entity.DeletedByUserId = userId;
            }
        }
    }
}
