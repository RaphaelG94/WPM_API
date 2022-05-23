using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Globalization;
using System.Linq.Dynamic.Core;
using System.Net;
using System.Net.Mail;
using System.Text;
using WPM_API.Code.Infrastructure;
using WPM_API.Code.Infrastructure.LogOn;
using WPM_API.Common;
using WPM_API.Data.DataContext;
using WPM_API.Data.DataContext.Entities;
using WPM_API.Data.DataContext.Entities.AssetMgmt;
using WPM_API.Models;
using WPM_API.Models.AssetMgmt;
using WPM_API.Options;
using DATA = WPM_API.Data.DataContext.Entities;

namespace WPM_API.Controllers.AssetMgmt
{
    [Route("asset-mgmt")]
    public class AssetMgmtCotnroller : BasisController
    {
        public AssetMgmtCotnroller(AppSettings appSettings, ConnectionStrings connectionStrings, OrderEmailOptions orderEmailOptions, AgentEmailOptions agentEmailOptions, SendMailCreds sendMailCreds, SiteOptions siteOptions, ILogonManager logonManager) : base(appSettings, connectionStrings, orderEmailOptions, agentEmailOptions, sendMailCreds, siteOptions, logonManager)
        {
        }

        [HttpGet]
        [Authorize(Policy = Constants.Policies.Systemhouse)]
        public IActionResult GetAssetLabels()
        {
            using (var unitOfWork = CreateUnitOfWork())
            {
                AssetMgmtViewModels result = new AssetMgmtViewModels();
                result.Assets = new List<AssetModelViewModel>();
                List<AssetModel> assetModels = unitOfWork.AssetModels.GetAll("Customer", "Client", "AssetClass", "AssetType").ToList();
                foreach (AssetModel assetModel in assetModels)
                {
                    ClientDetailInformation clientDetails = null;
                    if (assetModel.Client != null)
                    {
                        /*
                        List<Inventory> inventoriesSerialNumber = unitOfWork.Inventories.GetAll("Values")
                            .Where(x => x.ClientId == assetModel.Client.Id && x.KeyName == "SerialNumber-Clear" && x.Type == "ComputerSystem").ToList();
                        List<Inventory> inventoriesMACAddress = unitOfWork.Inventories.GetAll("Values")
                            .Where(x => x.ClientId == assetModel.Client.Id && x.KeyName == "MACAddress" && x.Type == "Network Interface").ToList();
                        */
                        string serialNumber = String.Empty;
                        string macAddress = String.Empty;
                        /*
                        if (inventoriesSerialNumber.Count > 0)
                        {
                            serialNumber = inventoriesSerialNumber[0].Values[0].Value;
                        }
                        if (inventoriesMACAddress.Count > 0)
                        {
                            macAddress = inventoriesMACAddress[0].Values[0].Value;
                        }
                        */
                        clientDetails = new ClientDetailInformation();
                        clientDetails.Id = assetModel.Client.Id;
                        clientDetails.Name = assetModel.Client.Name;
                        clientDetails.UUID = assetModel.Client.UUID;
                        clientDetails.SerialNumber = serialNumber;
                        clientDetails.MACAddress = macAddress;
                    }

                    AssetModelViewModel currentViewModel = Mapper.Map<AssetModelViewModel>(assetModel);
                    if (clientDetails != null)
                    {
                        currentViewModel.Client = clientDetails;
                    }
                    result.Assets.Add(currentViewModel);
                }

                // Return the asset models
                var json = JsonConvert.SerializeObject(result, serializerSettings);
                return Ok(json);
            }
        }

        [HttpGet]
        [Authorize(Policy = Constants.Policies.Customer)]
        [Route("{id}")]
        public IActionResult GetAssetModel([FromRoute] string id)
        {
            using (var unitOfWork = CreateUnitOfWork())
            {
                AssetModel asset = UnitOfWork.AssetModels.Get(id, "Customer", "Client", "Person", "Location", "AssetType", "VendorModel", "Invoice");
                AssetModelViewModel result = Mapper.Map<AssetModelViewModel>(asset);
                if (asset.Client != null)
                {
                    ClientDetailInformation clientDetails = new ClientDetailInformation();
                    clientDetails.Id = asset.Client.Id;
                    clientDetails.Name = asset.Client.Name;
                    clientDetails.UUID = asset.Client.UUID;
                    /*
                    List<Inventory> inventoriesSerialNumber = unitOfWork.Inventories.GetAll("Values")
                                .Where(x => x.ClientId == asset.Client.Id && x.KeyName == "SerialNumber-Clear" && x.Type == "ComputerSystem").ToList();
                    List<Inventory> inventoriesMACAddress = unitOfWork.Inventories.GetAll("Values")
                        .Where(x => x.ClientId == asset.Client.Id && x.KeyName == "MACAddress" && x.Type == "Network Interface").ToList();
                        */
                    string serialNumber = String.Empty;
                    string macAddress = String.Empty;
                    /*
                    if (inventoriesSerialNumber.Count > 0)
                    {
                        serialNumber = inventoriesSerialNumber[0].Values[0].Value;
                    }
                    if (inventoriesMACAddress.Count > 0)
                    {
                        macAddress = inventoriesMACAddress[0].Values[0].Value;
                    }
                    */
                    clientDetails.SerialNumber = serialNumber;
                    clientDetails.MACAddress = macAddress;
                    result.Client = clientDetails;
                }
                var json = JsonConvert.SerializeObject(result, serializerSettings);
                return Ok(json);
            }
        }

        [HttpPost]
        [Authorize(Policy = Constants.Policies.Systemhouse)]
        [Route("{customerId}/{amount}")]
        public IActionResult AddAssetModel([FromRoute] string customerId, [FromRoute] int amount)
        {
            // Get now as create time for the whole set of asset models
            DateTime createdTime = DateTime.Now;

            // Prepare view model to return all assets created
            AssetMgmtViewModels result = new AssetMgmtViewModels();
            result.Assets = new List<AssetModelViewModel>();

            // Create a bunch of asset models with GUID as AssetID
            for (int i = 0; i < amount; i++)
            {
                // Create Asset ID & check for being unique
                string id = RandomString();
                bool isUnique = false;
                while (!isUnique)
                {
                    AssetModel temp = UnitOfWork.AssetModels.GetAll().Where(x => x.AssetID == id).FirstOrDefault();
                    if (temp == null)
                    {
                        isUnique = true;
                    }
                }

                AssetModel newModel = UnitOfWork.AssetModels.CreateEmpty();
                newModel.AssetStatus = "new";
                newModel.CustomerId = customerId;
                newModel.AssetID = id;
                newModel.CreatedByUserId = GetCurrentUser().Id;
                newModel.CreatedDate = createdTime;
                UnitOfWork.SaveChanges();
                newModel = UnitOfWork.AssetModels.Get(newModel.Id, "Customer");
                result.Assets.Add(Mapper.Map<AssetModelViewModel>(newModel));
            }

            // Send email to info@bitstream.de to print the asset labels
            var emailSuccess = SendNotificationMail(result.Assets, customerId);

            // TODO: Maybe check if sent correctly!

            // Return all created assets
            var json = JsonConvert.SerializeObject(result, serializerSettings);
            return Ok(json);
        }

        [HttpDelete]
        [Authorize(Policy = Constants.Policies.Systemhouse)]
        [Route("{assetModelId}")]
        public IActionResult DeleteAssetModel([FromRoute] string assetModelId)
        {
            // Fetch AssetModel & assigned client
            AssetModel toDelete = UnitOfWork.AssetModels.GetOrNull(assetModelId);
            DATA.Client client = UnitOfWork.Clients.GetAll().Where(x => x.AssetModelId == assetModelId).FirstOrDefault();

            if (toDelete == null)
            {
                return BadRequest("ERROR: The asset model does not exist!");
            }

            // Check if a client was assigned & remove asset model
            if (client != null)
            {
                client.AssetModelId = null;
            }

            // Delete asset model
            UnitOfWork.AssetModels.MarkForDelete(toDelete, GetCurrentUser().Id);
            UnitOfWork.SaveChanges();

            // Return deleted asset model
            var json = JsonConvert.SerializeObject(Mapper.Map<AssetModelViewModel>(toDelete), serializerSettings);
            return Ok(json);
        }

        [HttpPut]
        [Authorize(Policy = Constants.Policies.Customer)]
        [Route("{assetModelId}")]
        public IActionResult UnassignAssetModel([FromRoute] string assetModelId)
        {
            // Load asset model data
            AssetModel asset = UnitOfWork.AssetModels.GetOrNull(assetModelId, "Client");

            // Check existence
            if (asset == null)
            {
                return BadRequest("ERROR: The asset model does not exist!");
            }

            // Unassign asset model
            DATA.Client client = asset.Client;
            client.AssetModelId = asset.Id;
            asset.Client = null;
            asset.AssetStatus = "new";

            // Save changes and return asset model
            UnitOfWork.AssetModels.MarkForUpdate(asset, GetCurrentUser().Id);
            UnitOfWork.Clients.MarkForUpdate(client, GetCurrentUser().Id);
            UnitOfWork.SaveChanges();

            var json = JsonConvert.SerializeObject(Mapper.Map<AssetModelViewModel>(asset), serializerSettings);
            return Ok(json);
        }

        [HttpPut]
        [Authorize(Policy = Constants.Policies.Customer)]
        [Route("{clientId}/{assetModelId}")]
        public IActionResult AssignAssetModel([FromRoute] string clientId, [FromRoute] string assetModelId)
        {
            using (var unitOfWork = CreateUnitOfWork())
            {
                // Fetch client and asset model
                DATA.Client client = unitOfWork.Clients.GetOrNull(clientId);
                AssetModel assetModel = UnitOfWork.AssetModels.GetOrNull(assetModelId, "Client", "Customer");

                // Check for existence
                if (client == null)
                {
                    return BadRequest("ERROR: The client does not exist!");
                }

                if (assetModel == null)
                {
                    return BadRequest("ERROR: The asset model does not exist!");
                }



                // Check if asset was assigned before
                if (assetModel.Client != null)
                {
                    if (assetModel.ClientId != client.Id)
                    {
                        var oldClient = assetModel.Client;
                        oldClient.AssetModelId = null;
                        unitOfWork.Clients.MarkForUpdate(oldClient, GetCurrentUser().Id);
                        assetModel.Client = null;
                        assetModel.AssetStatus = "new";
                        unitOfWork.AssetModels.MarkForUpdate(assetModel, GetCurrentUser().Id);
                        unitOfWork.SaveChanges();
                    }
                }

                // Assign Asset
                assetModel.Client = client;
                assetModel.AssetStatus = "active";
                client.AssetModelId = assetModel.Id;
                unitOfWork.AssetModels.MarkForUpdate(assetModel, GetCurrentUser().Id);
                unitOfWork.Clients.MarkForUpdate(client, GetCurrentUser().Id);
                unitOfWork.SaveChanges();

                /*
                // Check former assigned asset 
                if (client.AssetModel != null && client.AssetModel.Id != assetModelId)
                {
                    AssetModel formerAsset = client.AssetModel;
                    formerAsset.Client = null;
                    formerAsset.AssetStatus = "new";
                    client.AssetModel = null;
                    unitOfWork.SaveChanges();
                }

                if (assetModel.Client != null && client.AssetModel != null)
                {
                    // Delete reference to old asssigned status
                    if (assetModel.Client.Id != client.Id)
                    {
                        client.AssetModel = null;
                        assetModel.Client = null;
                        unitOfWork.SaveChanges();
                    }
                }

                // Assign asset model to client
                string userId = GetCurrentUser().Id;
                client.AssetModel = assetModel;
                assetModel.AssetStatus = "active";
                assetModel.Client = client;
                unitOfWork.SaveChanges();
                */

                AssetModelViewModel result = Mapper.Map<AssetModelViewModel>(assetModel);
                var json = JsonConvert.SerializeObject(result, serializerSettings);

                // Return result
                return Ok(json);
            }
        }

        [HttpPut]
        [Route("person/{assetId}/{personId}")]
        [Authorize(Policy = Constants.Policies.Customer)]
        public IActionResult AssignPerson([FromRoute] string assetId, [FromRoute] string personId)
        {
            AssetModel assetModel = UnitOfWork.AssetModels.GetOrNull(assetId);

            if (assetModel == null)
            {
                return BadRequest("ERROR: The asset model does not exist");
            }

            Person person = UnitOfWork.Persons.GetOrNull(personId);

            if (person == null)
            {
                return BadRequest("ERROR: The person does not exist");
            }

            assetModel.Person = person;

            UnitOfWork.AssetModels.MarkForUpdate(assetModel, GetCurrentUser().Id);
            UnitOfWork.SaveChanges();

            var json = JsonConvert.SerializeObject(Mapper.Map<AssetModelViewModel>(assetModel), serializerSettings);

            return Ok(json);
        }

        [HttpPut]
        [Route("location/{assetId}/{locationId}")]
        [Authorize(Policy = Constants.Policies.Customer)]
        public IActionResult AssignLocation([FromRoute] string assetId, [FromRoute] string locationId)
        {
            AssetModel assetModel = UnitOfWork.AssetModels.GetOrNull(assetId);

            if (assetModel == null)
            {
                return BadRequest("ERROR: The asset model does not exist");
            }

            Location location = UnitOfWork.Locations.GetOrNull(locationId);

            if (location == null)
            {
                return BadRequest("ERROR: The location does not exist");
            }

            assetModel.Location = location;

            UnitOfWork.AssetModels.MarkForUpdate(assetModel, GetCurrentUser().Id);
            UnitOfWork.SaveChanges();

            var json = JsonConvert.SerializeObject(Mapper.Map<AssetModelViewModel>(assetModel), serializerSettings);

            return Ok(json);
        }

        [HttpPut]
        [Route("vendor/{assetModelId}/{vendorModelId}")]
        [Authorize(Policy = Constants.Policies.Customer)]
        public IActionResult AssignVendorModel([FromRoute] string assetModelId, [FromRoute] string vendorModelId)
        {
            VendorModel vendor = UnitOfWork.VendorModels.GetOrNull(vendorModelId);

            if (vendor == null)
            {
                return BadRequest("ERROR: The vendor model does not exist");
            }

            AssetModel asset = UnitOfWork.AssetModels.GetOrNull(assetModelId);

            if (asset == null)
            {
                return BadRequest("ERROR: The asset model does not exist");
            }

            asset.VendorModel = vendor;
            UnitOfWork.AssetModels.MarkForUpdate(asset);
            UnitOfWork.SaveChanges();

            var json = JsonConvert.SerializeObject(Mapper.Map<AssetModelViewModel>(asset), serializerSettings);
            return Ok(json);
        }

        [HttpPut]
        [Route("update")]
        [Authorize(Policy = Constants.Policies.Customer)]
        public IActionResult UpdateAssetModel([FromBody] AssetModelViewModel data)
        {
            AssetModel assetModel = UnitOfWork.AssetModels.GetOrNull(data.Id);

            if (assetModel == null)
            {
                return BadRequest("ERROR: The asset model does not exist");
            }

            // Fetch AssetType and AssetClass
            AssetType assetType = UnitOfWork.AssetTypes.GetOrNull(data.AssetTypeId);
            AssetClass assetClass = UnitOfWork.AssetClasses.GetOrNull(data.AssetClassId);

            /*
            if (assetType == null)
            {
                return BadRequest("ERROR: The asset type does not exist");
            }

            if (assetClass == null)
            {
                return BadRequest("ERROR: The asset class does not exist");
            }
            */

            if (data.Invoice != null)
            {
                // Fetch invoice file
                DATA.File invoice = UnitOfWork.Files.GetOrNull(data.Invoice.Id);

                if (invoice == null)
                {
                    return BadRequest("ERROR: The invoice file does not exist");
                }
                assetModel.Invoice = invoice;
            }

            assetModel.AssetClass = assetClass;
            assetModel.AssetType = assetType;
            assetModel.Description = data.Description;
            assetModel.CINumber = data.CINumber;
            assetModel.PurchaseDate = data.PurchaseDate;
            assetModel.PurchaseValue = data.PurchaseValue;
            assetModel.DepreciationMonths = data.DepreciationMonths;
            assetModel.Currency = data.Currency;
            assetModel.Building = data.Building;
            assetModel.Floor = data.Floor;
            assetModel.Coordinates = data.Coordinates;
            assetModel.Room = data.Room;

            UnitOfWork.AssetModels.MarkForUpdate(assetModel, GetCurrentUser().Id);
            UnitOfWork.SaveChanges();

            var json = JsonConvert.SerializeObject(Mapper.Map<AssetModelViewModel>(assetModel), serializerSettings);
            return Ok(json);
        }

        [HttpPost]
        [Authorize(Policy = Constants.Policies.Customer)]
        [Route("export")]
        public IActionResult ExportAssetLabelsCSV([FromBody] AssetMgmtViewModels toExport)
        {
            try
            {
                List<AssetModelCSV> assetsCSV = new List<AssetModelCSV>();
                using (var unitOfWork = CreateUnitOfWork())
                {

                    // Fetching all asset models to export
                    foreach (AssetModelViewModel assetData in toExport.Assets)
                    {
                        var tempAsset = unitOfWork.AssetModels.GetOrNull(assetData.Id, "Client", "AssetType", "AssetClass");
                        if (tempAsset != null)
                        {
                            string qrValue = "";
                            string clientName = "";
                            if (tempAsset.Client != null)
                            {
                                qrValue = siteOptions.SiteUrl + "assetmanagementdetails/" + tempAsset.Client.Id;
                                clientName = tempAsset.Client.Name;
                            }

                            AssetModelCSV newAssetCSV = new AssetModelCSV(
                                tempAsset.Id,
                                tempAsset.AssetID,
                                tempAsset.AssetType.Name,
                                clientName,
                                qrValue,
                                tempAsset.AssetClass.Name,
                                tempAsset.AssetStatus,
                                tempAsset.Description,
                                tempAsset.Building,
                                tempAsset.Floor,
                                tempAsset.Room,
                                tempAsset.Coordinates
                              );

                            assetsCSV.Add(newAssetCSV);
                        }
                    }

                    // Export as CSV                    }
                    string filePath = "assetLabels-" + DateTime.UtcNow.ToShortDateString() + ".csv";
                    WriteCSVData(assetsCSV, filePath);

                    // Return file
                    FileStream result = new FileStream(filePath, FileMode.Open);
                    return File(result, System.Net.Mime.MediaTypeNames.Application.Octet, filePath);
                }
            }
            catch (Exception e)
            {
                return BadRequest("ERROR: " + e.Message);
            }
        }

        private string RandomString()
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 8)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private void WriteCSVData(List<AssetModelCSV> data, string filePath)
        {
            using (var writer = new StreamWriter(filePath))
            {
                CsvConfiguration csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    Delimiter = ";",
                    IgnoreBlankLines = true,
                    HasHeaderRecord = true,
                    MissingFieldFound = null
                };
                using (var csv = new CsvWriter(writer, csvConfig))
                {
                    csv.WriteRecords(data);
                }
            }
        }

        private bool SendNotificationMail(List<AssetModelViewModel> assets, string cusotmerId)
        {
            try
            {
                SmtpClient client = new SmtpClient(sendMailCreds.Host, sendMailCreds.Port);
                NetworkCredential data = new NetworkCredential(sendMailCreds.Email, sendMailCreds.Password);
                client.Credentials = data;
                MailAddress from = new MailAddress(sendMailCreds.Email, sendMailCreds.DisplayName);
                MailAddress to = new MailAddress("order@bitstream.de");
                MailMessage message = new MailMessage(from, to);
                string assetText = getAssetsEmailText(assets);
                message.Body = new StringBuilder("The following Asset labels have to be printed for the Customer ID " +
                                           cusotmerId + " (# " + assets.Count + "):<br /><br />" + assetText).ToString();
                message.BodyEncoding = System.Text.Encoding.UTF8;
                message.Subject = "Printing new Asset Labels";
                message.SubjectEncoding = System.Text.Encoding.UTF8;
                message.IsBodyHtml = true;
                client.EnableSsl = true;
                client.Send(message);
                message.Dispose();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        private string getAssetsEmailText(List<AssetModelViewModel> assets)
        {
            string url = siteOptions.SiteUrl + "#/assetmanagementdetails/";
            StringBuilder sr = new StringBuilder();
            sr.Append("<table><tr><th>Asset ID</th><th>QR value</th></tr>");
            foreach (AssetModelViewModel asset in assets)
            {
                sr.Append("<tr><td>" + asset.AssetID + "</td><td>" + url + asset.Id + "</td></tr>");
            }
            sr.Append("</table>");

            return sr.ToString();
        }

        private double CalculateDepreciationValue(string price, int numberOfMonts, DateTime purchaseDate)
        {
            if (price != null && price != String.Empty)
            {
                var now = DateTime.Now;
                int monthsDifference = ((now.Year - purchaseDate.Year) * 12) + now.Month - purchaseDate.Month;
                var priceDouble = Double.Parse(price);
                return priceDouble - (priceDouble / numberOfMonts) * monthsDifference;
            }
            else
            {
                return 0;
            }
        }
    }
}
