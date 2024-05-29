using System;
using System.Text.Json;
using System.Text.Json.Serialization;

public static class Extensions
{
    public static T Inspect<T>(this T item)
    {
        try
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                Converters = { new SystemTypeJsonConverter() }
            };
            var json = JsonSerializer.Serialize(item, options);
            Console.WriteLine(json);
        }
        catch (JsonException jsonEx)
        {
            Console.WriteLine($"JSON serialization error: {jsonEx.Message}");
        }
        catch (NotSupportedException notSupEx)
        {
            Console.WriteLine($"Type not supported for serialization: {notSupEx.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error: {ex.Message}");
        }
        return item;
    }
}

public class SystemTypeJsonConverter : JsonConverter<Type>
{
    public override Type Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        throw new NotSupportedException("Deserialization of System.Type is not supported.");
    }

    public override void Write(Utf8JsonWriter writer, Type value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.FullName);
    }
}
