using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using WPM_API.Code.Extensions;
using WPM_API.Code.Infrastructure.TokenAuth;
using WPM_API.Data.Infrastructure;

namespace WPM_API.Code.Infrastructure.LogOn
{
    public class LogonManager : ILogonManager
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IMemoryCache _memoryCache;
        private readonly IWebHostEnvironment _hostEnv;
        private readonly UnitOfWork _unitOfWork;
        private readonly TokenAuthOptions _tokenAuthOptions;

        public LogonManager(IHttpContextAccessor contextAccessor, IMemoryCache memoryCache, IUnitOfWorkFactory unitOfWorkFactory, IOptions<TokenAuthOptions> tokenAuthOptions, IWebHostEnvironment env)
        {
            _contextAccessor = contextAccessor;
            _memoryCache = memoryCache;
            _unitOfWork = unitOfWorkFactory.UnitOfWork;
            _tokenAuthOptions = tokenAuthOptions.Value;
            _hostEnv = env;
        }

        public LoggedClaims LoggedClaims => IsAuthenticated
            ? new LoggedClaims(_contextAccessor.HttpContext.User.Claims.ToList())
            : null;

        public LoggedUserDbInfo LoggedUserDbInfo
        {
            get
            {
                var claims = LoggedClaims;
                if (claims == null)
                    return null;

                var dbInfo = _memoryCache.GetOrAdd(GetUserDbInfoCacheKey(claims.Login), () => GetUserDbInfo(claims.Login, claims.GeneratedDateTicks));
                if (dbInfo.GeneratedDateTicks != claims.GeneratedDateTicks)
                {
                    /*This mean that our cache out of date. This can occur on Web server farm*/
                    dbInfo = GetUserDbInfo(claims.Login, claims.GeneratedDateTicks);
                    _memoryCache.Set(GetUserDbInfoCacheKey(claims.Login), dbInfo);
                }
                return dbInfo;
            }
        }

        public void SignInViaCookies(LoggedClaims loggedClaims, bool isPersistent)
        {
            //    var identity = new ClaimsIdentity(loggedClaims.GetAsClaims(), CookieAuthenticationDefaults.AuthenticationScheme);
            //    _contextAccessor.HttpContext.Authentication.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity)
            //        , new AuthenticationProperties() { IsPersistent = isPersistent })
            //        .Wait();
        }

        public void SignOutAsCookies()
        {
            //    _contextAccessor.HttpContext.Authentication.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme)
            //        .Wait();
        }

        public string GenerateToken(LoggedClaims loggedClaims, DateTime? expires)
        {
            var handler = new JwtSecurityTokenHandler();

            var token = new JwtSecurityToken(
                issuer: _tokenAuthOptions.Issuer,
                audience: _tokenAuthOptions.Audience,
                signingCredentials: _tokenAuthOptions.SigningCredentials,
                claims: loggedClaims.GetAsClaims(),
                expires: expires);


            //var identity = new ClaimsIdentity(loggedClaims.GetAsClaims(), JwtBearerDefaults.AuthenticationScheme);
            //var descriptor = new SecurityTokenDescriptor
            //{
            //    Issuer = _tokenAuthOptions.Issuer,
            //    Audience = _tokenAuthOptions.Audience,
            //    SigningCredentials = _tokenAuthOptions.SigningCredentials,
            //    Subject = identity,
            //    Expires = expires
            //};

            //var securityToken = handler.CreateToken(descriptor);

            return handler.WriteToken(token);
        }

        public bool ValidateToken(string token)
        {
            try
            {
                string[] tokenParts = token.Split('.');
                var handler = new JwtSecurityTokenHandler();
                handler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = _tokenAuthOptions.IssuerSigningKey,
                    CryptoProviderFactory = new CryptoProviderFactory()
                    {
                        CacheSignatureProviders = false
                    }
                }, out SecurityToken validatedToken);
                if (validatedToken.ValidTo.CompareTo(DateTime.UtcNow) > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public void RefreshCurrentLoggedUserInfo(bool refreshClaims = true)
        {
            if (LoggedClaims == null)//we are not signed in
                return;

            _memoryCache.Remove(GetUserDbInfoCacheKey(LoggedClaims.Login));
            if (refreshClaims)
            {
                var authType = _contextAccessor.HttpContext.User.Identity.AuthenticationType;
                switch (authType)
                {
                    case CookieAuthenticationDefaults.AuthenticationScheme:
                        var newClaims = new LoggedClaims(_unitOfWork.Users.GetAccountById(LoggedClaims.UserId));
                        //SignInViaCookies(newClaims, true /*TODO: detect if current cookies persistent or not*/);
                        break;
                    default:
                        throw new Exception($"RefreshCurrentLoggedUserInfo does not support {authType} authentication");
                }
            }
        }

        private static string GetUserDbInfoCacheKey(string login)
        {
            return "userLogon_" + login;
        }

        private LoggedUserDbInfo GetUserDbInfo(string login, long generatedDateTicks)
        {
            var user = _unitOfWork.Users.GetAccountByLoginOrNull(login);
            if (user == null)
                return null;

            return new LoggedUserDbInfo(user.Login, user.UserName, user.LastName, generatedDateTicks);
        }

        private bool IsAuthenticated => _contextAccessor.HttpContext.User?.Identity?.IsAuthenticated ?? false;
    }
}
