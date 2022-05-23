using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Linq.Dynamic.Core;
using WPM_API.Code.Infrastructure;
using WPM_API.Code.Infrastructure.LogOn;
using WPM_API.Common;
using WPM_API.Data.DataContext;
using WPM_API.Data.DataContext.Entities;
using WPM_API.Models;
using WPM_API.Options;

namespace WPM_API.Controllers.AssetMgmt
{
    [Route("asset-classes")]
    public class AssetClassController : BasisController
    {
        public AssetClassController(AppSettings appSettings, ConnectionStrings connectionStrings, OrderEmailOptions orderEmailOptions, AgentEmailOptions agentEmailOptions, SendMailCreds sendMailCreds, SiteOptions siteOptions, ILogonManager logonManager) : base(appSettings, connectionStrings, orderEmailOptions, agentEmailOptions, sendMailCreds, siteOptions, logonManager)
        {
        }

        [HttpPost]
        [Authorize(Policy = Constants.Policies.Customer)]
        public IActionResult AddAssetClass(AssetClassViewModel data)
        {
            WPM_API.Data.DataContext.Entities.Customer customer = UnitOfWork.Customers.Get(data.CustomerId);
            AssetType assetType = UnitOfWork.AssetTypes.GetOrNull(data.AssetTypeId);
            if (assetType == null)
            {
                return BadRequest("ERROR: The asset type does not exist");
            }

            AssetClass newAssetType = UnitOfWork.AssetClasses.CreateEmpty();
            newAssetType.Name = data.Name;
            newAssetType.fromAdmin = data.fromAdmin;
            newAssetType.Customer = customer;
            newAssetType.AssetType = assetType;

            UnitOfWork.SaveChanges();

            var json = JsonConvert.SerializeObject(Mapper.Map<AssetClassViewModel>(newAssetType), serializerSettings);

            return Ok(json);
        }

        [HttpGet]
        [Authorize(Policy = Constants.Policies.Customer)]
        [Route("{customerId}")]
        public IActionResult GetAssetTypes([FromRoute] string customerId)
        {
            List<AssetClassViewModel> result = new List<AssetClassViewModel>();
            List<AssetClass> assetClasses = UnitOfWork.AssetClasses.GetAll("Customer", "AssetType").Where(x => x.CustomerId == customerId || x.fromAdmin).ToList();
            foreach (AssetClass assetClass in assetClasses)
            {
                result.Add(Mapper.Map<AssetClassViewModel>(assetClass));
            }

            var json = JsonConvert.SerializeObject(result, serializerSettings);

            return Ok(json);
        }
    }
}
