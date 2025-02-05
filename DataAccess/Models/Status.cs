using DataAccess.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DataAccess.DTOs;

public class Status
{
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; }

    public StatusType StatusType;

    [BsonElement("errorMessage")]
    public string? ErrorMessage = string.Empty;
}
