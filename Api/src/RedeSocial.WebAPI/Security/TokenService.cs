using Microsoft.IdentityModel.Tokens;
using RedeSocial.Application.Contacts.Documents.Response;
using RedeSocial.WebAPI.Security.Interface;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RedeSocial.WebAPI.Security;

public class TokenService : ITokenService
{
    public string GenerateToken(LoginResponse login)
    {
        var secretKey = Encoding.UTF8.GetBytes(TokenSettings.SecretKey);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Expires = DateTime.UtcNow.AddMinutes(TokenSettings.ExpiresInMinutes),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey),
                SecurityAlgorithms.HmacSha256Signature),
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("Id", login.Id.ToString()),
                new Claim(ClaimTypes.Email, login.Email),
            })
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var stringToken = tokenHandler.WriteToken(token);

        return stringToken;
    }
}
