using konsi_api.Models.Responses;

namespace konsi_api.Models.Interfaces
{
    public interface IBenefitsCache
    {
        Task SetAsync(string cpf, IEnumerable<Benefit> benefits);

        Task<IEnumerable<Benefit>> GetAsync(string cpf);

        Task SetAuthAsync(AuthResponse auth);

        Task<AuthResponse?> GetAuthAsync();
    }
}
