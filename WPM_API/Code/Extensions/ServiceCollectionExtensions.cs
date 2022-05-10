using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using WPM_API.Common.Emails;
using WPM_API.Common.Emails.Impl;
using WPM_API.Common.Files;
using WPM_API.Common.Files.Impl;
using WPM_API.Common.Files.Models;
using WPM_API.Common.Utils.Email;
using WPM_API.Data.Files;
using WPM_API.Data.Files.Impl;
using WPM_API.Data.Infrastructure;
using WPM_API.Code.Infrastructure;
using WPM_API.Code.Infrastructure.Api;
using WPM_API.Code.Infrastructure.LogOn;
using WPM_API.Code.Infrastructure.Menu;
using WPM_API.Code.Infrastructure.TokenAuth;
using WPM_API.Code.Mappers;
using WPM_API.Code.Scheduler;
using WPM_API.Code.Scheduler.Queue;
using WPM_API.Code.Scheduler.Queue.Workers;
using WPM_API.Code.Scheduler.Queue.Workers.Impl;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;
using static WPM_API.Common.Constants;
using Microsoft.AspNetCore.Authentication.Cookies;
using System;
using WPM_API.Options;
using WPM_API.Middlewares;

namespace WPM_API.Code.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddAppWeb(this IServiceCollection services, IConfigurationRoot configurationRoot)
        {
            services.Configure<SiteOptions>(configurationRoot.GetSection("SiteOptions"));
            // Add AppSettings
            var appSettingsOptions = configurationRoot.GetSection(nameof(AppSettings));
            services.Configure<AppSettings>(appSettings =>
            {
                appSettings.ClientId = appSettingsOptions[nameof(AppSettings.ClientId)];
                appSettings.ClientSecret = appSettingsOptions[nameof(AppSettings.ClientSecret)];
                appSettings.TenantId = appSettingsOptions[nameof(AppSettings.TenantId)];

                appSettings.DevelopmentClientSecret = appSettingsOptions[nameof(AppSettings.DevelopmentClientSecret)];
                appSettings.DevelopmentClientId = appSettingsOptions[nameof(AppSettings.DevelopmentClientId)];
                appSettings.DevelopmentTenantId = appSettingsOptions[nameof(AppSettings.DevelopmentTenantId)];

                appSettings.AzureStoragePath = appSettingsOptions[nameof(AppSettings.AzureStoragePath)];
                appSettings.StorageAccountKey = appSettingsOptions[nameof(AppSettings.StorageAccountKey)];
                appSettings.StorageAccountName = appSettingsOptions[nameof(AppSettings.StorageAccountName)];

                appSettings.SmartDeploySources = appSettingsOptions[nameof(AppSettings.SmartDeploySources)];
                appSettings.ResourcesRepositoryFolder = appSettingsOptions[nameof(AppSettings.ResourcesRepositoryFolder)];
                appSettings.FileRepositoryFolder = appSettingsOptions[nameof(AppSettings.FileRepositoryFolder)];
                appSettings.TempFolder= appSettingsOptions[nameof(AppSettings.TempFolder)];
                appSettings.IconsAndBanners = appSettingsOptions[nameof(AppSettings.IconsAndBanners)];
                appSettings.FileDestConnectionString = appSettingsOptions[nameof(AppSettings.FileDestConnectionString)];
                appSettings.LiveSystemConnectionString = appSettingsOptions[nameof(AppSettings.LiveSystemConnectionString)];
            });

            var connectionStringsOptions = configurationRoot.GetSection(nameof(ConnectionStrings));
            services.Configure<ConnectionStrings>(appSettings =>
            {
                appSettings.FileRepository = connectionStringsOptions[nameof(ConnectionStrings.FileRepository)];
            });
            var agentEmailOptions = configurationRoot.GetSection(nameof(AgentEmailOptions));
            services.Configure<AgentEmailOptions>(appSettings =>
            {
                appSettings.Email = agentEmailOptions[nameof(AgentEmailOptions.Email)];
                appSettings.Host = agentEmailOptions[nameof(AgentEmailOptions.Host)];
                appSettings.Password = agentEmailOptions[nameof(AgentEmailOptions.Password)];
                appSettings.Port = int.Parse(agentEmailOptions[nameof(AgentEmailOptions.Port)]);
                appSettings.EnableSsl = agentEmailOptions[nameof(AgentEmailOptions.EnableSsl)];
                appSettings.DisplayName = agentEmailOptions[nameof(AgentEmailOptions.DisplayName)];
            });
            var orderEmailOptions = configurationRoot.GetSection(nameof(OrderEmailOptions));
            services.Configure<OrderEmailOptions>(appSettings =>
            {
                appSettings.ReceiverEmail = orderEmailOptions[nameof(OrderEmailOptions.ReceiverEmail)];
                appSettings.Host = orderEmailOptions[nameof(OrderEmailOptions.Host)];
                appSettings.Password = orderEmailOptions[nameof(OrderEmailOptions.Password)];
                appSettings.Port = orderEmailOptions[nameof(OrderEmailOptions.Port)];
                appSettings.EnableSsl = orderEmailOptions[nameof(OrderEmailOptions.EnableSsl)];
            });
            var sendEmailCreds = configurationRoot.GetSection(nameof(SendMailCreds));
            services.Configure<SendMailCreds>(appSettings => 
            {
                appSettings.DisplayName = sendEmailCreds[nameof(SendMailCreds.DisplayName)];
                appSettings.Host = sendEmailCreds[nameof(SendMailCreds.Host)];
                appSettings.Password = sendEmailCreds[nameof(SendMailCreds.Password)];
                appSettings.Port = int.Parse(sendEmailCreds[nameof(SendMailCreds.Port)]);
                appSettings.EnableSsl = sendEmailCreds[nameof(SendMailCreds.EnableSsl)];
                appSettings.Email = sendEmailCreds[nameof(SendMailCreds.Email)];
            });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IActionContextAccessor, ActionContextAccessor>();
            services.AddSingleton<IPathResolver, PathResolver>();

            services.AddScoped<IUnitOfWorkFactory, UnitOfWorkFactory>();

            services.AddScoped<ILogonManager, LogonManager>();
            services.AddScoped<ILoggedUserAccessor, LoggedUserAccessor>();

            services.AddScoped<IMenuBuilderFactory, MenuBuilderFactory>();
            services.AddScoped<ViewDataItems>();

            services.AddSingleton(sp => MapInit.CreateConfiguration().CreateMapper());

            AddFiles(services, configurationRoot);

            services.AddSingleton<IEmailSenderService, EmailSenderService>();
            services.Configure<EmailSenderOptions>(configurationRoot.GetSection("EmailSenderOptions"));

            services.AddTransient<IEmailWorkerService, EmailWorkerService>();
            services.AddTransient<ISchedulerWorkerService, SchedulerWorkerService>();
            services.AddSingleton<WorkersQueue>();
            services.AddSingleton<ISchedulerService, SchedulerService>();
        }

        private static void AddFiles(IServiceCollection services, IConfigurationRoot configurationRoot)
        {
            services.Configure<FileFactoryOptions>(configurationRoot.GetSection("FileOptions"));
            services.AddSingleton<IFileFactoryService, FileFactoryService>();
            services.AddSingleton<IAttachmentService, AttachmentService>();
        }

       public static void AddAppWebSecurity(this IServiceCollection services, IWebHostEnvironment env)
        {
            RSAKeyUtils.GenerateKeyAndSave(env.ContentRootPath + "\\App_Data\\RSAkey.txt");
            RSAParameters keyParams = RSAKeyUtils.GetKeyParameters(env.ContentRootPath + "\\App_Data\\RSAkey.txt");
            var key = new RsaSecurityKey(keyParams);

            // JWT-Token:
            var tokenValidationParameters = new TokenValidationParameters
            {
                // The signing key must match!  
                ValidateIssuerSigningKey = false,
                IssuerSigningKey = key,

                // Validate the JWT Issuer (iss) claim  
                ValidateIssuer = true,
                ValidIssuer = "Bitstream GmbH",

                // Validate the JWT Audience (aud) claim  
                ValidateAudience = true,
                ValidAudience = "http://bitstream.azurewebsites.com/",

                // Validate the token expiry  
                ValidateLifetime = true,

                ClockSkew = TimeSpan.Zero
            };

            // used to register signing for local jwt and validating local jwts
            services.Configure<TokenAuthOptions>(tokenAuthOptions =>
            {
                tokenAuthOptions.Audience = tokenValidationParameters.ValidAudience;
                tokenAuthOptions.Issuer = tokenValidationParameters.ValidIssuer;
                tokenAuthOptions.SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.RsaSha256Signature);
                tokenAuthOptions.IssuerSigningKey = key;
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(o =>
            {
                o.TokenValidationParameters = tokenValidationParameters;
            })
            .AddJwtBearer("AzureADB2C", options =>
            {
                options.Authority = "https://bitstreamtest.b2clogin.com/bitstreamtest.onmicrosoft.com/v2.0/";
                options.Audience = "97ced207-3b0d-446c-a189-0d8aecde0502";
                options.MetadataAddress =
                "https://bitstreamtest.b2clogin.com/bitstreamtest.onmicrosoft.com/v2.0/.well-known/openid-configuration?p=B2C_1_bitstreamtest_signup_signin";
            });

            services.AddAuthorization(auth =>
            {
                auth.DefaultPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    // .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme, "AzureADB2C")
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .Build();

                // Admin = Adminflag + Systemhouse_Manager
                auth.AddPolicy(Policies.Admin, policyBuilder => policyBuilder.RequireAssertion(
                    context => context.User.HasClaim(claim =>
                           (claim.Type == ClaimTypes.Role && claim.Value.Contains(Roles.Systemhouse)))
                        && context.User.HasClaim(claim => (claim.Type == BitstreamClaimTypes.Admin && bool.Parse(claim.Value))))
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .Build());

                // Systemhouse_Manager = Admin or Systemhouse_Manager
                auth.AddPolicy(Policies.Systemhouse, policyBuilder => policyBuilder.RequireAssertion(
                    context => context.User.HasClaim(claim =>
                           (claim.Type == ClaimTypes.Role && claim.Value.Contains(Roles.Systemhouse))
                        || (claim.Type == BitstreamClaimTypes.Admin && bool.Parse(claim.Value))))
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .Build());

                // Customer_Manager = Admin or Systemhouse_Manager or Customer_Manager
                auth.AddPolicy(Policies.Customer, policyBuilder => policyBuilder.RequireAssertion(
                    context => context.User.HasClaim(claim =>
                           (claim.Type == ClaimTypes.Role && claim.Value.Contains(Roles.Customer))
                        || (claim.Type == ClaimTypes.Role && claim.Value.Contains(Roles.Systemhouse))
                        || (claim.Type == BitstreamClaimTypes.Admin && bool.Parse(claim.Value))))
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .Build());
            });                        
        }
    }
}
