using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WPM_API.Middlewares
{
    public class ClaimsMiddleware
    {
        private readonly RequestDelegate _next;

        public ClaimsMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                var authHeaderValues = httpContext.Request.Headers.GetCommaSeparatedValues("Authorization");
                if (authHeaderValues.Length != 0)
                {
                    var token = authHeaderValues[0].Split(" ")[1];
                    var handler = new JwtSecurityTokenHandler();
                    var jsonToken = handler.ReadJwtToken(token); 
                    if (jsonToken.Issuer == "https://bitstreamtest.b2clogin.com/85469981-451b-4862-b1d0-32dd2b77be68/v2.0/")
                    {
                        var claims = new List<Claim>
                        {
                            new Claim("isAdmin", "true")
                        };

                        var appIdentity = new ClaimsIdentity(claims);
                        httpContext.User.AddIdentity(appIdentity);
                    }                    
                }               

                await _next(httpContext);
            } catch (Exception e)
            {
                return;
            }
            
        }
    }
}
