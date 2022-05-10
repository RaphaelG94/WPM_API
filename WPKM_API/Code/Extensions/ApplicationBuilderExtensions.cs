﻿using System;
using WPM_API.Code.Infrastructure.TokenAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace WPM_API.Code.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static void UseAppWebSecurity(this IApplicationBuilder app)
        {
            //var cookieOptions = new CookieAuthenticationOptions
            //        {
            //            CookieName = ".WPM_API.Web-Core-AUTH",
            //            LoginPath = new PathString("/Account/LogOn/"),
            //            AccessDeniedPath = new PathString("/Account/Forbidden/"),
            //            //AutomaticAuthenticate = true,
            //            //AutomaticChallenge = true
            //        };
            ////app.UseCookieAuthentication(cookieOptions);

            //var tokenOptions = app.ApplicationServices.GetRequiredService<IOptions<TokenAuthOptions>>().Value;

            //var jwtBearerOptions = new JwtBearerOptions();
            //// Basic settings - signing key to validate with, audience and issuer.
            //jwtBearerOptions.TokenValidationParameters.IssuerSigningKey = tokenOptions.IssuerSigningKey;
            //jwtBearerOptions.TokenValidationParameters.ValidAudience = tokenOptions.Audience;
            //jwtBearerOptions.TokenValidationParameters.ValidIssuer = tokenOptions.Issuer;


            //// When receiving a token, check that we've signed it.
            //jwtBearerOptions.TokenValidationParameters.ValidateIssuerSigningKey = true;


            //// When receiving a token, check that it is still valid.
            //jwtBearerOptions.TokenValidationParameters.ValidateLifetime = true;

            //// This defines the maximum allowable clock skew - i.e. provides a tolerance on the token expiry time 
            //// when validating the lifetime. As we're creating the tokens locally and validating them on the same 
            //// machines which should have synchronised time, this can be set to zero. Where external tokens are
            //// used, some leeway here could be useful.
            //jwtBearerOptions.TokenValidationParameters.ClockSkew = TimeSpan.FromMinutes(0);
            ////app.UseJwtBearerAuthentication(jwtBearerOptions);
        }
    }
}
