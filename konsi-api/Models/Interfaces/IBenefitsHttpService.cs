namespace konsi_api.Models.Interfaces
{
    public interface IBenefitsHttpService
    {
        Task<IEnumerable<Benefit>> GetBenefitsAsync(string cpf);
    }
}
