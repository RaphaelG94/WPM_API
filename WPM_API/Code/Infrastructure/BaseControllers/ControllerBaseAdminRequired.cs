using WPM_API.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WPM_API.Code.Infrastructure.BaseControllers
{
    [Area("Admin")]
    [Authorize(Roles = Constants.Roles.Admin)]
    public abstract class ControllerBaseAdminRequired : ControllerBaseNoAuthorize
    {

    }
}
