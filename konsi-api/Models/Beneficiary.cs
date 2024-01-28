using System.Text.Json.Serialization;

namespace konsi_api.Models
{
    public class Beneficiary
    {
        [JsonPropertyName("cpf")]
        public string Cpf { get; set; }

        [JsonPropertyName("beneficios")]
        public IEnumerable<Benefit> Benefits { get; set; }
    }
}
