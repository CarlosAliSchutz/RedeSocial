using RedeSocial.Domain.Models;
using RedeSocial.Domain.Models.Enums;

namespace RedeSocial.Domain.Contracts.Repositories;

public interface IAmizadeRepository
{
    Task<bool> VerificarPedidoAmizadeAsync(int usuarioId, int amigoId);

    Task CriarPedidoAmizadeAsync(Amizade pedidoAmizade);

    Task<List<Amizade>> PedidosSolicitados(int usuarioId);

    Amizade ObterAmizadePorId(int pedidoAmizadeId);

    void AtualizarPedidoAmizade(Amizade pedidoAmizade);

    Task<List<Amizade>> ObterAmizadesDoUsuario(int usuarioId, StatusAmizade statusAmizade);

    Task RemoverAmizade(Amizade amizade);

    Task<Amizade> ObterAmizade(int usuarioId, int amigoId);

    Task<bool> VerificarAmizadeAceita(int usuarioId, int visitanteId);

    Task<bool> VerificarAmizadeSolicitada(int usuarioAutenticadoId, int usuarioSolicitadoId);
}
