using konsi_api.Models.Interfaces;
using konsi_api.Models.Responses;
using System.Text;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;

namespace konsi_api.Services.HttpServices
{
    public class AuthHttpService : IAuthHttpService
    {
        private readonly IHttpClientFactory _factory;
        private readonly IBenefitsCache _cache;
        private readonly IConfiguration _configuration;
        public AuthHttpService(IHttpClientFactory factory, IBenefitsCache cache, IConfiguration configuration)
        {
            _factory = factory;
            _cache = cache;
            _configuration = configuration;
        }

        public async Task<AuthResponse?> GenerateToken()
        {
            var cachedResult = await _cache.GetAuthAsync();

            if (cachedResult == null)
            {
                var client = _factory.CreateClient("konsi");

                var bodyContent = new StringContent(
                    JsonSerializer.Serialize(new { username = _configuration.GetValue<string>("KonsiApiSettings:Username"),
                        password = _configuration.GetValue<string>("KonsiApiSettings:Password")
                    }),
                    Encoding.UTF8,
                    Application.Json
                );

                var result = await client.PostAsync("/api/v1/token", bodyContent);

                if (result.IsSuccessStatusCode)
                {
                    var contentString = await result.Content.ReadAsStringAsync();

                    var resultContent = JsonSerializer.Deserialize<AuthHttpResponse>(contentString);

                    if(resultContent is not null)
                        await _cache.SetAuthAsync(resultContent.Data);

                    return resultContent?.Data;
                }
            }

            return cachedResult;
        }
    }
}
