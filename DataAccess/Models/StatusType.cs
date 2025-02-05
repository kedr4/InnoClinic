using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DataAccess.Models;

public enum StatusType
{
    [BsonRepresentation(BsonType.String)]
    Pending,

    [BsonRepresentation(BsonType.String)]
    Success,

    [BsonRepresentation(BsonType.String)]
    Failed
}
