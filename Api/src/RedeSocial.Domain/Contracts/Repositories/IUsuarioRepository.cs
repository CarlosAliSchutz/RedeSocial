using RedeSocial.Domain.Models;
using RedeSocial.Domain.Utils;

namespace RedeSocial.Domain.Contracts.Repositories;

public interface IUsuarioRepository : IBaseRepository
{
    Task<Usuario?> ObterCredenciaisUsuario(string username);

    Task<Usuario> AddUsuario(Usuario usuario);

    Task<Usuario> ObterUsuarioPorEmail(string email);

    Usuario ObterIdUsuario(int id);

    Task<IEnumerable<Usuario>> Listar();

    Task<Usuario> ObterUsuarioIdAsync(int userId);

    Task<Usuario> ObterId(int id);

    Task<PaginatedList<Usuario>> ListarUsuarios(int usuarioAutenticado, string busca, int pagina, int quantidadePorPagina);

    Task<PaginatedList<Usuario>> ListarAmigos(int usuarioAutenticado, string busca, int pagina, int quantidadePorPagina);

    Task AtualizarUsuario(Usuario usuario);
}
