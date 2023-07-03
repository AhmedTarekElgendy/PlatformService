using Newtonsoft.Json;
using PlatformService.Data.Dto;
using System.Text;

namespace PlatformService.DAL.Sync
{
    public class HttpCommandsService : IHttpCommandsService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<HttpCommandsService> _logger;

        public HttpCommandsService(HttpClient httpClient, IConfiguration configuration,
            ILogger<HttpCommandsService> logger)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task TestCommandsServiceConnection(PlatformDataDto platformDataDto)
        {
            var url = _configuration.GetValue<string>("CommandsServiceClient") + "Platforms/Index";
            var payload = JsonConvert.SerializeObject(platformDataDto);

            _logger.LogInformation($"Calling Command Service with url: {url} and resquest {payload}");
            var stringContent = new StringContent(payload, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(url, stringContent);

            if (!response.IsSuccessStatusCode)
                _logger.LogError("Calling Commands Service failed");

            else
                _logger.LogInformation($"Calling Commands Service succeded with response {response.Content}");
        }
    }
}
