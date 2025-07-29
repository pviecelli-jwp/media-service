using JWPlayer.MediaApiService.Model;
using System.Text.Json.Serialization;

namespace JWPlayer.MediaApiService.Requests;

public struct MediaReuploadParameters
{
    [JsonPropertyName("upload")]
    public MediaUpload Upload { get; set; }
}
