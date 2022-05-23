using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WPM_API.Code.Infrastructure;
using WPM_API.Code.Infrastructure.LogOn;
using WPM_API.Common;
using WPM_API.Models;
using WPM_API.Options;

namespace WPM_API.Controllers.Extern
{
    [Route("api")]
    public class CertificationController : BasisController
    {
        public CertificationController(AppSettings appSettings, ConnectionStrings connectionStrings, OrderEmailOptions orderEmailOptions, AgentEmailOptions agentEmailOptions, SendMailCreds sendMailCreds, SiteOptions siteOptions, ILogonManager logonManager) : base(appSettings, connectionStrings, orderEmailOptions, agentEmailOptions, sendMailCreds, siteOptions, logonManager)
        {
        }

        [HttpGet]
        [Route("inventar")]
        [Authorize(Policy = Constants.Roles.Customer)]
        public IActionResult GetInventar([FromRoute] string customerId)
        {
            InventarViewModel inventar = new InventarViewModel();
            inventar.Clients = new List<InvClientViewModel>();
            inventar.Server = new List<ServerViewModel>();
            for (int i = 0; i < 100; i++)
            {
                Random gen = new Random();
                int prob = gen.Next(100);
                bool loc = prob <= 20;
                inventar.Clients.Add(new InvClientViewModel()
                {
                    Id = Guid.NewGuid().ToString(),
                    Ip = "192.168.1." + i.ToString(),
                    Location = loc ? "Germany" : "Austria",
                    Name = "Client " + i.ToString()
                });
            }
            for (int i = 0; i < 13; i++)
            {
                Random gen = new Random();
                int prob = gen.Next(100);
                bool loc = prob <= 20;
                inventar.Server.Add(new ServerViewModel()
                {
                    Id = Guid.NewGuid().ToString(),
                    Ip = "10.10.15." + i.ToString(),
                    AzureId = Guid.NewGuid().ToString(),
                    Name = "Server00" + i.ToString()
                });
            }
            // Serialize and return the response
            var json = JsonConvert.SerializeObject(inventar, serializerSettings);
            return new OkObjectResult(json);
        }
    }
}