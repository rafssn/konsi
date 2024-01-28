using Elastic.Clients.Elasticsearch;
using konsi_api.Models;
using konsi_api.Models.Interfaces;

namespace konsi_api.Services.ElasticServuces
{

    public class ElasticService : IElasticService
    {
        private readonly ElasticsearchClient _elasticClient;
        private const string IXBeneficiary = "ix_beneficiary";

        public ElasticService()
        {
            _elasticClient = new ElasticsearchClient(new Uri("http://localhost:9200"));
        }

        public async Task IndexBeneficiary(Beneficiary beneficiary)
        {
            if (!_elasticClient.Indices.Exists(IXBeneficiary).Exists)
                _elasticClient.Indices.Create(IXBeneficiary);

            await _elasticClient.IndexAsync(beneficiary, IXBeneficiary);
        }

        public async Task<Beneficiary?> GetBeneficiaryByCpf(string cpf)
        {
            var response = await _elasticClient.SearchAsync<Beneficiary>(s => s
                .Index(IXBeneficiary)
                .Query(q => q
                    .Term(t => t.Cpf, cpf)
                )
                .Size(1)
            );

            if (response.IsValidResponse)
            {
                return response.Documents.FirstOrDefault();
            }

            return null;
        }
    }
}
