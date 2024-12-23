using System.ComponentModel.DataAnnotations;

namespace Application.Options;

public class JwtSettingsOptions
{
    [Required(ErrorMessage = "The Secret key is required.")]
    [MinLength(16, ErrorMessage = "The Secret key must be at least 16 characters long.")]
    public string Secret { get; set; }

    [Required(ErrorMessage = "The Issuer is required.")]
    public string Issuer { get; set; }

    [Required(ErrorMessage = "The Audience is required.")]
    public string Audience { get; set; }

    [MinLength(0, ErrorMessage = "ExpiryMinutes must be greater than 0.")]
    public int ExpiryMinutes { get; set; }
}


