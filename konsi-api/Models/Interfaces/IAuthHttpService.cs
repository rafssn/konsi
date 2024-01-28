using konsi_api.Models.Responses;

namespace konsi_api.Models.Interfaces
{
    public interface IAuthHttpService
    {
        Task<AuthResponse?> GenerateToken();
    }
}
