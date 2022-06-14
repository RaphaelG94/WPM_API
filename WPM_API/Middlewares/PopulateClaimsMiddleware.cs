using System.Security.Claims;

namespace WPM_API.Middlewares
{
    public class PopulateClaimsMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string[] AnonymousPaths = { "/auth", "/connect/", "/zip/", "/agent/" };

        public PopulateClaimsMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.User.Identity.IsAuthenticated)
            {
                // TODO: Enable all anonymous endpoints to be accessed
                bool skip = false;
                foreach (string path in AnonymousPaths)
                {
                    if (context.Request.Path.ToString().Contains(path))
                    {
                        skip = true;
                        break;
                    }
                }
                if (!skip)
                {
                    string token = context.Request.Headers["Authorization"];
                    if (string.IsNullOrEmpty(token))
                    {
                        context.Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
                        await _next(context);
                    }
                    else
                    {
                        if (token.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                        {
                            token = token.Substring("Bearer ".Length).Trim();
                            var identity = context.User.Identity as ClaimsIdentity;
                            if (identity != null)
                            {
                                IEnumerable<Claim> claims = identity.Claims;
                            }
                            if (string.IsNullOrEmpty(token))
                            {
                                context.Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
                                await _next(context);
                            }
                            else
                            {
                                // todo: check if token is valid -> local or microsoft
                                // TODO: find user in db and add claims if found
                                await _next(context);
                            }
                        }

                    }
                }
                else
                {
                    context.Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
                    await _next(context);
                }
            }
            else
            {
                await _next(context);
            }
        }
    }
}
