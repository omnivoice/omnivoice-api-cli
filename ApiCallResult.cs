namespace omnivoice {
    using System.Text.Json.Serialization;

    public class ApiCallResult {
        [JsonPropertyName("success")]
        public bool Success { get; set; }
        [JsonPropertyName("error")]
        public string? Error { get; set; }
    }
};