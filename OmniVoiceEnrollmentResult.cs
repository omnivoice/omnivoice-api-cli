namespace omnivoice {
    using System.Text.Json.Serialization;

    public class OmniVoiceEnrollVoiceSampleResult: ApiCallResult {
        [JsonPropertyName("metadata")]
        public string? Metadata { get; set; }
        [JsonPropertyName("netSpeechDuration_sec")]
        public double NetSpeechDuration_sec { get; set; }
        [JsonPropertyName("identifier")]
        public string? Identifier { get; set; }
        [JsonPropertyName("identifierValidated")]
        public bool IdentifierValidated { get; set; }
    };
};