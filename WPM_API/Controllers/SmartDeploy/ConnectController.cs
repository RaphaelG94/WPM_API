using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using System.Diagnostics;
using System.IO.Compression;
using System.Linq.Dynamic.Core;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Xml.Linq;
using WPM_API.Azure;
using WPM_API.Code.Infrastructure;
using WPM_API.Code.Infrastructure.LogOn;
using WPM_API.Data.DataContext.Entities;
using WPM_API.Data.DataContext.Entities.Storages;
using WPM_API.Data.Models;
using WPM_API.FileRepository;
using WPM_API.FileRepository.SmartDeploy;
using WPM_API.Options;
using WPM_API.TransferModels;
using WPM_API.TransferModels.SmartDeploy;
//using bsshared.database;
//using bsshared;
//using System.Data.Entity;
using DATA = WPM_API.Data.DataContext.Entities;

namespace WPM_API.Controllers
{
    [AllowAnonymous]
    [Route("connect")]
    public class ConnectController : BasisController
    {
        [HttpGet]
        [Route("getTime")]
        public string GetTime()
        {
            DateTime now = DateTime.UtcNow;
            return now.ToString("dd.MM.yyyy HH:mm:ss");
        }

        [HttpPost]
        [Route("getTimeZone/{uuid}")]
        public IActionResult GetClientTimezone([FromRoute] string uuid, [FromBody] AgentsAuthenticationModel data)
        {
            using (var unitOfWork = CreateUnitOfWork())
            {
                DATA.Client fetchedClient = null;
                List<DATA.Client> fetchedClients = unitOfWork.Clients.GetAll("Customer").Where(x => x.UUID == uuid && x.SerialNumber == data.SerialNumber).ToList();

                List<DATA.Client> fittingClients = new List<DATA.Client>();

                var fittingClientes = new List<DATA.Client>();

                // Find to remove clients
                foreach (DATA.Client client in fetchedClients)
                {
                    List<MacAddress> macAddresses = new List<MacAddress>();
                    macAddresses = unitOfWork.MacAddresses.GetAll()
                        .Where(x => x.ClientId == client.Id).ToList();
                    for (int i = 0; i < macAddresses.Count; i++)
                    {
                        foreach (string mac in data.MacAddresses)
                        {
                            if (macAddresses.ElementAt(i).Address.ToLower() == mac.ToLower())
                            {
                                if (!fittingClientes.Contains(client))
                                {
                                    fittingClientes.Add(client);
                                }
                            }
                        }
                    }
                }

                // Check if a client was identified
                if (!(fittingClientes.Count() == 0 || fittingClientes.Count() > 1))
                {
                    fetchedClient = fittingClientes.First();
                }

                if (fetchedClient == null)
                {
                    return BadRequest("ERROR: The client does not exist");
                }

                string result;
                if (fetchedClient.Timezone != null)
                {
                    result = fetchedClient.Timezone;
                }
                else
                {
                    result = "W. Europe Standard Time";
                }
                return Ok(result);
            }
        }


        [HttpPost]
        [Route("getComputerName/{uuid}")]
        public IActionResult GetComputerName([FromRoute] string uuid)
        {
            using (var unitOfWork = CreateUnitOfWork())
            {
                DATA.Client client = unitOfWork.Clients.GetAll().Where(x => x.UUID == uuid).FirstOrDefault();
                if (client == null)
                {
                    return BadRequest("The client does not exist");
                }

                ClientParameter clientName = unitOfWork.ClientParameters.GetAll().Where(x => x.ParameterName == "$ComputerTargetName" && x.ClientId == client.Id).FirstOrDefault();
                if (clientName == null)
                {
                    return BadRequest("The client name is not known");
                }

                return Content(clientName.Value);
            }
        }

        public static JsonSerializer ser = new JsonSerializer();

        public ConnectController(AppSettings appSettings, ConnectionStrings connectionStrings, OrderEmailOptions orderEmailOptions, AgentEmailOptions agentEmailOptions, SendMailCreds sendMailCreds, SiteOptions siteOptions, ILogonManager logonManager) : base(appSettings, connectionStrings, orderEmailOptions, agentEmailOptions, sendMailCreds, siteOptions, logonManager)
        {
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("{CompMail}")]
        public async Task<IActionResult> NotifyCustomerAsync([FromRoute] string CompMail, [FromBody] ClientAddViewModel ClientData)
        {
            try
            {
                SmtpClient client = new SmtpClient
                {
                    Host = agentEmailOptions.Host,
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(agentEmailOptions.Email, agentEmailOptions.Password),

                    TargetName = "STARTTLS/smtp.office365.com"
                };

                /*
                MailMessage message = new MailMessage();

                message.From = from;
                message.Sender = from;
                message.To.Add(to);
                message.;
                message.;
                message.;
                message.IsBodyHtml = true;
                */
                //MailAddress from = new MailAddress(agentEmailOptions.Email, agentEmailOptions.DisplayName);
                MailAddress from = new MailAddress(agentEmailOptions.Email, agentEmailOptions.DisplayName);
                MailAddress to = new MailAddress(CompMail.Trim());
                using (var message = new MailMessage(from, to)
                {
                    BodyEncoding = System.Text.Encoding.UTF8,
                    IsBodyHtml = true,
                    Subject = "New Client wants to join your Systemhouse.",
                    SubjectEncoding = System.Text.Encoding.UTF8,
                    Body = "Dear Customer,<br> a new Client wants to Join your Systemhouse.<br><br> Client data:<br><br> Device Name : " + ClientData.Name + "<br><br> Description : " + ClientData.Description + " <br><br> UUID :  " + ClientData.uuid + "</br></br></br></br></br></br></br>Please go to <a href=\"http://portal.bitstream.de\"> http://portal.bitstream.de </a> in order to register your client." + "</br></br></br></br></br></br> With Kind Regards,</br> BitStream Team",

                })
                {
                    await client.SendMailAsync(message);
                }
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("helpdesk/{uuid}")]
        public IActionResult GetHelpdeskInformation([FromRoute] string uuid, [FromBody] AgentsAuthenticationModel data)
        {
            using (var unitOfWork = CreateUnitOfWork())
            {
                DATA.Client fetchedClient = null;
                List<DATA.Client> fetchedClients = unitOfWork.Clients.GetAll("Customer").Where(x => x.UUID == uuid && x.SerialNumber == data.SerialNumber).ToList();

                List<DATA.Client> fittingClients = new List<DATA.Client>();

                var fittingClientes = new List<DATA.Client>();

                // Find to remove clients
                foreach (DATA.Client client in fetchedClients)
                {
                    List<MacAddress> macAddresses = new List<MacAddress>();
                    macAddresses = unitOfWork.MacAddresses.GetAll()
                        .Where(x => x.ClientId == client.Id).ToList();
                    for (int i = 0; i < macAddresses.Count; i++)
                    {
                        foreach (string mac in data.MacAddresses)
                        {
                            if (macAddresses.ElementAt(i).Address.ToLower() == mac.ToLower())
                            {
                                if (!fittingClientes.Contains(client))
                                {
                                    fittingClientes.Add(client);
                                }
                            }
                        }
                    }
                }

                // Check if a client was identified
                if (!(fittingClientes.Count() == 0 || fittingClientes.Count() > 1))
                {
                    fetchedClient = fittingClientes.First();
                }

                if (fetchedClient == null)
                {
                    return BadRequest("ERROR: The client does not exist");
                }

                DATA.Customer customer = fetchedClient.Customer;
                HelpdeskInfoViewModel result = new HelpdeskInfoViewModel();
                result.Email = customer.Email;
                result.Phone = customer.Phone;
                result.OpeningTimes = customer.OpeningTimes;
                result.CustomerName = customer.Name;
                result.Btn1Label = customer.Btn1Label;
                result.Btn2Label = customer.Btn2Label;
                result.Btn3Label = customer.Btn3Label;
                result.Btn4Label = customer.Btn4Label;
                result.CmdBtn1 = customer.CmdBtn1;
                result.CmdBtn2 = customer.CmdBtn2;
                result.CmdBtn3 = customer.CmdBtn3;
                result.CmdBtn4 = customer.CmdBtn4;
                result.BannerLink = customer.BannerLink;

                var json = JsonConvert.SerializeObject(result, serializerSettings);
                return Ok(json);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("activityLog/{uuid}")]
        [NonAction]
        public IActionResult PushActivityLog([FromRoute] string uuid, [FromBody] AddActivityLogViewModel data)
        {
            try
            {
                using (var unitOfWork = CreateUnitOfWork())
                {
                    DATA.Client fetchedClient = null;
                    List<DATA.Client> fetchedClients = UnitOfWork.Clients.GetAll("ActivityLogs").Where(x => x.UUID == uuid && x.SerialNumber == data.SerialNumber).ToList();
                    List<DATA.Client> fittingClients = new List<DATA.Client>();

                    var fittingClientes = new List<DATA.Client>();

                    // Find to remove clients
                    foreach (DATA.Client client in fetchedClients)
                    {
                        List<MacAddress> macAddresses = new List<MacAddress>();
                        macAddresses = unitOfWork.MacAddresses.GetAll()
                            .Where(x => x.ClientId == client.Id).ToList();
                        for (int i = 0; i < macAddresses.Count; i++)
                        {
                            foreach (string mac in data.MacAddresses)
                            {
                                if (macAddresses.ElementAt(i).Address.ToLower() == mac.ToLower())
                                {
                                    if (!fittingClientes.Contains(client))
                                    {
                                        fittingClientes.Add(client);
                                    }
                                }
                            }
                        }
                    }

                    // Check if a client was identified
                    if (!(fittingClientes.Count() == 0 || fittingClientes.Count() > 1))
                    {
                        fetchedClient = fittingClientes.First();
                    }

                    if (fetchedClient == null)
                    {
                        return BadRequest("ERROR: The client does not exist");
                    }

                    if (fetchedClient.ActivityLogs == null)
                    {
                        fetchedClient.ActivityLogs = new List<DATA.ActivityLog>();
                    }
                    if (fetchedClient.ActivityLogs.Count == 0)
                    {
                        foreach (WPM_API.Models.ActivityLogViewModel al in data.ActivityLogs)
                        {
                            DATA.ActivityLog newLog = Mapper.Map<DATA.ActivityLog>(al);
                            fetchedClient.ActivityLogs.Add(newLog);
                        }
                    }
                    else
                    {
                        foreach (WPM_API.Models.ActivityLogViewModel al in data.ActivityLogs)
                        {
                            DateTime toSearchFor = DateTime.Parse(al.TimeStamp);
                            if (fetchedClient.ActivityLogs.Find(x => x.TimeStamp == toSearchFor) == null)
                            {
                                DATA.ActivityLog newLog = Mapper.Map<DATA.ActivityLog>(al);
                                fetchedClient.ActivityLogs.Add(newLog);
                            }
                        }
                    }
                    unitOfWork.Clients.MarkForUpdate(fetchedClient);
                    unitOfWork.SaveChanges();
                }
                return Ok();
            }
            catch (Exception e)
            {
                var st = new StackTrace(e, true);
                // Get the top stack frame
                var frame = st.GetFrame(0);
                // Get the line number from the stack frame
                var line = frame.GetFileLineNumber();
                return BadRequest("ERROR: Could not push activity log: " + e.Message + " LINENR: " + line);
            }
        }

        public class AddActivityLogViewModel : AgentsAuthenticationModel
        {
            public List<WPM_API.Models.ActivityLogViewModel> ActivityLogs { get; set; }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("iconOrBanner/getName/{clientUUID}/{specFile}")]
        public IActionResult GetIconOrBannerName([FromRoute] string clientUUID, [FromRoute] string specFile, [FromBody] AgentsAuthenticationModel data)
        {
            using (var unitOfWork = CreateUnitOfWork())
            {
                DATA.Client fetchedClient = null;
                var fetchedClients = unitOfWork.Clients.GetAll(ClientIncludes.GetAllIncludes()).Where(x => x.UUID == clientUUID && x.SerialNumber == data.SerialNumber).ToList();
                var fittingClientes = new List<DATA.Client>();

                // Find to remove clients
                foreach (DATA.Client client in fetchedClients)
                {
                    List<MacAddress> macAddresses = new List<MacAddress>();
                    macAddresses = unitOfWork.MacAddresses.GetAll()
                        .Where(x => x.ClientId == client.Id).ToList();
                    for (int i = 0; i < macAddresses.Count; i++)
                    {
                        foreach (string mac in data.MacAddresses)
                        {
                            if (macAddresses.ElementAt(i).Address.ToLower() == mac.ToLower())
                            {
                                if (!fittingClientes.Contains(client))
                                {
                                    fittingClientes.Add(client);
                                }
                            }
                        }
                    }
                }

                // Check if a client was identified
                if (!(fittingClientes.Count() == 0 || fittingClientes.Count() > 1))
                {
                    fetchedClient = fittingClientes.First();
                }

                if (fetchedClient != null)
                {
                    DATA.Customer customer = fetchedClient.Customer;
                    DATA.File toDownload = null;
                    switch (specFile)
                    {
                        case "right":
                            toDownload = customer.IconRight;
                            break;
                        case "left":
                            toDownload = customer.IconLeft;
                            break;
                        case "banner":
                            toDownload = customer.Banner;
                            break;
                    }
                    if (toDownload != null)
                    {
                        return Ok(toDownload.Name);
                    }
                    else
                    {
                        return BadRequest("ERROR: The customer has not set this file yet.");
                    }
                }
                else
                {
                    return BadRequest("ERROR: The client is not registered yet.");
                }
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("iconOrBanner/{clientUUID}/{specFile}")]
        public async Task<IActionResult> DownloadCustomerBannerOrIcons([FromRoute] string clientUUID, [FromRoute] string specFile, [FromBody] AgentsAuthenticationModel data)
        {
            using (var unitOfWork = CreateUnitOfWork())
            {
                DATA.Client fetchedClient = null;
                var fetchedClients = unitOfWork.Clients.GetAll(ClientIncludes.GetAllIncludes()).Where(x => x.UUID == clientUUID && x.SerialNumber == data.SerialNumber).ToList();
                var fittingClientes = new List<DATA.Client>();

                // Find to remove clients
                foreach (DATA.Client client in fetchedClients)
                {
                    List<MacAddress> macAddresses = new List<MacAddress>();
                    macAddresses = unitOfWork.MacAddresses.GetAll()
                        .Where(x => x.ClientId == client.Id).ToList();
                    for (int i = 0; i < macAddresses.Count; i++)
                    {
                        foreach (string mac in data.MacAddresses)
                        {
                            if (macAddresses.ElementAt(i).Address.ToLower() == mac.ToLower())
                            {
                                if (!fittingClientes.Contains(client))
                                {
                                    fittingClientes.Add(client);
                                }
                            }
                        }
                    }
                }

                // Check if a client was identified
                if (!(fittingClientes.Count() == 0 || fittingClientes.Count() > 1))
                {
                    fetchedClient = fittingClientes.First();
                }

                if (fetchedClient != null)
                {
                    DATA.Customer customer = fetchedClient.Customer;
                    DATA.File toDownload = null;
                    switch (specFile)
                    {
                        case "right":
                            toDownload = customer.IconRight;
                            break;
                        case "left":
                            toDownload = customer.IconLeft;
                            break;
                        case "banner":
                            toDownload = customer.Banner;
                            break;
                    }
                    if (toDownload != null)
                    {
                        try
                        {
                            // Download the specific file 
                            FileRepository.FileRepository fileRepo = new FileRepository.FileRepository(connectionStrings.FileRepository, appSettings.IconsAndBanners);
                            var blob = fileRepo.GetBlobFile(toDownload.Guid);
                            var ms = new MemoryStream();
                            await blob.DownloadToAsync(ms);
                            ms.Seek(0, SeekOrigin.Begin);
                            return File(ms, System.Net.Mime.MediaTypeNames.Application.Octet, toDownload.Name);
                        }
                        catch (Exception e)
                        {
                            return BadRequest("ERROR: " + e.Message);
                        }
                    }
                    else
                    {
                        return BadRequest("ERROR: The customer has not set this file yet.");
                    }
                }
                else
                {
                    return BadRequest("ERROR: The client is not registered yet.");
                }
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("icon/{fileId}")]
        public async Task<IActionResult> DownloadFileAsync([FromRoute] string fileId)
        {
            try
            {
                FileRepository.FileRepository fileRepo = new FileRepository.FileRepository(connectionStrings.FileRepository, appSettings.FileRepositoryFolder);
                var file = UnitOfWork.Files.Get(fileId);
                var blob = fileRepo.GetBlobFile(file.Guid);
                var ms = new MemoryStream();
                await blob.DownloadToAsync(ms);
                ms.Seek(0, SeekOrigin.Begin);
                return File(ms, System.Net.Mime.MediaTypeNames.Application.Octet, file.Name);
            }
            catch (Exception e)
            {
                return BadRequest("ERROR: " + e.Message);
            }
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("connectionTest")]
        public ActionResult ConnectionTest()
        {
            return Content("alive");
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("getClientCustomer/{uuid}")]
        public IActionResult GetClientCustomer([FromRoute] string uuid, [FromBody] AgentsAuthenticationModel data)
        {
            try
            {
                if (data == null)
                {
                    return BadRequest("ERROR: The data to identify the client is missing");
                }

                using (var unitOfWork = CreateUnitOfWork())
                {
                    DATA.Client fetchedClient = null;
                    var fetchedClients = unitOfWork.Clients.GetAll(ClientIncludes.Customer).Where(x => x.UUID == uuid && x.SerialNumber == data.SerialNumber).ToList();
                    if (fetchedClients == null || fetchedClients.Count() == 0)
                    {
                        return BadRequest("Error: No client list found for uuid and SerialNr");
                    }

                    var fittingClientes = new List<DATA.Client>();

                    // Find to remove clients
                    foreach (DATA.Client client in fetchedClients)
                    {
                        List<MacAddress> macAddresses = new List<MacAddress>();
                        macAddresses = unitOfWork.MacAddresses.GetAll()
                            .Where(x => x.ClientId == client.Id).ToList();
                        if (macAddresses == null || macAddresses.Count == 0)
                        {
                            return BadRequest("Error with fetched mac addresses: " + string.Join(",", macAddresses.Select(x => x.Address).ToArray()) + " " + string.Join(",", fetchedClients.Select(x => x.Id).ToArray()));
                        }
                        for (int i = 0; i < macAddresses.Count; i++)
                        {
                            foreach (string mac in data.MacAddresses)
                            {
                                if (macAddresses.ElementAt(i).Address.ToLower() == mac.ToLower())
                                {
                                    if (!fittingClientes.Contains(client))
                                    {
                                        fittingClientes.Add(client);
                                    }
                                }
                            }
                        }
                    }

                    // Check if a client was identified
                    if (!(fittingClientes.Count() == 0 || fittingClientes.Count() > 1))
                    {
                        fetchedClient = fittingClientes.First();
                    }
                    else
                    {
                        return BadRequest("FittingClientes: " + string.Join(",", fittingClientes.Select(x => x.Id)));
                    }

                    // DATA.Client client = unitOfWork.Clients.GetByUuid(uuid);                
                    if (fetchedClient != null)
                    {
                        if (fetchedClient.Customer == null)
                        {
                            return BadRequest("ERROR: The customer of the client is null: " + fetchedClient.CustomerId);
                        }
                        string result = JsonConvert.SerializeObject(fetchedClient.Customer, serializerSettings);
                        return Ok(result);
                    }
                    else
                    {
                        DATA.Customer dummy = new DATA.Customer();
                        dummy.Name = "Not registered yet";
                        string result = JsonConvert.SerializeObject(dummy, serializerSettings);
                        return Ok(result);
                    }
                }
            }
            catch (Exception e)
            {
                var st = new StackTrace(e, true);
                // Get the top stack frame
                var frame = st.GetFrame(0);
                // Get the line number from the stack frame
                var line = frame.GetFileLineNumber();
                return BadRequest("ERROR: " + e.Message + " " + line + "\n" + JsonConvert.SerializeObject(data));
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("addClient")]
        public IActionResult AddClient([FromBody] ClientAddViewModel ClientAdd)
        {
            // TODO: possible attacks and multiple client creation ALERT!!!!
            if (ClientAdd.uuid == null || ClientAdd.uuid == "")
            {
                return BadRequest("ERROR: The UUID of the client is not set");
            }

            if (ClientAdd.MACAdresses == null || ClientAdd.MACAdresses.Count == 0)
            {
                return BadRequest("ERROR: The MAC Addresses of the client were not sent");
            }

            if (ClientAdd.SerialNumber == null || ClientAdd.SerialNumber == String.Empty)
            {
                return BadRequest("ERROR: The serial number of the client was not sent");
            }

            using (var unitOfWork = CreateUnitOfWork())
            {
                try
                {
                    DATA.Customer cust = unitOfWork.Customers.GetAll().Single(x => x.Name.ToLower().Equals(ClientAdd.CustomerName.ToLower()));
                    // Check for existing client and delete it
                    var fetchedClients = unitOfWork.Clients.GetAll(ClientIncludes.GetAllIncludes()).Where(x => x.UUID == ClientAdd.uuid && x.SerialNumber == ClientAdd.SerialNumber).ToList();
                    var fittingClientes = new List<DATA.Client>();

                    // Find to remove clients
                    foreach (DATA.Client client in fetchedClients)
                    {
                        List<MacAddress> macAddresses = new List<MacAddress>();
                        macAddresses = unitOfWork.MacAddresses.GetAll()
                            .Where(x => x.ClientId == client.Id).ToList();
                        for (int i = 0; i < macAddresses.Count; i++)
                        {
                            foreach (string mac in ClientAdd.MACAdresses)
                            {
                                if (macAddresses.ElementAt(i).Address.ToLower() == mac.ToLower())
                                {
                                    if (!fittingClientes.Contains(client))
                                    {
                                        fittingClientes.Add(client);
                                    }
                                }
                            }
                        }
                    }

                    // Check if a client was identified
                    if (fittingClientes.Count() != 0)
                    {
                        return BadRequest("ERROR: The client already exists");
                    }

                    DATA.Client newClient = new DATA.Client() { Description = ClientAdd.Description, Name = ClientAdd.Name, UUID = ClientAdd.uuid, CustomerId = cust.Id };
                    newClient.Model = ClientAdd.Model;
                    newClient.SerialNumber = ClientAdd.SerialNumber;
                    newClient.Type = ClientAdd.Type;
                    newClient.HyperVisor = ClientAdd.HyperVisor;
                    newClient.Manufacturer = ClientAdd.Manufacturer;
                    newClient.MacAddresses = new List<MacAddress>();
                    if (ClientAdd.MACAdresses != null)
                    {
                        foreach (string macAdress in ClientAdd.MACAdresses)
                        {
                            MacAddress temp = new MacAddress();
                            temp.Address = macAdress;
                            temp.Client = newClient;
                            newClient.MacAddresses.Add(temp);
                        }
                    }

                    /*
                    newClient.PreinstalledSoftwares = new List<DATA.SmartDeploy.PreinstalledSoftware>();
                    if (ClientAdd.InstalledSoftware != null)
                    {
                        foreach (InstalledSoftwareViewModel installedSoftwareData in ClientAdd.InstalledSoftware)
                        {
                            PreinstalledSoftware preSoftware = unitOfWork.PreinstalledSoftwareRepositories.CreateEmpty();
                            preSoftware.Name = installedSoftwareData.DisplayName;
                            preSoftware.Size = installedSoftwareData.Size;
                            preSoftware.Version = installedSoftwareData.Version;
                            preSoftware.InstalledAt = installedSoftwareData.InstalledAt;
                            newClient.PreinstalledSoftwares.Add(preSoftware);
                        }
                    }
                    */
                    unitOfWork.Clients.MarkForInsert(newClient);
                    unitOfWork.SaveChanges();

                    var FixedParams = GenerateFixedClientParams(newClient);
                    foreach (var param in FixedParams)
                    {
                        param.ClientId = newClient.Id;
                        param.Client = newClient;
                        unitOfWork.ClientParameters.MarkForInsert(param);
                        unitOfWork.SaveChanges();
                    }

                    ClientAddViewModel result = new ClientAddViewModel();
                    result.CustomerName = ClientAdd.CustomerName;
                    result.uuid = ClientAdd.uuid;
                    var json = JsonConvert.SerializeObject(result);
                    return new OkObjectResult(json);
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("autoRegisterClient")]
        public IActionResult AddClientAutomatically([FromBody] ClientAddRegisterAuto ClientAdd)
        {
            if (ClientAdd.uuid == null || ClientAdd.uuid == "" ||
                ClientAdd.SerialNumber == null || ClientAdd.SerialNumber == "" ||
                ClientAdd.MacAddresses == null && ClientAdd.MacAddresses.Count() == 0)
            {
                return BadRequest("The sent data is missing data to identify the client");
            }
            using (var unitOfWork = CreateUnitOfWork())
            {
                try
                {
                    DATA.Customer cust = unitOfWork.Customers.GetAll().Where(x => x.Name.ToLower().Equals(ClientAdd.CustomerName.ToLower())).FirstOrDefault();
                    if (cust == null)
                    {
                        return BadRequest("ERROR: The customer does not exist");
                    }
                    if (cust.AutoRegisterPassword != null && DecryptString(cust.AutoRegisterPassword) != ClientAdd.AutoRegisterPassword)
                    {
                        return BadRequest("The password for the registration is not correct or set");
                    }

                    // Check for existing client and delete it
                    DATA.Client fetchedClient = null;
                    var fetchedClients = unitOfWork.Clients.GetAll(ClientIncludes.GetAllIncludes()).Where(x => x.UUID == ClientAdd.uuid && x.SerialNumber == ClientAdd.SerialNumber).ToList();
                    var fittingClientes = new List<DATA.Client>();

                    // Find to remove clients
                    foreach (DATA.Client client in fetchedClients)
                    {
                        List<MacAddress> macAddresses = new List<MacAddress>();
                        macAddresses = unitOfWork.MacAddresses.GetAll()
                            .Where(x => x.ClientId == client.Id).ToList();
                        for (int i = 0; i < macAddresses.Count; i++)
                        {
                            foreach (string mac in ClientAdd.MacAddresses)
                            {
                                if (macAddresses.ElementAt(i).Address.ToLower() == mac.ToLower())
                                {
                                    if (!fittingClientes.Contains(client))
                                    {
                                        fittingClientes.Add(client);
                                    }
                                }
                            }
                        }
                    }

                    // Check if a client was identified
                    if (!(fittingClientes.Count() == 0 || fittingClientes.Count() > 1))
                    {
                        fetchedClient = fittingClientes.First();
                    }

                    if (fetchedClient != null && cust.AutoRegisterClients)
                    {
                        // Delete former ClientClientProperties
                        List<ClientClientProperty> properties = unitOfWork.ClientClientProperties.GetAll().Where(x => x.ClientId == fetchedClient.Id).ToList();
                        foreach (ClientClientProperty property in properties)
                        {
                            unitOfWork.ClientClientProperties.MarkForDelete(property);
                        }

                        // Delete former ClientSoftware
                        foreach (ClientSoftware software in fetchedClient.AssignedSoftware)
                        {
                            unitOfWork.ClientSoftwares.MarkForDelete(software);
                        }

                        // Delete former ActivityLog
                        foreach (ActivityLog log in fetchedClient.ActivityLogs)
                        {
                            unitOfWork.ActivityLogs.MarkForDelete(log);
                        }

                        // Delete former ClientTasks
                        List<DATA.ClientTask> tasks = unitOfWork.ClientTasks.GetAll().Where(x => x.ClientId == fetchedClient.Id).ToList();
                        foreach (DATA.ClientTask task in tasks)
                        {
                            unitOfWork.ClientTasks.MarkForDelete(task);
                        }

                        // Delete ClientParameters
                        List<DATA.ClientParameter> parameters = unitOfWork.ClientParameters.GetAll()
                            .Where(x => x.ClientId == fetchedClient.Id)
                            .ToList();

                        // Delete MacAddresses
                        List<DATA.MacAddress> macAddresses = unitOfWork.MacAddresses.GetAll()
                            .Where(x => x.ClientId == fetchedClient.Id).ToList();
                        foreach (MacAddress mac in macAddresses)
                        {
                            unitOfWork.MacAddresses.MarkForDelete(mac);
                        }

                        foreach (ClientParameter param in parameters)
                        {
                            unitOfWork.ClientParameters.MarkForDelete(param);
                        }

                        List<Inventory> inventories = unitOfWork.Inventories.GetAll().Where(x => x.ClientId == fetchedClient.Id).ToList();
                        foreach (Inventory inventory in inventories)
                        {
                            unitOfWork.Inventories.Delete(inventory);
                        }

                        // Delete all ExecutionLogs
                        List<ExecutionLog> executionLogs = unitOfWork.Logs.GetAll().Where(x => x.ClientId == fetchedClient.Id).ToList();
                        foreach (ExecutionLog log in executionLogs)
                        {
                            unitOfWork.Logs.MarkForDelete(log);
                            unitOfWork.SaveChanges();
                        }

                        unitOfWork.Clients.MarkForDelete(fetchedClient);
                        unitOfWork.SaveChanges();
                    }
                    fetchedClient = unitOfWork.Clients.GetAll("AssignedSoftware", "ActivityLogs").Where(x => x.UUID == ClientAdd.uuid).FirstOrDefault();
                    if (fetchedClient != null)
                    {
                        if (fetchedClient.DeletedByUserId == null)
                        {
                            return BadRequest("ERROR: The client already exists");
                        }
                    }

                    DATA.Client newClient = new DATA.Client() { Description = ClientAdd.Description, Name = ClientAdd.Name, UUID = ClientAdd.uuid, CustomerId = cust.Id };
                    newClient.Model = ClientAdd.Model;
                    newClient.SerialNumber = ClientAdd.SerialNumber;
                    newClient.Type = ClientAdd.Type;
                    newClient.HyperVisor = ClientAdd.HyperVisor;
                    newClient.Manufacturer = ClientAdd.Manufacturer;
                    newClient.MacAddresses = new List<MacAddress>();
                    newClient.OSEdition = ClientAdd.OSEdition;
                    newClient.OSMemorySize = ClientAdd.OSMemorySize;
                    newClient.OSOperatingSystemSKU = ClientAdd.OSOperatingSystemSKU;
                    newClient.OSArchitecture = ClientAdd.OSArchitecture;
                    newClient.CSPvendor = ClientAdd.CSPvendor;
                    newClient.OSInstallDateUTC = ClientAdd.OSInstallDateUTC;
                    newClient.OSType = ClientAdd.OSType;
                    newClient.OSVersion = ClientAdd.OSVersion;
                    newClient.ModelSeries = ClientAdd.ModelSeries;
                    newClient.CSPversion = ClientAdd.CSPversion;
                    newClient.CSPname = ClientAdd.CSPname;
                    newClient.OSLanguage = ClientAdd.OSLanguage;
                    newClient.OSProductSuite = ClientAdd.OSProductSuite;
                    newClient.Processor = ClientAdd.Processor;
                    newClient.MainFrequentUser = ClientAdd.MainFrequentUser;

                    if (ClientAdd.MACAdresses != null)
                    {
                        foreach (string macAdress in ClientAdd.MACAdresses)
                        {
                            MacAddress temp = new MacAddress();
                            temp.Address = macAdress;
                            temp.Client = newClient;
                            newClient.MacAddresses.Add(temp);
                        }
                    }
                    /*
                    newClient.PreinstalledSoftwares = new List<DATA.SmartDeploy.PreinstalledSoftware>();
                    if (ClientAdd.InstalledSoftware != null)
                    {
                        foreach (InstalledSoftwareViewModel installedSoftwareData in ClientAdd.InstalledSoftware)
                        {
                            PreinstalledSoftware preSoftware = unitOfWork.PreinstalledSoftwareRepositories.CreateEmpty();
                            preSoftware.Name = installedSoftwareData.DisplayName;
                            preSoftware.Size = installedSoftwareData.Size;
                            preSoftware.Version = installedSoftwareData.Version;
                            preSoftware.InstalledAt = installedSoftwareData.InstalledAt;
                            newClient.PreinstalledSoftwares.Add(preSoftware);
                        }
                    }
                    */
                    unitOfWork.Clients.MarkForInsert(newClient);
                    unitOfWork.SaveChanges();

                    try
                    {
                        var FixedParams = GenerateFixedClientParams(newClient);
                        foreach (var param in FixedParams)
                        {
                            param.ClientId = newClient.Id;
                            param.Client = newClient;
                            unitOfWork.ClientParameters.MarkForInsert(param);
                            unitOfWork.SaveChanges();
                        }

                        ClientAddViewModel result = new ClientAddViewModel();
                        result.CustomerName = ClientAdd.CustomerName;
                        result.uuid = ClientAdd.uuid;
                        var json = JsonConvert.SerializeObject(result);
                        return new OkObjectResult(json);
                    }
                    catch (Exception exc)
                    {
                        return BadRequest("ERROR AFTER SAVING NEW CLIENT");
                    }
                }
                catch (Exception e)
                {
                    string innerExceptionMessage = "";
                    var innerException = e.InnerException;
                    while (innerException != null)
                    {
                        innerExceptionMessage += " " + innerException.Message;
                        if (innerException.InnerException != null)
                        {
                            innerException = innerException.InnerException;
                        }
                        else
                        {
                            innerException = null;
                        }
                    }
                    return BadRequest(e.Message + " " + innerExceptionMessage);
                }
            }
        }

        private List<DATA.ClientParameter> GenerateFixedClientParams(DATA.Client client)
        {
            var VM = UnitOfWork.VirtualMachines.GetAll().FirstOrDefault(x => x.Name == client.Name);
            var FixedParams = new List<DATA.ClientParameter>();
            FixedParams.Add(new DATA.ClientParameter() { ParameterName = "$ComputerTargetName", Value = client.Name, IsEditable = false });
            FixedParams.Add(new DATA.ClientParameter() { ParameterName = "$LocalAdminName", Value = (VM != null ? VM.AdminUserName : null), IsEditable = true });
            FixedParams.Add(new DATA.ClientParameter() { ParameterName = "$LocalAdminPw", Value = (VM != null ? VM.AdminUserPassword : null), IsEditable = true });
            FixedParams.Add(new DATA.ClientParameter() { ParameterName = "$SubnetName", Value = (VM != null ? VM.Subnet : client.Subnet), IsEditable = false });
            FixedParams.Add(new DATA.ClientParameter() { ParameterName = "$DataDriveLetter", Value = null, IsEditable = true });
            return FixedParams;

        }

        [AllowAnonymous]
        [HttpGet]
        [Route("{uuid}")]
        public IActionResult CheckUUIDAndForget([FromRoute] string UUID)
        {
            DATA.Client dbClient = null;
            dbClient = UnitOfWork.Clients.GetByUuid(UUID, Data.Models.ClientIncludes.GetAllIncludes());
            if (dbClient == null)
            {
                return new NotFoundResult();
            }
            return Ok();
        }
        //[AllowAnonymous]
        //[HttpGet]
        //[Route("upload")]
        //public ActionResult GetTaskList()
        //{
        //    return View();
        //}

        [AllowAnonymous]
        [HttpPost]
        [Route("getTaskSWList/{uuid}/{serviceOrClient}")]
        public IActionResult GetSWTaskList([FromRoute] string serviceOrClient, [FromRoute] string uuid, [FromBody] AgentsAuthenticationSWApplicabilityModel data)
        {
            bool isServer = false;
            WindowsVersions windowsVersions = null;
            WindowsVersion osInformation = null;
            var assembly = typeof(Program).GetTypeInfo().Assembly;
            using (Stream stream = assembly.GetManifestResourceStream("WPM_API.Resources.WindowsVersions.json"))
            {
                using (StreamReader sr = new StreamReader(stream))
                {
                    string jsonContent = sr.ReadToEnd();
                    windowsVersions = JsonConvert.DeserializeObject<WindowsVersions>(jsonContent, serializerSettings);
                }
            }

            if (windowsVersions == null)
            {
                return BadRequest("ERROR: Could not load Windows Versions comparison table");
            }

            if (data.ClientOrServer == "Client")
            {
                osInformation = windowsVersions.VersionsClient.Find(x => x.BuildNr == data.BuildNr);
            }
            else
            {
                osInformation = windowsVersions.VersionsServer.Find(x => x.BuildNr == data.BuildNr);
                isServer = true;
            }

            if (osInformation == null)
            {
                return BadRequest("ERROR: Could not load the clients OS information");
            }

            string runningContext;
            if (serviceOrClient == "service")
            {
                runningContext = "Run in service";
            }
            else
            {
                runningContext = "Run in user context";
            }
            using (var unitOfWork = CreateUnitOfWork())
            {
                try
                {
                    List<WPM_API.TransferModels.SmartDeploy.ClientTaskWithDetection> result = new List<WPM_API.TransferModels.SmartDeploy.ClientTaskWithDetection>();

                    // TODO: Fix identifying client with SerialNr and MacAddress
                    DATA.Client fetchedClient = null;
                    List<DATA.Client> fetchedClients = unitOfWork.Clients.GetAll(ClientIncludes.GetAllIncludes()).Where(x => x.UUID == uuid && x.SerialNumber == data.SerialNumber).ToList();
                    var fittingClientes = new List<DATA.Client>();

                    // Find to remove clients
                    foreach (DATA.Client client in fetchedClients)
                    {
                        List<MacAddress> macAddresses = new List<MacAddress>();
                        macAddresses = unitOfWork.MacAddresses.GetAll()
                            .Where(x => x.ClientId == client.Id).ToList();
                        for (int i = 0; i < macAddresses.Count; i++)
                        {
                            foreach (string mac in data.MacAddresses)
                            {
                                if (macAddresses.ElementAt(i).Address.ToLower() == mac.ToLower())
                                {
                                    if (!fittingClientes.Contains(client))
                                    {
                                        fittingClientes.Add(client);
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    // Check if a client was identified
                    if (!(fittingClientes.Count() == 0 || fittingClientes.Count() > 1))
                    {
                        fetchedClient = fittingClientes.First();
                    }

                    if (fetchedClient == null)
                    {
                        return BadRequest("ERROR: The client could not be found");
                    }

                    var tasks = unitOfWork.ClientTasks.GetAll("Task", "Task.Files", "Task.ExecutionFile")
                        .Where(x => x.ClientId == fetchedClient.Id && x.Status != "executed" && x.Type == "software" && x.Task.RunningContext == runningContext).ToList();
                    foreach (DATA.ClientTask t in tasks)
                    {
                        if (t != null)
                        {
                            CustomerSoftware cs = unitOfWork.CustomerSoftwares.GetAll("RuleApplicability.Architecture", "RuleApplicability.OsVersionNames", "RuleApplicability.Win10Versions", "TaskInstall", "RuleApplicability.Win11Versions").Where(x => x.TaskInstall.Id == t.TaskId).FirstOrDefault();
                            // TODO: Check applicability rule with sent data ()
                            if (CheckApplicability(osInformation, cs, Mapper.Map<RuleViewModel>(cs.RuleApplicability), data.Is64Bit, isServer))
                            {
                                result.Add(ConvertToClientTaskDetectionRule(t));
                            }
                        }
                    }
                    string json = JsonConvert.SerializeObject(result, serializerSettings);
                    return new OkObjectResult(json);
                }
                catch (Exception ex)
                {
                    // TODO: Add line for exception
                    var st = new StackTrace(ex, true);
                    // Get the top stack frame
                    var frame = st.GetFrame(0);
                    // Get the line number from the stack frame
                    var line = frame.GetFileLineNumber();
                    return new BadRequestObjectResult("Exception: " + ex.Message + "\n" + line);
                }
            }
        }

        [HttpPost]
        [Route("validateOS")]
        [AllowAnonymous]
        public IActionResult ValidateOS([FromBody] AgentsAuthenticationSWApplicabilityModel data)
        {
            WindowsVersions windowsVersions = null;
            WindowsVersion osInformation = null;
            var assembly = typeof(Program).GetTypeInfo().Assembly;
            using (Stream stream = assembly.GetManifestResourceStream("WPM_API.Resources.WindowsVersions.json"))
            {
                using (StreamReader sr = new StreamReader(stream))
                {
                    string jsonContent = sr.ReadToEnd();
                    windowsVersions = JsonConvert.DeserializeObject<WindowsVersions>(jsonContent, serializerSettings);
                }
            }

            if (windowsVersions == null)
            {
                return BadRequest("ERROR: Could not load Windows Versions comparison table");
            }

            if (data.ClientOrServer == "Client")
            {
                osInformation = windowsVersions.VersionsClient.Find(x => x.BuildNr == data.BuildNr);
            }
            else
            {
                osInformation = windowsVersions.VersionsServer.Find(x => x.BuildNr == data.BuildNr);
            }

            if (osInformation == null)
            {
                return BadRequest();
            }
            else
            {
                return Ok("OS is valid");
            }
        }

        private bool CheckApplicability(WindowsVersion osInformation, CustomerSoftware cs, RuleViewModel applicabilityRule, bool is64Bit, bool isServer)
        {
            if (is64Bit)
            {
                if (!applicabilityRule.Architecture.Contains("64bit"))
                {
                    return false;
                }
            }
            else
            {
                if (!applicabilityRule.Architecture.Contains("32bit"))
                {
                    return false;
                }
            }
            if (isServer)
            {
                if (!applicabilityRule.OsVersionNames.Contains(osInformation.Name))
                {
                    return false;
                }
            }
            if (applicabilityRule.OsVersionNames.Contains(osInformation.Name))
            {
                if (osInformation.Name == "Win 10")
                {
                    if (!cs.AllWin10Versions)
                    {
                        if (!applicabilityRule.Win10Versions.Contains(osInformation.ReleaseId))
                        {
                            return false;
                        }
                    }
                }
                else if (osInformation.Name == "Win 11")
                {
                    if (!cs.AllWin11Versions)
                    {
                        if (!applicabilityRule.Win11Versions.Contains(osInformation.ReleaseId))
                        {
                            return false;
                        }
                    }
                }
            }
            else
            {
                return false;
            }
            return true;
        }

        [HttpPost]
        [Route("getTaskList/{uuid}/{serviceOrClient}")]
        [AllowAnonymous]
        public IActionResult GetTaskList([FromRoute] string uuid, [FromRoute] string serviceOrClient, [FromBody] AgentsAuthenticationModel data)
        {
            string runningContext;
            if (serviceOrClient == "service")
            {
                runningContext = "Run in service";
            }
            else
            {
                runningContext = "Run in user context";
            }
            using (var unitOfWork = CreateUnitOfWork())
            {
                try
                {
                    DATA.Client fetchedClient = null;
                    var fetchedClients = unitOfWork.Clients.GetAll(ClientIncludes.GetAllIncludes()).Where(x => x.UUID == uuid && x.SerialNumber == data.SerialNumber).ToList();
                    var fittingClientes = new List<DATA.Client>();

                    // Find to remove clients
                    foreach (DATA.Client client in fetchedClients)
                    {
                        List<MacAddress> macAddresses = new List<MacAddress>();
                        macAddresses = unitOfWork.MacAddresses.GetAll()
                            .Where(x => x.ClientId == client.Id).ToList();
                        for (int i = 0; i < macAddresses.Count; i++)
                        {
                            foreach (string mac in data.MacAddresses)
                            {
                                if (macAddresses.ElementAt(i).Address.ToLower() == mac.ToLower())
                                {
                                    if (!fittingClientes.Contains(client))
                                    {
                                        fittingClientes.Add(client);
                                    }
                                }
                            }
                        }
                    }

                    // Check if a client was identified
                    if (!(fittingClientes.Count() == 0 || fittingClientes.Count() > 1))
                    {
                        fetchedClient = fittingClientes.First();
                    }

                    if (fetchedClient == null)
                    {
                        return BadRequest("ERROR: The client could not be found");
                    }
                    var tasks = unitOfWork.ClientTasks.GetAll("Task", "Task.Files", "Task.ExecutionFile")
                        .Where(x => x.ClientId == fetchedClient.Id && x.Status != "executed" && x.Type == "task" && x.Task.RunningContext == runningContext).ToList();
                    List<WPM_API.TransferModels.SmartDeploy.ClientTask> result = new List<WPM_API.TransferModels.SmartDeploy.ClientTask>();
                    foreach (DATA.ClientTask t in tasks)
                    {
                        if (t != null)
                        {
                            result.Add(ConvertToClientTask(t));
                        }
                    }
                    string json = JsonConvert.SerializeObject(result, serializerSettings);
                    return new OkObjectResult(json);
                }
                catch (Exception e)
                {
                    var st = new StackTrace(e, true);
                    // Get the top stack frame
                    var frame = st.GetFrame(0);
                    // Get the line number from the stack frame
                    var line = frame.GetFileLineNumber();
                    return BadRequest("ERROR: " + e.Message + " " + line);
                }
            }
        }

        private WPM_API.TransferModels.SmartDeploy.ClientTask ConvertToClientTask(DATA.ClientTask c)
        {
            List<DATA.Software> sw = UnitOfWork.Software.GetAll().Where(x => x.TaskInstall.Id == c.TaskId).ToList();
            WPM_API.TransferModels.SmartDeploy.ClientTask tempTask = new WPM_API.TransferModels.SmartDeploy.ClientTask()
            {
                clienttask_id = c.Id,
                status_status = c.Status,
                task_commandline = c.Task.CommandLine,
                task_executionfilename = c.Task.ExecutionFile.Name,
                task_uid = c.Task.Id,
                task_useshellexecute = c.Task.UseShellExecute.ToString(),
                task_filenames = new List<string>(),
                versionNr = c.Task.VersionNr,
                checkVersionNr = c.Task.CheckVersionNr,
                exePath = c.Task.ExePath,
                executionContext = c.Task.ExecutionContext,
                visibility = c.Task.Visibility,
                restartRequired = c.Task.RestartRequired,
                Name = c.Task.Name,
                Checksum = sw.First().Checksum
            };
            foreach (DATA.File file in c.Task.Files)
            {
                tempTask.task_filenames.Add(file.Name);
            }
            return tempTask;
        }

        private WPM_API.TransferModels.SmartDeploy.ClientTaskWithDetection ConvertToClientTaskDetectionRule(DATA.ClientTask c)
        {
            List<DATA.Software> sw = UnitOfWork.Software.GetAll("RuleDetection.Type", "RuleApplicability").Where(x => x.TaskInstall.Id == c.TaskId).ToList();
            WPM_API.TransferModels.SmartDeploy.ClientTaskWithDetection tempTask = new WPM_API.TransferModels.SmartDeploy.ClientTaskWithDetection()
            {
                clienttask_id = c.Id,
                status_status = c.Status,
                task_commandline = c.Task.CommandLine,
                task_executionfilename = c.Task.ExecutionFile.Name,
                task_uid = c.Task.Id,
                task_useshellexecute = c.Task.UseShellExecute.ToString(),
                task_filenames = new List<string>(),
                versionNr = sw.First().RuleApplicability.VersionNr,
                checkVersionNr = c.Task.CheckVersionNr,
                exePath = c.Task.ExePath,
                executionContext = c.Task.ExecutionContext,
                visibility = c.Task.Visibility,
                restartRequired = c.Task.RestartRequired,
                Name = c.Task.Name,
                Checksum = sw.First().Checksum,
                DetectionRule = Mapper.Map<RuleViewModel>(sw.First().RuleDetection)
            };
            if (tempTask.versionNr == null)
            {
                tempTask.versionNr = sw.First().RuleDetection.VersionNr;
            }
            foreach (DATA.File file in c.Task.Files)
            {
                tempTask.task_filenames.Add(file.Name);
            }
            return tempTask;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("getClientSoftwareList/{clientOrService}/{uuid}")]
        public IActionResult GetClientSoftwareList([FromRoute] string clientOrService, [FromRoute] string uuid, [FromBody] AgentsAuthenticationSWApplicabilityModel data)
        {
            bool isServer = false;
            WindowsVersions windowsVersions = null;
            WindowsVersion osInformation = null;
            try
            {
                var assembly = typeof(Program).GetTypeInfo().Assembly;
                using (Stream stream = assembly.GetManifestResourceStream("WPM_API.Resources.WindowsVersions.json"))
                {
                    using (StreamReader sr = new StreamReader(stream))
                    {
                        string jsonContent = sr.ReadToEnd();
                        windowsVersions = JsonConvert.DeserializeObject<WindowsVersions>(jsonContent, serializerSettings);
                    }
                }

                if (windowsVersions == null)
                {
                    return BadRequest("ERROR: Could not load Windows Versions comparison table");
                }

                if (data.ClientOrServer == "Client")
                {
                    osInformation = windowsVersions.VersionsClient.Find(x => x.BuildNr == data.BuildNr);
                }
                else
                {
                    osInformation = windowsVersions.VersionsServer.Find(x => x.BuildNr == data.BuildNr);
                    isServer = true;
                }

                if (osInformation == null)
                {
                    return BadRequest("ERROR: Could not load the clients OS information");
                }
            }
            catch (Exception e)
            {
                var st = new StackTrace(e, true);
                // Get the top stack frame
                var frame = st.GetFrame(0);
                // Get the line number from the stack frame
                var line = frame.GetFileLineNumber();
                string innerException = String.Empty;
                if (e.InnerException != null)
                {
                    innerException = e.InnerException.Message;
                }
                return BadRequest("ERROR: Could not get information of the specific OS" + "\n" + innerException + "\n" + line);
            }

            string runningContext = null;
            if (clientOrService == "service")
            {
                runningContext = "Run in service";
            }
            else if (clientOrService == "app")
            {
                runningContext = "Run in user context";
            }
            using (var unitOfWork = CreateUnitOfWork())
            {
                try
                {
                    DATA.Client fetchedClient = null;
                    var fetchedClients = unitOfWork.Clients.GetAll(ClientIncludes.GetAllIncludes()).Where(x => x.UUID == uuid && x.SerialNumber == data.SerialNumber).ToList();
                    var fittingClientes = new List<DATA.Client>();

                    // Find to remove clients
                    foreach (DATA.Client client in fetchedClients)
                    {
                        List<MacAddress> macAddresses = new List<MacAddress>();
                        macAddresses = unitOfWork.MacAddresses.GetAll()
                            .Where(x => x.ClientId == client.Id).ToList();
                        for (int i = 0; i < macAddresses.Count; i++)
                        {
                            foreach (string mac in data.MacAddresses)
                            {
                                if (macAddresses.ElementAt(i).Address.ToLower() == mac.ToLower())
                                {
                                    if (!fittingClientes.Contains(client))
                                    {
                                        fittingClientes.Add(client);
                                    }
                                }
                            }
                        }
                    }

                    // Check if a client was identified
                    if (!(fittingClientes.Count() == 0 || fittingClientes.Count() > 1))
                    {
                        fetchedClient = fittingClientes.First();
                    }

                    if (fetchedClient == null)
                    {
                        return BadRequest("ERROR: The client could not be found");
                    }
                    List<WPM_API.Data.DataContext.Entities.CustomerSoftwareStream> customerSoftwareStreams = unitOfWork.CustomerSoftwareStreamss.GetAll("StreamMembers").Where(x => x.CustomerId == fetchedClient.CustomerId).ToList();
                    List<WPM_API.Data.DataContext.Entities.CustomerSoftware> customerSoftwares = new List<DATA.CustomerSoftware>();
                    foreach (WPM_API.Data.DataContext.Entities.CustomerSoftwareStream stream in customerSoftwareStreams)
                    {
                        foreach (WPM_API.Data.DataContext.Entities.CustomerSoftware software in stream.StreamMembers)
                        {
                            if (software.Type == "Software" && software.CustomerStatus != "Outdated")
                            {
                                var temp = unitOfWork.CustomerSoftwares.Get(software.Id,
                                    "RuleDetection",
                                    "RuleDetection.Type",
                                    "RuleDetection.Data",
                                    "RuleDetection.Architecture",
                                    "RuleApplicability",
                                    "RuleApplicability.Type",
                                    "RuleApplicability.Data",
                                    "TaskInstall",
                                    "TaskUninstall",
                                    "TaskUpdate",
                                    "TaskInstall.Files",
                                    "TaskUninstall.Files",
                                    "TaskUpdate.Files",
                                    "TaskInstall.ExecutionFile",
                                    "TaskUninstall.ExecutionFile",
                                    "TaskUpdate.ExecutionFile",
                                    "RuleApplicability.Architecture",
                                    "RuleApplicability.OsVersionNames",
                                    "RuleApplicability.Win10Versions",
                                    "RuleApplicability.Win11Versions"
                                    );
                                if (CheckApplicability(osInformation, temp, Mapper.Map<RuleViewModel>(temp.RuleApplicability), data.Is64Bit, isServer))
                                {
                                    customerSoftwares.Add(temp);
                                }
                            }
                        }
                    }

                    string customerId = fetchedClient.CustomerId;
                    string syshouseId = fetchedClient.Customer.SystemhouseId;

                    List<SoftwareClientViewModel> result = new List<SoftwareClientViewModel>();
                    foreach (CustomerSoftware cs in customerSoftwares)
                    {
                        DATA.Software origin = unitOfWork.Software.Get(cs.SoftwareId);
                        var temp = Mapper.Map<SoftwareClientViewModel>(cs);
                        temp.AllWin10Versions = origin.AllWin10Versions;
                        result.Add(temp);
                    }

                    foreach (SoftwareClientViewModel s in result)
                    {
                        if (s.Required == null)
                        {
                            s.Required = false;
                        }
                    }
                    if (fetchedClient != null)
                    {
                        List<DATA.ClientSoftware> clientSoftware = fetchedClient.AssignedSoftware;
                        // Check required value for ClientSoftware entries
                        if (clientSoftware != null)
                        {
                            foreach (DATA.ClientSoftware cs in clientSoftware)
                            {
                                var tempSoftware = result.Find(x => x.Id == cs.CustomerSoftware.Id);
                                if (tempSoftware != null)
                                {
                                    tempSoftware.Required = cs.Install;
                                }
                            }
                        }
                    }
                    var json = JsonConvert.SerializeObject(result, serializerSettings);
                    return new OkObjectResult(json);
                }
                catch (Exception ex)
                {
                    var st = new StackTrace(ex, true);
                    // Get the top stack frame
                    var frame = st.GetFrame(0);
                    // Get the line number from the stack frame
                    var line = frame.GetFileLineNumber();
                    string innerException = String.Empty;
                    if (ex.InnerException != null)
                    {
                        innerException = ex.InnerException.Message;
                    }
                    return new BadRequestObjectResult("Exception: " + ex.Message + "\n" + innerException + "\n" + line);
                }
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("getSWTaskList/{uuid}")]
        public IActionResult GetClientSWTaskList([FromBody] AgentsAuthenticationModel data, [FromRoute] string uuid)
        {
            using (var unitOfWork = CreateUnitOfWork())
            {
                DATA.Client fetchedClient = null;
                List<DATA.Client> fetchedClients = unitOfWork.Clients.GetAll(ClientIncludes.GetAllIncludes()).Where(x => x.UUID == uuid && x.SerialNumber == data.SerialNumber).ToList();
                var fittingClientes = new List<DATA.Client>();

                // Find to remove clients
                foreach (DATA.Client client in fetchedClients)
                {
                    List<MacAddress> macAddresses = new List<MacAddress>();
                    macAddresses = unitOfWork.MacAddresses.GetAll()
                        .Where(x => x.ClientId == client.Id).ToList();
                    for (int i = 0; i < macAddresses.Count; i++)
                    {
                        foreach (string mac in data.MacAddresses)
                        {
                            if (macAddresses.ElementAt(i).Address.ToLower() == mac.ToLower())
                            {
                                if (!fittingClientes.Contains(client))
                                {
                                    fittingClientes.Add(client);
                                    break;
                                }
                            }
                        }
                    }
                }

                // Check if a client was identified
                if (!(fittingClientes.Count() == 0 || fittingClientes.Count() > 1))
                {
                    fetchedClient = fittingClientes.First();
                }

                if (fetchedClient == null)
                {
                    return BadRequest("ERROR: The client could not be found");
                }

                string customerId = fetchedClient.CustomerId;
                string syshouseId = fetchedClient.Customer.SystemhouseId;
                List<CustomerSoftwareStream> streams = UnitOfWork.CustomerSoftwareStreamss.GetAll("StreamMembers").Where(x => x.CustomerId == fetchedClient.CustomerId).ToList();
                List<WPM_API.Data.DataContext.Entities.CustomerSoftware> customerSoftwares = new List<DATA.CustomerSoftware>();
                foreach (CustomerSoftwareStream stream in streams)
                {
                    foreach (CustomerSoftware sw in stream.StreamMembers)
                    {
                        if (sw.Type == "Task" && sw.CustomerStatus != "Outdated")
                        {
                            customerSoftwares.Add(unitOfWork.CustomerSoftwares.Get(sw.Id,
                                        "RuleDetection",
                                        "RuleDetection.Type",
                                        "RuleDetection.Data",
                                        "RuleDetection.Architecture",
                                        "RuleApplicability",
                                        "RuleApplicability.Type",
                                        "RuleApplicability.Data",
                                        "TaskInstall",
                                        "TaskUninstall",
                                        "TaskUpdate",
                                        "TaskInstall.Files",
                                        "TaskUninstall.Files",
                                        "TaskUpdate.Files",
                                        "TaskInstall.ExecutionFile",
                                        "TaskUninstall.ExecutionFile",
                                        "TaskUpdate.ExecutionFile",
                                        "RuleApplicability.Architecture",
                                        "RuleApplicability.OsVersionNames",
                                        "RuleApplicability.Win10Versions",
                                        "RuleApplicability.Win11Versions"
                                        ));
                        }
                    }
                }

                List<SoftwareClientViewModel> result = new List<SoftwareClientViewModel>();
                result = Mapper.Map<List<SoftwareClientViewModel>>(customerSoftwares);

                var json = JsonConvert.SerializeObject(result, serializerSettings);
                return new OkObjectResult(json);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("getSoftwareList")]
        public IActionResult GetSoftwareList([FromForm] IFormCollection _form)
        {
            try
            {
                string _sn = Request.Form["_serialnumber"];
                var software = UnitOfWork.Software.GetAll(Data.Models.ServerIncludes.GetTaskAndRuleIncludes());
                var json = JsonConvert.SerializeObject(Mapper.Map<List<SoftwareViewModel>>(software), serializerSettings);
                return new OkObjectResult(json);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult("Exception: " + ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("getRuleDetails")]
        public IActionResult GetRuleDetails([FromForm] IFormCollection _form)
        {
            try
            {
                string _rule_id = Request.Form["_rule_id"];
                var rule = UnitOfWork.Rules.Get(_rule_id, "Type", "Data");
                var json = JsonConvert.SerializeObject(Mapper.Map<RuleViewModel>(rule), serializerSettings);
                return new OkObjectResult(json);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult("Exception: " + ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("getTaskDetails")]
        public IActionResult GetTaskDetails([FromForm] IFormCollection _form)
        {
            try
            {
                string _sn = Request.Form["_serialnumber"];
                string _tuid = Request.Form["_task_uid"];
                var task = UnitOfWork.Clients.Get(_sn, "Tasks", "Tasks.Task").Tasks.Find(x => x.TaskId.Equals(_tuid)).Task;
                var json = JsonConvert.SerializeObject(Mapper.Map<TaskViewModel>(task), serializerSettings);
                return new OkObjectResult(json);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult("Exception: " + ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("getFile/{uuid}/{fileId}")]
        public async Task<IActionResult> GetFileAsync([FromBody] AgentsAuthenticationModel data, [FromRoute] string uuid, [FromRoute] string fileId)
        {
            try
            {
                using (var unitOfWork = CreateUnitOfWork())
                {
                    DATA.Client fetchedClient = null;
                    var fetchedClients = unitOfWork.Clients.GetAll(ClientIncludes.GetAllIncludes()).Where(x => x.UUID == uuid && x.SerialNumber == data.SerialNumber).ToList();
                    var fittingClientes = new List<DATA.Client>();

                    // Find to remove clients
                    foreach (DATA.Client client in fetchedClients)
                    {
                        List<MacAddress> macAddresses = new List<MacAddress>();
                        macAddresses = unitOfWork.MacAddresses.GetAll()
                            .Where(x => x.ClientId == client.Id).ToList();
                        for (int i = 0; i < macAddresses.Count; i++)
                        {
                            foreach (string mac in data.MacAddresses)
                            {
                                if (macAddresses.ElementAt(i).Address.ToLower() == mac.ToLower())
                                {
                                    if (!fittingClientes.Contains(client))
                                    {
                                        fittingClientes.Add(client);
                                    }
                                }
                            }
                        }
                    }

                    // Check if a client was identified
                    if (!(fittingClientes.Count() == 0 || fittingClientes.Count() > 1))
                    {
                        fetchedClient = fittingClientes.First();
                    }

                    if (fetchedClient == null)
                    {
                        return BadRequest("ERROR: The client could not be found");
                    }

                    // TODO: Fix identifying client
                    WPM_API.Azure.AzureCommunicationService azure;
                    if (fetchedClient.Customer == null)
                    {
                        return BadRequest("ERROR: The customer is null");
                    }
                    StorageEntryPoint csdp = fetchedClient.Customer.StorageEntryPoints.Find(x => x.IsCSDP == true);
                    if (csdp == null)
                    {
                        return null;
                    }

                    if (csdp.Managed)
                    {
                        // TODO: chceck system; fix for livesystem
                        azure = new Azure.AzureCommunicationService(appSettings.DevelopmentTenantId, appSettings.DevelopmentClientId, appSettings.DevelopmentClientSecret);
                    }
                    else
                    {
                        // Load file from customer csdp
                        var cep = GetCEP(fetchedClient.Customer.Id);
                        if (cep == null)
                        {
                            return BadRequest("ERROR: The Cloud Entry Point is not set");
                        }
                        // Get files from Azure
                        azure = new Azure.AzureCommunicationService(cep.TenantId, cep.ClientId, cep.ClientSecret);
                    }

                    string connectionString = azure.StorageService().GetStorageAccConnectionString(csdp.SubscriptionId, csdp.ResourceGrpName, csdp.StorageAccount);
                    FileRepository.FileRepository csdpCustomer = new FileRepository.FileRepository(connectionString, "csdp");
                    var file = UnitOfWork.Files.Get(fileId);
                    string sasUri = csdpCustomer.GetSASFile(file.Guid, true);
                    if (sasUri == null)
                    {
                        return BadRequest("ERROR: The SAS Uri could not be fetched");
                    }
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(sasUri);
                    request.Method = "GET";
                    WebResponse response = request.GetResponse();
                    return File(response.GetResponseStream(), System.Net.Mime.MediaTypeNames.Application.Octet, file.Name);
                }
            }
            catch (Exception e)
            {
                var innerEx = "";
                if (e.InnerException != null)
                {
                    innerEx = e.InnerException.Message;
                }
                return BadRequest("ERROR: " + e.Message + " Inner Exception: " + innerEx);
            }
        }

        // MAYBE OBSOLETE
        [AllowAnonymous]
        [HttpPost]
        [Route("downloadFile")]
        public async Task<MemoryStream> GetFile([FromForm] IFormCollection _form)
        {
            string _fn = Request.Form["_filepath"]; ;
            string _fp = Request.Form["_filename"]; ;
            try
            {
                SmartDeployRepository storage = new SmartDeployRepository(connectionStrings.FileRepository, appSettings.SmartDeploySources);
                var cf = storage.DownloadFile(_fn, _fp);
                var ms = new System.IO.MemoryStream();
                await cf.DownloadToAsync(ms);
                ms.Seek(0, System.IO.SeekOrigin.Begin);
                return ms;
            }
            catch (Exception)
            {
                return null;
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("getSoftwareIcon")]
        public FileResult GetSoftwareIcon([FromForm] IFormCollection _form)
        {
            try
            {
                string _fp = "sw_icons";
                string _sw_id = Request.Form["_sw_id"];
                string _fn = _sw_id + ".ico";
                SmartDeployRepository storage = new SmartDeployRepository(connectionStrings.FileRepository, appSettings.SmartDeploySources);
                var cf = storage.DownloadFile(_fp, _fn);
                var ms = new MemoryStream();
                cf.DownloadToAsync(ms);
                ms.Seek(0, SeekOrigin.Begin);
                return File(ms, System.Net.Mime.MediaTypeNames.Application.Octet, _fn);
            }
            catch (Exception)
            {
                return null;
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("getFileList")]
        public IActionResult GetFileList([FromForm] IFormCollection _form)
        {
            try
            {
                string _tuid = Request.Form["_task_uid"];
                var fileList = UnitOfWork.Tasks.Get(_tuid, "Files").Files;
                var json = JsonConvert.SerializeObject(Mapper.Map<List<FileRefViewModel>>(fileList), serializerSettings);
                return new OkObjectResult(json);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult("Exception: " + ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("setTaskStatus")]
        public IActionResult SetTaskStatus([FromForm] IFormCollection _form)
        {
            try
            {
                //string _sn = nvc.Get("_serialnumber");
                string _ctid = Request.Form["_clienttask_id"];
                string _status = Request.Form["_status"];
                // string _message = Request.Form["_message"];

                string statusId = string.Empty;
                using (var unitOfWork = CreateUnitOfWork())
                {
                    DATA.ClientTask ct = unitOfWork.ClientTasks.GetAll().Where(x => x.Id == _ctid).FirstOrDefault();
                    if (ct != null)
                    {
                        ct.Status = _status;
                        unitOfWork.SaveChanges();
                    }
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult("Exception: " + ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("assignTask")]
        public IActionResult AssignTask([FromForm] IFormCollection _form)
        {
            try
            {
                string _tid = Request.Form["_task_id"];
                string _cuid = Request.Form["_client_uid"];

                var Client = UnitOfWork.Clients.GetByUuid(_cuid);
                DATA.ClientTask ct = new DATA.ClientTask() { ClientId = Client.Id, TaskId = _tid };
                ct.Status = "assigned";
                ct.Type = "software";
                Client.Tasks.Add(ct);
                UnitOfWork.Clients.MarkForUpdate(Client);
                UnitOfWork.SaveChanges();
                return new OkResult();
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult("Exception: " + ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("assignSoftware")]
        public IActionResult AssignSoftware([FromForm] IFormCollection _form)
        {
            try
            {
                string _sw_id = Request.Form["_sw_id"];
                string _cuid = Request.Form["_client_uid"];

                var Client = UnitOfWork.Clients.GetByUuid(_cuid);
                DATA.ClientSoftware cs = new DATA.ClientSoftware() { ClientId = Client.Id, CustomerSoftwareId = _sw_id };

                if (Client.AssignedSoftware == null)
                {
                    Client.AssignedSoftware = new List<DATA.ClientSoftware>();
                }
                Client.AssignedSoftware.Add(cs);
                UnitOfWork.Clients.MarkForUpdate(Client);
                return new OkResult();
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult("Exception: " + ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("installSoftware/{uuid}/{swId}/{type}")]
        public IActionResult InstallSoftware(
            [FromRoute] string uuid,
            [FromRoute] string swId,
            [FromRoute] string type,
            [FromBody] AgentsAuthenticationModel data)
        {
            using (var unitOfWork = CreateUnitOfWork())
            {
                try
                {
                    DATA.Client fetchedClient = null;
                    List<DATA.Client> fetchedClients = unitOfWork.Clients.GetAll().Where(x => x.UUID == uuid && x.SerialNumber == data.SerialNumber).ToList();
                    List<DATA.Client> fittingClients = new List<DATA.Client>();

                    var fittingClientes = new List<DATA.Client>();

                    // Find to remove clients
                    foreach (DATA.Client client in fetchedClients)
                    {
                        List<MacAddress> macAddresses = new List<MacAddress>();
                        macAddresses = unitOfWork.MacAddresses.GetAll()
                            .Where(x => x.ClientId == client.Id).ToList();
                        for (int i = 0; i < macAddresses.Count; i++)
                        {
                            foreach (string mac in data.MacAddresses)
                            {
                                if (macAddresses.ElementAt(i).Address.ToLower() == mac.ToLower())
                                {
                                    if (!fittingClientes.Contains(client))
                                    {
                                        fittingClientes.Add(client);
                                    }
                                }
                            }
                        }
                    }

                    // Check if a client was identified
                    if (!(fittingClientes.Count() == 0 || fittingClientes.Count() > 1))
                    {
                        fetchedClient = fittingClientes.First();
                    }
                    if (fetchedClient != null)
                    {
                        // Check if a uninstall-Task exists
                        var software = unitOfWork.CustomerSoftwares.Get(swId, SoftwareIncludes.GetAllTasks());
                        if (software.TaskInstall != null)
                        {
                            List<DATA.ClientTask> clientTasks = unitOfWork.ClientTasks.GetAll().Where(x => x.ClientId == fetchedClient.Id && x.TaskId == software.TaskInstall.Id && x.Status == "assigned").ToList();
                            // Assign Task to Client
                            if (clientTasks.Count() == 0)
                            {
                                unitOfWork.Clients.AddTaskById(software.TaskInstall.Id, fetchedClient.Id, type, null);
                                unitOfWork.SaveChanges();
                            }
                        }
                        else
                        {
                            return new NotFoundObjectResult("No task for install found.");
                        }
                        return new OkObjectResult("true");
                    }
                    else
                    {
                        return new BadRequestObjectResult("ERROR: The client was not found");
                    }
                }
                catch (Exception ex)
                {
                    return new BadRequestObjectResult("Exception: " + ex.Message);
                }
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("unassignSoftware")]
        public IActionResult UnassignSoftware([FromForm] IFormCollection _form)
        {
            try
            {
                string _sw_id = Request.Form["_sw_id"];
                string _cuid = Request.Form["_client_uid"];
                var Client = UnitOfWork.Clients.GetByUuid(_cuid, "AssignedSoftware");
                Client.AssignedSoftware.RemoveAll(x => x.CustomerSoftwareId.Equals(_sw_id));
                UnitOfWork.Clients.MarkForUpdate(Client);
                UnitOfWork.SaveChanges();
                return new OkResult();
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult("Exception: " + ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("uninstallSoftware")]
        public IActionResult UninstallSoftware([FromForm] IFormCollection _form)
        {
            try
            {
                string _sw_id = Request.Form["_sw_id"];
                string _cuid = Request.Form["_client_uid"];
                var Client = UnitOfWork.Clients.GetByUuid(_cuid);
                // Check if a uninstall-Task exists
                var software = UnitOfWork.Software.Get(_sw_id, SoftwareIncludes.GetAllTasks());
                if (software.TaskUninstall != null)
                {
                    // Assign Task to Client
                    UnitOfWork.Clients.AddTaskById(software.TaskUninstall.Id, Client.Id, "software", null);
                    UnitOfWork.SaveChanges();
                }
                else
                {
                    return new NotFoundObjectResult("No task for uninstall found.");
                }
                return new OkResult();
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult("Exception: " + ex.Message);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("download")]
        public async Task<IActionResult> DownloadAgentAsync()
        {
            try
            {
                ResourcesRepository resources = new ResourcesRepository(connectionStrings.FileRepository, appSettings.ResourcesRepositoryFolder);
                var ms = new MemoryStream();
                var blob = resources.GetBlobFile("SmartDeployPackageInstaller.msi");
                await blob.DownloadToAsync(ms);
                ms.Seek(0, SeekOrigin.Begin);
                return File(ms, System.Net.Mime.MediaTypeNames.Application.Octet, "SmartDeployPackageInstaller.msi");
            }
            catch (Exception e)
            {
                return BadRequest("ERROR: " + e.Message);
            }
        }
        [HttpGet]
        [AllowAnonymous]
        [Route("download/3.5")]
        public async Task<IActionResult> DownloadWin7AgentAsync()
        {
            ResourcesRepository resources = new ResourcesRepository(connectionStrings.FileRepository, appSettings.ResourcesRepositoryFolder);
            var ms = new MemoryStream();
            var blob = resources.GetBlobFile("SmartDeployWin7.exe");
            await blob.DownloadToAsync(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, System.Net.Mime.MediaTypeNames.Application.Octet, "SmartDeployWin7.exe");
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("download/SmartDeployEXE")]
        public async Task<IActionResult> DownloadSmartDeployExeAsync()
        {
            ResourcesRepository resources = new ResourcesRepository(connectionStrings.FileRepository, appSettings.ResourcesRepositoryFolder);
            var ms = new MemoryStream();
            var blob = resources.GetBlobFile("SmartDeploy.exe");
            await blob.DownloadToAsync(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, System.Net.Mime.MediaTypeNames.Application.Octet, "SmartDeploy.exe");
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("inventory/{uuid}/{methodType}")]
        public IActionResult StoreHardwareInformation([FromRoute] string uuid, [FromRoute] string methodType, IFormFile file)
        {
            using (var unitOfWork = CreateUnitOfWork())
            {
                // Load client & inventory of client
                DATA.Client client = unitOfWork.Clients.GetAll().Where(x => x.UUID == uuid).First();
                if (client != null)
                {
                    // Delete existing inventory
                    List<Inventory> dbEntries = unitOfWork.Inventories.GetAll()
                        .Where(x => x.ClientId == client.Id).ToList();
                    foreach (Inventory entry in dbEntries)
                    {
                        unitOfWork.Inventories.Delete(entry);
                    }
                    unitOfWork.SaveChanges();
                    using (var stream = file.OpenReadStream())
                    using (var archive = new ZipArchive(stream))
                    {
                        var entries = archive.Entries;
                        foreach (var entry in entries)
                        {
                            var dataFile = archive.GetEntry(entry.FullName);
                            Inventory temp = new Inventory();
                            temp.ClientId = client.Id;
                            temp.Type = entry.Name.Split(".")[0];
                            temp.OperationType = methodType;
                            using (StreamReader sr = new StreamReader(dataFile.Open()))
                            {
                                temp.Value = sr.ReadToEnd();
                            }
                            unitOfWork.Inventories.MarkForInsert(temp);
                            unitOfWork.SaveChanges();
                        }
                        client.LastInventoryUpdate = DateTime.Now;
                        unitOfWork.Clients.MarkForUpdate(client);
                        unitOfWork.SaveChanges();
                        return Ok();
                    }
                }
                else
                {
                    return BadRequest("ERROR: The client does not exist");
                }
            }
        }


        [AllowAnonymous]
        [HttpPost]
        [Route("serviceStart")]
        public IActionResult FetchServiceStartData([FromBody] ClientAddViewModel data)
        {
            try
            {
                using (var unitOfWork = CreateUnitOfWork())
                {
                    DATA.Client toUpdate = unitOfWork.Clients.GetAll().Where(x => x.UUID == data.uuid).FirstOrDefault();
                    List<DATA.MacAddress> clientsMacAdresses = unitOfWork.MacAddresses.GetAll().Where(x => x.ClientId == toUpdate.Id).ToList();
                    if (toUpdate != null)
                    {
                        toUpdate.Model = data.Model;
                        toUpdate.SerialNumber = data.SerialNumber;
                        toUpdate.Type = data.Type;
                        toUpdate.HyperVisor = data.HyperVisor;
                        toUpdate.Manufacturer = data.Manufacturer;
                        toUpdate.MacAddresses = new List<MacAddress>();

                        // Delete old MACAddresses
                        foreach (MacAddress macAddress in clientsMacAdresses)
                        {
                            var existingMacAddress = data.MACAdresses.Find(x => x == macAddress.Address);
                            if (existingMacAddress == null)
                            {
                                unitOfWork.MacAddresses.MarkForDelete(macAddress);
                                unitOfWork.SaveChanges();
                            }
                        }

                        // Add new MACAddresses
                        foreach (string macAdress in data.MACAdresses)
                        {
                            var existingMacAddress = clientsMacAdresses.Find(x => x.Address == macAdress);
                            if (existingMacAddress == null)
                            {
                                MacAddress temp = new MacAddress();
                                temp.Address = macAdress;
                                temp.Client = toUpdate;
                                toUpdate.MacAddresses.Add(temp);
                            }
                        }

                        // Set client as online
                        toUpdate.IsOnline = true;

                        // Save DB changes
                        unitOfWork.Clients.MarkForUpdate(toUpdate);
                        unitOfWork.SaveChanges();
                    }
                    return Ok();
                }
            }
            catch (Exception e)
            {
                return BadRequest("ERROR: " + e.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("clientOffline/{uuid}")]
        public IActionResult SetClientOffline([FromRoute] string uuid)
        {
            using (var unitOfWork = CreateUnitOfWork())
            {
                DATA.Client client = unitOfWork.Clients.GetAll().Where(x => x.UUID == uuid).FirstOrDefault();
                if (client != null)
                {
                    client.IsOnline = false;
                    unitOfWork.Clients.MarkForUpdate(client);
                    unitOfWork.SaveChanges();
                }
            }
            return Ok();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("setOnline/{uuid}")]
        public IActionResult SetClientOnline([FromRoute] string uuid, [FromBody] AgentsAuthenticationModel data)
        {
            using (var unitOfWork = CreateUnitOfWork())
            {
                DATA.Client fetchedClient = null;
                List<DATA.Client> fetchedClients = unitOfWork.Clients.GetAll().Where(x => x.UUID == uuid && x.SerialNumber == data.SerialNumber).ToList();
                List<DATA.Client> fittingClients = new List<DATA.Client>();

                var fittingClientes = new List<DATA.Client>();

                // Find to remove clients
                foreach (DATA.Client client in fetchedClients)
                {
                    List<MacAddress> macAddresses = new List<MacAddress>();
                    macAddresses = unitOfWork.MacAddresses.GetAll()
                        .Where(x => x.ClientId == client.Id).ToList();
                    for (int i = 0; i < macAddresses.Count; i++)
                    {
                        foreach (string mac in data.MacAddresses)
                        {
                            if (macAddresses.ElementAt(i).Address.ToLower() == mac.ToLower())
                            {
                                if (!fittingClientes.Contains(client))
                                {
                                    fittingClientes.Add(client);
                                }
                            }
                        }
                    }
                }

                // Check if a client was identified
                if (!(fittingClientes.Count() == 0 || fittingClientes.Count() > 1))
                {
                    fetchedClient = fittingClientes.First();
                }

                if (fetchedClient != null)
                {
                    fetchedClient.IsOnline = true;
                    fetchedClient.LastOnlineStatusUpdate = DateTime.Now;
                    unitOfWork.Clients.MarkForUpdate(fetchedClient);
                    unitOfWork.SaveChanges();
                }

                return Ok();
            }
        }

        /// <summary>
        /// Fetching unattend.xml file for a windows image
        /// 
        /// </summary>
        /// <param name="uuid"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("unattend/{uuid}")]
        public async Task<IActionResult> GetUnattendFile([FromRoute] string uuid, [FromBody] AgentsAuthenticationModel data)
        {
            try
            {
                using (var unitOfWork = CreateUnitOfWork())
                {
                    DATA.Client fetchedClient = null;
                    List<DATA.Client> fetchedClients = unitOfWork.Clients.GetAll("Customer").Where(x => x.UUID == uuid && x.SerialNumber == data.SerialNumber).ToList();

                    List<DATA.Client> fittingClients = new List<DATA.Client>();

                    var fittingClientes = new List<DATA.Client>();

                    // Find to remove clients
                    foreach (DATA.Client client in fetchedClients)
                    {
                        List<MacAddress> macAddresses = new List<MacAddress>();
                        macAddresses = unitOfWork.MacAddresses.GetAll()
                            .Where(x => x.ClientId == client.Id).ToList();
                        for (int i = 0; i < macAddresses.Count; i++)
                        {
                            foreach (string mac in data.MacAddresses)
                            {
                                if (macAddresses.ElementAt(i).Address.ToLower() == mac.ToLower())
                                {
                                    if (!fittingClientes.Contains(client))
                                    {
                                        fittingClientes.Add(client);
                                    }
                                }
                            }
                        }
                    }

                    // Check if a client was identified
                    if (!(fittingClientes.Count() == 0 || fittingClientes.Count() > 1))
                    {
                        fetchedClient = fittingClientes.First();
                    }

                    if (fetchedClient == null)
                    {
                        return BadRequest("ERROR: The client does not exist");
                    }

                    if (fetchedClient.LocalAdminUsername == null || fetchedClient.LocalAdminPassword == null)
                    {
                        return BadRequest("ERROR: The local admin password and/or username");
                    }
                    DATA.Customer customer;

                    CustomerImage image = unitOfWork.CustomerImages.Get(fetchedClient.OSSettingsImageId, "Unattend");
                    CustomerImageStream imageStream = unitOfWork.CustomerImageStreams.Get(image.CustomerImageStreamId);
                    if (imageStream == null)
                    {
                        return BadRequest("ERROR: The settings for the image stream are not set");
                    }

                    if (imageStream.Type != "Windows")
                    {
                        return BadRequest("ERROR: The image is not a windows image");
                    }

                    customer = unitOfWork.Customers.GetAll("StorageEntryPoints").Where(x => x.Id == fetchedClient.CustomerId).FirstOrDefault();

                    if (customer == null)
                    {
                        return BadRequest("ERROR: The customer does not exist");
                    }

                    if (image.Unattend == null)
                    {
                        return BadRequest("ERROR: The unattend file is not existing");
                    }

                    var cep = GetCEP(customer.Id);
                    var csdp = customer.StorageEntryPoints.Find(x => x.IsCSDP);
                    if (cep == null && !csdp.Managed)
                    {
                        return BadRequest("ERROR: The csdp Cloud Entry Point is not set");
                    }
                    if (csdp == null)
                    {
                        return BadRequest("ERROR: The csdp storage entry point does not exist or has not been created yet");
                    }

                    AzureCommunicationService azureCustomer;
                    if (!csdp.Managed)
                    {
                        azureCustomer = new AzureCommunicationService(cep.TenantId, cep.ClientId, cep.ClientSecret);
                    }
                    else
                    {
                        // TODO: Check for system; fix for live system
                        azureCustomer = new AzureCommunicationService(appSettings.DevelopmentTenantId, appSettings.DevelopmentClientId, appSettings.DevelopmentClientSecret);
                    }
                    string connectionString = azureCustomer.StorageService().GetStorageAccConnectionString(csdp.SubscriptionId, csdp.ResourceGrpName, csdp.StorageAccount);

                    // Azure connection & get standard file
                    CloudStorageAccount storage = CloudStorageAccount.Parse(connectionString);
                    CloudBlobClient customerClient = storage.CreateCloudBlobClient();
                    CloudBlobContainer csdpContainer = customerClient.GetContainerReference("csdp");
                    CloudBlockBlob blob = csdpContainer.GetBlockBlobReference("Image_Repository/" + imageStream.SubFolderName + "/" + image.Unattend.Name);
                    bool exists = await blob.ExistsAsync();
                    if (!exists)
                    {
                        return BadRequest("ERROR: The unattend file does not exist in Azure");
                    }
                    string content = await blob.DownloadTextAsync();
                    XDocument unattend = XDocument.Parse(content);
                    XNamespace xmlNameSpace = "urn:schemas-microsoft-com:unattend";

                    // Get needed parameters
                    Parameter registeredOwner = unitOfWork.Parameters.GetAll()
                        .Where(x => x.CustomerId == fetchedClient.CustomerId && x.Key == "$RegisteredOwnerNewName").FirstOrDefault();
                    Parameter registeredOrganization = unitOfWork.Parameters.GetAll()
                        .Where(x => x.CustomerId == fetchedClient.CustomerId && x.Key == "$RegisteredOrganizationNewName").FirstOrDefault();

                    // Edit xml

                    var registeredOwnerTag = unattend.Descendants()?.Elements(xmlNameSpace + "RegisteredOwner").FirstOrDefault();
                    if (registeredOwnerTag != null && registeredOwner != null)
                    {
                        registeredOwnerTag.Value = registeredOwner.Value;
                    }

                    var registeredOrganizationTag = unattend.Descendants()?.Elements(xmlNameSpace + "RegisteredOrganization").FirstOrDefault();
                    if (registeredOrganizationTag != null && registeredOrganization != null)
                    {
                        registeredOrganizationTag.Value = registeredOrganization.Value;
                    }

                    // Remove node if product key not in stream                     
                    if (imageStream.ProductKey == null || !(imageStream.ProductKey.Length == 29 && imageStream.ProductKey.Split('-').Length == 5))
                    {
                        var productKey = unattend.Descendants()?.Elements(xmlNameSpace + "ProductKey").FirstOrDefault();
                        productKey.Remove();
                    }
                    else
                    {
                        var productKey = unattend.Descendants()?.Elements(xmlNameSpace + "ProductKey").FirstOrDefault();
                        productKey.Value = imageStream.ProductKey;
                    }

                    var timeZone = unattend.Descendants()?.Elements(xmlNameSpace + "TimeZone").FirstOrDefault();
                    if (timeZone != null)
                    {
                        timeZone.Value = fetchedClient.TimeZoneWindows;
                    }

                    var keyboarLayout = unattend.Descendants()?.Elements(xmlNameSpace + "InputLocale").FirstOrDefault();
                    if (keyboarLayout != null)
                    {
                        keyboarLayout.Value = fetchedClient.KeyboardLayoutWindows;
                    }

                    var clientName = unattend.Descendants()?.Elements(xmlNameSpace + "ComputerName").FirstOrDefault();
                    if (clientName != null)
                    {
                        clientName.Value = fetchedClient.Name;
                    }

                    var localAccounts = unattend.Descendants()?.Elements(xmlNameSpace + "LocalAccount").FirstOrDefault();
                    if (localAccounts != null)
                    {
                        var localAccountsElements = localAccounts.Elements();
                        for (int i = 0; i < localAccountsElements.Count(); i++)
                        {
                            var temp = localAccountsElements.ElementAt(i);
                            if (temp.Name.LocalName == "Password")
                            {
                                var localAccountPWTags = temp.Elements();
                                foreach (XElement el in localAccountPWTags)
                                {
                                    if (el.Name.LocalName == "Value")
                                    {
                                        string localAdminPW = DecryptString(fetchedClient.LocalAdminPassword) + "Password";
                                        el.Value = Base64Encode(localAdminPW);
                                    }
                                }

                            }
                            if (temp.Name.LocalName == "Name")
                            {
                                temp.Value = DecryptString(fetchedClient.LocalAdminUsername);
                            }
                        }
                    }

                    var autoLogon = unattend.Descendants()?.Elements(xmlNameSpace + "AutoLogon").FirstOrDefault();
                    if (autoLogon != null)
                    {
                        var tempElements = autoLogon.Elements();
                        var tempDescendants = autoLogon.Descendants();
                        foreach (XElement el in tempElements)
                        {
                            if (el.Name.LocalName == "Username")
                            {
                                el.Value = DecryptString(fetchedClient.LocalAdminUsername);
                            }
                            if (el.Name.LocalName == "Password")
                            {
                                var autoLogonPWTags = el.Elements();
                                foreach (XElement autoLogonPWel in autoLogonPWTags)
                                {
                                    if (autoLogonPWel.Name.LocalName == "Value")
                                    {
                                        string localAdminPW = DecryptString(fetchedClient.LocalAdminPassword) + "Password";
                                        autoLogonPWel.Value = Base64Encode(localAdminPW);
                                    }
                                }
                            }
                        }
                    }
                    // Return unattend file for download
                    return Ok(unattend.ToString());
                }
            }
            catch (Exception e)
            {
                var st = new StackTrace(e, true);
                // Get the top stack frame
                var frame = st.GetFrame(0);
                // Get the line number from the stack frame
                var line = frame.GetFileLineNumber();
                return BadRequest("ERROR: " + e.Message + " " + line);
            }
        }

        private string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.Unicode.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("seed/{uuid}")]
        public async Task<IActionResult> GetSeedFile([FromRoute] string uuid, [FromBody] AgentsAuthenticationModel data)
        {
            using (var unitOfWork = CreateUnitOfWork())
            {
                DATA.Client fetchedClient = null;
                var fetchedClients = unitOfWork.Clients.GetAll(ClientIncludes.GetAllIncludes()).Where(x => x.UUID == uuid && x.SerialNumber == data.SerialNumber).ToList();
                var fittingClientes = new List<DATA.Client>();

                // Find to remove clients
                foreach (DATA.Client client in fetchedClients)
                {
                    List<MacAddress> macAddresses = new List<MacAddress>();
                    macAddresses = unitOfWork.MacAddresses.GetAll()
                        .Where(x => x.ClientId == client.Id).ToList();
                    for (int i = 0; i < macAddresses.Count; i++)
                    {
                        foreach (string mac in data.MacAddresses)
                        {
                            if (macAddresses.ElementAt(i).Address.ToLower() == mac.ToLower())
                            {
                                if (!fittingClientes.Contains(client))
                                {
                                    fittingClientes.Add(client);
                                }
                            }
                        }
                    }
                }

                // Check if a client was identified
                if (!(fittingClientes.Count() == 0 || fittingClientes.Count() > 1))
                {
                    fetchedClient = fittingClientes.First();
                }

                if (fetchedClient == null)
                {
                    return BadRequest("ERROR: The client could not be found");
                }
                DATA.Customer customer;
                if (fetchedClient == null)
                {
                    return BadRequest("ERROR: There is no client with the sent UUID");
                }

                customer = unitOfWork.Customers.GetAll("StorageEntryPoints").Where(x => x.Id == fetchedClient.CustomerId).FirstOrDefault();

                if (customer == null)
                {
                    return BadRequest("ERROR: The customer does not exist");
                }

                if (fetchedClient.OSSettingsImageId == null)
                {
                    return BadRequest("The clients OS Settings have not been set");
                }
                CustomerImage image = unitOfWork.CustomerImages.Get(fetchedClient.OSSettingsImageId);
                CustomerImageStream stream = unitOfWork.CustomerImageStreams.Get(image.CustomerImageStreamId);

                // Azure connection & get standard file
                CloudStorageAccount storage = CloudStorageAccount.Parse(appSettings.LiveSystemConnectionString);
                CloudBlobClient bitstreamClient = storage.CreateCloudBlobClient();
                CloudBlobContainer csdpContainer = bitstreamClient.GetContainerReference("bsdp-v202011");
                CloudBlockBlob blob = csdpContainer.GetBlockBlobReference("Image_Repository/Configuration_Files/test_unattend.seed");
                string content = await blob.DownloadTextAsync();

                // Edit seed
                content = content.Replace("@@languagepack", stream.LocalSettingLinux);
                content = content.Replace("@@partitionpassword", DecryptString(fetchedClient.PartitionEncryptionPassLinux));
                content = content.Replace("@@timezone", fetchedClient.TimeZoneLinux);
                content = content.Replace("@@username", fetchedClient.UsernameLinux);
                content = content.Replace("@@userpassword", fetchedClient.UserPasswordLinux);
                content = content.Replace("@@adminpassword", fetchedClient.AdminPasswordLinux);
                content = content.Replace("@@keyboardlayout", fetchedClient.KeyboardLayoutLinux);
                content = content.Replace("@@computertargetname", fetchedClient.Name);

                if (fetchedClient.LanguagePackLinux != null)
                {
                    content = content.Replace("@@local", fetchedClient.LanguagePackLinux);
                }
                else
                {
                    content = content.Replace("@@local", "en-US.UTF-8");
                }

                // Upload seed file to customer csdp
                var cep = GetCEP(customer.Id);
                StorageEntryPoint csdp = customer.StorageEntryPoints.Find(x => x.IsCSDP == true);

                if (cep == null && csdp != null && !csdp.Managed)
                {
                    return BadRequest("ERROR: The csdp storage entry point does not exist or has not been created yet");
                }
                if (csdp == null)
                {
                    return BadRequest("ERROR: The csdp storage entry point does not exist or has not been created yet");
                }

                AzureCommunicationService azureCustomer;
                if (!csdp.Managed)
                {
                    azureCustomer = new AzureCommunicationService(cep.TenantId, cep.ClientId, cep.ClientSecret);
                }
                else
                {
                    // TODO: Check system; fix for live system
                    azureCustomer = new AzureCommunicationService(appSettings.DevelopmentTenantId, appSettings.DevelopmentClientId, appSettings.DevelopmentClientSecret);
                }
                string connectionString = azureCustomer.StorageService().GetStorageAccConnectionString(csdp.SubscriptionId, csdp.ResourceGrpName, csdp.StorageAccount);
                CloudStorageAccount customerStorageAcc = CloudStorageAccount.Parse(connectionString);
                CloudBlobClient customerClient = customerStorageAcc.CreateCloudBlobClient();
                CloudBlobContainer customerContainer = customerClient.GetContainerReference("csdp");
                string fileName = fetchedClient.Id + "-unattend.seed";
                CloudBlockBlob customerDestBlob = customerContainer.GetBlockBlobReference("Image_Repository/Configurations/" + fileName);
                await customerDestBlob.UploadTextAsync(content);

                // Save download url for seed file
                string sas = customerDestBlob.GetSharedAccessSignature(new SharedAccessBlobPolicy()
                {
                    SharedAccessExpiryTime = DateTime.UtcNow.AddDays(1),
                    Permissions = SharedAccessBlobPermissions.Read
                });

                Uri uri = customerDestBlob.Uri;
                string url = uri.ToString();

                fetchedClient.DownloadSeedURL = url + sas;
                unitOfWork.Clients.MarkForUpdate(fetchedClient);
                unitOfWork.SaveChanges();

                // Return unattend file for download
                return Ok(content.ToString());
            }
        }

        [HttpPost]
        [Route("grub/{uuid}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetGrubConfig([FromRoute] string uuid, [FromBody] AgentsAuthenticationModel data)
        {
            using (var unitOfWork = CreateUnitOfWork())
            {
                DATA.Client fetchedClient = null;
                var fetchedClients = unitOfWork.Clients.GetAll(ClientIncludes.GetAllIncludes()).Where(x => x.UUID == uuid && x.SerialNumber == data.SerialNumber).ToList();
                var fittingClientes = new List<DATA.Client>();

                // Find to remove clients
                foreach (DATA.Client client in fetchedClients)
                {
                    List<MacAddress> macAddresses = new List<MacAddress>();
                    macAddresses = unitOfWork.MacAddresses.GetAll()
                        .Where(x => x.ClientId == client.Id).ToList();
                    for (int i = 0; i < macAddresses.Count; i++)
                    {
                        foreach (string mac in data.MacAddresses)
                        {
                            if (macAddresses.ElementAt(i).Address.ToLower() == mac.ToLower())
                            {
                                if (!fittingClientes.Contains(client))
                                {
                                    fittingClientes.Add(client);
                                }
                            }
                        }
                    }
                }

                // Check if a client was identified
                if (!(fittingClientes.Count() == 0 || fittingClientes.Count() > 1))
                {
                    fetchedClient = fittingClientes.First();
                }

                if (fetchedClient == null)
                {
                    return BadRequest("ERROR: The client could not be found");
                }

                DATA.Customer customer;
                if (fetchedClient == null)
                {
                    return BadRequest("ERROR: There is no client with the sent UUID");
                }

                customer = unitOfWork.Customers.GetAll("StorageEntryPoints").Where(x => x.Id == fetchedClient.CustomerId).FirstOrDefault();

                if (customer == null)
                {
                    return BadRequest("ERROR: The customer does not exist");
                }

                if (fetchedClient.DownloadSeedURL == null)
                {
                    return BadRequest("ERROR: The client has no seed file url set yet");
                }

                // Get default grub.cfg and replace value
                CloudStorageAccount storage = CloudStorageAccount.Parse(appSettings.LiveSystemConnectionString);
                CloudBlobClient bitstreamClient = storage.CreateCloudBlobClient();
                CloudBlobContainer csdpContainer = bitstreamClient.GetContainerReference("bsdp-v202011");
                CloudBlockBlob blob = csdpContainer.GetBlockBlobReference("Image_Repository/Configuration_Files/grub.cfg");
                string content = await blob.DownloadTextAsync();
                content = content.Replace("@@WPMseed", fetchedClient.DownloadSeedURL);

                // return file content
                return Ok(content.ToString());
            }
        }

        [HttpGet]
        [Route("getSAS/{uuid}")]
        public IActionResult GetSASCustomerSEP([FromRoute] string uuid)
        {
            using (var unitOfWork = CreateUnitOfWork())
            {
                DATA.Client client = unitOfWork.Clients.GetAll().Where(x => x.UUID == uuid).FirstOrDefault();
                if (client == null)
                {
                    return BadRequest("ERROR: The client does not exist");
                }

                DATA.Customer customer = unitOfWork.Customers.GetOrNull(client.CustomerId, "StorageEntryPoints");
                if (customer == null)
                {
                    return BadRequest("ERROR: The customer does not exist");
                }

                var cep = GetCEP(customer.Id);
                var csdp = customer.StorageEntryPoints.Find(x => x.IsCSDP);
                if (cep == null && !csdp.Managed)
                {
                    return BadRequest("ERROR: The csdp Cloud Entry Point is not set");
                }
                if (csdp == null)
                {
                    return BadRequest("ERROR: The csdp storage entry point does not exist or has not been created yet");
                }

                AzureCommunicationService azureCustomer;
                if (!csdp.Managed)
                {
                    azureCustomer = new AzureCommunicationService(cep.TenantId, cep.ClientId, cep.ClientSecret);
                }
                else
                {
                    // TODO: Check for system; fix for live system
                    azureCustomer = new AzureCommunicationService(appSettings.DevelopmentTenantId, appSettings.DevelopmentClientId, appSettings.DevelopmentClientSecret);
                }
                string connectionString = azureCustomer.StorageService().GetStorageAccConnectionString(csdp.SubscriptionId, csdp.ResourceGrpName, csdp.StorageAccount);
                CloudStorageAccount customerStorageAccount = CloudStorageAccount.Parse(connectionString);
                CloudBlobClient customerClient = customerStorageAccount.CreateCloudBlobClient();
                CloudBlobContainer csdpContainer = customerClient.GetContainerReference(csdp.BlobContainerName);

                // Create sas key for 2 hours with reading permission
                string sasKey = csdpContainer.GetSharedAccessSignature(new SharedAccessBlobPolicy()
                {
                    SharedAccessExpiryTime = DateTime.UtcNow.AddHours(2),
                    Permissions = SharedAccessBlobPermissions.Read | SharedAccessBlobPermissions.List
                });
                return Ok(sasKey);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("getOfficeConfig/{uuid}")]
        public IActionResult GetOfficeConfig([FromRoute] string uuid)
        {
            DATA.Client client = UnitOfWork.Clients.GetAll().Where(x => x.UUID == uuid).FirstOrDefault();
            if (client == null)
            {
                return BadRequest("ERROR: The client does not exist");
            }

            Customer customer = UnitOfWork.Customers.GetAll().Where(x => x.Id == client.CustomerId).FirstOrDefault();

            if (customer == null)
            {
                return BadRequest("ERROR: The customer does not exist");
            }

            if (!customer.UseCustomConfig)
            {
                return BadRequest("ERROR: The customer chose the default config file");
            }

            if (customer.OfficeConfig == null || customer.OfficeConfig == "")
            {
                return BadRequest("ERROR: The office config has not been set yet");
            }

            return Ok(customer.OfficeConfig);
        }

        [HttpPost]
        [Route("getClientId/{uuid}")]
        public IActionResult GetClientId([FromBody] AgentsAuthenticationModel data, [FromRoute] string uuid)
        {
            using (var unitOfWork = CreateUnitOfWork())
            {
                DATA.Client fetchedClient = null;
                List<DATA.Client> fetchedClients = unitOfWork.Clients.GetAll("Customer").Where(x => x.UUID == uuid && x.SerialNumber == data.SerialNumber).ToList();

                List<DATA.Client> fittingClients = new List<DATA.Client>();

                var fittingClientes = new List<DATA.Client>();

                // Find to remove clients
                foreach (DATA.Client client in fetchedClients)
                {
                    List<MacAddress> macAddresses = new List<MacAddress>();
                    macAddresses = unitOfWork.MacAddresses.GetAll()
                        .Where(x => x.ClientId == client.Id).ToList();
                    for (int i = 0; i < macAddresses.Count; i++)
                    {
                        foreach (string mac in data.MacAddresses)
                        {
                            if (macAddresses.ElementAt(i).Address.ToLower() == mac.ToLower())
                            {
                                if (!fittingClientes.Contains(client))
                                {
                                    fittingClientes.Add(client);
                                }
                            }
                        }
                    }
                }

                // Check if a client was identified
                if (!(fittingClientes.Count() == 0 || fittingClientes.Count() > 1))
                {
                    fetchedClient = fittingClientes.First();
                }

                if (fetchedClient == null)
                {
                    return BadRequest("ERROR: The client does not exist");
                }

                return Ok(fetchedClient.Id);
            }
        }

        [HttpPost]
        [Route("setBaselineResult/{clientId}")]
        public async Task<IActionResult> SetBaselineResult([FromRoute] string clientId, IFormFile file)
        {
            try
            {
                using (var unitOfWork = CreateUnitOfWork())
                {
                    DATA.Client client = unitOfWork.Clients.Get(clientId);
                    DATA.Customer customer = unitOfWork.Customers.Get(client.CustomerId, CustomerIncludes.GetAllIncludes());
                    var cep = GetCEP(customer.Id);
                    StorageEntryPoint csdp = customer.StorageEntryPoints.Find(x => x.IsCSDP == true);

                    // Connect to Azure Storage Account
                    AzureCommunicationService azureCustomer;
                    if (!csdp.Managed)
                    {
                        azureCustomer = new AzureCommunicationService(cep.TenantId, cep.ClientId, cep.ClientSecret);
                    }
                    else
                    {
                        // TODO: Check system; fix for live system
                        azureCustomer = new AzureCommunicationService(appSettings.DevelopmentTenantId, appSettings.DevelopmentClientId, appSettings.DevelopmentClientSecret);
                    }
                    string connectionString = azureCustomer.StorageService().GetStorageAccConnectionString(csdp.SubscriptionId, csdp.ResourceGrpName, csdp.StorageAccount);
                    CloudStorageAccount customerStorageAcc = CloudStorageAccount.Parse(connectionString);
                    CloudBlobClient customerClient = customerStorageAcc.CreateCloudBlobClient();
                    CloudBlobContainer customerContainer = customerClient.GetContainerReference("csdp");

                    using (var stream = file.OpenReadStream())
                    {
                        using (var archive = new ZipArchive(stream))
                        {
                            var entries = archive.Entries;
                            for (int i = 0; i < entries.Count; i++)
                            {
                                var entry = entries[i];
                                var dataFile = archive.GetEntry(entry.FullName);
                                using (StreamReader sr = new StreamReader(dataFile.Open(), System.Text.Encoding.Default))
                                {
                                    string value = sr.ReadToEnd();

                                    // Upload to azure                                    
                                    CloudBlockBlob customerDestBlob = customerContainer.GetBlockBlobReference("Customer_Repository/BaselineResults/" + clientId + "/" + entry.FullName);
                                    await customerDestBlob.UploadTextAsync(value);
                                    // Problem with file settings
                                    if (entry.FullName == "BaselineResult.json")
                                    {
                                        client.BaseLineFile1 = entry.FullName;
                                    }
                                    else if (entry.FullName == "HardwareInventoryResult.json")
                                    {
                                        client.BaseLineFile2 = entry.FullName;
                                    }
                                    else if (entry.FullName == "OSInventoryResult.json")
                                    {
                                        client.BaseLineFile3 = entry.FullName;
                                    }
                                }
                            }
                        }
                    }
                    unitOfWork.Clients.MarkForUpdate(client);
                    unitOfWork.SaveChanges();
                    return Ok();
                }
            }
            catch (Exception e)
            {
                return BadRequest("ERROR: " + e.Message);
            }
        }

        [HttpGet]
        [Route("sdEmergencyVersion")]
        public async Task<IActionResult> GetSDEmergencyVersion()
        {
            try
            {
                CloudStorageAccount storage = CloudStorageAccount.Parse(appSettings.LiveSystemConnectionString);
                CloudBlobClient bitstreamClient = storage.CreateCloudBlobClient();
                CloudBlobContainer csdpContainer = bitstreamClient.GetContainerReference("download-repository");
                CloudBlockBlob blob = csdpContainer.GetBlockBlobReference("BitStream/SD-emergency-settings.json");
                if (!await blob.ExistsAsync())
                {
                    return BadRequest("ERROR: The emergency settings file does not exist!");
                }
                SDEmergencyViewModel sDEmergency = JsonConvert.DeserializeObject<SDEmergencyViewModel>(await blob.DownloadTextAsync());
                return Ok(sDEmergency.EmergencyVersionNumber);
            }
            catch (Exception e)
            {
                return BadRequest("ERROR: " + e.Message);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("sdEmergencyInstallScript")]
        public async Task<IActionResult> GetSDEmergencyInstallScript()
        {
            try
            {
                CloudStorageAccount storage = CloudStorageAccount.Parse(appSettings.LiveSystemConnectionString);
                CloudBlobClient bitstreamClient = storage.CreateCloudBlobClient();
                CloudBlobContainer csdpContainer = bitstreamClient.GetContainerReference("download-repository");
                CloudBlockBlob blob = csdpContainer.GetBlockBlobReference("BitStream/SD-emergency-settings.json");
                SDEmergencyViewModel sDEmergency = JsonConvert.DeserializeObject<SDEmergencyViewModel>(await blob.DownloadTextAsync());
                CloudBlockBlob installScriptBlob = csdpContainer.GetBlockBlobReference("BitStream/" + sDEmergency.InstallScriptName);
                if (!await installScriptBlob.ExistsAsync())
                {
                    return BadRequest("ERROR: The install script does not exist");
                }
                string content = String.Empty;
                content = await installScriptBlob.DownloadTextAsync();
                return Ok(content);
            }
            catch (Exception e)
            {
                return BadRequest("ERROR: " + e.Message);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("sdEmergencyMSI")]
        public async Task<IActionResult> GetSDEmergencyMSI()
        {
            try
            {
                CloudStorageAccount storage = CloudStorageAccount.Parse(appSettings.LiveSystemConnectionString);
                CloudBlobClient bitstreamClient = storage.CreateCloudBlobClient();
                CloudBlobContainer csdpContainer = bitstreamClient.GetContainerReference("download-repository");
                CloudBlockBlob blob = csdpContainer.GetBlockBlobReference("BitStream/SD-emergency-settings.json");
                SDEmergencyViewModel sDEmergency = JsonConvert.DeserializeObject<SDEmergencyViewModel>(await blob.DownloadTextAsync());
                CloudBlockBlob msiCriptBlob = csdpContainer.GetBlockBlobReference("BitStream/" + sDEmergency.MSIName);
                if (!await msiCriptBlob.ExistsAsync())
                {
                    return BadRequest("ERROR: The emergency msi does not exist");
                }
                string content = String.Empty;
                content = await msiCriptBlob.DownloadTextAsync();
                var ms = new MemoryStream();
                await msiCriptBlob.DownloadToStreamAsync(ms);
                ms.Seek(0, SeekOrigin.Begin);
                return File(ms, System.Net.Mime.MediaTypeNames.Application.Octet, "SmartDeployPackageInstaller.msi");
            }
            catch (Exception e)
            {
                return BadRequest("ERROR: " + e.Message);
            }
        }

        public class SDEmergencyViewModel
        {
            public string EmergencyVersionNumber { get; set; }
            public string MSIName { get; set; }
            public string InstallScriptName { get; set; }
        }

        public class WindowsVersions
        {
            public List<WindowsVersion> VersionsClient { get; set; }
            public List<WindowsVersion> VersionsServer { get; set; }
        }

        public class WindowsVersion
        {
            public string Name { get; set; }
            public string BuildNr { get; set; }
            public string ReleaseId { get; set; }
        }
    }
}