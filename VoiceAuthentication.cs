namespace omnivoice;

using System.Text.Json.Serialization;

public record VoiceAuthentication {
    [JsonPropertyName("timestamp")]
    public DateTimeOffset Timestamp { get; init; }

    [JsonPropertyName("sampleLength_sec")]
    public double SampleLength_sec { get; init; }

    [JsonPropertyName("sampleQuality")]
    public double SampleQuality { get; init; }

    [JsonPropertyName("score")]
    public double Score { get; init; }
};