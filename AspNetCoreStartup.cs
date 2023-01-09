using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit.DependencyInjection;
using Xunit.DependencyInjection.Logging;

namespace xunit_dependency_tet;

public class AspNetCoreStartup
{
    public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory, ITestOutputHelperAccessor accessor, ILogger<AspNetCoreStartup> logger)
    {
        loggerFactory.AddProvider(
            new XunitTestOutputLoggerProvider(accessor, (_, _) => true));

        logger.LogDebug("In Configure");
        //app.UseCorrelationId();

        app.Run(context =>
        {
            logger.LogDebug("In run");
            return context.Response.WriteAsync(context.Request.Headers.ToString());
        });
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddLogging(logging =>
        {
            logging.SetMinimumLevel(LogLevel.Trace);
        });
    }
}