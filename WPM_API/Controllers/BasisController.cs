using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Security.Cryptography;
using System.Text;
using WPM_API.Code.Infrastructure;
using WPM_API.Code.Infrastructure.Api;
using WPM_API.Code.Infrastructure.LogOn;
using WPM_API.Data.DataContext.Entities;
using WPM_API.Data.DataContext.Projections.Users;
using WPM_API.Data.Infrastructure;
using WPM_API.Options;

namespace WPM_API.Controllers
{
    public class BasisController : ControllerBaseApi
    {
        //protected SiteOptions _siteOptions => HttpContext.RequestServices.GetRequiredService<IOptions<SiteOptions>>().Value;
        protected readonly AppSettings appSettings;
        protected readonly ConnectionStrings connectionStrings;
        protected readonly OrderEmailOptions orderEmailOptions;
        protected readonly AgentEmailOptions agentEmailOptions;
        protected readonly SendMailCreds sendMailCreds;
        protected readonly SiteOptions siteOptions;
        protected readonly ILogonManager logonManager;
        protected readonly UnitOfWork unitOfWork;
        protected readonly JsonSerializerSettings serializerSettings = new JsonSerializerSettings { Formatting = Formatting.Indented, ContractResolver = new CamelCasePropertyNamesContractResolver() };

        //public BasisController(
        //    AppSettings appSettings,
        //    ConnectionStrings connectionStrings,
        //    OrderEmailOptions orderEmailOptions,
        //    AgentEmailOptions agentEmailOptions,
        //    SendMailCreds sendMailCreds,
        //    SiteOptions siteOptions,
        //    ILogonManager logonManager,
        //    UnitOfWork unitOfWork)
        //{
        //    this.appSettings = appSettings;
        //    this.connectionStrings = connectionStrings;
        //    this.orderEmailOptions = orderEmailOptions;
        //    this.agentEmailOptions = agentEmailOptions;
        //    this.sendMailCreds = sendMailCreds;
        //    this.siteOptions = siteOptions;
        //    this.logonManager = logonManager;
        //    this.unitOfWork = unitOfWork;
        //}

        public BasisController(
            AppSettings appSettings,
            ConnectionStrings connectionStrings,
            OrderEmailOptions orderEmailOptions,
            AgentEmailOptions agentEmailOptions,
            SendMailCreds sendMailCreds,
            SiteOptions siteOptions,
            ILogonManager logonManager)
        {
            this.appSettings = appSettings;
            this.connectionStrings = connectionStrings;
            this.orderEmailOptions = orderEmailOptions;
            this.agentEmailOptions = agentEmailOptions;
            this.sendMailCreds = sendMailCreds;
            this.siteOptions = siteOptions;
            this.logonManager = logonManager;
        }

        protected Boolean CurrentUserIsInRole(string role)
        {
            return LoggedUser.IsInRole(role);
        }

        protected AccountProjection GetCurrentUser()
        {
            return UnitOfWork.Users.GetAccountById(LoggedUser.Id);
        }

        protected CloudEntryPoint GetCEP(String customerId = null, string cepId = null)
        {
            using (var unitOfWork = CreateUnitOfWork())
            {
                CloudEntryPoint result = null;
                if (string.IsNullOrEmpty(customerId))
                {
                    AccountProjection user = GetCurrentUser();
                    customerId = user.CustomerId;
                }
                WPM_API.Data.DataContext.Entities.Customer customer = unitOfWork.Customers.GetOrNull(customerId, "CloudEntryPoints");
                if (customer.CloudEntryPoints == null)
                {
                    return null;
                }
                if (!(customer.CloudEntryPoints == null || customer.CloudEntryPoints.Count == 0))
                {
                    if (cepId == null)
                    {
                        result = customer.CloudEntryPoints.Find(x => x.IsStandard == true);
                    }
                    else
                    {
                        result = customer.CloudEntryPoints.Find(x => x.Id == cepId);
                    }
                }
                if (result != null)
                {
                    result.ClientId = DecryptString(result.ClientId);
                    result.ClientSecret = DecryptString(result.ClientSecret);
                    result.TenantId = DecryptString(result.TenantId);
                }
                return result;
            }
        }

        private static readonly string key = "N43Kn90tbubxJZeLCIZIIjxagKyq4ik0";

        /*
         * Encrypts a string with a symmetric key using AES
         */
        public static string EncryptString(string plainText)
        {
            byte[] iv = new byte[16];
            byte[] array;
            using (Aes aes = Aes.Create())
            {
                aes.IV = iv;
                aes.Key = Encoding.UTF8.GetBytes(key);
                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream((Stream)ms, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter sw = new StreamWriter((Stream)cs))
                        {
                            sw.Write(plainText);
                        }
                        array = ms.ToArray();
                    }
                }
            }
            return Convert.ToBase64String(array);
        }

        protected static string DecryptString(string cipherText)
        {
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(cipherText);

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream(buffer))
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }

        protected UnitOfWork CreateUnitOfWork()
        {
            return AppDependencyResolver.Current.CreateUoWinCurrentThread();
        }

        public class AgentsAuthenticationModel
        {
            public string SerialNumber { get; set; }
            public List<string> MacAddresses { get; set; }
            public string OSType { get; set; }
        }

        public class AgentsAuthenticationSWApplicabilityModel : AgentsAuthenticationModel
        {
            public bool Is64Bit { get; set; }
            public string ClientOrServer { get; set; }
            public string BuildNr { get; set; }
        }
    }
}