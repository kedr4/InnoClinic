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
    [BsonElement("city")]
    public string City { get; set; } = string.Empty;

    [Required]
    [BsonElement("street")]
    public string Street { get; set; } = string.Empty;

    [Required]
    [BsonElement("houseNumber")]
    public string HouseNumber { get; set; } = string.Empty;

    [BsonElement("officeNumber")]
    public string OfficeNumber { get; set; } = string.Empty;

    [Required]
    [BsonElement("address")]
    public string Address { get; set; } = string.Empty;

    [BsonElement("photo")]
    public string? Photo { get; set; }

    [Required]
    [BsonElement("registryPhoneNumber")]
    public string RegistryPhoneNumber { get; set; } = string.Empty;

    [Required]
    [BsonElement("isActive")]
    public bool IsActive { get; set; } = true;
}
