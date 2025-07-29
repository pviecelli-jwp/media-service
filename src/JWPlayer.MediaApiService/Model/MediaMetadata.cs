using System.Text.Json.Serialization;

namespace JWPlayer.MediaApiService.Model;

public record MediaMetadata
{
    [JsonPropertyName("title")]
    public string? Title { get; init; }
    [JsonPropertyName("description")]
    public string? Description { get; set; }
    [JsonPropertyName("author")]
    public string? Author { get; set; }

    /// <summary>
    /// Length of the media in seconds.
    /// This can only be defined manually for externally hosted media.The duration is set automatically for hosted media.
    /// </summary>
    [JsonPropertyName("duration")]
    public double? Duration { get; set; }

    /// <summary>
    /// URL of the page where this media is published
    /// </summary>
    [JsonPropertyName("permalink")]
    public string? Permalink { get; set; }

    /// <summary>
    /// IAB category
    /// </summary>
    [JsonPropertyName("category")]
    public string? Category { get; set; }

    /// <summary>
    /// Start date and time in ISO 8601 format when media is available for streaming
    /// </summary>
    [JsonPropertyName("publish_start_date")]
    public DateTime? PublishStartDate { get; set; }

    /// <summary>
    /// End date and time in ISO 8601 format when media is no longer available for streaming
    /// </summary>
    [JsonPropertyName("publish_end_date")]
    public DateTime? PublishEndDate { get; set; }

    /// <summary>
    /// User-generated labels used to classify a video
    ///Tags are case insensitive and trailing whitespace is removed.
    /// </summary>
    [JsonPropertyName("tags")]
    public string[]? Tags { get; set; }

    /// <summary>
    /// Two-letter ISO-639-1 language code for the media
    /// This is used to index the media by language, to provide relevant playlist recommendations.
    /// </summary>
    [JsonPropertyName("language")]
    public string? Language { get; set; }

    /// <summary>
    /// User-generated key-value pairs
    ///When defining custom_params, include all custom parameters that should be associated with the target resource.
    ///When updating existing custom_params, include all custom_params keys with their updated values. Any custom_params key-value pair not included within the updated custom_params in the request body is deleted.
    /// </summary>
    [JsonPropertyName("custom_params")]
    public Dictionary<string, string>? CustomParams { get; set; }
}
