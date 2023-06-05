namespace RedeSocial.WebAPI.Security;

public class TokenSettings
{
    public string SecretKey { get; set; }
    public int ExpiresInMinutes { get; set; }
}
