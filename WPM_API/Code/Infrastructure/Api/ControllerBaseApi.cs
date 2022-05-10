using WPM_API.Code.Infrastructure.BaseControllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WPM_API.Code.Infrastructure.Api
{
    //todo controllerbase and webapi
    [ApiExceptionFilter]
    [ApiInvalidModelStateFilter]
    //[Authorize(ApiConstants.ApiPolicy)]
    //[Route("api/[controller]/[action]")]
    public abstract class ControllerBaseApi: ControllerSiteBase
    {

    }
}
