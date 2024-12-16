﻿using MimeKit.Cryptography;

namespace Application.Helpers;

public class JwtSettingsOptions()
{
    public string Secret { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; } 
    public int ExpiryMinutes { get; set; }

}
