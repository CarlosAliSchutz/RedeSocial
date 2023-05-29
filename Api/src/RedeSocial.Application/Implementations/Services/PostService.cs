using RedeSocial.Application.Contacts;
using RedeSocial.Application.Contacts.Documents.Request;
using RedeSocial.Application.Contacts.Documents.Response;
using RedeSocial.Domain.Contracts.Repositories;
using RedeSocial.Domain.Models;
using RedeSocial.Domain.Models.Enums;

namespace RedeSocial.Application.Implementations.Services;

public class PostService : IPostService
{
    private readonly IPostRepository _postRepository;
    private readonly IUsuarioService _usuarioService;
    private readonly IAmizadeRepository _amizadeRepository;
    private readonly ICurtidaRepository _curtidaRepository;

    public PostService(IPostRepository postRepository, IUsuarioService usuarioService, IAmizadeRepository amizadeRepository, ICurtidaRepository curtidaRepository)
    {
        _postRepository = postRepository;
        _usuarioService = usuarioService;
        _amizadeRepository = amizadeRepository;
        _curtidaRepository = curtidaRepository;
    }

    public async Task<PostResponse> CriarPost(int usuarioId, CriarPostRequest request)
    {
        var usuario = await _usuarioService.ObterUsuarioAsync(usuarioId);

        if (usuario == null)
        {
            throw new Exception($"Usuário não encontrado.");
        }


        var post = new Post
        {
            AutorId = usuarioId,
            Autor = usuario, 
            Conteudo = request.Conteudo,
            Criacao = DateTime.Now,
            Curtidas = 0,
            PermissaoVisualizar = request.PermissaoVisualizar
        };

        await _postRepository.Criar(post);

        var response = new PostResponse
        {
            PostId = post.Id,
            AutorId = post.AutorId,
            AutorName = usuario.Nome,
            ImagemPerfil = usuario.ImagemPerfil,
            Conteudo = post.Conteudo,
            Criacao = post.Criacao,
            Curtidas = post.Curtidas,
            PermissaoVisualizar = post.PermissaoVisualizar
        };

        return response;
    }

    public Post ObterPost(int postId)
    {
        return _postRepository.ObterId(postId);
    }

    public async Task<List<PostResponse>> ListarPostsDoUsuarioEAmigos(int usuarioId, int pagina, int quantidadePorPagina)
    {
        var posts = await _postRepository.ListarPosts(usuarioId, pagina, quantidadePorPagina);

        var response = new List<PostResponse>();

        foreach (var post in posts)
        {
            var autor = await _usuarioService.ObterUsuarioAsync(post.AutorId);

            var postResponse = new PostResponse
            {
                PostId = post.Id,
                AutorId = post.AutorId,
                AutorName = autor.Nome,
                ImagemPerfil = autor.ImagemPerfil,
                Conteudo = post.Conteudo,
                Criacao = post.Criacao,
                Curtidas = post.Curtidas,
                PermissaoVisualizar = post.PermissaoVisualizar
            };

            response.Add(postResponse);
        }

        return response;
    }

    public async Task<Post> ListarPostAsync(int postId)
    {
        return await _postRepository.ObterPostIdAsync(postId);
    }

    public async Task<List<Post>> ObterPostsDoUsuario(int usuarioId, int visitanteId)
    {
        var amizadeAceita = await _amizadeRepository.VerificarAmizadeAceita(usuarioId, visitanteId);

        if (amizadeAceita)
        {
            return await _postRepository.ListarTodosPostsUsuario(usuarioId);
        }

        return await _postRepository.ListarPostsPublicosUsuario(usuarioId);
    }

    public async Task AlterarPermissaoPostUsuario(int postId, Permissao permissao)
    {
        var post = await _postRepository.ObterPostIdAsync(postId);

        if (post == null)
        {
            throw new Exception("Post com não encontrado.");
        }

        post.PermissaoVisualizar = permissao;

        await _postRepository.AtualizarPost(post);
    }

    public async Task AtualizarContadorCurtidas(int postId)
    {
        var post = await _postRepository.ObterPostIdAsync(postId);

        if (post != null)
        {
            var curtidas = await _curtidaRepository.ContarCurtidasDoPost(postId);
            post.Curtidas = curtidas;

            await _postRepository.AtualizarPost(post);
        }
    }


}
