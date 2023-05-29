using RedeSocial.Domain.Models;

namespace RedeSocial.Domain.Contracts.Repositories;

public interface IComentarioRepository
{
    Task Add(Comentario comentario);

    Task<List<Comentario>> Obter(int postId);
}
