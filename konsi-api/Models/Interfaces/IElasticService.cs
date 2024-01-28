namespace konsi_api.Models.Interfaces
{
    public interface IElasticService
    {
        Task IndexBeneficiary(Beneficiary beneficiary);

        Task<Beneficiary?> GetBeneficiaryByCpf(string cpf);
    }
}
