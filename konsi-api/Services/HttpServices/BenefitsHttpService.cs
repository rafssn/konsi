using konsi_api.Models;
using konsi_api.Models.Interfaces;
using konsi_api.Models.Responses;
using System.Text.Json;

namespace konsi_api.Services.HttpServices
{
    public class BenefitsHttpService : IBenefitsHttpService
    {
        private readonly IHttpClientFactory _factory;
        private readonly IAuthHttpService _authHttpService;

        public BenefitsHttpService(IHttpClientFactory factory, IAuthHttpService authHttpService)
        {
            _factory = factory;
            _authHttpService = authHttpService;
        }

        public async Task<IEnumerable<Benefit>> GetBenefitsAsync(string cpf)
        {
            var authResponse = await _authHttpService.GenerateToken();

            if (authResponse is null)
                throw new InvalidOperationException();

            var client = _factory.CreateClient("konsi");

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authResponse.Token);

            var result = await client.GetAsync($"/api/v1/inss/consulta-beneficios?cpf={cpf}");

            if (result.IsSuccessStatusCode is true)
            {
                var beneficiary = JsonSerializer.Deserialize<BeneficiaryResponse>(await result.Content.ReadAsStringAsync());

                return beneficiary.Data?.Benefits;
            }

            return Enumerable.Empty<Benefit>();
        }
    }
}
