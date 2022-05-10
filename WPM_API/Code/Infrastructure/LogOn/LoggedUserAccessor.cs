using System;
using System.Linq;
using WPM_API.Common;

namespace WPM_API.Code.Infrastructure.LogOn
{
    public class LoggedUserAccessor: ILoggedUserAccessor
    {
        private readonly ILogonManager _logonManager;

        public LoggedClaims Claims => _logonManager.LoggedClaims;

        public LoggedUserDbInfo DbInfo => _logonManager.LoggedUserDbInfo;

        public string Id
        {
            get
            {
                if (Claims == null)
                    throw new Exception("User not signed in");

                return Claims.UserId;
            }
        }

        public string IdOrNull => Claims?.UserId;

        public bool IsAuthenticated => Claims != null;

        public bool IsAdmin => IsInRole(Constants.Roles.Admin);
        public bool IsCustomer => IsInRole(Constants.Roles.Customer);
        public bool IsSystemhouse => IsInRole(Constants.Roles.Systemhouse);

        public bool IsInRole(string role)
        {
            return Claims?.Roles.Contains(role) ?? false;
        }


        public LoggedUserAccessor(ILogonManager logonManager)
        {
            _logonManager = logonManager;
        }
    }
}
