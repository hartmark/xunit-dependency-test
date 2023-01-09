using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Xunit.DependencyInjection;
using Xunit.DependencyInjection.Logging;

namespace xunit_dependency_tet;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddLogging(logging => { logging.SetMinimumLevel(LogLevel.Trace); });
    }

    public void Configure(ILoggerFactory loggerFactory, ITestOutputHelperAccessor accessor)
    {
        loggerFactory.AddProvider(
            new XunitTestOutputLoggerProvider(accessor, (_, _) => true));
    }
    
    public void ConfigureHost(IHostBuilder hostBuilder) =>
        hostBuilder.ConfigureWebHostDefaults(webHostBuilder => webHostBuilder
            .UseTestServer()
            .UseStartup<AspNetCoreStartup>());
}