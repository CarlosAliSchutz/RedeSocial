using Microsoft.AspNetCore.Http;

namespace RedeSocial.Application.Utils;

public class AuthenticationUtils
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthenticationUtils(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public int BuscarUsuarioAutenticado()
    {
        var userIdClaim = _httpContextAccessor.HttpContext.User.Claims
            .FirstOrDefault(x => x.Type.Equals("Id"))?.Value;

        if (!string.IsNullOrEmpty(userIdClaim) && int.TryParse(userIdClaim, out int usuarioId))
        {
            return usuarioId;
        }

        throw new Exception("Usuário autenticado não encontrado.");
    }
}
