using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using WPM_API.Data.DataContext.Interfaces;

namespace  WPM_API.Data.Infrastructure
{
    public abstract class RepositoryBase
    {
        protected DataContextProvider Context { get; set; }

        protected RepositoryBase(DataContextProvider context)
        {
            Context = context;
        }

        protected virtual T CreateEmpty<T>() where T : class, new()
        {
            return Context.Set<T>().Add(new T()).Entity;
        }

        protected void MarkForInsert<T>(T entity) where T : class
        {
            // TODO CreateDate & User setzen
            Context.Set<T>().Add(entity);
        }

        protected void MarkForUpdate<T>(T entity) where T : class
        {
            // TODO ModifyDate & User setzen
            Context.Set<T>().Update(entity);
        }

        protected void MarkForDelete<T>(T entity) where T : class
        {
            Context.Set<T>().Remove(entity);
        }

        protected void MarkForDelete<T>(IEnumerable<T> entities) where T : class
        {
            Context.Set<T>().RemoveRange(entities);
        }

        protected void Delete<T>(T entity) where T : class
        {
            Context.Set<T>().Remove(entity);
        }

        protected T GetRepository<T>() where T : RepositoryBase
        {
            return Context.GetRepository<T>();
        }
    }
}
