using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using WPM_API.Code.Infrastructure;
using WPM_API.Code.Infrastructure.LogOn;
using WPM_API.Common;
using WPM_API.Common.Utils;
using WPM_API.Data.DataContext.Entities;
using WPM_API.Data.Models;
using WPM_API.FileRepository;
using WPM_API.Models;
using WPM_API.Options;
using WPM_API.TransferModels;
using DATA = WPM_API.Data.DataContext.Entities;

namespace WPM_API.Controllers.Client
{
    [Route("agent")]
    public class AgentController : BasisController
    {
        public AgentController(AppSettings appSettings, ConnectionStrings connectionStrings, OrderEmailOptions orderEmailOptions, AgentEmailOptions agentEmailOptions, SendMailCreds sendMailCreds, SiteOptions siteOptions, ILogonManager logonManager) : base(appSettings, connectionStrings, orderEmailOptions, agentEmailOptions, sendMailCreds, siteOptions, logonManager)
        {
        }

        [HttpPut]
        [AllowAnonymous]
        [Route("{Token}/AddClient")]
        public IActionResult AddClientUsingToken([FromBody] AgentArgsViewModel args)
        {
            List<DATA.Token> tokensToCustomer = null;
            tokensToCustomer = UnitOfWork.Token.GetByCustomerId(args.CustomerId);
            if (tokensToCustomer == null)
            {
                return new NotFoundResult();
            }
            foreach (DATA.Token t in tokensToCustomer)
            {
                if (t.Hash.Length >= 128 && PasswordHash.ValidatePassword(args.SecurityToken, t.Hash))
                {
                    t.ValidTo = DateTime.Now;
                    DATA.Client client = new DATA.Client() { UUID = args.uuid, CustomerId = args.CustomerId, CloudFlag = args.CloudFlag.ToString(), Name = args.ComputerName.Trim() };
                    UnitOfWork.Clients.MarkForInsert(client);
                    UnitOfWork.SaveChanges();
                    return Ok();
                }
            }
            return BadRequest("Client could not be added.");
        }
        /// <summary>
        /// Assigns all properties to client and return the list to the agent.
        /// </summary>
        /// <param name="uuid"></param>
        /// <returns>List of commands for DeviceProperties</returns>
        [HttpPost]
        [Route("{uuid}")]
        public IActionResult GetCommands([FromRoute] string uuid, [FromBody] AgentsAuthenticationModel data)
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
                    return new NotFoundResult();
                }

                // Get all existing Properties and assign all to the Client
                List<DATA.ClientProperty> properties = UnitOfWork.ClientProperties.GetAll().ToList();
                foreach (DATA.ClientProperty p in properties)
                {
                    if (!fetchedClient.Properties.Exists(x => x.ClientPropertyId.Equals(p.Id)))
                    {
                        fetchedClient.Properties.Add(new DATA.ClientClientProperty() { ClientPropertyId = p.Id });
                    }
                }
                unitOfWork.SaveChanges();


                fetchedClients = unitOfWork.Clients.GetAll(ClientIncludes.GetAllIncludes()).Where(x => x.UUID == uuid && x.SerialNumber == data.SerialNumber).ToList();
                fittingClientes = new List<DATA.Client>();

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

                List<ClientPropertyViewModel> props = new List<ClientPropertyViewModel>();
                foreach (var entry in fetchedClient.Properties)
                {
                    ClientPropertyViewModel newEntry = new ClientPropertyViewModel() { Id = entry.ClientProperty.Id, Name = entry.ClientProperty.PropertyName, Command = entry.ClientProperty.Command };
                    props.Add(newEntry);
                }

                var json = JsonConvert.SerializeObject(props);
                return Ok(json);
            }
        }

        [HttpPost]
        [Route("registerFreeCustomer")]
        public IActionResult RegisterAtFreeCustomer([FromBody] ClientAddViewModel data)
        {
            using (var unitOfWork = CreateUnitOfWork())
            {
                DATA.Client fetchedClient = null;
                List<DATA.Client> fetchedClients = unitOfWork.Clients.GetAll(ClientIncludes.GetAllIncludes()).Where(x => x.UUID == data.uuid && x.SerialNumber == data.SerialNumber).ToList();
                var fittingClientes = new List<DATA.Client>();

                // Find to remove clients
                foreach (DATA.Client client in fetchedClients)
                {
                    List<MacAddress> macAddresses = new List<MacAddress>();
                    macAddresses = unitOfWork.MacAddresses.GetAll()
                        .Where(x => x.ClientId == client.Id).ToList();
                    for (int i = 0; i < macAddresses.Count; i++)
                    {
                        foreach (string mac in data.MACAdresses)
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

                DATA.Customer customer = unitOfWork.Customers.GetAll().Where(x => x.Name == data.CustomerName).FirstOrDefault();
                if (fetchedClient != null)
                {
                    if (customer.AutoRegisterClients)
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
                        List<ClientTask> tasks = unitOfWork.ClientTasks.GetAll().Where(x => x.ClientId == fetchedClient.Id).ToList();
                        foreach (ClientTask task in tasks)
                        {
                            unitOfWork.ClientTasks.MarkForDelete(task);
                        }
                        foreach (MacAddress macAddress in fetchedClient.MacAddresses)
                        {
                            unitOfWork.MacAddresses.MarkForDelete(macAddress);
                        }
                        unitOfWork.Clients.MarkForDelete(fetchedClient);
                        unitOfWork.SaveChanges();
                    }
                    else
                    {
                        return BadRequest("ERROR: The client already exists");
                    }
                }

                DATA.Client newClient = new DATA.Client() { Description = data.Description, Name = data.Name, UUID = data.uuid, CustomerId = customer.Id };
                newClient.Model = data.Model;
                newClient.SerialNumber = data.SerialNumber;
                newClient.Type = data.Type;
                newClient.HyperVisor = data.HyperVisor;
                newClient.Manufacturer = data.Manufacturer;
                newClient.MacAddresses = new List<MacAddress>();
                newClient.CustomerId = customer.Id;

                foreach (string macAdress in data.MACAdresses)
                {
                    MacAddress temp = new MacAddress();
                    temp.Address = macAdress;
                    temp.Client = newClient;
                    newClient.MacAddresses.Add(temp);
                }
                unitOfWork.Clients.MarkForInsert(newClient);
                unitOfWork.SaveChanges();

                var FixedParams = GenerateFixedClientParams(newClient);
                foreach (var param in FixedParams)
                {
                    param.ClientId = newClient.Id;
                    param.Client = newClient;
                    unitOfWork.ClientParameters.MarkForInsert(param);
                }
                unitOfWork.SaveChanges();
                return Ok();
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

        /// <summary>
        /// Check if a client UUID is registered.
        /// If the UUID is registered, save the client's datasheet.
        /// </summary>
        /// <param name="uuid"></param>
        /// <param name="clientDatasheet"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{uuid}")]
        public IActionResult CheckUUID([FromRoute] string uuid, [FromBody] List<ClientPropertyAddViewModel> clientProperties)
        {
            try
            {
                DATA.Client dbClient = UnitOfWork.Clients.GetByUuid(uuid, ClientIncludes.GetAllIncludes());
                if (dbClient == null)
                {
                    return new StatusCodeResult(404);
                }

                foreach (var prop in clientProperties)
                {
                    // This must exists, beacuse the agent only executes assigned Properties.
                    var dbProp = dbClient.Properties.Find(x => x.ClientPropertyId.Equals(prop.Id));
                    dbClient.Properties.Find(x => x.ClientPropertyId.Equals(prop.Id)).Value = prop.Value;
                    if (prop.Value != null && dbProp.ClientProperty.ParameterName != null && dbProp.ClientProperty.ParameterName.Trim().Length != 0)
                    {

                        var newPropValue = UnitOfWork.ClientParameters.getByClientId(dbClient.Id, dbProp.ClientProperty.ParameterName);
                        if (newPropValue != null)
                        {
                            newPropValue.Value = prop.Value;
                            UnitOfWork.ClientParameters.MarkForUpdate(newPropValue);
                            UnitOfWork.SaveChanges();
                        }

                    }

                }

                UnitOfWork.Clients.MarkForUpdate(dbClient);
                UnitOfWork.SaveChanges();
                return Ok();
            }
            catch (Exception e)
            {
                ErrorLog errorLog = UnitOfWork.ErrorLogs.CreateEmpty();
                errorLog.Time = DateTime.Now;
                errorLog.UserId = GetCurrentUser().Id;
                errorLog.Source = "AgentController - CheckUUID";
                errorLog.Error = "ERROR: " + e.Message + " -- " + e.InnerException.Message;
                UnitOfWork.SaveChanges();
                return BadRequest("The device properties could not be saved. " + e.Message);
            }
        }

        /*   ClientProperties customProp = new ClientProperties() { Name = "CustomProp to Get UUID", Command = "wmic csproduct get UUID" };
           UnitOfWork.ClientProps.MarkForInsert(customProp);
           UnitOfWork.SaveChanges();
           ClientClientProperties customPropVal = new ClientClientProperties { ClientId = dbClient.Id, Property = customProp };
           UnitOfWork.PropValues.MarkForInsert(customPropVal);
           UnitOfWork.SaveChanges();*/

        // authorize client, return jwt
        /* dbClient.Os = new OS();
         dbClient.Os = Mapper.Map<OsViewModels, DATA.OS>(clientDatasheet.Os);
         dbClient.Bios = new Bios();
         dbClient.Bios = Mapper.Map<BiosViewModels, DATA.Bios>(clientDatasheet.Bios);
         dbClient.Hardware = new Hardware();
         dbClient.Hardware = Mapper.Map<HardwareViewModels, DATA.Hardware>(clientDatasheet.Hardware);
         dbClient.Network = new NetworkConfiguration();
         dbClient.Network = Mapper.Map<NetworkConfigurationViewModels, DATA.NetworkConfiguration>(clientDatasheet.Network);
         dbClient.Purchase = new Purchase();
         dbClient.Purchase = Mapper.Map<PurchaseViewModels, DATA.Purchase>(clientDatasheet.Purchase);
         dbClient.Partition = new List<HDDPartition>();
         dbClient.Partition = Mapper.Map<List<HDDPartitionViewModels>, List<DATA.HDDPartition>>(clientDatasheet.Partition);
         dbClient.JoinedDomain = clientDatasheet.JoinedDomain;
         dbClient.Proxy = clientDatasheet.Proxy;
         dbClient.Location = clientDatasheet.Location;
         dbClient.UsageStatus = clientDatasheet.UsageStatus;
         dbClient.InstallationDate = clientDatasheet.InstallationDate;
         dbClient.MainUser = clientDatasheet.MainUser;
         dbClient.Vendor = clientDatasheet.Vendor;
         dbClient.MacAddresses = new List<MacAddress>();
         dbClient.MacAddresses = Mapper.Map<List<MacAddressViewModels>, List<MacAddress>>(clientDatasheet.MacAddresses);
         UnitOfWork.Clients.MarkForUpdate(dbClient);
         try
         {
             UnitOfWork.SaveChanges();
         }
         catch (Exception)
         {
             return BadRequest("Client could not be changed.");
         }
     }*/

        //// Serialize and return the response


        [HttpGet]
        [Authorize(Policy = Constants.Roles.Customer)]
        [Route("downloads/winpe")]
        public IActionResult DownloadPEAsync()
        {
            try
            {
                ResourcesRepository agent = new ResourcesRepository(connectionStrings.FileRepository, appSettings.ResourcesRepositoryFolder);
                string sasUri = agent.GetSASFile("WinPEv14.iso", false);
                if (sasUri == null)
                {
                    return BadRequest("ERROR: The SAS Uri could not be fetched");
                }
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(sasUri);
                request.Method = "GET";
                WebResponse response = request.GetResponse();
                return File(response.GetResponseStream(), System.Net.Mime.MediaTypeNames.Application.Octet, "winpe.iso");
            }
            catch (Exception e)
            {
                return BadRequest("Error: " + e.Message);
            }
            /*
            var stream = await agent.DownloadWithLocalStorageAsync("WinPEv14.iso", Path.Combine(Directory.GetCurrentDirectory(), "Resources", "Temp"));
            return new FileStreamResult(stream, System.Net.Mime.MediaTypeNames.Application.Octet) { FileDownloadName = "WinPEv14.iso" }; 
            */
        }

        [HttpGet]
        [Route("downloads/unattend/{UUID}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetUserUnattend([FromRoute] string UUID)
        {
            var Client = UnitOfWork.Clients.GetByUuid(UUID);
            if (Client.Unattend == null)
                return new NotFoundResult();
            FileRepository.FileRepository unattends = new FileRepository.FileRepository(connectionStrings.FileRepository, appSettings.FileRepositoryFolder);
            var downloadStream = await unattends.DownloadWithLocalStorageAsync(Client.Unattend, Path.Combine(Directory.GetCurrentDirectory(), "Resources", "Temp"));
            downloadStream.Seek(0, SeekOrigin.Begin);
            return File(downloadStream, System.Net.Mime.MediaTypeNames.Application.Octet, "unattend.xml");
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("downloads/unattend")]
        public async Task<IActionResult> DownloadUnattend()
        {
            ResourcesRepository agent = new ResourcesRepository(connectionStrings.FileRepository, appSettings.ResourcesRepositoryFolder);
            var stream = await agent.DownloadWithLocalStorageAsync("unattend.xml", Path.Combine(Directory.GetCurrentDirectory(), "Resources", "Temp"));
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, System.Net.Mime.MediaTypeNames.Application.Octet, "unattend.xml");
        }

        [HttpGet]
        [Authorize(Policy = Constants.Roles.Customer)]
        [Route("downloads/firstagent")]
        public async Task<IActionResult> DownloadFirstAgent()
        {
            ResourcesRepository agent = new ResourcesRepository(connectionStrings.FileRepository, appSettings.ResourcesRepositoryFolder);
            var stream = await agent.DownloadWithLocalStorageAsync("FirstAgent.exe", Directory.GetCurrentDirectory());
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, System.Net.Mime.MediaTypeNames.Application.Octet, "FirstAgent.exe");
        }

        [HttpPost]
        [Route("notify/{CompMail}")]
        public IActionResult NotifySyshouse([FromRoute] string CompMail, [FromBody] ClientFields emailFields)
        {
            // Try to add Client to Customer
            string result = string.Empty;
            int StatusCode = 200;
            using (var unitOfWork = CreateUnitOfWork())
            {
                try
                {
                    DATA.Customer cust = unitOfWork.Customers.GetAll().Single(x => x.Name.ToLower().Equals(emailFields.CompName.ToLower()));
                    DATA.Client newClient = new DATA.Client() { Description = emailFields.AddInfo, Name = emailFields.UserName, UUID = emailFields.UserUUID, CustomerId = cust.Id };
                    unitOfWork.Clients.MarkForInsert(newClient);
                    unitOfWork.SaveChanges();
                }
                catch (Exception)
                {
                    StatusCode = 202;
                }
            }
            try
            {
                SmtpClient client = new SmtpClient(agentEmailOptions.Host, agentEmailOptions.Port);
                NetworkCredential data = new NetworkCredential(agentEmailOptions.Email, agentEmailOptions.Password);
                client.Credentials = data;
                MailAddress from = new MailAddress(agentEmailOptions.Email, agentEmailOptions.DisplayName);
                MailAddress to = new MailAddress((emailFields.CompMail).Trim());
                MailMessage message = new MailMessage(from, to);
                message.Body = new StringBuilder("Dear Customer,</br></br>we would like to notify you that a new client has been registred under your Systemhouse.</br></br></br><u>Determined Data : " +
                    " </u> </br>&nbsp;&nbsp; Vendor : " + emailFields.UserVendor + "</br>&nbsp;&nbsp; Model : " + emailFields.UserVersion + " </br>&nbsp;&nbsp; UUID: " + emailFields.UserUUID + "</br>&nbsp;&nbsp; Serial Number : " + emailFields.UserSerialnumber + "</br>&nbsp;&nbsp; MAC-Address : " + emailFields.UserMacaddress + "</br> </br> <u>Reported Target Data : </u></br>&nbsp;&nbsp; IT-Department/Systemhouse : " + emailFields.ITDep + "</br>&nbsp;&nbsp; Location : " + emailFields.UserLocation + "</br>&nbsp;&nbsp; Company Name : " + emailFields.CompName + "</br>&nbsp;&nbsp; Company E-mail : " + emailFields.CompMail + "</br></br><u>Issuer Information:</u></br>" + "&nbsp;&nbsp; Name : " + emailFields.UserName + "</br>&nbsp;&nbsp; E-mail : " + emailFields.UserMail + "</br> &nbsp;&nbsp; Phone Number: " + emailFields.UserPhone +
                    "</br></br> <u>Additional Information :</u> " + emailFields.AddInfo + "</br></br></br></br></br></br></br>Please go to <a href=\"http://portal.bitstream.de\"> http://portal.bitstream.de </a> in order to register your client." + "</br></br></br></br></br></br> With Kind Regards,</br> BitStream Team").ToString();
                message.BodyEncoding = System.Text.Encoding.UTF8;
                message.Subject = "New Client Registred under your Systemhouse";
                message.SubjectEncoding = System.Text.Encoding.UTF8;
                message.IsBodyHtml = true;
                client.EnableSsl = true;
                client.Send(message);
                message.Dispose();
            }
            catch (Exception)
            {
                StatusCode = 400;
            }

            try
            {
                SmtpClient client = new SmtpClient(agentEmailOptions.Host, agentEmailOptions.Port);
                NetworkCredential data = new NetworkCredential(agentEmailOptions.Email, agentEmailOptions.Password);
                client.Credentials = data;
                MailAddress from = new MailAddress(agentEmailOptions.Email, agentEmailOptions.DisplayName);
                MailAddress to = new MailAddress((emailFields.UserMail).Trim());
                MailMessage message = new MailMessage(from, to);
                message.Body = new StringBuilder("Dear Customer,</br></br>we have notified " + emailFields.ITDep + ".</br></br></br><u>Determined Data : " +
                    " </u> </br>&nbsp;&nbsp; Vendor : " + emailFields.UserVendor + "</br>&nbsp;&nbsp; Model : " + emailFields.UserVersion + " </br>&nbsp;&nbsp; UUID: " + emailFields.UserUUID + "</br>&nbsp;&nbsp; Serial Number : " + emailFields.UserSerialnumber + "</br>&nbsp;&nbsp; MAC-Address : " + emailFields.UserMacaddress + "</br> </br> <u>Reported Target Data : </u></br>&nbsp;&nbsp; IT-Department/Systemhouse : " + emailFields.ITDep + "</br>&nbsp;&nbsp; Location : " + emailFields.UserLocation + "</br>&nbsp;&nbsp; Company Name : " + emailFields.CompName + "</br>&nbsp;&nbsp; Company E-mail : " + emailFields.CompMail + "</br></br><u>Issuer Information:</u></br>" + "&nbsp;&nbsp; Name : " + emailFields.UserName + "</br>&nbsp;&nbsp; E-mail : " + emailFields.UserMail + "</br> &nbsp;&nbsp; Phone Number: " + emailFields.UserPhone +
                    "</br></br> <u>Additional Information :</u> " + emailFields.AddInfo + "</br></br></br></br></br></br> With Kind Regards,</br> BitStream Team").ToString();
                message.BodyEncoding = System.Text.Encoding.UTF8;
                message.Subject = emailFields.ITDep + " has been notified";
                message.SubjectEncoding = System.Text.Encoding.UTF8;
                message.IsBodyHtml = true;
                client.EnableSsl = true;
                client.Send(message);
                message.Dispose();
            }
            catch (SmtpException)
            {
                StatusCode = 400;
            }

            return new StatusCodeResult(StatusCode);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("downloads/initialagent")]
        public IActionResult DownloadAgent()
        {
            var assembly = typeof(Program).GetTypeInfo().Assembly;
            Stream stream = assembly.GetManifestResourceStream("WPM_API.Resources.InstallAgent.exe");
            byte[] exeBytes = new byte[(int)stream.Length];
            stream.Read(exeBytes, 0, exeBytes.Length);
            return File(exeBytes, "application/octet-stream", "Bitstream-FirstAgent.exe");
        }
        [HttpGet]
        [AllowAnonymous]
        [Route("downloads/installagent")]
        public async Task<IActionResult> DownloadInstallAgent()
        {
            ResourcesRepository agent = new ResourcesRepository(connectionStrings.FileRepository, appSettings.ResourcesRepositoryFolder);
            var stream = await agent.DownloadWithLocalStorageAsync("InstallAgent.exe", Directory.GetCurrentDirectory());
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, System.Net.Mime.MediaTypeNames.Application.Octet, "InstallAgent.exe");
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("DeviceOptions/{uuid}/{peOnly}")]
        public async Task<IActionResult> GetNextDeviceOption([FromRoute] string UUID, [FromBody] AgentsAuthenticationModel authData, [FromRoute] bool peOnly)
        {
            try
            {
                if (authData == null)
                {
                    return BadRequest("ERROR: The data to identify the client is missing");
                }
                using (var unitOfWork = CreateUnitOfWork())
                {
                    // DATA.Client client = UnitOfWork.Clients.GetByUuid(UUID, ClientIncludes.ClientOptions, ClientIncludes.ClientOptionsDeviceOption, ClientIncludes.ClientOptionsParameters);
                    DATA.Client fetchedClient = null;
                    var fetchedClients = unitOfWork.Clients.GetAll(ClientIncludes.GetAllIncludes()).Where(x => x.UUID == UUID && x.SerialNumber == authData.SerialNumber).ToList();
                    var fittingClientes = new List<DATA.Client>();

                    // Find to remove clients
                    foreach (DATA.Client client in fetchedClients)
                    {
                        List<MacAddress> macAddresses = new List<MacAddress>();
                        macAddresses = unitOfWork.MacAddresses.GetAll()
                            .Where(x => x.ClientId == client.Id).ToList();
                        for (int i = 0; i < macAddresses.Count; i++)
                        {
                            foreach (string mac in authData.MacAddresses)
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
                        return new NotFoundResult();
                    if (fetchedClient.AssignedOptions.Count == 0)
                        return BadRequest("User does not have any AssignedOptions in Queue");
                    DATA.ClientOption opt = null;
                    foreach (var current in fetchedClient.AssignedOptions)
                    {
                        if (current.OSType == "Windows")
                        {
                            if (peOnly)
                            {
                                if (opt == null)
                                {
                                    opt = current;
                                    continue;
                                }
                                if (current.Order < opt.Order)
                                {
                                    opt = current;
                                }
                            }
                            else
                            {
                                if (!current.PEOnly)
                                {
                                    if (opt == null)
                                    {
                                        opt = current;
                                        continue;
                                    }
                                    if (current.Order < opt.Order)
                                    {
                                        opt = current;
                                    }
                                }
                            }
                        }
                    }
                    if (opt != null)
                    {
                        opt = unitOfWork.ClientOptions.Get(opt.Id, "Parameters");
                    }
                    else
                    {
                        return BadRequest("There is not suitable Option assigned to run");
                    }
                    ScriptVersion version = unitOfWork.ScriptVersions.Get(opt.DeviceOptionId);

                    DATA.ExecutionLog executionLog = new DATA.ExecutionLog() { ClientId = fetchedClient.Id, ExecutionDate = DateTime.Now, ScriptVersionId = opt.DeviceOptionId, ScriptVersion = version, Client = fetchedClient, Script = opt.DeviceOption.ContentUrl, ScriptArguments = new List<DATA.Parameter>() };
                    unitOfWork.Logs.MarkForInsert(executionLog);

                    fetchedClient.AssignedOptions.Remove(opt);
                    unitOfWork.Clients.MarkForUpdate(fetchedClient, fetchedClient.Id);
                    unitOfWork.SaveChanges();
                    ParameterStringViewModel parameters = new ParameterStringViewModel();
                    parameters.ParameterList = new List<ParameterViewModel>();
                    var paramString = "";

                    // List<Parameter> parameters = unitOfWork.Parameters.GetAll().Where(x => x.)
                    opt.Parameters.ForEach(x => paramString += AssignParameter(x));
                    opt.Parameters.ForEach(x => parameters.ParameterList.Add(Mapper.Map<ParameterViewModel>(x)));
                    // opt.Parameters.ForEach(x => paramString += " " + x.Value);
                    parameters.Parameters = paramString;
                    parameters.OptionId = opt.DeviceOption.Id;
                    //            return File(ms, System.Net.Mime.MediaTypeNames.Application.Octet, opt.DeviceOption.ContentUrl);
                    //            var json = JsonConvert.SerializeObject(Mapper.Map<DATA.ClientOption, OptionAssignRefViewModel>(opt),
                    //                serializerSettings);
                    return new OkObjectResult(parameters);
                }
            }
            catch (Exception e)
            {
                return BadRequest("ERROR: " + e.Message + " " + e.InnerException.Message);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("DeviceOptionsLinux/{uuid}/{linuxType}")]
        public async Task<IActionResult> GetNextLinuxDeviceOption([FromRoute] string uuid, [FromRoute] string linuxType, [FromBody] AgentsAuthenticationModel authData)
        {
            try
            {
                if (authData == null)
                {
                    return BadRequest("ERROR: The data to identify the client is missing");
                }
                using (var unitOfWork = CreateUnitOfWork())
                {
                    // DATA.Client client = UnitOfWork.Clients.GetByUuid(UUID, ClientIncludes.ClientOptions, ClientIncludes.ClientOptionsDeviceOption, ClientIncludes.ClientOptionsParameters);
                    DATA.Client fetchedClient = null;
                    var fetchedClients = unitOfWork.Clients.GetAll(ClientIncludes.GetAllIncludes()).Where(x => x.UUID.ToLower() == uuid.ToLower() && x.SerialNumber == authData.SerialNumber).ToList();
                    var fittingClientes = new List<DATA.Client>();

                    // Find to remove clients
                    foreach (DATA.Client client in fetchedClients)
                    {
                        List<MacAddress> macAddresses = new List<MacAddress>();
                        macAddresses = unitOfWork.MacAddresses.GetAll()
                            .Where(x => x.ClientId == client.Id).ToList();
                        for (int i = 0; i < macAddresses.Count; i++)
                        {
                            foreach (string mac in authData.MacAddresses)
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
                        return new NotFoundResult();

                    if (fetchedClient.AssignedOptions.Count == 0)
                        return BadRequest("User does not have any AssignedOptions in Queue");
                    DATA.ClientOption opt = null;
                    foreach (var current in fetchedClient.AssignedOptions)
                    {
                        if (current.OSType == linuxType)
                        {
                            if (opt == null)
                            {
                                opt = current;
                                continue;
                            }
                            if (current.Order < opt.Order)
                            {
                                opt = current;
                            }
                        }
                    }

                    // Load latest ScriptVersion
                    if (opt != null)
                    {
                        opt = unitOfWork.ClientOptions.Get(opt.Id, "Parameters");
                    }
                    else
                    {
                        return BadRequest("There is not suitable Option assigned to run");
                    }
                    ScriptVersion version = unitOfWork.ScriptVersions.Get(opt.DeviceOptionId);

                    // Log eecution
                    DATA.ExecutionLog executionLog = new DATA.ExecutionLog() { ClientId = fetchedClient.Id, ExecutionDate = DateTime.Now, ScriptVersionId = opt.DeviceOptionId, ScriptVersion = version, Client = fetchedClient, Script = opt.DeviceOption.ContentUrl, ScriptArguments = new List<DATA.Parameter>() };
                    unitOfWork.Logs.MarkForInsert(executionLog);

                    fetchedClient.AssignedOptions.Remove(opt);
                    unitOfWork.Clients.MarkForUpdate(fetchedClient, fetchedClient.Id);
                    unitOfWork.SaveChanges();

                    // TODO: set Parameter with saved values from DB
                    FileRepository.FileRepository fileRepository = new FileRepository.FileRepository(connectionStrings.FileRepository, appSettings.FileRepositoryFolder);
                    var blob = fileRepository.GetBlobFile(version.ContentUrl);

                    BlobDownloadResult dlResult = await blob.DownloadContentAsync();
                    string content = dlResult.Content.ToString();

                    // Set parameters
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < opt.Parameters.Count; i++)
                    {
                        var param = opt.Parameters[i];
                        sb.AppendLine(param.Key + "=" + "\"" + param.Value + "\"");
                    }

                    int startIndex = content.IndexOf("###param(");
                    int endIndex = content.IndexOf("###)");

                    string toReplace = content.Substring(startIndex, (endIndex - startIndex));
                    content = content.Replace(toReplace, sb.ToString());
                    content = content.Replace("###)", "");
                    // TODO: Replace windows characters with unix
                    content.Replace("\r\n", "\n");

                    // Dispose string builder
                    sb = null;
                    // TODO: Set OK() when ready
                    DeviceOptionLinuxViewModel result = new DeviceOptionLinuxViewModel();
                    result.ScriptName = version.Name + ".sh";
                    result.ScriptName.Replace(" ", "_");
                    result.ScriptContent = content;

                    string json = JsonConvert.SerializeObject(result, serializerSettings);
                    return Ok(json);
                }
            }
            catch (Exception e)
            {
                return BadRequest("ERROR: " + e.Message);
            }
        }

        public class DeviceOptionLinuxViewModel
        {
            public string ScriptName { get; set; }
            public string ScriptContent { get; set; }
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("downloads/DeviceOptions/{deviceOptionId}")]
        public async Task<IActionResult> DownloadNextDeviceOption([FromRoute] string deviceOptionId)
        {
            try
            {
                var opt = UnitOfWork.ScriptVersions.Get(deviceOptionId);
                FileRepository.FileRepository fileRepository = new FileRepository.FileRepository(connectionStrings.FileRepository, appSettings.FileRepositoryFolder);

                var ms = new MemoryStream();
                var blob = fileRepository.GetBlobFile(opt.ContentUrl);
                await blob.DownloadToAsync(ms);
                ms.Seek(0, SeekOrigin.Begin);

                return File(ms, System.Net.Mime.MediaTypeNames.Application.Octet, opt.ContentUrl);
            }
            catch (Exception e)
            {
                return BadRequest("ERROR: " + e.Message);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("getEnterpriseSoftware/{type}")]
        public IActionResult GetEnterpriseSoftware([FromBody] UUIDViewModel data, [FromRoute] string type)
        {
            List<DATA.CustomerSoftwareStream> customerSWStreams;
            List<DATA.CustomerSoftware> swPackages;

            try
            {
                using (var unitOfWork = CreateUnitOfWork())
                {
                    swPackages = new List<CustomerSoftware>();
                    if (data != null && data.UUID != null)
                    {
                        DATA.Client fetchedClient = null;
                        List<DATA.Client> fetchedClients = unitOfWork.Clients.GetAll(ClientIncludes.GetAllIncludes()).Where(x => x.UUID == data.UUID && x.SerialNumber == data.SerialNumber).ToList();
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

                        if (fetchedClient == null || fetchedClient.Customer == null)
                        {
                            return BadRequest("ERROR: The client is not registered yet!");
                        }
                        customerSWStreams = unitOfWork.CustomerSoftwareStreamss.GetAll("StreamMembers.TaskInstall.Files", "StreamMembers.TaskInstall.ExecutionFile")
                            .Where(x => x.CustomerId == fetchedClient.CustomerId && x.IsEnterpriseStream)
                            .ToList();
                        if (type == "3")
                        {
                            customerSWStreams = customerSWStreams.FindAll(x => x.ClientOrServer != "Client only");
                        }
                        // Order customer software streams by priority
                        customerSWStreams = customerSWStreams.OrderBy(x => x.Priority).ToList();
                        if (customerSWStreams != null)
                        {
                            foreach (DATA.CustomerSoftwareStream swStream in customerSWStreams)
                            {
                                List<CustomerSoftware> activeSW = swStream.StreamMembers.FindAll(x => x.CustomerStatus == "Active");
                                if (activeSW.Count != 0)
                                {
                                    int index = 0;
                                    for (int i = 0; i < activeSW.Count; i++)
                                    {
                                        double indexVersion = double.Parse(activeSW.ElementAt(index).Version.Replace(".", ""));
                                        double iteratorVersion = double.Parse(activeSW.ElementAt(i).Version.Replace(".", ""));
                                        if (iteratorVersion > indexVersion)
                                        {
                                            index = i;
                                        }
                                    }
                                    swPackages.Add(activeSW.ElementAt(index));
                                }
                            }

                            foreach (CustomerSoftware sw in swPackages)
                            {
                                sw.TaskInstall.ExecutionFile.Task = null;
                                foreach (DATA.File file in sw.TaskInstall.Files)
                                {
                                    file.Task = null;
                                }
                            }
                        }

                        return Ok(JsonConvert.SerializeObject(swPackages));
                    }
                    else
                    {
                        return BadRequest("ERROR: The request payload is corrupt");
                    }

                }
            }
            catch (Exception e)
            {
                return BadRequest("ERROR: " + e.Message + e.InnerException);
            }

            // return JsonConvert.SerializeObject(null);
        }

        private string AssignParameter(Parameter parameter)
        {
            if (parameter.Value.Equals("$true") || parameter.Value.Equals("$false") || parameter.Value.Equals("$null"))
            {
                return " -" + parameter.Key.Substring(1).Trim() + " " + parameter.Value + "";
            }
            else
            {
                return " -" + parameter.Key.Substring(1).Trim() + " \"\"\"" + parameter.Value + "\"\"\"";
            }
        }

        public class UUIDViewModel : AgentsAuthenticationModel
        {
            public string UUID { get; set; }
        }
    }
}