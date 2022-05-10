using Microsoft.AspNetCore.Authorization;

namespace WPM_API.Code.Infrastructure.BaseControllers
{
    [Authorize]
    public class ControllerBaseAuthorizeRequired: ControllerBaseNoAuthorize
    {
    }
}
