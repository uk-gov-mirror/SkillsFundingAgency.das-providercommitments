using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.CommitmentsV2.Api.Client.Configuration;
using SFA.DAS.Http;

namespace SFA.DAS.ProviderCommitments.Web.Controllers
{
    [Route("test")]
    public class TestController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<TestController> _logger;

        public TestController(CommitmentsClientApiConfiguration configuration, ILogger<TestController> logger)
        {
            _httpClient = new AzureActiveDirectoryHttpClientFactory(configuration).CreateHttpClient();
            _logger = logger;
        }

        [Route("400")]
        public async Task<IActionResult> GetDomainException()
        {
            var response = await _httpClient.PostAsJsonAsync("api/test/400", new { Foo = "Foo" });
            
            _logger.LogInformation($"X-Sfa-Sub-Status-Code header present: {response.Headers.Contains("X-Sfa-Sub-Status-Code")}");
            
            return Ok();
        }
        
        [Route("provider/400")]
        public async Task<IActionResult> GetProviderDomainException()
        {
            var response = await _httpClient.PostAsJsonAsync("api/test/provider/400", new { Foo = "Foo" });
            
            _logger.LogInformation($"X-Sfa-Sub-Status-Code header present: {response.Headers.Contains("X-Sfa-Sub-Status-Code")}");
            
            return Ok();
        }
        
        [Route("throw")]
        public async Task<IActionResult> ThrowDomainException()
        {
            var response = await _httpClient.PostAsJsonAsync("api/test/throw", new { Foo = "Foo" });
            
            _logger.LogInformation($"X-Sfa-Sub-Status-Code header present: {response.Headers.Contains("X-Sfa-Sub-Status-Code")}");
            
            return Ok();
        }
        
        [Route("provider/throw")]
        public async Task<IActionResult> ThrowProviderDomainException()
        {
            var response = await _httpClient.PostAsJsonAsync("api/test/provider/throw", new { Foo = "Foo" });
            
            _logger.LogInformation($"X-Sfa-Sub-Status-Code header present: {response.Headers.Contains("X-Sfa-Sub-Status-Code")}");
            
            return Ok();
        }
    }
}