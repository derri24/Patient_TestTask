using System.Text.Json;
using System.Text.Json.Serialization;
using MongoDB.Bson;

namespace Patient.TestTask.RestApi.Helpers;

public class GuidJsonConverter : JsonConverter<Guid>
{
    public override Guid Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var id = reader.GetString();
        Guid.TryParse(id, out Guid objectId);
        return objectId;
    }

    public override void Write(Utf8JsonWriter writer, Guid value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
    
}