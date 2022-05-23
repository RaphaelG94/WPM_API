using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace WPM_API.Code.Infrastructure.Api
{
    public class ApiExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            var errorCode = ApiResult.ApiResultCodes.Exception;

            var apiException = context.Exception as ApiException;
            if (apiException != null)
            {
                errorCode = apiException.ErrorCode;
            }

            // LogHolder.MainLog.Error(context.Exception, $"Api exception code: {errorCode}");

            var errorMessage = context.Exception.Message;
            context.Result = new ObjectResult(ApiResult.Error(errorCode, errorMessage));
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.ExceptionHandled = true;
            base.OnException(context);
        }
    }
}
