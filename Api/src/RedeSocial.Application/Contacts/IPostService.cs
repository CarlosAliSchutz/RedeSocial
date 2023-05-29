using RedeSocial.Application.Contacts.Documents.Request;
using RedeSocial.Application.Contacts.Documents.Response;
using RedeSocial.Domain.Models;
using RedeSocial.Domain.Models.Enums;

namespace RedeSocial.Application.Contacts;

public interface IPostService
{
    Task<PostResponse> CriarPost(int usuarioId, CriarPostRequest request);

    Post ObterPost(int postId);

    Task<Post> ListarPostAsync(int postId);

    Task<List<PostResponse>> ListarPostsDoUsuarioEAmigos(int usuarioId, int pagina, int quantidadePorPagina);

    Task<List<Post>> ObterPostsDoUsuario(int usuarioId, int visitanteId);

    Task AlterarPermissaoPostUsuario(int postId, Permissao permissao);

    Task AtualizarContadorCurtidas(int postId);
}
