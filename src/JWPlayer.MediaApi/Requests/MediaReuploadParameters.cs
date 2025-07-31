using JWPlayer.MediaApi.Model;
using System.Text.Json.Serialization;

namespace JWPlayer.MediaApi.Requests;

public struct MediaReuploadParameters
{
    [JsonPropertyName("upload")]
    public MediaUpload Upload { get; set; }
}
