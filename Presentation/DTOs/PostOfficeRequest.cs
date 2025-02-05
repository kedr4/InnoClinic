namespace Presentation.DTOs;

public class PostOfficeRequest
{
    public string City { get; set; }
    public string Street { get; set; }
    public string HouseNumber { get; set; }
    public string OfficeNumber { get; set; }
    public IFormFile? Photo { get; set; }
    public string RegistryPhoneNumber { get; set; }
    public bool IsActive { get; set; } = true;
}
