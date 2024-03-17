using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Patient.TestTask.RestApi.DataAccess;

public abstract class Document
{
    [BsonId]
    [BsonIgnoreIfDefault]
    public Guid Id { get; set; }
    public DateTime CreationDate { get; set; }
    
    protected Document()
    {
        var now = DateTime.UtcNow;
        CreationDate = new DateTime(now.Ticks / 100000 * 100000, now.Kind);
    }

}