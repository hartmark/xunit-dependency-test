using System.Linq;
using CorrelationId;
using CorrelationId.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Xunit.DependencyInjection;
using Xunit.DependencyInjection.Logging;

namespace Test;

public class Startup
{
    public void ConfigureHost(IHostBuilder hostBuilder) =>
        hostBuilder.ConfigureWebHost(webHostBuilder => webHostBuilder
            .UseTestServer(options => options.PreserveExecutionContext = true)
            .UseStartup<AspNetCoreStartup>());

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDefaultCorrelationId(options =>
        { 
            options.AddToLoggingScope = true;
            options.IncludeInResponse = true;
        });
    }

    public void Configure(ILoggerFactory loggerFactory, ITestOutputHelperAccessor accessor)
    {
        loggerFactory.AddProvider(new XunitTestOutputLoggerProvider(accessor, (_, _) => true));
    }

    private class AspNetCoreStartup
    {
        public void Configure(IApplicationBuilder app, ITestOutputHelperAccessor accessor, ILogger<AspNetCoreStartup> logger)
        {
            logger.LogInformation("In AspNetCoreStartup.Configure");

            app.UseCorrelationId();
            
            app.Run(context =>
            {
                logger.LogInformation("In run");
                return context.Response.WriteAsync("Headers: " + string.Join(",", context.Request.Headers.Select(x => x.ToString())));
            });
        }
    }
}
