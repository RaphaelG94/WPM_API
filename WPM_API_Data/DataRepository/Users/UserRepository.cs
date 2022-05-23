using Microsoft.EntityFrameworkCore;
using WPM_API.Data.DataContext.Entities;
using WPM_API.Data.DataContext.Projections.Users;
using WPM_API.Data.Extensions;
using WPM_API.Data.Infrastructure;
using WPM_API.Data.Models;

namespace WPM_API.Data.DataRepository.Users
{
    internal static class UserExtensions
    {
        public static IQueryable<User> IncludeRoles(this IQueryable<User> users)
        {
            return users.Include(x => x.UserRoles).ThenInclude(x => x.Role);
        }

        public static IQueryable<User> IncludeSystemhouse(this IQueryable<User> users)
        {
            return users.Include(x => x.Systemhouse).ThenInclude(x => x.Customer);
        }

        public static IQueryable<AccountProjection> SelectAccountProjection(this IQueryable<User> users)
        {
            return users.Select(m => new AccountProjection
            {
                Id = m.Id,
                Login = m.Login,
                Password = m.Password,
                UserName = m.UserName,
                Email = m.Email,
                CustomerId = m.CustomerId,
                CustomerName = m.Customer.Name,
                SystemhouseId = m.SystemhouseId,
                SystemhouseName = m.Systemhouse.Name,
                Active = m.Active,
                Roles = m.UserRoles.Select(t => t.Role.Name)
            });
        }
    }

    public class UserRepository : RepositoryEntityDeletableBase<User>
    {
        public RoleRepository Roles => GetRepository<RoleRepository>();

        public UserRepository(DataContextProvider context)
            : base(context)
        {
        }

        public List<User> GetUsersForAdmin(string search, PagingSortingInfo pagingSorting)
        {
            var query = EntitySetNotDeleted;
            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();
                query = query.Where(x => x.UserName.Contains(search));
            }
            return query.IncludeRoles().PagingSorting(pagingSorting).ToList();
        }

        public List<User> GetUsersByFilter(string prefix, int count)
        {
            if (string.IsNullOrWhiteSpace(prefix))
                prefix = null;
            return EntitySetNotDeleted
                .Where(m => (prefix == null || m.UserName.StartsWith(prefix)))
                .Take(count)
                .ToList();
        }

        public User GetWithRolesOrNull(string id)
        {
            return EntitySet.IncludeRoles().IncludeSystemhouse()
                .FirstOrDefault(m => m.Id == id);
        }

        public User GetByEmailOrNull(string email, bool includeDeleted = false)
        {
            return GetUserView(includeDeleted).IncludeRoles()
                .FirstOrDefault(m => m.Email == email);
        }

        public User GetByLoginOrNull(string login, bool includeDeleted = false)
        {
            return GetUserView(includeDeleted).IncludeRoles()
                .FirstOrDefault(m => m.Login == login);
        }

        public AccountProjection GetAccountByLoginOrNull(string login)
        {
            return EntitySetNotDeleted.Where(m => m.Login == login)
                .SelectAccountProjection().FirstOrDefault();
        }

        public AccountProjection GetAccountById(string id)
        {
            return EntitySet.Where(m => m.Id == id)
                .SelectAccountProjection().FirstOrDefault();
        }

        private IQueryable<User> GetUserView(bool includeDeleted = false)
        {
            var q = EntitySet.AsQueryable();
            if (!includeDeleted)
            {
                q = q.GetNotDeleted();
            }
            return q;
        }

        public List<User> GetDeleted()
        {
            return EntitySet.Where(x => x.DeletedDate != null).ToList();
        }

        public override User CreateEmpty(string creationUserId)
        {
            var item = base.CreateEmpty();
            item.CreatedDate = DateTime.Now;
            item.CreatedByUserId = creationUserId;
            return item;
        }

        public UserForgotPassword GetForgotPasswordRequest(Guid id)
        {
            return Context.Set<UserForgotPassword>()
                .Include(x => x.User)
                .SingleOrDefault(m => m.RequestGuid == id);
        }

        public UserForgotPassword GetForgotPasswordRequest(string id)
        {
            return Context.Set<UserForgotPassword>().SingleOrDefault(m => m.Id == id);
        }
    }
}
