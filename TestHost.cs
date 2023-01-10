using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Logging;
using Xunit;

namespace Test;

public class TestHost
{
    private readonly ILogger<TestHost> _logger;
    private readonly HttpClient _httpClient;

    public TestHost(IServer server, ILogger<TestHost> logger)
    {
        _logger = logger;
        _httpClient = ((TestServer)server).CreateClient();
    }

    [Fact]
    public async Task Test1()
    {
        using var response = await _httpClient.GetAsync("/");

        response.EnsureSuccessStatusCode();

        var responseString = await response.Content.ReadAsStringAsync();

        _logger.LogInformation("Response from server: {ResponseString}", responseString);
        _logger.LogInformation("X-Correlation-ID from server: {X-Correlation-ID}",
            string.Join(",", response.Headers.GetValues("X-Correlation-ID")));
        
        _logger.LogDebug("doesn't show");
    }
}