using RedeSocial.Application.Contacts;
using RedeSocial.Application.Contacts.Documents.Request;
using RedeSocial.Application.Contacts.Documents.Response;
using RedeSocial.Domain.Contracts.Repositories;
using RedeSocial.Domain.Models;

namespace RedeSocial.Application.Implementations.Services;

public class ComentarioService : IComentarioService
{
    private readonly IPostRepository _postRepository;
    private readonly IUsuarioService _usuarioService;
    private readonly IComentarioRepository _comentarioRepository;

    public ComentarioService(IPostRepository postRepository, IUsuarioService usuarioService, IComentarioRepository comentarioRepository)
    {
        _postRepository = postRepository;
        _usuarioService = usuarioService;
        _comentarioRepository = comentarioRepository;
    }

    public async Task<ComentarioResponse> Comentar(int postId, ComentarioRequest comentarioRequest, int autorId)
    {
        var post = await _postRepository.ObterPostIdAsync(postId);
        var usuario = await _usuarioService.ObterDadosUsuarioLogadoAsync(autorId);

        if (post == null)
        {
            return null; 
        }

        var comentario = new Comentario
        {
            Conteudo = comentarioRequest.Conteudo,
            PostId = postId,
            AutorId = autorId,
            DataPublicacao = DateTime.Now,
            Autor = usuario.Nome
        };

        await _comentarioRepository.Add(comentario);

        var comentarioResponse = new ComentarioResponse
        {
            Id = comentario.Id,
            Conteudo = comentario.Conteudo,
            DataPublicacao = comentario.DataPublicacao,
            AutorId = comentario.AutorId,
            AutorNome = comentario.Autor
        };

        return comentarioResponse;
    }

    public async Task<List<Comentario>> ObterComentariosDoPostAsync(int postId)
    {
        return await _comentarioRepository.Obter(postId);
    }
}
