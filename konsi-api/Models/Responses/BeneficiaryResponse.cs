using System.Text.Json.Serialization;

namespace konsi_api.Models.Responses
{
    public class BeneficiaryResponse
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("data")]
        public Beneficiary Data { get; set; }
    }
}
