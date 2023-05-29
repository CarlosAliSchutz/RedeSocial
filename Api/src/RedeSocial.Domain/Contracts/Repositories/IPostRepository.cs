using RedeSocial.Domain.Models;

namespace RedeSocial.Domain.Contracts.Repositories;

public interface IPostRepository
{
    Task Criar(Post post);

    Post ObterId(int postId);

    Task<Post> ObterPostIdAsync(int postId);

    Task<List<Post>> ListarPosts(int usuarioId, int pagina, int quantidadePorPagina);

    Task<List<Post>> ListarTodosPostsUsuario(int userId);

    Task<List<Post>> ListarPostsPublicosUsuario(int userId);

    Task AtualizarPost(Post post);

    Task DecrementarCurtidas(int postId);

    Task IncrementarCurtidas(int postId);
}
