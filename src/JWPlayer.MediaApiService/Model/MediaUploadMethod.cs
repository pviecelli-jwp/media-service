using JWPlayer.MediaApiService.Utils;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace JWPlayer.MediaApiService.Model;

[JsonConverter(typeof(EnumerationConverter<MediaUploadMethod>))]
public enum MediaUploadMethod
{
    [EnumMember(Value = "direct")]
    Direct,
    [EnumMember(Value = "external")]
    External,
    [EnumMember(Value = "fetch")]
    Fetch,
    [EnumMember(Value = "multipart")]
    Multipart,
}
