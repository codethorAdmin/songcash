﻿namespace Songcash.Model.Configuration;

public class AuthenticationConfiguration
{
    public string Audience { get; set; }
    public string Issuer { get; set; }
    public string Secret { get; set; }
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }
}

