using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Linq.Dynamic.Core;
using WPM_API.Code.Infrastructure;
using WPM_API.Code.Infrastructure.LogOn;
using WPM_API.Common;
using WPM_API.Data.DataContext.Entities;
using WPM_API.Models;
using WPM_API.Options;

namespace WPM_API.Controllers.AssetMgmt
{
    [Route("asset-types")]
    public class AssetTypeController : BasisController
    {
        public AssetTypeController(AppSettings appSettings, ConnectionStrings connectionStrings, OrderEmailOptions orderEmailOptions, AgentEmailOptions agentEmailOptions, SendMailCreds sendMailCreds, SiteOptions siteOptions, ILogonManager logonManager) : base(appSettings, connectionStrings, orderEmailOptions, agentEmailOptions, sendMailCreds, siteOptions, logonManager)
        {
        }

        [HttpPost]
        [Authorize(Policy = Constants.Policies.Customer)]
        public IActionResult AddAssetType(AssetTypeViewModel data)
        {
            WPM_API.Data.DataContext.Entities.Customer customer = UnitOfWork.Customers.Get(data.CustomerId);
            AssetType newAssetType = UnitOfWork.AssetTypes.CreateEmpty();
            newAssetType.Name = data.Name;
            newAssetType.Customer = customer;
            newAssetType.fromAdmin = data.fromAdmin;

            UnitOfWork.SaveChanges();

            var json = JsonConvert.SerializeObject(Mapper.Map<AssetTypeViewModel>(newAssetType), serializerSettings);

            return Ok(json);
        }

        [HttpGet]
        [Authorize(Policy = Constants.Policies.Customer)]
        [Route("{customerId}")]
        public IActionResult GetAssetTypes([FromRoute] string customerId)
        {
            List<AssetTypeViewModel> result = new List<AssetTypeViewModel>();
            List<AssetType> assetTypes = UnitOfWork.AssetTypes.GetAll("Customer").Where(x => x.CustomerId == customerId || x.fromAdmin).ToList();
            foreach (AssetType assetType in assetTypes)
            {
                result.Add(Mapper.Map<AssetTypeViewModel>(assetType));
            }

            var json = JsonConvert.SerializeObject(result, serializerSettings);

            return Ok(json);
        }
    }
}
