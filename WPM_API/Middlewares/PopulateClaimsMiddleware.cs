using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Security.Claims;
using WPM_API.Data.DataContext;
using WPM_API.Data.DataContext.Entities;
using static WPM_API.Common.Constants;

namespace WPM_API.Middlewares
{
    public class PopulateClaimsMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string[] AnonymousPaths = { "/auth", "/connect/", "/zip/", "/agent/" };

        public PopulateClaimsMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async System.Threading.Tasks.Task InvokeAsync(HttpContext context, DBData dbData)
        {
            if (context.User.Identity.IsAuthenticated)
            {

                string token = context.Request.Headers["Authorization"];
                if (string.IsNullOrEmpty(token))
                {
                    context.Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
                    await _next(context);
                }
                else
                {
                    if (token.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                    {
                        if (string.IsNullOrEmpty(token))
                        {
                            context.Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
                            await _next(context);
                        }
                        else
                        {
                            token = token.Substring("Bearer ".Length).Trim();
                            var identity = context.User.Identity as ClaimsIdentity;
                            if (identity != null)
                            {
                                IEnumerable<Claim> claims = identity.Claims;
                                var isAdminClaim = context.User.FindFirst(BitstreamClaimTypes.Admin);
                                if (isAdminClaim == null)
                                {
                                    // Find user in db and populate new claims
                                    string userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                                    List<Claim> emailClaims = claims.Where(x => x.Type.ToString() == "emails").ToList();
                                    if (userId == null)
                                    {
                                        context.Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
                                        await _next(context);
                                    }
                                    User user = dbData.Users.Include(x => x.Customer).Include(x => x.Systemhouse).Include("UserRoles.Role").FirstOrDefault(x => x.B2CID == userId);
                                    if (user == null)
                                    {
                                        foreach (Claim emailClaim in emailClaims)
                                        {
                                            // Check with email address
                                            user = dbData.Users.Include(x => x.Customer).Include(x => x.Systemhouse).Include("UserRoles.Role").FirstOrDefault(x => x.Email == emailClaim.Value);
                                            if (user != null)
                                            {
                                                break;
                                            }
                                        }
                                    }
                                    if (user != null)
                                    {
                                        if (user.B2CID == null || user.B2CID == String.Empty)
                                        {
                                            // Save b2c userid in user entry
                                            user.B2CID = userId;
                                            dbData.Users.Update(user);
                                            dbData.SaveChanges();
                                        }

                                        // Populate claims
                                        JsonSerializerSettings serializerSettings = new JsonSerializerSettings { Formatting = Formatting.None, ContractResolver = new CamelCasePropertyNamesContractResolver() };

                                        List<string> userRoles = new List<string>();
                                        foreach (UserRole userRole in user.UserRoles)
                                        {
                                            userRoles.Add(userRole.Role.Name);
                                        }
                                        List<Claim> newClaims = new List<Claim>();
                                        newClaims.Add(new Claim(BitstreamClaimTypes.UserId, user.Id));
                                        newClaims.Add(new Claim(BitstreamClaimTypes.Name, user.UserName));
                                        newClaims.Add(new Claim(BitstreamClaimTypes.Admin, user.Admin.ToString()));
                                        newClaims.Add(new Claim(BitstreamClaimTypes.Sub, user.Login));
                                        newClaims.Add(new Claim(ClaimTypes.Role, string.Join(",", userRoles)));
                                        if (user.Customer != null)
                                        {
                                            newClaims.Add(new Claim(BitstreamClaimTypes.Customer, JsonConvert.SerializeObject(new { Id = user.CustomerId, Name = user.Customer.Name }, serializerSettings)));
                                        }
                                        if (user.Systemhouse != null)
                                        {
                                            newClaims.Add(new Claim(BitstreamClaimTypes.Systemhouse, JsonConvert.SerializeObject(new { Id = user.SystemhouseId, Name = user.Systemhouse.Name }, serializerSettings)));
                                        }

                                        newClaims.AddRange(claims);
                                        var appIdentity = new ClaimsIdentity(newClaims);
                                        context.User.AddIdentity(appIdentity);
                                    }
                                }
                            }
                            await _next(context);
                        }
                    }
                }
            }
            else
            {
                await _next(context);
            }
        }
    }
}
