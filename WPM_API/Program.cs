using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Security.Claims;
using System.Security.Cryptography;
using WPM_API.Code.Infrastructure;
using WPM_API.Code.Infrastructure.LogOn;
using WPM_API.Code.Infrastructure.TokenAuth;
using WPM_API.Code.Mappers;
using WPM_API.Code.Scheduler;
using WPM_API.Code.Scheduler.Queue;
using WPM_API.Code.Scheduler.Queue.Workers;
using WPM_API.Code.Scheduler.Queue.Workers.Impl;
using WPM_API.Common.Emails;
using WPM_API.Common.Emails.Impl;
using WPM_API.Common.Files;
using WPM_API.Common.Files.Impl;
using WPM_API.Common.Files.Models;
using WPM_API.Data;
using WPM_API.Data.DataContext;
using WPM_API.Data.DataContext.Entities;
using WPM_API.Data.Files;
using WPM_API.Data.Files.Impl;
using WPM_API.Data.Infrastructure;
using WPM_API.Options;
using static WPM_API.Common.Constants;

var builder = WebApplication.CreateBuilder(args);

var corsBuilder = new CorsPolicyBuilder();
corsBuilder.AllowAnyHeader();
corsBuilder.AllowAnyMethod();
corsBuilder.AllowAnyOrigin();

// Setup app default values
var siteOptions = new SiteOptions();
var appSettings = new AppSettings();
var connectionStringsOptions = new ConnectionStrings();
var agentEmailOptions = new AgentEmailOptions();
var orderEmailOptions = new OrderEmailOptions();
var sendEmailCreds = new SendMailCreds();

builder.Configuration.GetSection(nameof(SiteOptions)).Bind(siteOptions);
builder.Configuration.GetSection(nameof(AppSettings)).Bind(appSettings);
builder.Configuration.GetSection(nameof(ConnectionStrings)).Bind(connectionStringsOptions);
builder.Configuration.GetSection(nameof(AgentEmailOptions)).Bind(agentEmailOptions);
builder.Configuration.GetSection(nameof(OrderEmailOptions)).Bind(orderEmailOptions);
builder.Configuration.GetSection(nameof(SendMailCreds)).Bind(sendEmailCreds);

builder.Services.AddSingleton(siteOptions);
builder.Services.AddSingleton(appSettings);
builder.Services.AddSingleton(connectionStringsOptions);
builder.Services.AddSingleton(agentEmailOptions);
builder.Services.AddSingleton(orderEmailOptions);
builder.Services.AddSingleton(sendEmailCreds);

builder.Services.AddCors(options =>
{
    options.AddPolicy("SiteCorsPolicy", corsBuilder.Build());
});

builder.Services.AddControllers();

// Add DB connection
var connectionString = builder.Configuration.GetConnectionString("Bitstream");
builder.Services.AddDbContext<DBData>(options =>
{
    options.UseSqlServer(
       connectionString,
        b => b.MigrationsAssembly("WPM_API.Data.ProjectMigration"));
}
);

builder.Services.AddMemoryCache();
builder.Services.AddOptions();
// Probleme Dependency Injection
// builder.Services.AddScoped<IActionContextAccessor, ActionContextAccessor>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Scheme = "Bearer",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

// Custom Services
builder.Services.AddScoped<DBData>();
builder.Services.Configure<FileFactoryOptions>(builder.Configuration.GetSection("FileOptions"));
builder.Services.AddSingleton<IFileFactoryService, FileFactoryService>();
builder.Services.AddSingleton<IAttachmentService, AttachmentService>();
builder.Services.AddSingleton<IPathResolver, PathResolver>();
builder.Services.AddScoped<IUnitOfWorkFactory, UnitOfWorkFactory>();
builder.Services.AddScoped<ILogonManager, LogonManager>();
builder.Services.AddScoped<ILoggedUserAccessor, LoggedUserAccessor>();
builder.Services.AddScoped<ViewDataItems>();
builder.Services.AddSingleton(sp => MapInit.CreateConfiguration().CreateMapper());
builder.Services.AddSingleton<IEmailSenderService, EmailSenderService>();
builder.Services.AddTransient<IEmailWorkerService, EmailWorkerService>();
builder.Services.AddTransient<ISchedulerWorkerService, SchedulerWorkerService>();
builder.Services.AddSingleton<WorkersQueue>();
builder.Services.AddSingleton<ISchedulerService, SchedulerService>();


// Define jwt criteria
RSAKeyUtils.GenerateKeyAndSave(builder.Environment.ContentRootPath + "\\App_Data\\RSAkey.txt");
RSAParameters keyParams = RSAKeyUtils.GetKeyParameters(builder.Environment.ContentRootPath + "\\App_Data\\RSAkey.txt");
var key = new RsaSecurityKey(keyParams);
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

// Validation parameters for local jwt token
builder.Services.Configure<TokenAuthOptions>(tokenAuthOptions =>
{
    tokenAuthOptions.Audience = tokenValidationParameters.ValidAudience;
    tokenAuthOptions.Issuer = tokenValidationParameters.ValidIssuer;
    tokenAuthOptions.SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.RsaSha256Signature);
    tokenAuthOptions.IssuerSigningKey = key;
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o =>
    {
        o.TokenValidationParameters = tokenValidationParameters;
    })
    .AddJwtBearer("AzureADB2C", options =>
    {
        options.Audience = "97ced207-3b0d-446c-a189-0d8aecde0502";
        options.Authority = "https://bitstreamtest.b2clogin.com/bitstreamtest.onmicrosoft.com/v2.0/";
        options.MetadataAddress = "https://bitstreamtest.b2clogin.com/bitstreamtest.onmicrosoft.com/v2.0/.well-known/openid-configuration?p=B2C_1_bitstreamtest_signup_signin";
        options.Events = new JwtBearerEvents();
        options.Events.OnTokenValidated = async context =>
        {
            var dbData = context.HttpContext.RequestServices.GetRequiredService<DBData>();
            var identity = context.Principal.Identity as ClaimsIdentity;
            IEnumerable<Claim> claims = identity.Claims;
            List<Claim> newClaims = new List<Claim>();
            var isAdminClaim = context.Principal.FindFirst(BitstreamClaimTypes.Admin);
            if (isAdminClaim == null)
            {
                string userId = context.Principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                List<Claim> emailClaims = claims.Where(x => x.Type.ToString() == "emails").ToList();
                if (userId != null)
                {
                    User user = dbData.Users.Include(x => x.Customer).Include(x => x.Systemhouse).Include("UserRoles.Role").FirstOrDefault(x => x.B2CID == userId);
                    if (user != null)
                    {
                        // Save b2c user id in db
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

                        // newClaims.AddRange(claims);
                        var appIdentity = new ClaimsIdentity(newClaims);
                        context.Principal.AddIdentity(appIdentity);
                    }
                }
            }
            else { }
        };
    });

builder.Services.AddAuthorization(auth =>
{
    auth.DefaultPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme, "AzureADB2C")
        .Build();

    // Admin = Adminflag + Systemhouse_Manager
    auth.AddPolicy(Policies.Admin, policyBuilder => policyBuilder.RequireAssertion(
        context =>
        {
            return context.User.HasClaim(claim =>
                (claim.Type == ClaimTypes.Role && claim.Value.Contains(Roles.Systemhouse)))
        && context.User.HasClaim(claim => (claim.Type == BitstreamClaimTypes.Admin && bool.Parse(claim.Value)));
        })
         .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme, "AzureADB2C")
        .Build());

    // Systemhouse_Manager = Admin or Systemhouse_Manager
    auth.AddPolicy(Policies.Systemhouse, policyBuilder => policyBuilder.RequireAssertion(
        context =>
        {
            return context.User.HasClaim(claim =>
               (claim.Type == ClaimTypes.Role && claim.Value.Contains(Roles.Systemhouse))
            || (claim.Type == BitstreamClaimTypes.Admin && bool.Parse(claim.Value)));
        })
        .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme, "AzureADB2C")
        .Build());

    // Customer_Manager = Admin or Systemhouse_Manager or Customer_Manager
    auth.AddPolicy(Policies.Customer, policyBuilder => policyBuilder.RequireAssertion(
        context =>
        {
            return context.User.HasClaim(claim =>
               (claim.Type == ClaimTypes.Role && claim.Value.Contains(Roles.Customer))
            || (claim.Type == ClaimTypes.Role && claim.Value.Contains(Roles.Systemhouse))
            || (claim.Type == BitstreamClaimTypes.Admin && bool.Parse(claim.Value)));
        })
        .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme, "AzureADB2C")
        .Build());
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.MapControllers();
//app.UseMiddleware<PopulateClaimsMiddleware>();
app.UseAuthorization();


AppDependencyResolver.Init(app.Services);


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<DBData>();
        // Initialize database
        await DbInitializer.InitializeAsync(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occured while inializing database");
    }
}

app.Run();
