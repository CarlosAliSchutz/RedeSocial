using RedeSocial.Application.Contacts.Documents.Response;
using RedeSocial.Domain.Models;

namespace RedeSocial.Application.Contacts;

public interface IAmizadeService
{
    Task<bool> EnviarPedidoAmizadeAsync(int usuarioId, int amigoId);

    Task<List<PedidoAmizadeResponse>> ListarPedidosAmizadeSolicitados(int usuarioId);

    public PedidoAmizadeResponse ResponderPedidoAmizade(int pedidoAmizadeId, bool aceitar);

    Task<List<Usuario>> ObterAmigos(int usuarioId);

    Task<PedidoAmizadeResponse> RemoverAmizade(int usuarioId, int amigoId);

    Task<bool> VerificarAmizadeSolicitada(int usuarioAutenticadoId, int usuarioSolicitadoId);
}
