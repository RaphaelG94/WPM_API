using WPM_API.Code.Infrastructure.LogOn;

namespace WPM_API.Middlewares
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, ILogonManager _logonManager)
        {
            var ctxPath = context.Request.Path.Value;
            if (ctxPath == null)
            {
                await _next.Invoke(context);
                return;
            }

            //if (ctxPath == "/connect/getTaskList/app")
            //{
            //    context.Response.StatusCode = 401;
            //    await context.Response.WriteAsync("Forbidden");
            //    return;
            //}

            if (!ctxPath.StartsWith("/swagger"))
            {
                await _next.Invoke(context);
                return;
            }
            else
            {
                context.Request.Cookies.TryGetValue("aackjwt", out var jwt);
                if (jwt == null)
                {
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync("Forbidden");
                    return;
                }
                bool result = false;
                try
                {
                    result = _logonManager.ValidateToken(jwt);
                }
                catch (Exception e)
                {
                    context.Response.StatusCode = 500;
                    await context.Response.WriteAsync(e.Message);
                }
                if (result)
                {
                    await _next.Invoke(context);
                }
                else
                {
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync("Forbidden");
                    return;
                }
            }
        }
    }
}
