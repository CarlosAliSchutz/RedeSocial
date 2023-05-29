using RedeSocial.Domain.Models;

namespace RedeSocial.Domain.Contracts.Repositories;

public interface ICurtidaRepository
{
    Task<Curtida> ObterCurtida(int postId, int usuarioId);
    Task Criar(Curtida curtida);
    Task Remover(Curtida curtida);
    Task<int> ContarCurtidasDoPost(int postId);
}
