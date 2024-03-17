using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDbGenericRepository.Attributes;
using Newtonsoft.Json.Converters;
using Patient.TestTask.RestApi.DataAccess;
using Prakle.ManagementWarehouse.Api.Base.DataAccess;

namespace Patient.TestTask.RestApi.Entities;

[CollectionName("patients")]
[BsonIgnoreExtraElements]
public class Patient : Document
{
    public Name Name { get; set; }
    public Gender Gender { get; set; }
    public DateTime BirthDate { get; set; }
    public bool Active { get; set; }
}

public class Name
{
    public string Use { get; set; }
    public string Family { get; set; }
    public List<string> Given { get; set; }
}

[JsonConverter(typeof(StringEnumConverter))]
public enum Gender
{
    [EnumMember(Value = "male")]
    Male,
    [EnumMember(Value = "female")]
    Female,
    [EnumMember(Value = "other")]
    Other,
    [EnumMember(Value = "unknown")]
    Unknown
}