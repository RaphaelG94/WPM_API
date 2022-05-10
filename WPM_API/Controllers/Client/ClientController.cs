using Azure.Storage.Blobs.Models;
using WPM_API.Azure;
using WPM_API.Common;
using WPM_API.Data.DataContext.Entities;
using WPM_API.Data.Models;
using WPM_API.TransferModels;
using WPM_API.TransferModels.SmartDeploy;
using WPM_API.Code;
using WPM_API.Models;
using WPM_API.Models.Inventory;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DATA = WPM_API.Data.DataContext.Entities;

namespace WPM_API.Controllers.Client
{
    [Route("customers/{customerId}/clients")]
    public class ClientController : BasisController
    {
        private string RandomPassword(int size = 0)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(RandomString(4, true));
            builder.Append(RandomNumber(1000, 9999));
            builder.Append(RandomString(2, false));
            return builder.ToString();
        }

        private int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }

        private string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }
        /// <summary>
        /// Retrieve all clients from the customer.
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Policy = Constants.Roles.Customer)]
        public IActionResult GetClients([FromRoute] string customerId)
        {
            List<ClientViewModel> clients = new List<ClientViewModel>();
            List<DATA.Client> dbEntries = UnitOfWork.Clients.GetAll("Base").Where(x => x.CustomerId == customerId).ToList();
            clients = Mapper.Map<List<DATA.Client>, List<ClientViewModel>>(dbEntries);
            // Serialize and return the response
            var json = JsonConvert.SerializeObject(clients, _serializerSettings);
            return new OkObjectResult(json);
        }

        [HttpPut]
        [Route("osSettings/{customerImageId}/{clientId}")]
        public IActionResult SetOSSettings([FromRoute] string customerImageId, [FromRoute] string clientId, [FromBody] OSSettingModel data)
        {
            using ( var unitOfWork = CreateUnitOfWork())
            {
                DATA.Client client = unitOfWork.Clients.Get(clientId);
                CustomerImage customerImage = unitOfWork.CustomerImages.Get(customerImageId, "Unattend", "OEMPartition");
                CustomerImageStream customerImageStream = unitOfWork.CustomerImageStreams.Get(customerImage.CustomerImageStreamId);
                List<ClientParameter> clientParameters = unitOfWork.ClientParameters.GetAll().Where(x => x.ClientId == clientId).ToList();
                ClientParameter PathToImage = null;
                // TODO: Search or create TargetOSBuild with Path2Image value
                List<ClientParameter> PathToImageList = clientParameters.FindAll(x => x.ParameterName == "$Path2Image");
                if (PathToImageList.Count == 1)
                {
                    PathToImage = PathToImageList.First();
                }
                else if (PathToImageList.Count > 1)
                {
                    foreach (ClientParameter toDelete in PathToImageList)
                    {
                        unitOfWork.ClientParameters.Delete(toDelete);
                        unitOfWork.SaveChanges();
                    }
                }

                ClientParameter TargetOSBuild = null;
                List<ClientParameter> TargetOSBuildList = clientParameters.FindAll(x => x.ParameterName == "$TargetOSbuild");
                if (TargetOSBuildList.Count == 1)
                {
                    TargetOSBuild = TargetOSBuildList.First();
                }
                else if (TargetOSBuildList.Count > 1)
                {
                    foreach (ClientParameter toDelete in TargetOSBuildList)
                    {
                        unitOfWork.ClientParameters.Delete(toDelete);
                        unitOfWork.SaveChanges();
                    }
                }

                ClientParameter InstallWim = null;
                List<ClientParameter> InstallWimList = clientParameters.FindAll(x => x.ParameterName == "$InstallWIM");
                if (InstallWimList.Count == 1)
                {
                    InstallWim = InstallWimList.First();
                }
                else if (InstallWimList.Count > 1)
                {
                    foreach (ClientParameter toDelete in InstallWimList)
                    {
                        unitOfWork.ClientParameters.Delete(toDelete);
                        unitOfWork.SaveChanges();
                    }
                }

                ClientParameter UnattendFile = null;
                List<ClientParameter> UnattendFileList = clientParameters.FindAll(x => x.ParameterName == "$UnattendFile");
                if (UnattendFileList.Count == 1)
                {
                    UnattendFile = UnattendFileList.First();
                }
                else if (UnattendFileList.Count > 1)
                {
                    foreach (ClientParameter toDelete in UnattendFileList)
                    {
                        unitOfWork.ClientParameters.Delete(toDelete);
                        unitOfWork.SaveChanges();
                    }
                }

                List<ClientParameter> PartitionUnattendFileList = clientParameters.FindAll(x => x.ParameterName == "$PartitionLayoutFile");
                ClientParameter PartitionUnattendFile = null;
                if (PartitionUnattendFileList.Count == 1)
                {
                    PartitionUnattendFile = PartitionUnattendFileList.First();
                } else if (PartitionUnattendFileList.Count > 1)
                {
                    foreach (ClientParameter toDelete in PartitionUnattendFileList)
                    {
                        unitOfWork.ClientParameters.Delete(toDelete);
                        unitOfWork.SaveChanges();
                    }
                }


                if (PathToImage == null)
                {
                    // Create new parameter
                    PathToImage = new ClientParameter();
                    PathToImage.ParameterName = "$Path2Image";
                    PathToImage.IsEditable = false;
                    PathToImage.Value = customerImageStream.SubFolderName;
                    PathToImage.ClientId = clientId;
                    unitOfWork.ClientParameters.MarkForInsert(PathToImage);
                } else
                {
                    // Update Parameter
                    PathToImage.Value = customerImageStream.SubFolderName;
                    unitOfWork.ClientParameters.MarkForUpdate(PathToImage);
                }

                if (TargetOSBuild == null)
                {
                    TargetOSBuild = new ClientParameter();
                    TargetOSBuild.ParameterName = "$TargetOSbuild";
                    TargetOSBuild.IsEditable = false;
                    TargetOSBuild.Value = customerImageStream.SubFolderName;
                    TargetOSBuild.ClientId = clientId;
                    unitOfWork.ClientParameters.MarkForInsert(TargetOSBuild);
                } else
                {
                    TargetOSBuild.Value = customerImageStream.SubFolderName;
                    unitOfWork.ClientParameters.MarkForUpdate(TargetOSBuild);
                }

                if (InstallWim == null)
                {
                    // Create new parameter
                    InstallWim = new ClientParameter();
                    InstallWim.ParameterName = "$InstallWIM";
                    InstallWim.IsEditable = false;
                    InstallWim.Value = customerImage.FileName;
                    InstallWim.ClientId = clientId;
                    unitOfWork.ClientParameters.MarkForInsert(InstallWim);
                }
                else
                {
                    // Update Parameter
                    InstallWim.Value = customerImage.FileName;
                    unitOfWork.ClientParameters.MarkForUpdate(InstallWim);
                }
                if (customerImage.Unattend != null)
                {
                    if (UnattendFile == null)
                    {
                        // Create new parameter
                        UnattendFile = new ClientParameter();
                        UnattendFile.ParameterName = "$UnattendFile";
                        UnattendFile.IsEditable = false;
                        UnattendFile.Value = customerImage.Unattend.Name;
                        UnattendFile.ClientId = clientId;
                        unitOfWork.ClientParameters.MarkForInsert(UnattendFile);
                    }
                    else
                    {
                        // Update Parameter
                        UnattendFile.Value = customerImage.Unattend.Name;
                        unitOfWork.ClientParameters.MarkForUpdate(UnattendFile);
                    }
                }
                if (customerImage.OEMPartition != null)
                {
                    if (PartitionUnattendFile == null)
                    {
                        // Create new parameter
                        PartitionUnattendFile = new ClientParameter();
                        PartitionUnattendFile.IsEditable = false;
                        PartitionUnattendFile.ParameterName = "$PartitionLayoutFile";
                        PartitionUnattendFile.Value = customerImage.OEMPartition.Name;
                        PartitionUnattendFile.ClientId = clientId;
                        unitOfWork.ClientParameters.MarkForInsert(PartitionUnattendFile);
                    }
                    else
                    {
                        // Update Parameter
                        PartitionUnattendFile.Value = customerImage.OEMPartition.Name;
                        unitOfWork.ClientParameters.MarkForUpdate(PartitionUnattendFile);
                    }
                }
                // Set OS Settings
                if (customerImageStream.Type == "Windows")
                {
                    client.TimeZoneWindows = data.TimeZoneWindows;
                    client.KeyboardLayoutWindows = data.KeyboardLayoutWindows;
                    client.KeyboardLayoutLinux = null;
                    client.TimeZoneLinux = null;
                    client.LanguagePackLinux = null;
                } else if (customerImageStream.Type == "Linux")
                {
                    client.KeyboardLayoutLinux = data.KeyboardLayoutLinux;
                    client.TimeZoneLinux = data.TimeZoneLinux;
                    client.TimeZoneWindows = null;
                    client.KeyboardLayoutWindows = null;
                    client.LanguagePackLinux = data.LanguagePackLinux;
                }
                client.InstallScript = data.InstallScript;

                // Set Windows Local Admin Credentials
                if (data.LocalAdminPassword != null)
                {
                    client.LocalAdminPassword = EncryptString(data.LocalAdminPassword);
                }
                if (data.LocalAdminUsername != null)
                {
                    client.LocalAdminUsername = EncryptString(data.LocalAdminUsername);
                }

                // Generating initial passwords for client
                if (client.UserPasswordLinux == null)
                {
                    string userPW = RandomPassword();
                    client.UserPasswordLinux = EncryptString(userPW);
                }
                if (client.AdminPasswordLinux == null)
                {
                    string adminPW = RandomPassword();
                    client.AdminPasswordLinux = EncryptString(adminPW);
                }
                if (client.PartitionEncryptionPassLinux == null)
                {
                    string partitionPW = RandomPassword();
                    client.PartitionEncryptionPassLinux = EncryptString(partitionPW);
                }
                client.UsernameLinux = data.UsernameLinux;

                client.OSSettingsImageId = customerImage.Id;

                unitOfWork.Clients.MarkForUpdate(client, GetCurrentUser().Id);
                unitOfWork.SaveChanges();
                return Ok();
            }
        }

        [HttpGet]
        [Route("osSettings/{clientId}")]
        public IActionResult GetOsSettings([FromRoute] string clientId)
        {
            DATA.Client client = UnitOfWork.Clients.Get(clientId);
            if (client.UserPasswordLinux != null)
            {
                client.UserPasswordLinux = DecryptString(client.UserPasswordLinux);
            }
            if (client.AdminPasswordLinux != null)
            {
                client.AdminPasswordLinux = DecryptString(client.AdminPasswordLinux);
            }
            if (client.PartitionEncryptionPassLinux != null)
            {
                client.PartitionEncryptionPassLinux = DecryptString(client.PartitionEncryptionPassLinux);
            }
            if (client.LocalAdminPassword != null)
            {
                client.LocalAdminPassword = DecryptString(client.LocalAdminPassword);
            }
            if (client.LocalAdminUsername != null)
            {
                client.LocalAdminUsername = DecryptString(client.LocalAdminUsername);
            }

            var json = JsonConvert.SerializeObject(client, _serializerSettings);
            return Ok(json);
        }

        [HttpGet]
        [Authorize(Policy = Constants.Roles.Customer)]
        [Route("datasheets")]
        public IActionResult getAllClientDatasheets([FromRoute] string customerId)
        {
            List<DATA.Client> dbEntries = UnitOfWork.Clients.GetAll(ClientIncludes.GetAllIncludes()).Where(x => x.CustomerId == customerId).ToList();
            List<DatasheetViewModel> datasheetList = Mapper.Map<List<DATA.Client>, List<DatasheetViewModel>>(dbEntries);
            var json = JsonConvert.SerializeObject(datasheetList, _serializerSettings);
            return new OkObjectResult(json);
        }

        [HttpPost]
        [Authorize(Policy = Constants.Policies.Customer)]
        [Route("add-parameter")]
        public IActionResult AddCustomClientParameter([FromBody] ClientParameterViewModel parameterData)
        {
            DATA.ClientParameter cp = UnitOfWork.ClientParameters.GetAll().Where(x => x.ParameterName == parameterData.ParameterName && x.ClientId == parameterData.ClientId)
                .FirstOrDefault();
            if (cp != null)
            {
                return BadRequest("The parameter with the name '" + parameterData.ParameterName +
                                  "' does already exist. Please rename it.");
            }

            cp = UnitOfWork.ClientParameters.CreateEmpty();
            cp.ParameterName = parameterData.ParameterName;
            cp.Value = parameterData.Value;
            cp.IsEditable = true;
            cp.ClientId = parameterData.ClientId;

            try
            {
                UnitOfWork.SaveChanges();
            }
            catch (Exception e)
            {
                return BadRequest("The parameter could not be created. " + e.Message);
            }

            cp = UnitOfWork.ClientParameters.Get(cp.Id, "Client");
            var json = JsonConvert.SerializeObject(Mapper.Map<DATA.ClientParameter, ClientParameterViewModel>(cp),
                _serializerSettings);
            return new OkObjectResult(json);
        }

        [Route("edit-parameter")]
        [Authorize(Policy = Constants.Policies.Customer)]
        [HttpPost]
        public IActionResult EditDeviceParameter([FromBody] ClientParameterViewModel payload)
        {
            DATA.ClientParameter toEdit = UnitOfWork.ClientParameters.GetOrNull(payload.Id, "Client");
            if (toEdit == null)
            {
                return BadRequest("The parameter does not exist");
            }

            toEdit.ParameterName = payload.ParameterName;
            toEdit.Value = payload.Value;

            try
            {
                UnitOfWork.SaveChanges();
            }
            catch (Exception e)
            {
                return BadRequest("The parameter could not be edited. " + e.Message);
            }

            var json = JsonConvert.SerializeObject(Mapper.Map<DATA.ClientParameter, ClientParameterViewModel>(toEdit),
                _serializerSettings);
            return new OkObjectResult(json);
        }


        /// <summary>
        /// Add a client to the Customer.
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="clientAdd"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = Constants.Roles.Customer)]
        [Route("{userId}")]
        public IActionResult AddClients([FromRoute] string customerId, [FromRoute] string userId, [FromBody] WPM_API.Models.ClientAddViewModelWeb clientAdd)
        {
            try
            {
                if (userId != null)
                {
                    using (var unitOfWork = CreateUnitOfWork())
                    {
                        DATA.Subnet subnet = unitOfWork.Subnets.GetOrNull(clientAdd.Subnet);
                        DATA.User loggedInUser = unitOfWork.Users.GetWithRolesOrNull(userId);
                        DATA.Client fetchedClient = null;
                        if (clientAdd.Uuid != null && clientAdd.Uuid != "" && clientAdd.SerialNumber != null && clientAdd.SerialNumber != "")
                        {
                            // TODO: Search for clients with same serial, uuid and mac addresses
                            var fetchedClients = unitOfWork.Clients.GetAll(ClientIncludes.GetAllIncludes()).Where(x => x.UUID == clientAdd.Uuid && x.SerialNumber == clientAdd.SerialNumber).ToList();
                            foreach (DATA.Client client in fetchedClients)
                            {
                                List<MacAddress> macAddresses = unitOfWork.MacAddresses.GetAll().Where(x => x.ClientId == client.Id).ToList();
                                if (macAddresses.Count() != 0)
                                {
                                    foreach (MacAddress mac in macAddresses)
                                    {
                                        if (!clientAdd.MacAddresses.Contains(mac.Address))
                                        {
                                            fetchedClients.Remove(client);
                                        }
                                    }                                    
                                }
                            }
                            if (!(fetchedClients.Count() == 0 || fetchedClients.Count() > 1))
                            {
                                fetchedClient = fetchedClients.First();
                            }
                        }
                        if (fetchedClient == null)
                        {
                            ClientViewModel client = new ClientViewModel();
                            DATA.Client newClient = null;
                            if (clientAdd.BaseId.Length > 0)
                            {
                                newClient = new DATA.Client
                                {
                                    Description = clientAdd.Description,
                                    Name = clientAdd.Name,
                                    UUID = clientAdd.Uuid,
                                    CustomerId = customerId,
                                    BaseId = clientAdd.BaseId,
                                    Timezone = clientAdd.Timezone,
                                    // PreinstalledSoftwares = new List<DATA.SmartDeploy.PreinstalledSoftware>()
                                };
                                if (subnet != null)
                                {
                                    newClient.Subnet = subnet.Name;
                                }
                            }
                            else
                            {
                                newClient = new DATA.Client
                                {
                                    Description = clientAdd.Description,
                                    Name = clientAdd.Name,
                                    UUID = clientAdd.Uuid,
                                    CustomerId = customerId
                                };
                            }

                            newClient.SerialNumber = clientAdd.SerialNumber;                            
    
                            unitOfWork.Clients.MarkForInsert(newClient, GetCurrentUser().Id);
                            unitOfWork.SaveChanges();

                            newClient = unitOfWork.Clients.Get(newClient.Id, "Base");
                            foreach (string mac in clientAdd.MacAddresses)
                            {
                                MacAddress newMac = new MacAddress();
                                newMac.Address = mac;
                                newMac.ClientId = newClient.Id;
                                unitOfWork.MacAddresses.MarkForInsert(newMac);
                                unitOfWork.SaveChanges();
                            }
                            client = Mapper.Map<DATA.Client, ClientViewModel>(newClient);
                            var FixedParams = GenerateFixedClientParams(newClient);
                            foreach (var param in FixedParams)
                            {
                                param.ClientId = newClient.Id;
                                param.Client = newClient;
                                unitOfWork.ClientParameters.MarkForInsert(param);
                            }
                            unitOfWork.SaveChanges();

                            var json = JsonConvert.SerializeObject(client, _serializerSettings);
                            return new OkObjectResult(json);
                        }
                        else
                        {
                            string errorMsg = "ERROR: client is already registered";
                            bool isAdmin = false;
                            bool isSysHouseAdmin = false;
                            bool isCustomerAdmin = false;
                            if (loggedInUser.Admin)
                            {
                                isAdmin = true;
                                errorMsg += "in Customer: " + fetchedClient.Customer.Name;
                            }
                            else
                            {
                                List<DATA.UserRole> userRoles = loggedInUser.UserRoles;
                                for (int i = 0; i < userRoles.Count(); i++)
                                {
                                    DATA.UserRole tempRole = userRoles[i];
                                    if (tempRole.Role.Name == "customer")
                                    {
                                        isCustomerAdmin = true;
                                    }
                                    else if (tempRole.Role.Name == "systemhouse")
                                    {
                                        isSysHouseAdmin = true;
                                    }
                                }
                                if (isSysHouseAdmin && !isAdmin)
                                {
                                    if (loggedInUser.Systemhouse.Customer.Contains(fetchedClient.Customer))
                                    {
                                        errorMsg += "in Customer: " + fetchedClient.Customer.Name;
                                    }
                                }
                                else if (isCustomerAdmin && !isAdmin)
                                {
                                    if (fetchedClient.Customer == loggedInUser.Customer)
                                    {
                                        errorMsg += "in Customer: " + fetchedClient.Customer.Name;
                                    }
                                }
                            }
                            return BadRequest(errorMsg);
                        }
                    }                    
                } else
                {
                    return BadRequest("ERROR: The user does not exist");
                }
            }
            catch (Exception e)
            {
                return BadRequest("The client could not be added. " + e.Message);
            }

        }

        private List<DATA.ClientParameter> GenerateFixedClientParams(DATA.Client client)
        {
            var VM = UnitOfWork.VirtualMachines.GetAll().FirstOrDefault(x => x.Name == client.Name);
            var FixedParams = new List<DATA.ClientParameter>();
            FixedParams.Add(new DATA.ClientParameter() { ParameterName = "$ComputerTargetName", Value = client.Name, IsEditable = false });
            FixedParams.Add(new DATA.ClientParameter() { ParameterName = "$LocalAdminName", Value = (VM != null ? VM.AdminUserName : null), IsEditable = true });
            FixedParams.Add(new DATA.ClientParameter() { ParameterName = "$LocalAdminPw", Value = (VM != null ? VM.AdminUserPassword : null), IsEditable = true });
            // FixedParams.Add(new DATA.ClientParameter() { ParameterName = "$SubnetName", Value = (VM != null ? VM.Subnet : client.Subnet), IsEditable = false });
            FixedParams.Add(new DATA.ClientParameter() { ParameterName = "$DataDriveLetter", Value = null, IsEditable = true });
            return FixedParams;

        }

        /// <summary>
        /// Edit a client for the Customer.
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="clientEdit"></param>
        /// <returns></returns>
        [HttpPut]
        [Authorize(Policy = Constants.Roles.Customer)]
        public IActionResult UpdateClients([FromRoute] string customerId, [FromBody] ClientViewModel clientEdit)
        {
            try
            {
                DATA.Client dbClient = null;
                using (var unitOfWork = CreateUnitOfWork())
                {
                    dbClient = unitOfWork.Clients.Get(clientEdit.Id);
                    if (dbClient == null)
                    {
                        return new NotFoundResult();
                    }

                    dbClient.SerialNumber = clientEdit.SerialNumber;                    
                    dbClient.Description = clientEdit.Description;
                    dbClient.UUID = clientEdit.Uuid;
                    dbClient.CustomerId = customerId;
                    dbClient.Name = clientEdit.Name;
                    dbClient.Timezone = clientEdit.Timezone;
                    if (clientEdit.BaseId != null)
                    {
                        if (clientEdit.BaseId.Length > 0)
                        {
                            dbClient.BaseId = clientEdit.BaseId;
                        }
                    }

                    // Change parameter values
                    List<DATA.ClientParameter> parameters = unitOfWork.ClientParameters.GetAll().Where(x => x.ClientId == dbClient.Id).ToList();
                    foreach (DATA.ClientParameter parameter in parameters)
                    {
                        if (parameter.ParameterName.Equals("$ComputerTargetName"))
                        {
                            parameter.Value = clientEdit.Name;
                            // unitOfWork.SaveChanges();
                        }
                    }

                    if (parameters.Find(x => x.ParameterName == "$ComputerTargetName") == null)
                    {
                        DATA.ClientParameter computerNameParam = new DATA.ClientParameter() { ParameterName = "$ComputerTargetName", Value = clientEdit.Name, IsEditable = false };
                        computerNameParam.ClientId = clientEdit.Id;
                        computerNameParam.Client = dbClient;
                        unitOfWork.ClientParameters.MarkForInsert(computerNameParam);
                    }

                    unitOfWork.Clients.MarkForUpdate(dbClient, GetCurrentUser().Id);
                    unitOfWork.SaveChanges();

                    List<MacAddress> macAddresses = unitOfWork.MacAddresses.GetAll().Where(x => x.ClientId == dbClient.Id).ToList();
                    foreach (MacAddress mac in macAddresses)
                    {
                        unitOfWork.MacAddresses.Delete(mac);
                        unitOfWork.SaveChanges();
                    }

                    foreach (string mac in clientEdit.MacAddresses)
                    {
                        MacAddress newMac = new MacAddress();
                        newMac.ClientId = dbClient.Id;
                        newMac.Address = mac;

                        unitOfWork.MacAddresses.MarkForInsert(newMac);
                        unitOfWork.SaveChanges();
                    }

                    dbClient = unitOfWork.Clients.Get(dbClient.Id, "Base");
                }

                ClientViewModel client = new ClientViewModel();
                //  DATA.Client updatedClient = new DATA.Client() { Description = clientEdit.Description, Name = clientEdit.Name, UUID = clientEdit.Uuid, CustomerId = customerId };

                //   UnitOfWork.Clients.MarkForUpdate(updatedClient, GetCurrentUser().Id);
                //  UnitOfWork.SaveChanges();

                client = Mapper.Map<DATA.Client, ClientViewModel>(dbClient);
                // Serialize and return the response
                var json = JsonConvert.SerializeObject(client, _serializerSettings);
                return new OkObjectResult(json);
            } catch (Exception e)
            {
                return BadRequest("ERROR: " + e.Message);
            }
        }

        [HttpGet]
        [Route("{clientId}/match-parameters")]
        public IActionResult MatchDeviceParameters([FromBody] ParameterNames parameterNames,
            [FromRoute] string clientId)
        {
            ParameterKeyValueModel result = new ParameterKeyValueModel();
            List<DATA.ClientParameter> parameters = UnitOfWork.ClientParameters.GetAll("Client")
                .Where(x => x.ClientId == clientId).ToList();
            foreach (DATA.ClientParameter param in parameters)
            {
                if (parameterNames.ParamNames.Contains(param.ParameterName))
                {
                    result.Parameters.Add(new ParameterModel(param.ParameterName, param.Value));
                }
            }

            var json = JsonConvert.SerializeObject(result, _serializerSettings);
            return new OkObjectResult(json);
        }

        [HttpGet]
        [Route("{clientId}/activityLog")]
        [Authorize(Policy = Constants.Policies.Customer)]
        public IActionResult GetActivityLog([FromRoute] string clientId)
        {
            DATA.Client client = UnitOfWork.Clients.GetOrNull(clientId, "ActivityLogs");
            if (client == null)
            {
                return BadRequest("ERROR: The client does not exist");
            }

            List<ActivityLogViewModel> result = Mapper.Map<List<ActivityLogViewModel>>(client.ActivityLogs);

            var json = JsonConvert.SerializeObject(result, _serializerSettings);

            return Ok(json);
        }

        [HttpPost]
        [Route("{clientId}/upload/unattend")]
        [Authorize(Policy = Constants.Roles.Customer)]
        public async Task<IActionResult> UploadUnattendAsync([FromRoute] string clientId, [FromForm] IFormFile file)
        {
            DATA.Client clientDbDatasheet = null;
            var unitOfWork = CreateUnitOfWork();
            clientDbDatasheet = UnitOfWork.Clients.Get(clientId, ClientIncludes.GetAllIncludes());
            if (clientDbDatasheet == null)
                return new NotFoundResult();
            FileRepository.FileRepository unattend =
                new FileRepository.FileRepository(_connectionStrings.FileRepository, _appSettings.FileRepositoryFolder);
            if (clientDbDatasheet.Unattend != null)
                await unattend.DeleteFile(clientDbDatasheet.Unattend);
            string id = await unattend.UploadFile(file.OpenReadStream());
            clientDbDatasheet.Unattend = id;
            unitOfWork.Clients.MarkForUpdate(clientDbDatasheet, clientId);
            unitOfWork.SaveChanges();
            var json = JsonConvert.SerializeObject(new { Id = id }, _serializerSettings);
            return new OkObjectResult(json);
        }

        [HttpPut]
        [Route("{clientId}/{wdsip}")]
        [Authorize(Policy = Constants.Roles.Customer)]
        public IActionResult UpdateWdsIp([FromRoute] string clientId, [FromRoute] string WdsIp)
        {
            DATA.Client clientDbDatasheet = null;
            var unitOfWork = CreateUnitOfWork();
            clientDbDatasheet = UnitOfWork.Clients.Get(clientId, ClientIncludes.GetAllIncludes());
            if (clientDbDatasheet == null)
                return new NotFoundResult();
            clientDbDatasheet.WdsIp = WdsIp;
            unitOfWork.Clients.MarkForUpdate(clientDbDatasheet, clientId);
            unitOfWork.SaveChanges();
            ClientViewModel updatedClient = new ClientViewModel();
            updatedClient = Mapper.Map<DATA.Client, ClientViewModel>(clientDbDatasheet);
            var json = JsonConvert.SerializeObject(updatedClient, _serializerSettings);
            return new OkObjectResult(json);
        }

        /// <summary>
        /// Retrieve assigned Software.
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="clientId"></param>
        /// <returns></returns>
        [Route("{clientId}/software")]
        [HttpGet]
        [Authorize(Policy = Constants.Roles.Customer)]
        public IActionResult GetSoftware([FromRoute] string customerId, [FromRoute] string clientId)
        {
            List<ClientViewModel> clients = new List<ClientViewModel>();
            var clientSoftware = UnitOfWork.Clients.Get(clientId, "AssignedSoftware", "AssignedSoftware.CustomerSoftware")
                .AssignedSoftware;
            List<SoftwareAssignViewModel> software = new List<SoftwareAssignViewModel>();
            if (clientSoftware != null)
            {
                clientSoftware.ForEach(x =>
                {
                    SoftwareAssignViewModel s = Mapper.Map<SoftwareAssignViewModel>(x.CustomerSoftware);
                    s.Required = x.Install == true ? true : false;
                    software.Add(s);
                });
            }

            // Serialize and return the response
            var json = JsonConvert.SerializeObject(software, _serializerSettings);
            return new OkObjectResult(json);
        }

        /// <summary>
        /// Assign new Software to Client.
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="clientId"></param>
        /// <param name="softwareRef"></param>
        /// <returns></returns>
        [Route("{clientId}/software")]
        [HttpPost]
        [Authorize(Policy = Constants.Roles.Customer)]
        public IActionResult AssignSoftware([FromRoute] string customerId, [FromRoute] string clientId,
            [FromBody] SoftwareAssignRefViewModel softwareAssign)
        {
            // Create new Object
            DATA.ClientSoftware clientSoftware = new DATA.ClientSoftware
            {
                CustomerSoftwareId = softwareAssign.Id,
                ClientId = clientId,
                Install = softwareAssign.Required
            };

            var client = UnitOfWork.Clients.Get(clientId, "AssignedSoftware");
            if (client.AssignedSoftware == null)
            {
                client.AssignedSoftware = new List<DATA.ClientSoftware>();
            }
            else
            {
                if (client.AssignedSoftware.Where(x => x.CustomerSoftwareId.Equals(softwareAssign.Id)).Count() > 0)
                {
                    var assignedSoftware = client.AssignedSoftware.First(x => x.CustomerSoftwareId.Equals(softwareAssign.Id));
                    // Remove to add with new changes again.
                    client.AssignedSoftware.Remove(assignedSoftware);
                }
            }

            client.AssignedSoftware.Add(clientSoftware);
            UnitOfWork.Clients.MarkForUpdate(client, GetCurrentUser().Id);
            UnitOfWork.SaveChanges();
            InstallSoftware(customerId, clientId, softwareAssign.Id);

            // Serialize and return the response
            var json = JsonConvert.SerializeObject(Mapper.Map<ClientViewModel>(client), _serializerSettings);
            return new OkObjectResult(json);
        }

        /// <summary>
        /// Delete a Clients assigned software.
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="clientId"></param>
        /// <param name="softwareRef"></param>
        /// <returns></returns>
        [Route("{clientId}/software/{softwareId}")]
        [HttpDelete]
        [Authorize(Policy = Constants.Roles.Customer)]
        public IActionResult UnassignSoftware([FromRoute] string customerId, [FromRoute] string clientId,
            [FromRoute] string softwareId)
        {
            var client = UnitOfWork.Clients.Get(clientId, "AssignedSoftware");
            var deleteSoftware = client.AssignedSoftware.Find(x => x.CustomerSoftwareId == softwareId);
            client.AssignedSoftware.Remove(deleteSoftware);
            UnitOfWork.Clients.MarkForUpdate(client, GetCurrentUser().Id);
            UnitOfWork.SaveChanges();
            /*
            if (softwareAssign.Required)
            {
                UninstallSoftware(customerId, clientId, softwareId);
            }
            */
            // Serialize and return the response
            var json = JsonConvert.SerializeObject(Mapper.Map<ClientViewModel>(client), _serializerSettings);
            return new OkObjectResult(json);
        }

        /// <summary>
        /// Get the consigned datasheet for a client.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{clientId}")]
        [Authorize(Policy = Constants.Roles.Customer)]
        public IActionResult GetClientDatasheet([FromRoute] string clientId)
        {
            DatasheetViewModel result = new DatasheetViewModel { Categories = new List<DatasheetEntryViewModel>() };
            DATA.Client client = UnitOfWork.Clients.Get(clientId, ClientIncludes.GetAllIncludes());
            if (client == null)
            {
                return new NotFoundResult();
            }
            // Get client properties
            List<DATA.Category> categories = UnitOfWork.Categories.GetAll()
                .Where(x => x.Type.Equals(DATA.CategoryType.DeviceProperty)).ToList();
            foreach (DATA.Category c in categories)
            {
                List<DATA.ClientClientProperty> props =
                    client.Properties.FindAll(x => x.ClientProperty.Category.Name.Equals(c.Name)).ToList();
                List<PropertyResultViewModel> p = new List<PropertyResultViewModel>();
                props.ForEach(x => p.Add(new PropertyResultViewModel { Name = x.ClientProperty.PropertyName, Value = x.Value }));
                //if (p.Count > 0)
                //{
                result.Categories.Add(new DatasheetEntryViewModel { Category = c.Name, Values = p });
                //}
            }

            // Serialize and return the response
            var json = JsonConvert.SerializeObject(result, _serializerSettings);
            return new OkObjectResult(json);
        }

        private void InstallSoftware(string customerId, string clientId, string softwareId)
        {
            // set Client Task
            using (var unitOfWork = CreateUnitOfWork())
            {
                if (!unitOfWork.Clients.Get(clientId, "AssignedSoftware").AssignedSoftware
                    .Exists(x => x.CustomerSoftwareId == softwareId))
                {
                    throw new Exception("Software is not assigned");
                }

                CustomerSoftware customerSoftware = unitOfWork.CustomerSoftwares.Get(softwareId, "RuleDetection", "RuleApplicability", "TaskInstall", "TaskInstall.ExecutionFile", "TaskInstall.Files");

                if (customerSoftware.TaskInstall == null)
                {
                    throw new Exception("Software dont have an installtask.");
                }

                unitOfWork.Clients.AddTaskById(customerSoftware.TaskInstall.Id, clientId, "software", GetCurrentUser().Id);
                unitOfWork.SaveChanges();
            }
        }

        private void UninstallSoftware([FromRoute] string customerId, [FromRoute] string clientId,
            [FromRoute] string softwareId)
        {
            // set Client Task
            using (var unitOfWork = CreateUnitOfWork())
            {
                if (!unitOfWork.Clients.Get(clientId, "AssignedSoftware").AssignedSoftware
                    .Exists(x => x.CustomerSoftwareId == softwareId))
                {
                    throw new Exception("Software is not assigned");
                }

                var software = unitOfWork.Software.Get(softwareId, SoftwareIncludes.GetAllTasks());

                unitOfWork.Clients.AddTaskById(software.TaskUninstall.Id, clientId, "software", GetCurrentUser().Id);
            }
        }

        /// <summary>
        /// Assign new DeviceOption to Client.
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="clientId"></param>
        /// <param name="optionAssign"></param>
        /// <returns></returns>
        [Route("{clientId}/options")]
        [HttpPost]
        [Authorize(Policy = Constants.Roles.Customer)]
        public IActionResult AssignDeviceOptions([FromRoute] string customerId, [FromRoute] string clientId,
            [FromBody] List<OptionAssignRefViewModel> optionAssign)
        {
            using (var unitOfWork = CreateUnitOfWork())
            {
                var client = unitOfWork.Clients.Get(clientId, "AssignedOptions");
                bool PEOnly = false;
                string OSType = String.Empty;
                
                client.AssignedOptions = new List<DATA.ClientOption>();
                if (optionAssign != null)
                {
                    foreach (var option in optionAssign)
                    {
                        List<Script> scripts = unitOfWork.Scripts.GetAll("Versions").ToList();
                        foreach (Script script in scripts)
                        {
                            var temp = script.Versions.Find(x => x.Id == option.Id);
                            if (temp != null)
                            {
                                PEOnly = script.PEOnly;
                                OSType = script.OSType;
                                break;
                            }
                        }
                        DATA.ClientOption clientOption = new DATA.ClientOption
                        {
                            DeviceOptionId = option.Id,
                            ClientId = clientId,
                            Order = option.Order,
                            Parameters = Mapper.Map<List<DATA.Parameter>>(option.Parameters),
                            PEOnly = PEOnly,
                            OSType = OSType
                        };
                        client.AssignedOptions.Add(clientOption);
                    }

                    
                }
                unitOfWork.Clients.MarkForUpdate(client, GetCurrentUser().Id);
                unitOfWork.SaveChanges();

                // Serialize and return the response
                var json = JsonConvert.SerializeObject(Mapper.Map<ClientViewModel>(client), _serializerSettings);
                return new OkObjectResult(json);
            }            
        }
    

        [HttpPost]
        [Route("{clientId}/assignAdminOption")]
        public IActionResult AssignAdminOption ([FromRoute] string customerId, [FromRoute] string clientId,
            [FromBody] OptionAssignRefViewModel option)
        {
            using (var unitOfWork = CreateUnitOfWork())
            {
                string OSType = null;
                bool peOnly = false;
                var client = unitOfWork.Clients.Get(clientId, "AssignedOptions", "AssignedOptions.Parameters");
                if (option != null)
                {
                    List<AdminDeviceOption> adminOptions = unitOfWork.AdminOptions.GetAll("Versions").ToList();
                    foreach (AdminDeviceOption adminOption in adminOptions)
                    {
                        var temp = adminOption.Versions.Find(x => x.Id == option.ScriptVersionId);
                        if (temp != null)
                        {
                            peOnly = adminOption.PEOnly;
                            OSType = adminOption.OSType;
                            break;
                        }
                    }
                    DATA.ClientOption clientOption = new DATA.ClientOption
                    {
                        DeviceOptionId = option.ScriptVersionId,
                        ClientId = clientId,
                        Order = option.Order,
                        Parameters = Mapper.Map<List<DATA.Parameter>>(option.Parameters),
                        PEOnly = peOnly,
                        OSType = OSType
                    };

                    client.AssignedOptions.Add(clientOption);
                    unitOfWork.Clients.MarkForUpdate(client, GetCurrentUser().Id);
                    unitOfWork.SaveChanges();
                    return Ok();
                } else
                {
                    return BadRequest("ERROR: No option sent in request");
                }
            }
        }

        /// <summary>
        /// Retrieve assigned DeviceOption.
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="clientId"></param>
        /// <returns></returns>
        [Route("{clientId}/options")]
        [HttpGet]
        [Authorize(Policy = Constants.Roles.Customer)]
        public IActionResult GetOptions([FromRoute] string customerId, [FromRoute] string clientId)
        {
            List<ClientViewModel> clients = new List<ClientViewModel>();
            var clientOptions = UnitOfWork.Clients.Get(clientId, "AssignedOptions", "AssignedOptions.DeviceOption").AssignedOptions;
            List<OptionAssignViewModel> options = new List<OptionAssignViewModel>();
            clientOptions?.ForEach(x =>
            {
                OptionAssignViewModel o = new OptionAssignViewModel
                { Id = x.DeviceOption.Id, Name = x.DeviceOption.Name, Version = x.DeviceOption.Number.ToString() };
                options.Add(o);
            });
            // Serialize and return the response
            var json = JsonConvert.SerializeObject(options, _serializerSettings);
            return new OkObjectResult(json);
        }

        /// <summary>
        /// Delete a Clients assigned software.
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="clientId"></param>
        /// <param name="softwareRef"></param>
        /// <returns></returns>
        [Route("{clientId}/options/{optionId}")]
        [HttpDelete]
        [Authorize(Policy = Constants.Roles.Customer)]
        public IActionResult UnassignOption([FromRoute] string customerId, [FromRoute] string clientId,
            [FromRoute] string optionId)
        {
            var client = UnitOfWork.Clients.Get(clientId, "AssignedOptions");
            var deleteOption = client.AssignedOptions.Find(x => x.DeviceOptionId == optionId);
            client.AssignedOptions.Remove(deleteOption);
            UnitOfWork.Clients.MarkForUpdate(client, GetCurrentUser().Id);
            UnitOfWork.SaveChanges();
            /*
            if (softwareAssign.Required)
            {
                UninstallSoftware(customerId, clientId, softwareId);
            }
            */
            // Serialize and return the response
            var json = JsonConvert.SerializeObject(Mapper.Map<ClientViewModel>(client), _serializerSettings);
            return new OkObjectResult(json);
        }

        [Route("vms")]
        [Authorize(Policy = Constants.Policies.Customer)]
        [HttpGet]
        public IActionResult GetClientsAndVMs([FromRoute] string customerId)
        {
            // Fetch Azure credentials
            DATA.CloudEntryPoint creds = GetCEP(customerId);
            if (creds == null)
            {
                return BadRequest("There was an error with your credentials");
            }

            // Fetch clients of customer
            List<DATA.Client> clients = UnitOfWork.Clients.GetAll().Where(x => x.CustomerId == customerId).ToList();
            // Fetch VMs of customer via bases
            List<DATA.Base> bases = UnitOfWork.Bases.GetAll("VirtualMachines")
                .Where(x => x.CredentialsId == null)
                .ToList();
            List<DATA.VirtualMachine> vms = new List<DATA.VirtualMachine>();
            foreach (DATA.Base tempBase in bases)
            {
                vms.AddRange(tempBase.VirtualMachines);
            }

            // Serialize and return result
            ClientsAndVMs resultModel = new ClientsAndVMs();
            resultModel.VMs = Mapper.Map<List<VirtualMachineViewModel>>(vms);
            resultModel.Clients = Mapper.Map<List<ClientViewModel>>(clients);
            var json = JsonConvert.SerializeObject(resultModel, _serializerSettings);
            return new OkObjectResult(json);
        }

        [Route("{clientId}/get-parameters")]
        [Authorize(Policy = Constants.Policies.Customer)]
        [HttpGet]
        public IActionResult GetDeviceParameters([FromRoute] string clientId)
        {
            List<DATA.ClientParameter> parameters = UnitOfWork.ClientParameters.GetAll("Client")
                .Where(x => x.ClientId == clientId).ToList();
            var json = JsonConvert.SerializeObject(
                Mapper.Map<List<DATA.ClientParameter>, List<ClientParameterViewModel>>(parameters),
                _serializerSettings);
            return new OkObjectResult(json);
        }

        /// <summary>
        /// Retrieve parameter of a particular script version prefilled with information from this client and/or the base.
        /// Currently always uses the first version of a script. 
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="clientId"></param>
        /// <param name="optionId"></param>
        /// <returns></returns>
        [Route("{clientId}/options/{optionId}/parameter/{isAdminOption}")]
        [HttpGet]
        [Authorize(Policy = Constants.Roles.Customer)]
        public async Task<IActionResult> GetParameterAsync([FromRoute] string customerId, [FromRoute] string clientId, [FromRoute] string optionId, [FromRoute] bool isAdminOption)
        {
            using (var unitOfWork = CreateUnitOfWork())
            {
                List<ParameterViewModel> parameters = new List<ParameterViewModel>();
                FileRepository.FileRepository repository = new FileRepository.FileRepository(_connectionStrings.FileRepository, _appSettings.FileRepositoryFolder);

                DATA.Client client = unitOfWork.Clients.Get(clientId, ClientIncludes.Properties);
                DATA.Script deviceOption = null;
                DATA.AdminDeviceOption adminDeviceOption = null;
                DATA.ScriptVersion option = unitOfWork.ScriptVersions.Get(optionId);

                if (isAdminOption)
                {
                    List<DATA.AdminDeviceOption> adminDeviceOptions = unitOfWork.AdminOptions.GetAll("Versions").ToList();
                    foreach (DATA.AdminDeviceOption temp in adminDeviceOptions)
                    {
                        if (temp.Versions.Contains(option))
                        {
                            adminDeviceOption = temp;
                            break;
                        }
                    }
                } else
                {
                    List<DATA.Script> deviceOptions = unitOfWork.Scripts.GetAll("Versions").ToList();
                    foreach (DATA.Script temp in deviceOptions)
                    {
                        if (temp.Versions.Contains(option))
                        {
                            deviceOption = temp;
                            break;
                        }
                    }
                }

                BlobDownloadResult downloadResult = await repository.GetBlobFile(option.ContentUrl).DownloadContentAsync();
                string script = downloadResult.Content.ToString();
                // Prefill from Customerx   
                // $AzureBlobRoot, $CSDPContainer, $LtSASread
                DATA.Customer customer = unitOfWork.Customers.GetOrNull(client.CustomerId, "Parameters");
                if (customer != null)
                {
                    List<DATA.Parameter> customerParameters = customer.Parameters;
                    if (isAdminOption)
                    {
                        parameters = ScriptHelper.PrefillParameterWithDefaults(script, customerParameters, parameters, "From Customer", adminDeviceOption.OSType);
                    } else
                    {
                        parameters = ScriptHelper.PrefillParameterWithDefaults(script, customerParameters, parameters, "From Customer", deviceOption.OSType);
                    }                    
                }

                //  Prefill from Base
                if (!string.IsNullOrEmpty(client.BaseId))
                {
                    DATA.Base clientBase = unitOfWork.Bases.Get(client.BaseId, BaseIncludes.AdvancedProperties);
                    if (clientBase != null)
                    {
                        parameters =
                            ScriptHelper.PrefillParameterWithDefaults(script, clientBase.Properties, parameters, "From Base", deviceOption.OSType);
                    }
                }

                List<DATA.ClientParameter> clientParameters = UnitOfWork.ClientParameters.GetAll(ClientParameterIncludes.GetAllIncludes()).Where(x => x.ClientId == clientId).ToList();

                // Prefill from Client
                if (isAdminOption)
                {
                    parameters =
                        ScriptHelper.PrefillParameterWithDefaults(script, clientParameters, parameters, "From Client", adminDeviceOption.OSType);
                } else
                {
                    parameters =
                        ScriptHelper.PrefillParameterWithDefaults(script, clientParameters, parameters, "From Client", deviceOption.OSType);
                }                

                // Serialize and return the response
                var json = JsonConvert.SerializeObject(parameters, _serializerSettings);
                return new OkObjectResult(json);
            }            
        }

        [HttpGet]
        [Route("inventory/{clientId}")]
        [Authorize(Policy = Constants.Policies.Customer)]
        public async Task<IActionResult> GetInventoryData([FromRoute] string clientId)
        {
            // ANSI FILES -> FUTURE PROBLEMS MAYBE
            InventoryModel result = new InventoryModel();
            using (var unitOfWork = CreateUnitOfWork())
            {
                DATA.Client client = unitOfWork.Clients.Get(clientId, ClientIncludes.GetAllIncludes());
                DATA.Customer customer = unitOfWork.Customers.Get(client.CustomerId, CustomerIncludes.GetAllIncludes());
                var options = new JsonDocumentOptions { AllowTrailingCommas = true };
                var cep = GetCEP(customer.Id);
                DATA.Storages.StorageEntryPoint csdp = customer.StorageEntryPoints.Find(x => x.IsCSDP == true);

                // Connect to Azure Storage Account
                AzureCommunicationService azureCustomer;
                if (!csdp.Managed)
                {
                    azureCustomer = new AzureCommunicationService(cep.TenantId, cep.ClientId, cep.ClientSecret);
                }
                else
                {
                    // TODO: Check system; fix for live system
                    azureCustomer = new AzureCommunicationService(_appSettings.DevelopmentTenantId, _appSettings.DevelopmentClientId, _appSettings.DevelopmentClientSecret);
                }
                string connectionString = azureCustomer.StorageService().GetStorageAccConnectionString(csdp.SubscriptionId, csdp.ResourceGrpName, csdp.StorageAccount);
                CloudStorageAccount customerStorageAcc = CloudStorageAccount.Parse(connectionString);
                CloudBlobClient customerClient = customerStorageAcc.CreateCloudBlobClient();

                CloudBlobContainer customerContainer = customerClient.GetContainerReference("csdp");

                // OS Inventory 
                CloudBlockBlob osFile = customerContainer.GetBlockBlobReference("Customer_Repository/BaselineResults/" + clientId + "/" + client.BaseLineFile3);
                if (!await osFile.ExistsAsync())
                {
                    return BadRequest("The os inventory file does not exist in Azure");
                }

                string osContent = await osFile.DownloadTextAsync();

                // Hardware inventory
                CloudBlockBlob inventoryFile = customerContainer.GetBlockBlobReference("Customer_Repository/BaselineResults/" + clientId + "/" + client.BaseLineFile2);
                if (!await inventoryFile.ExistsAsync())
                {
                    return BadRequest("The hardware inventory file does not exist in Azure");
                }
                
                string hwContent = await inventoryFile.DownloadTextAsync();

                // FOR LAST LOGON
                /*
                 * int timeZone = 120;
                   TimeSpan time = TimeSpan.FromMilliseconds(1628084744119);
                   DateTime startdate = new DateTime(1970, 1, 1) + time;
                   startdate = startdate.AddMinutes(timeZone);
                 */

                try
                {
                    JObject hwObject = JObject.Parse(hwContent);
                    
                    foreach (JProperty property in hwObject.Properties())
                    {
                        if (property.Name == "HeaderInfo")
                        {
                            string value = property.Value.ToString();
                            JObject temp = JObject.Parse(value);
                            var timestampProperty = temp.Properties().Where(x => x.Name == "ExecutionTime").FirstOrDefault();
                            string timestamp = timestampProperty.Value.ToString() + " (UTC)";
                            result.Timestamp = timestamp;
                        }
                        // Win32_ComputerSystemProduct for SerialNr and Model
                        if (property.Name == "Win32_ComputerSystemProduct")
                        {
                            string value = property.Value.ToString();
                            JObject temp = JObject.Parse(value);
                            var modelProperty = temp.Properties().Where(x => x.Name == "Version").FirstOrDefault();
                            var serialNumberProperty = temp.Properties().Where(x => x.Name == "IdentifyingNumber").FirstOrDefault();
                            string model = modelProperty.Value.ToString();
                            result.Model = model;
                            string serialNumber = serialNumberProperty.Value.ToString();
                            result.SerialNumber = serialNumber;
                        }

                        // Win32_Processor for CPU
                        if (property.Name == "Win32_Processor")
                        {
                            string value = property.Value.ToString();
                            JObject temp = JObject.Parse(value);
                            var cpuProperty = temp.Properties().Where(x => x.Name == "Name").FirstOrDefault();
                            string cpu = cpuProperty.Value.ToString();
                            result.CPU = cpu;
                        }

                        // Win32_ComputerSystem for DNSHostName and Domain
                        if (property.Name == "Win32_ComputerSystem")
                        {
                            string value = property.Value.ToString();
                            JObject temp = JObject.Parse(value);
                            var domainProperty = temp.Properties().Where(x => x.Name == "Domain").FirstOrDefault();
                            string domain = domainProperty.Value.ToString();
                            result.Domain = domain;
                            var nameProperty = temp.Properties().Where(x => x.Name == "DNSHostName").FirstOrDefault();
                            string name = nameProperty.Value.ToString();
                            result.Name = name;
                            var ramProperty = temp.Properties().Where(x => x.Name == "TotalPhysicalMemory").FirstOrDefault();
                            string ram = ramProperty.Value.ToString();
                            float ramInteger = float.Parse(ram);
                            ramInteger = ramInteger / (1024 * 1024 * 1024);                            
                            double tempDouble = (double)ramInteger;
                            tempDouble = Math.Round(tempDouble, 0);
                            result.RAM = tempDouble.ToString();
                        }
                    }
                }
                catch (Exception e)
                {
                    return BadRequest("ERROR: " + e.Message);
                }



                try
                {
                    JObject osObject = JObject.Parse(osContent);

                    foreach (JProperty property in osObject.Properties())
                    {
                        if (property.Name == "Win32_OperatingSystem")
                        {
                            string value = property.Value.ToString();
                            JObject temp = JObject.Parse(value);
                            var osProperty = temp.Properties().Where(x => x.Name == "Caption").FirstOrDefault();
                            string os = osProperty.Value.ToString();
                            result.OS = os;
                            var versionProperty = temp.Properties().Where(x => x.Name == "Version").FirstOrDefault();
                            string version = versionProperty.Value.ToString();
                            result.Version = version;
                            var lastBootProperty = temp.Properties().Where(x => x.Name == "LastBootUpTime").FirstOrDefault();
                            string lastBootUp = lastBootProperty.Value.ToString() + " (UTC)";
                            result.LastBootUpTime = lastBootUp;
                            // lastBootUp = lastBootUp.Replace()
                        }
                    }
                }
                catch (Exception e)
                {
                    return BadRequest("ERROR: " + e.Message);
                }
            }
            // Return result
            var json = JsonConvert.SerializeObject(result, _serializerSettings);
            return new OkObjectResult(json);
        }

        [HttpGet]
        [Authorize(Policy = Constants.Policies.Customer)]
        [Route("getInventoryFileContents/{clientId}")]
        public async Task<IActionResult> GetInventoryFileContents([FromRoute] string clientId)
        {
            using (var unitOfWork = CreateUnitOfWork())
            {
                InventoryFilesContentViewModel result = new InventoryFilesContentViewModel();
                DATA.Client client = unitOfWork.Clients.Get(clientId, ClientIncludes.GetAllIncludes());
                DATA.Customer customer = unitOfWork.Customers.Get(client.CustomerId, CustomerIncludes.GetAllIncludes());
                var options = new JsonDocumentOptions { AllowTrailingCommas = true };
                var cep = GetCEP(customer.Id);
                DATA.Storages.StorageEntryPoint csdp = customer.StorageEntryPoints.Find(x => x.IsCSDP == true);

                // Connect to Azure Storage Account
                AzureCommunicationService azureCustomer;
                if (!csdp.Managed)
                {
                    azureCustomer = new AzureCommunicationService(cep.TenantId, cep.ClientId, cep.ClientSecret);
                }
                else
                {
                    // TODO: Check system; fix for live system
                    azureCustomer = new AzureCommunicationService(_appSettings.DevelopmentTenantId, _appSettings.DevelopmentClientId, _appSettings.DevelopmentClientSecret);
                }
                string connectionString = azureCustomer.StorageService().GetStorageAccConnectionString(csdp.SubscriptionId, csdp.ResourceGrpName, csdp.StorageAccount);
                CloudStorageAccount customerStorageAcc = CloudStorageAccount.Parse(connectionString);
                CloudBlobClient customerClient = customerStorageAcc.CreateCloudBlobClient();

                CloudBlobContainer customerContainer = customerClient.GetContainerReference("csdp");

                // OS Inventory 
                CloudBlockBlob baselineResult = customerContainer.GetBlockBlobReference("Customer_Repository/BaselineResults/" + clientId + "/" + client.BaseLineFile1);
                CloudBlockBlob hwFile = customerContainer.GetBlockBlobReference("Customer_Repository/BaselineResults/" + clientId + "/" + client.BaseLineFile2);
                CloudBlockBlob osFile = customerContainer.GetBlockBlobReference("Customer_Repository/BaselineResults/" + clientId + "/" + client.BaseLineFile3);

                if (await hwFile.ExistsAsync())
                {
                    result.HwInventory = JToken.Parse(await hwFile.DownloadTextAsync()).ToString(Formatting.Indented);
                }
                if (await osFile.ExistsAsync())
                {
                    result.OSInventory = JToken.Parse(await osFile.DownloadTextAsync()).ToString(Formatting.Indented);
                }
                if (await baselineResult.ExistsAsync())
                {                   
                    result.BaselineResult = JToken.Parse(await baselineResult.DownloadTextAsync()).ToString(Formatting.Indented);
                }
                string json = JsonConvert.SerializeObject(result, _serializerSettings);
                return Ok(json);
            }
        }

        [HttpGet]
        [Authorize(Policy = Constants.Policies.Customer)]
        [Route("export/csv")]
        public IActionResult ExportCustomersInventoryCSV([FromRoute] string customerId)
        {
            try
            {
                // Check customer
                DATA.Customer customer = UnitOfWork.Customers.Get(customerId);
                if (customer == null)
                {
                    return BadRequest("ERROR: Customer doey not exist.");
                }
                // Get inventory
                List<DATA.Inventory> inventory = UnitOfWork.Inventories.GetAll("Client").Where(x => x.Client.CustomerId == customerId).ToList();
                List<DATA.InventoryCSV> csvInventory = Mapper.Map<List<DATA.InventoryCSV>>(inventory);
                string filePath = "inventory-" + customer.Name + ".csv";
                WriteCSVData(csvInventory, filePath);
                // Return file
                FileStream result = new FileStream(filePath, FileMode.Open);
                return File(result, System.Net.Mime.MediaTypeNames.Application.Octet, filePath);
            }
            catch (Exception e)
            {
                return BadRequest("Error: " + e.Message);
            }
        }

        [HttpGet]
        [Authorize(Policy = Constants.Policies.Customer)]
        [Route("export/csv/{clientId}")]
        public IActionResult ExportClientInventoryCSV([FromRoute] string clientId)
        {
            try
            {
                DATA.Client client = UnitOfWork.Clients.Get(clientId);
                if (client == null)
                {
                    return BadRequest("Error: The client does not exist.");
                }
                // Load clients inventory & just use relevant values
                List<DATA.Inventory> inventory = UnitOfWork.Inventories.GetAll().Where(x => x.ClientId == clientId).ToList();
                List<DATA.InventoryCSV> csvInventory = Mapper.Map<List<DATA.InventoryCSV>>(inventory);
                string filePath = "inventory-" + client.Name + ".csv";
                WriteCSVData(csvInventory, filePath);

                // Return file
                FileStream result = new FileStream(filePath, FileMode.Open);
                return File(result, System.Net.Mime.MediaTypeNames.Application.Octet, filePath);
            } catch (Exception e)
            {
                return BadRequest("Error: " + e.Message);
            }
        }

        [HttpDelete]
        [Route("delete/{clientId}")]
        [Authorize(Policy = Constants.Policies.Customer)]
        public IActionResult DeleteClient ([FromRoute] string clientId)
        {
            try
            {
                using (var unitOfWork = CreateUnitOfWork())
                {
                    DATA.Client toDelete = unitOfWork.Clients.GetOrNull(clientId);

                    try
                    {
                        // Delete ActivityLog
                        List<ActivityLog> activityLogs = unitOfWork.ActivityLogs.GetAll().Where(x => x.ClientId == clientId).ToList();
                        foreach (ActivityLog log in activityLogs)
                        {
                            log.ClientId = null;
                            unitOfWork.ActivityLogs.MarkForUpdate(log, GetCurrentUser().Id);
                            unitOfWork.SaveChanges();
                        }
                    }
                    catch (Exception e)
                    {
                        string errMsg = "ERROR: Deleting Activity Log failed! " + e.Message;
                        if (e.InnerException != null)
                        {
                            if (e.InnerException.InnerException != null)
                            {
                                errMsg += "\n" + e.InnerException.InnerException.Message;
                            }
                            else
                            {
                                errMsg += "\n" + e.InnerException.Message;
                            }
                        }
                        return BadRequest(errMsg);
                    }
                    // Check existence
                    if (toDelete == null)
                    {
                        return BadRequest("ERROR: The client does not exist");
                    }

                    // Delete ClientParameter
                    List<ClientParameter> parameters = unitOfWork.ClientParameters.GetAll().Where(x => x.ClientId == clientId).ToList();
                    toDelete.ActivityLogs = null;
                    unitOfWork.Clients.MarkForUpdate(toDelete);
                    unitOfWork.SaveChanges();
                    foreach (ClientParameter param in parameters)
                    {
                        unitOfWork.ClientParameters.MarkForDelete(param);
                        unitOfWork.SaveChanges();
                    }
                    

                    try
                    {
                        unitOfWork.Clients.MarkForDelete(toDelete, GetCurrentUser().Id);
                        unitOfWork.SaveChanges();
                    } catch (Exception exc)
                    {
                        return BadRequest("ERROR: " + exc.Message);
                    }
                    
                    var json = JsonConvert.SerializeObject(Mapper.Map<Models.ClientAddViewModelWeb>(toDelete), _serializerSettings);

                    return Ok(json);
                }
            } catch (Exception e)
            {
                string errMsg = "ERROR: " + e.Message;
                if (e.InnerException != null)
                {
                    errMsg += "\n" + e.InnerException.Message;
                }
                return BadRequest(errMsg);
            }
        } 

        [HttpGet]
        [Route("getClientsMACAddresses/{clientId}")]
        public IActionResult GetMACAddresses([FromRoute] string clientId)
        {
            try
            {
                List<MacAddress> macAddresses;
                using (var unitOfWork = CreateUnitOfWork())
                {
                    macAddresses = unitOfWork.MacAddresses.GetAll().Where(x => x.ClientId == clientId).ToList();
                }

                List<string> result = new List<string>();
                foreach (MacAddress mac in macAddresses)
                {
                    result.Add(mac.Address);
                }

                return Ok(result);
            } catch (Exception e)
            {
                return BadRequest("ERROR: " + e.Message);
            }
        }

        [HttpGet]
        [Route("getBaselineData/{clientId}")]
        public IActionResult GetBaselineData([FromRoute]string clientId)
        {
            try
            {
                using (var unitOfWork = CreateUnitOfWork())
                {
                    DATA.Client client = unitOfWork.Clients.Get(clientId);
                    DATA.Customer customer = unitOfWork.Customers.Get(client.CustomerId, CustomerIncludes.GetAllIncludes());
                    var options = new JsonDocumentOptions { AllowTrailingCommas = true };
                    var cep = GetCEP(customer.Id);
                    DATA.Storages.StorageEntryPoint csdp = customer.StorageEntryPoints.Find(x => x.IsCSDP == true);

                    // Connect to Azure Storage Account
                    AzureCommunicationService azureCustomer;
                    if (!csdp.Managed)
                    {
                        azureCustomer = new AzureCommunicationService(cep.TenantId, cep.ClientId, cep.ClientSecret);
                    }
                    else
                    {
                        // TODO: Check system; fix for live system
                        azureCustomer = new AzureCommunicationService(_appSettings.DevelopmentTenantId, _appSettings.DevelopmentClientId, _appSettings.DevelopmentClientSecret);
                    }
                    string connectionString = azureCustomer.StorageService().GetStorageAccConnectionString(csdp.SubscriptionId, csdp.ResourceGrpName, csdp.StorageAccount);
                    CloudStorageAccount customerStorageAcc = CloudStorageAccount.Parse(connectionString);
                    CloudBlobClient customerClient = customerStorageAcc.CreateCloudBlobClient();
                    
                    CloudBlobContainer customerContainer = customerClient.GetContainerReference("csdp");
                    CloudBlockBlob file1 = customerContainer.GetBlockBlobReference("Customer_Repository/BaselineResults/" + clientId + "/" + client.BaseLineFile1);
                    CloudBlockBlob file2 = customerContainer.GetBlockBlobReference("Customer_Repository/BaselineResults/" + clientId + "/" + client.BaseLineFile2);
                    CloudBlockBlob file3 = customerContainer.GetBlockBlobReference("Customer_Repository/BaselineResults/" + clientId + "/" + client.BaseLineFile3);

                    // string contentFile1 = file1.DownloadTextAsync();
                    // string contentFile2 = file2.DownloadTextAsync();
                    // string contentFile3 = file3.DownloadTextAsync();
                    /*
                    using (var document = JsonDocument.Parse(client.BaselineResult, options))
                    {
                        var elementArray = document.RootElement.EnumerateArray();
                        foreach (var el in elementArray)
                        {
                            try 
                            {
                                var test = el.GetProperty("InstalledHotfixes_Full");
                            }
                            catch (Exception e) { }
                            /*
                            try
                            {
                                var test = el.GetProperty("InstalledHotfixes_Full");
                            }
                            catch (Exception e) { }
                            
                        }
                        /*
                        foreach (JsonElement element in )
                        {
                            var test = element.GetProperty("InstalledHotfixes_Full");
                        }                        
                    }
                    */
                }                    
                return Ok();
            } catch (Exception e)
            {
                return BadRequest("ERROR: " + e.Message);
            }
        }

        [HttpPost]
        [Route("importClients")]
        public IActionResult ImportClients([FromRoute] string customerId, [FromForm] IFormFile file)
        {
            using (StreamReader sr = new StreamReader(file.OpenReadStream()))
            {
                CsvConfiguration csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    Delimiter = ";",
                    IgnoreBlankLines = true,
                    HasHeaderRecord = true,
                    MissingFieldFound = null,
                    LeaveOpen = true
                };
                using (CsvReader csvReader = new CsvReader(sr, csvConfig))
                {                   
                    var content = csvReader.GetRecords<ImportClientsViewModel>();
                    using (var unitOfWork = CreateUnitOfWork())
                    {
                        foreach (ImportClientsViewModel el in content)
                        {
                            bool clientExisted = false;
                            DATA.Client fetchedClient = null;
                            List<string> MACAddresses = el.MacAddresses.Split(",").ToList();
                            // TODO: Check if client exists -> if not -> add new client to customer
                            var fetchedClients = unitOfWork.Clients.GetAll(ClientIncludes.GetAllIncludes()).Where(x => x.UUID == el.UUID && x.SerialNumber == el.Serialnumber).ToList();
                            foreach (DATA.Client client in fetchedClients)
                            {
                                List<MacAddress> macAddresses = unitOfWork.MacAddresses.GetAll().Where(x => x.ClientId == client.Id).ToList();
                                if (macAddresses.Count() != 0)
                                {
                                    foreach (MacAddress mac in macAddresses)
                                    {
                                        if (MACAddresses.Contains(mac.Address))
                                        {
                                            clientExisted = true;
                                            fetchedClients.Remove(client);                                            
                                        }
                                    }
                                }
                                if (fetchedClients.Count() == 0)
                                {
                                    break;
                                }
                            }
                            if (!(fetchedClients.Count() == 0 || fetchedClients.Count() > 1))
                            {
                                fetchedClient = fetchedClients.First();
                            }
                            if (fetchedClient == null && !clientExisted)
                            {
                                DATA.Client newClient = new DATA.Client
                                {
                                    Description = el.Description,
                                    Name = el.Name,
                                    UUID = el.UUID,
                                    CustomerId = customerId,                                    
                                    // PreinstalledSoftwares = new List<DATA.SmartDeploy.PreinstalledSoftware>(),
                                    SerialNumber = el.Serialnumber
                                };

                                unitOfWork.Clients.MarkForInsert(newClient, GetCurrentUser().Id);
                                unitOfWork.SaveChanges();

                                foreach (string mac in MACAddresses)
                                {
                                    MacAddress newMac = new MacAddress();
                                    newMac.Address = mac;
                                    newMac.ClientId = newClient.Id;
                                    unitOfWork.MacAddresses.MarkForInsert(newMac);
                                    unitOfWork.SaveChanges();
                                }

                                var FixedParams = GenerateFixedClientParams(newClient);
                                foreach (var param in FixedParams)
                                {
                                    param.ClientId = newClient.Id;
                                    param.Client = newClient;
                                    unitOfWork.ClientParameters.MarkForInsert(param);
                                }
                                unitOfWork.SaveChanges();
                            }
                        }                        
                    }                                      
                }
            }
            return Ok(); ;
        }

        public class ImportClientsViewModel
        {
            public string Name { get; set; }
            public string UUID { get; set; }
            public string Serialnumber { get; set; }
            public string MacAddresses { get; set; }
            public string Description { get; set; }
        }

        private void GetBaseLineFileData (string fileName, string content)
        {
            JsonDocument doc = JsonDocument.Parse(content);
            
        }

        private void WriteCSVData(List<DATA.InventoryCSV> inventory, string filePath)
        {
            // Write data into csv file
            using (var writer = new StreamWriter(filePath))
            {
                CsvConfiguration csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    Delimiter = "|",
                    IgnoreBlankLines = true,
                    HasHeaderRecord = true,
                    MissingFieldFound = null
                };
                using (var csv = new CsvWriter(writer, csvConfig))
                {
                    csv.WriteRecords(inventory);
                }
            }
        }

        public class OSSettingModel
        {
            public string KeyboardLayoutWindows { get; set; }
            public string KeyboardLayoutLinux { get; set; }
            public string TimeZoneWindows { get; set; }
            public string TimeZoneLinux { get; set; }
            public string UsernameLinux { get; set; }
            public string LanguagePackLinux { get; set; }
            public string InstallScript { get; set; }
            public string LocalAdminUsername { get; set; }
            public string LocalAdminPassword { get; set; }
        }

        public class InventoryFilesContentViewModel
        {
            public string HwInventory { get; set; }
            public string OSInventory { get; set; }
            public string BaselineResult { get; set; }
        }
    }
}