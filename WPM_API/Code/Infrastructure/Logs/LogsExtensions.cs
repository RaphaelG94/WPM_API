using NLog.LayoutRenderers;
using WPM_API.Code.Infrastructure.Logs.LogRenderers;

namespace WPM_API.Code.Infrastructure.Logs
{
    public static class LogsExtensions
    {
        public static void UseConfiguredLogs(this IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory, IConfigurationRoot configurationRoot)
        {
            // loggerFactory.AddConsole(configurationRoot.GetSection("Logging"));
            // loggerFactory.AddDebug();
            // loggerFactory.AddNLog();

            LayoutRenderer.Register("basedir", (logEvent) => env.ContentRootPath);
            LayoutRenderer.Register<AspNetRequestIpLayoutRenderer>("custom-aspnet-request-ip");
            LayoutRenderer.Register<AspNetRequestContentTypeLayoutRenderer>("custom-aspnet-request-contenttype");
            LayoutRenderer.Register<AspNetBuildDateLayoutRenderer>("custom-build-date");
            // env.ConfigureNLog("nlog.config");

            // LogHolder.Init(new NLogFactory());
        }
    }
}
