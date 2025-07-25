using JWPlayer.Identity;
using RestSharp;
using System.Text.Json;

namespace MediaService;

public static class MediaServiceExtensions
{
    public static Alpha GetMediaId(this IRestResponse response)
    {
        var content = JsonDocument.Parse(response.Content);
        var mediaId = GetJsonPropertyAsString(content.RootElement, "id") ?? throw new InvalidOperationException("There was no Media ID in the Response.");
        return new Alpha(mediaId);
    }


    private static string? GetJsonPropertyAsString(JsonElement element, string propertyName)
    {
        try
        {
            return element.GetProperty(propertyName).GetString() ?? default;
        }
        catch
        {
            return default;
        }
    }
}