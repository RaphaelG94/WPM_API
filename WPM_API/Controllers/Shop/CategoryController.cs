using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WPM_API.Code.Infrastructure;
using WPM_API.Code.Infrastructure.LogOn;
using WPM_API.Common;
using WPM_API.Data.DataContext.Entities;
using WPM_API.Models;
using WPM_API.Options;

namespace WPM_API.Controllers
{

    /// <summary>
    /// Category endpoint for ShopItems in the Shop.
    /// </summary>
    [Authorize(Policy = Constants.Roles.Customer)]
    [Route("/shop/categories")]
    public class CategoryController : BasisController
    {
        public CategoryController(AppSettings appSettings, ConnectionStrings connectionStrings, OrderEmailOptions orderEmailOptions, AgentEmailOptions agentEmailOptions, SendMailCreds sendMailCreds, SiteOptions siteOptions, ILogonManager logonManager) : base(appSettings, connectionStrings, orderEmailOptions, agentEmailOptions, sendMailCreds, siteOptions, logonManager)
        {
        }

        /// <summary>
        /// Get all Categories.
        /// </summary>
        /// <returns>[Category]</returns>
        [HttpGet]
        public IActionResult GetCategories()
        {
            var CategoryList = new List<Category>();
            using (var unitOfWork = CreateUnitOfWork())
            {
                CategoryList = unitOfWork.Categories.GetAll().Where(x => x.Type.Equals(CategoryType.ShopItem)).ToList();
            }
            var json = JsonConvert.SerializeObject(Mapper.Map<List<Category>, List<CategoryViewModel>>(CategoryList), serializerSettings);
            return new OkObjectResult(json);
        }

        /// <summary>
        /// Add a new Category for ShopItems.
        /// </summary>
        /// <param name="newCategory">New Category</param>
        /// <returns>Category</returns>
        [HttpPost]
        public IActionResult AddCategory([FromBody] CategoryAddViewModel category)
        {
            var json = "";
            using (var unitOfWork = CreateUnitOfWork())
            {
                var cat = unitOfWork.Categories.GetByName(category.Name.ToLower(), CategoryType.ShopItem);
                if (cat != null)
                {
                    return BadRequest("Category is already existant.");
                }
                Category newCategory = Mapper.Map<Category>(category);
                newCategory.Type = CategoryType.ShopItem;
                unitOfWork.Categories.MarkForInsert(newCategory);
                unitOfWork.SaveChanges();
                json = JsonConvert.SerializeObject(Mapper.Map<CategoryViewModel>(newCategory), serializerSettings);
            }

            return new OkObjectResult(json);
        }

        /// <summary>
        /// Update an existing Category.
        /// </summary>
        /// <param name="categoryId">Id of the Category</param>
        /// <param name="categoryEdit">Modified Category</param>
        /// <returns>Category</returns>
        [HttpPut]
        [Route("{categoryId}")]
        public IActionResult UpdateCategory([FromRoute] string categoryId, [FromBody] CategoryViewModel categoryEdit)
        {
            Category dbCategory = null;
            using (var unitOfWork = CreateUnitOfWork())
            {
                dbCategory = unitOfWork.Categories.Get(categoryId);
                if (dbCategory == null)
                {
                    return new NotFoundResult();
                }

                dbCategory.Name = categoryEdit.Name;
                dbCategory.Type = CategoryType.ShopItem;
                unitOfWork.Categories.MarkForUpdate(dbCategory);
                unitOfWork.SaveChanges();
            }

            // Category was changed and is returned.
            var json = JsonConvert.SerializeObject(Mapper.Map<CategoryViewModel>(dbCategory), serializerSettings);
            return new OkObjectResult(json);
        }
    }
}