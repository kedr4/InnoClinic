namespace Application.Helpers;

public class JwtSettings
{
    public string Secret { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;

    //Could not be built with a primary constructur
    //Caused an error in JwtTokenGenerator
    public JwtSettings(string secret, string issuer, string audience)
    {
        Secret = secret;
        Issuer = issuer;
        Audience = audience;
    }
}
