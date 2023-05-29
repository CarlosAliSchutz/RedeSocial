using RedeSocial.Application.Contacts.Documents.Response;

namespace RedeSocial.WebAPI.Security.Interface;

public interface ITokenService
{
    string GenerateToken(LoginResponse login);
}
