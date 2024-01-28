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

        public AuthHttpService(IHttpClientFactory factory, IBenefitsCache cache)
        {
            _factory = factory;
            _cache = cache;
        }

        public async Task<AuthResponse?> GenerateToken()
        {
            var cachedResult = await _cache.GetAuthAsync();

            if (cachedResult == null)
            {
                var client = _factory.CreateClient("konsi");

                var bodyContent = new StringContent(
                    JsonSerializer.Serialize(new { username = "test@konsi.com.br", password = "Test@Konsi2023*" }),
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
