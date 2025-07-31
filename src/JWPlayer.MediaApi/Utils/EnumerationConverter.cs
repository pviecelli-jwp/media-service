using System.Reflection;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace JWPlayer.MediaApi.Utils;
internal class EnumerationConverter<TEnum> : JsonConverter<TEnum> where TEnum : struct, Enum
{
    public override TEnum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // Use the standard JsonStringEnumConverter approach for deserialization  
        if (reader.TokenType != JsonTokenType.String)
        {
            throw new JsonException($"Unexpected token parsing enum. Expected String, got {reader.TokenType}.");
        }

        var enumText = reader.GetString();
        if (Enum.TryParse(enumText, ignoreCase: true, out TEnum value))
        {
            return value;
        }

        // Check if the value matches EnumMemberAttribute.Value  
        foreach (var field in typeof(TEnum).GetFields(BindingFlags.Public | BindingFlags.Static))
        {
            var attribute = field.GetCustomAttribute<EnumMemberAttribute>();
            if (attribute != null && attribute.Value == enumText)
            {
                return (TEnum)field.GetValue(null)!;
            }
        }

        throw new JsonException($"Unable to convert \"{enumText}\" to Enum \"{typeof(TEnum)}\".");
    }

    public override void Write(Utf8JsonWriter writer, TEnum value, JsonSerializerOptions options)
    {
        // Use EnumMember attribute value for serialization if it exists  
        var enumType = typeof(TEnum);
        var memberInfo = enumType.GetMember(value.ToString());
        var enumMemberAttribute = memberInfo[0].GetCustomAttribute<EnumMemberAttribute>();

        if (enumMemberAttribute != null && !string.IsNullOrEmpty(enumMemberAttribute.Value))
        {
            writer.WriteStringValue(enumMemberAttribute.Value);
        }
        else
        {
            // Fallback to default string representation  
            writer.WriteStringValue(value.ToString());
        }
    }
}
