using RedeSocial.Application.Contacts.Documents.Request;
using RedeSocial.Application.Contacts.Documents.Response;
using RedeSocial.Domain.Models;

namespace RedeSocial.Application.Implementations.Mappers;

public static class UsuarioMapper
{
    public static Usuario ToUsuario(this UsuarioRequest request)
        => new(request.Nome, request.Email, request.Apelido, request.DataNascimento, request.Cep, request.SenhaHash, request.ImagemPerfil);

    public static UsuarioResponse ToUsuarioResponse(this Usuario usuario) => new()
    {
        Nome = usuario.Nome,
        Email = usuario.Email,
    };

    public static ListarUsuariosResponse ToUsuarios(this Usuario usuario) => new()
    {
        Id = usuario.Id,
        Nome = usuario.Nome,
        Email = usuario.Email,
        Apelido = usuario.Apelido,
        DataNascimento = usuario.DataNascimento,
        Cep = usuario.Cep,
        ImagemPerfil = usuario.ImagemPerfil
    };

    public static IEnumerable<UsuarioResponse> ToUsuariosResponse(this IEnumerable<Usuario> usuarios)
        => usuarios.Select(passenger => passenger.ToUsuarioResponse());
}
