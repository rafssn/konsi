using System.Text.Json.Serialization;

namespace konsi_api.Models
{
    public class Benefit
    {
        [JsonPropertyName("numero_beneficio")]
        public string Number { get; set; }

        [JsonPropertyName("codigo_tipo_beneficio")]
        public string Code { get; set; }
    }
}
