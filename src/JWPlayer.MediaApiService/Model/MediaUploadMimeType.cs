using JWPlayer.MediaApiService.Extensions;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace JWPlayer.MediaApiService.Model;

[JsonConverter(typeof(EnumerationConverter<MediaUploadMimeType>))]
public enum MediaUploadMimeType
{
    [EnumMember(Value = "video/mp4")]
    VideoMp4,

    [EnumMember(Value = "video/webm")]
    VideoWebm,

    [EnumMember(Value = "video/flv")]
    VideoFlv,

    [EnumMember(Value = "audio/aac")]
    AudioAac,

    [EnumMember(Value = "audio/mpeg")]
    AudioMpeg,

    [EnumMember(Value = "audio/ogg")]
    AudioOgg,

    [EnumMember(Value = "application/vnd.apple.mpegurl")]
    ApplicationVndAppleMpegUrl,

    [EnumMember(Value = "application/smil+xml")]
    ApplicationSmilXml,

    [EnumMember(Value = "application/dash+xml")]
    ApplicationDashXml,

    [EnumMember(Value = "video/flash")]
    VideoFlash,

    [EnumMember(Value = "video/x-youtube")]
    VideoXYoutube
}
