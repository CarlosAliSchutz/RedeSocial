using RedeSocial.Application.Contacts.Documents.Request;
using RedeSocial.Application.Contacts.Documents.Response;
using RedeSocial.Domain.Models;
using RedeSocial.Domain.Utils;

namespace RedeSocial.Application.Contacts;

public interface IUsuarioService
{
    Task<UsuarioResponse> Cadastro(UsuarioRequest request);

    Task<LoginResponse> ValidateLogin(LoginRequest loginRequest);

    Usuario ObterUsuario(int usuarioId);

    Task<UsuarioLogadoResponse> ObterDadosUsuario(int userId);

    Task<Usuario> ObterUsuarioAsync(int usuarioId);

    IEnumerable<ListarUsuariosResponse> ListarUsuarios(int? id = null);

    Task<PaginatedList<Usuario>> PesquisarUsuarios(int usuarioAutenticado, string busca, int pagina, int quantidadePorPagina);

    Task<PaginatedList<Usuario>> PesquisarAmigos(int usuarioAutenticado, string busca, int pagina, int quantidadePorPagina);

    Task EditarPerfil(Usuario usuario);
}
