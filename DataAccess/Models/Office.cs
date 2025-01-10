using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.Models;

public class Office
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; }

    [Required]
    [BsonElement("City")]
    public string City { get; set; } = string.Empty;

    [Required]
    [BsonElement("Street")]
    public string Street { get; set; } = string.Empty;

    [Required]
    [BsonElement("HouseNumber")]
    public string HouseNumber { get; set; } = string.Empty;

    [BsonElement("OfficeNumber")]
    public string OfficeNumber { get; set; } = string.Empty;

    [Required]
    [BsonElement("Address")]
    public string Address { get; set; } = string.Empty;

    [BsonElement("PhotoUrl")]
    public string? PhotoUrl { get; set; }

    [Required]
    [BsonElement("RegistryPhoneNumber")]
    public string RegistryPhoneNumber { get; set; } = string.Empty;

    [Required]
    [BsonElement("IsActive")]
    public bool IsActive { get; set; } = true;
}
