using System.Text.Json.Serialization;

namespace konsi_api.Models.Responses
{
    public class AuthHttpResponse
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("data")]
        public AuthResponse Data { get; set; }

    }

    public class AuthResponse
    {
        [JsonPropertyName("token")]
        public string Token { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("expiresIn")]
        public string ExpiresIn { get; set; }
    }
}
