using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using WPM_API.Data.DataContext.Projections.Users;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using static WPM_API.Common.Constants;

namespace WPM_API.Code.Infrastructure.LogOn
{
    public class LoggedClaims
    {
        protected JsonSerializerSettings serializerSettings => new JsonSerializerSettings { Formatting = Formatting.None, ContractResolver = new CamelCasePropertyNamesContractResolver() };
        public string UserId { get; }
        public string Login { get; }
        public string Name { get; }
        public string Customer { get; }
        public string Systemhouse { get; }
        public bool Admin { get; }
        public IEnumerable<string> Roles { get; }
        public long GeneratedDateTicks { get; }

        public LoggedClaims(AccountProjection account)
            : this(account.Id, account.Login, account.UserName, account.Roles, account.CustomerId, account.CustomerName, account.SystemhouseId, account.SystemhouseName, account.Roles.Contains("admin"))
        {

        }

        public LoggedClaims(string userId, string login, string name, IEnumerable<string> roles, string customerId, string customerName, string systemhouseId, string systemhouseName, bool admin)
        {
            UserId = userId;
            Admin = admin;
            Login = login;
            Name = name;
            //Roles = roles.Where(r => r != Common.Constants.Roles.Admin);
            Roles = roles;//.Where(r => r != Common.Constants.Roles.Admin);
            if (!string.IsNullOrEmpty(customerId))
            {
                Customer = JsonConvert.SerializeObject(new { Id = customerId, Name = customerName }, serializerSettings);
            }
            if (!string.IsNullOrEmpty(systemhouseId))
            {
                Systemhouse = JsonConvert.SerializeObject(new { Id = systemhouseId, Name = systemhouseName }, serializerSettings);
            }
            GeneratedDateTicks = DateTime.UtcNow.Ticks;
        }

        public LoggedClaims(List<Claim> claims)
        {
            UserId = claims.Single(x => x.Type == BitstreamClaimTypes.UserId).Value;
            Login = claims.Single(x => x.Type == ClaimTypes.Name).Value;
            Name = claims.Single(x => x.Type == BitstreamClaimTypes.Name).Value;
            Admin = claims.Single(x => x.Type == BitstreamClaimTypes.Admin).Value == "true";
            Roles = claims.Single(x => x.Type == ClaimTypes.Role).Value.Split(",".ToCharArray());
            if (claims.Exists(x => x.Type == BitstreamClaimTypes.Customer))
            {
                Customer = claims.Single(x => x.Type == BitstreamClaimTypes.Customer).Value;
            }
            if (claims.Exists(x => x.Type == BitstreamClaimTypes.Systemhouse))
            {
                Systemhouse = claims.Single(x => x.Type == BitstreamClaimTypes.Systemhouse).Value;
            }
            GeneratedDateTicks = long.Parse(claims.Single(x => x.Type == BitstreamClaimTypes.GeneratedDate).Value);
        }

        public List<Claim> GetAsClaims()
        {
            List<Claim> result = new List<Claim>()
            {
                new Claim(BitstreamClaimTypes.UserId, UserId.ToString()),
                new Claim(ClaimTypes.Name, Login),
                new Claim(BitstreamClaimTypes.Sub, Login),
                new Claim(BitstreamClaimTypes.Name, Name),
                new Claim(ClaimTypes.Role, string.Join(",", Roles)),
                new Claim(BitstreamClaimTypes.Admin, Admin.ToString()),
                new Claim(BitstreamClaimTypes.GeneratedDate, GeneratedDateTicks.ToString())
            };
            if (Customer != null)
            {
                result.Add(new Claim(BitstreamClaimTypes.Customer, Customer));
            }
            if (Systemhouse != null)
            {
                result.Add(new Claim(BitstreamClaimTypes.Systemhouse, Systemhouse));
            }
            return result;
        }
    }
}
