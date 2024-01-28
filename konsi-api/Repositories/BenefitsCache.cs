using konsi_api.Models;
using konsi_api.Models.Interfaces;
using konsi_api.Models.Responses;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace konsi_api.Repositories
{
    public class BenefitsCache : IBenefitsCache
    {
        private readonly IDistributedCache _cache;

        public BenefitsCache(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task SetAsync(string cpf, IEnumerable<Benefit> benefits)
        {
            var jsonData = JsonSerializer.Serialize(benefits);

            var options = new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1)
            };

            await _cache.SetStringAsync(cpf, jsonData, options);
        }

        public async Task<IEnumerable<Benefit>> GetAsync(string cpf)
        {
            var jsonData = await _cache.GetStringAsync(cpf);

            if(!string.IsNullOrEmpty(jsonData))
                return JsonSerializer.Deserialize<IEnumerable<Benefit>>(jsonData);

            return Array.Empty<Benefit>();
        }

        public async Task SetAuthAsync(AuthResponse auth)
        {
            var jsonData = JsonSerializer.Serialize(auth);

            var options = new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
            };

            await _cache.SetStringAsync("authentication", jsonData, options);
        }

        public async Task<AuthResponse?> GetAuthAsync()
        {
            var jsonData = await _cache.GetStringAsync("authentication");

            if (!string.IsNullOrEmpty(jsonData))
                return JsonSerializer.Deserialize<AuthResponse>(jsonData);

            return null;
        }
    } 
}
