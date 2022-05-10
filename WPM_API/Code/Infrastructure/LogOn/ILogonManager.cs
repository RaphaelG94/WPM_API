using System;

namespace WPM_API.Code.Infrastructure.LogOn
{
    public interface ILogonManager
    {
        void SignInViaCookies(LoggedClaims loggedClaims, bool isPersistent);
        void SignOutAsCookies();
        string GenerateToken(LoggedClaims loggedClaims, DateTime? expires);
        bool ValidateToken(string token);
        LoggedClaims LoggedClaims { get; }
        LoggedUserDbInfo LoggedUserDbInfo { get; }

        void RefreshCurrentLoggedUserInfo(bool refreshClaims = true);
    }
}
