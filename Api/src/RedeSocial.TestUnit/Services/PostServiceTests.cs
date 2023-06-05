using RedeSocial.Application.Contacts.Documents.Request;
using RedeSocial.Application.Contacts.Documents.Response;
using RedeSocial.Domain.Models.Enums;
using RedeSocial.Domain.Models;
using Moq;
using RedeSocial.Application.Contacts;
using RedeSocial.Application.Implementations.Services;
using RedeSocial.Domain.Contracts.Repositories;
using BC = BCrypt.Net.BCrypt;

namespace RedeSocial.TestUnit.Services;

public class PostServiceTests
{
    private readonly Mock<IPostRepository> _postRepositoryMock;
    private readonly Mock<IUsuarioService> _usuarioServiceMock;
    private readonly Mock<IAmizadeRepository> _amizadeRepositoryMock;
    private readonly Mock<ICurtidaRepository> _curtidaRepositoryMock;
    private readonly IPostService _postService;

    public PostServiceTests()
    {
        _postRepositoryMock = new Mock<IPostRepository>();
        _usuarioServiceMock = new Mock<IUsuarioService>();
        _amizadeRepositoryMock = new Mock<IAmizadeRepository>();
        _curtidaRepositoryMock = new Mock<ICurtidaRepository>();
        _postService = new PostService(_postRepositoryMock.Object, _usuarioServiceMock.Object, _amizadeRepositoryMock.Object, _curtidaRepositoryMock.Object);
    }


    [Fact]
    public async Task CriarPost_DeveCriarPostERetornarPostResponse()
    {
        // Arrange
        int usuarioId = 1;
        var request = new CriarPostRequest
        {
            Conteudo = "Olá, amigos!",
            PermissaoVisualizar = Permissao.PUBLICO
        };

        var usuario = new Usuario("Carlos", "carlos@gmail.com", "Ali", new DateOnly(1999, 11, 19), "12345678", BC.HashPassword("senha123"), "perfil.jpg") { Id = usuarioId };

        var post = new Post
        {
            Id = 2,
            AutorId = usuarioId,
            Autor = usuario,
            Conteudo = request.Conteudo,
            Criacao = DateTime.UtcNow,
            Curtidas = 0,
            PermissaoVisualizar = request.PermissaoVisualizar
        };

        _usuarioServiceMock.Setup(s => s.ObterUsuarioAsync(usuarioId)).ReturnsAsync(usuario);
        _postRepositoryMock.Setup(r => r.Criar(It.IsAny<Post>())).Callback<Post>(post =>
        {
            post.Id = 2;
        }).Returns(Task.CompletedTask);

        var expectedResponse = new PostResponse
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

        // Act
        var result = await _postService.CriarPost(usuarioId, request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedResponse.PostId, result.PostId);
        Assert.Equal(expectedResponse.AutorId, result.AutorId);
        Assert.Equal(expectedResponse.AutorName, result.AutorName);
        Assert.Equal(expectedResponse.ImagemPerfil, result.ImagemPerfil);
        Assert.Equal(expectedResponse.Conteudo, result.Conteudo);
        Assert.Equal(expectedResponse.Curtidas, result.Curtidas);
        Assert.Equal(expectedResponse.PermissaoVisualizar, result.PermissaoVisualizar);
    }

    [Fact]
    public async Task CriarPost_DeveRetornarNotificacaoQuandoUsuarioNaoForEncontrado()
    {
        // Arrange
        int usuarioId = 1;
        var request = new CriarPostRequest
        {
            Conteudo = "Olá, amigos!",
            PermissaoVisualizar = Permissao.PUBLICO
        };

        _usuarioServiceMock.Setup(s => s.ObterUsuarioAsync(usuarioId)).ReturnsAsync((Usuario)null);

        // Act
        var result = await _postService.CriarPost(usuarioId, request);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result.Notifications); 
        Assert.Equal("Usuário não encontrado.", result.Notifications.First().Message);
    }

    [Fact]
    public void ObterPost_DeveRetornarPostExistente()
    {
        // Arrange
        int postId = 1;
        var post = new Post { Id = postId };
        _postRepositoryMock.Setup(r => r.ObterId(postId)).Returns(post);

        // Act
        var result = _postService.ObterPost(postId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(post, result);
    }

    [Fact]
    public async Task ListarPostsDoUsuarioEAmigos_DeveRetornarPostsCorretos()
    {
        // Arrange
        int usuarioId = 1;
        int pagina = 1;
        int quantidadePorPagina = 10;

        var posts = new List<Post>
    {
        new Post { Id = 4, AutorId = 1, Conteudo = "Post 1" },
        new Post { Id = 5, AutorId = 2, Conteudo = "Post 2" },
        new Post { Id = 6, AutorId = 3, Conteudo = "Post 3" }
    };

        var usuarios = new List<Usuario>
    {
        new Usuario("Carlos", "carlos@gmail.com", "Ali", new DateOnly(1999, 11, 19), "12345678", BC.HashPassword("senha123"), "perfil.jpg") { Id = 1 },
        new Usuario("João", "joao@gmail.com", "João", new DateOnly(2000, 5, 10), "87654321", BC.HashPassword("senha456"), "perfil.jpg") { Id = 2 },
        new Usuario("Maria", "maria@gmail.com", "Maria", new DateOnly(1995, 3, 15), "98765432", BC.HashPassword("senha789"), "perfil.jpg") { Id = 3 }
    };

        _postRepositoryMock.Setup(r => r.ListarPosts(usuarioId, pagina, quantidadePorPagina)).ReturnsAsync(posts);

        foreach (var post in posts)
        {
            var autor = usuarios.FirstOrDefault(u => u.Id == post.AutorId);
            _usuarioServiceMock.Setup(s => s.ObterUsuarioAsync(post.AutorId)).ReturnsAsync(autor);
        }

        // Act
        var result = await _postService.ListarPostsDoUsuarioEAmigos(usuarioId, pagina, quantidadePorPagina);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(posts.Count, result.Count);

        for (int i = 0; i < posts.Count; i++)
        {
            var post = posts[i];
            var postResponse = result[i];

            Assert.Equal(post.Id, postResponse.PostId);
            Assert.Equal(post.AutorId, postResponse.AutorId);
            Assert.Equal(post.Conteudo, postResponse.Conteudo);

            var autor = usuarios.FirstOrDefault(u => u.Id == post.AutorId);
            Assert.NotNull(autor);
            Assert.Equal(autor.Nome, postResponse.AutorName);
            Assert.Equal(autor.ImagemPerfil, postResponse.ImagemPerfil);
        }
    }

    [Fact]
    public async Task ListarPostAsync_DeveRetornarPostCorreto()
    {
        // Arrange
        int postId = 1;
        var post = new Post { Id = postId, Conteudo = "Olá, mundo!" };

        _postRepositoryMock.Setup(r => r.ObterPostIdAsync(postId)).ReturnsAsync(post);

        // Act
        var result = await _postService.ListarPostAsync(postId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(post.Id, result.Id);
        Assert.Equal(post.Conteudo, result.Conteudo);
    }

    [Fact]
    public async Task ObterPostsDoUsuario_CasoSejaAmigo_RetornaPostsAmigo()
    {
        // Arrange
        int usuarioId = 1;
        int visitanteId = 2;

        var usuario = new Usuario("Carlos", "carlos@gmail.com", "Ali", new DateOnly(1999, 11, 19), "12345678", BC.HashPassword("senha123"), "perfil.jpg") { Id = usuarioId };
        var visitante = new Usuario("João", "joao@gmail.com", "João", new DateOnly(2000, 5, 10), "87654321", BC.HashPassword("senha456"), "perfil.jpg") { Id = visitanteId };

        var amizades = new Amizade { Id = 1, UsuarioId = 1, Usuario = usuario, AmigoId = 2, Amigo = visitante, StatusAmizade = StatusAmizade.ACEITO };

        var postsAmizadeAceita = new List<Post>
    {
        new Post { Id = 1, Conteudo = "Post 1", AutorId = usuarioId, PermissaoVisualizar = Permissao.PRIVADO },
        new Post { Id = 2, Conteudo = "Post 2", AutorId = visitanteId, PermissaoVisualizar = Permissao.PRIVADO },
    };

        _amizadeRepositoryMock.Setup(r => r.VerificarAmizadeAceita(usuarioId, visitanteId)).ReturnsAsync(true);
        _postRepositoryMock.Setup(r => r.ListarTodosPostsUsuario(usuarioId)).ReturnsAsync(postsAmizadeAceita);

        // Act
        var result = await _postService.ObterPostsDoUsuario(usuarioId, visitanteId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(postsAmizadeAceita.Count, result.Count);

        for (int i = 0; i < postsAmizadeAceita.Count; i++)
        {
            var expectedPost = postsAmizadeAceita[i];
            var actualPost = result[i];

            Assert.Equal(expectedPost.Id, actualPost.Id);
            Assert.Equal(expectedPost.Conteudo, actualPost.Conteudo);
            Assert.Equal(expectedPost.AutorId, actualPost.AutorId);
        }
    }

    [Fact]
    public async Task ObterPostsDoUsuario_CasoNaoSejaAmigo_RetornaPostsPublicos()
    {
        // Arrange
        int usuarioId = 1;
        int visitanteId = 2;

        var usuario = new Usuario("Carlos", "carlos@gmail.com", "Ali", new DateOnly(1999, 11, 19), "12345678", BC.HashPassword("senha123"), "perfil.jpg") { Id = usuarioId };
        var visitante = new Usuario("João", "joao@gmail.com", "João", new DateOnly(2000, 5, 10), "87654321", BC.HashPassword("senha456"), "perfil.jpg") { Id = visitanteId };

        var postsPublicos = new List<Post>
    {
        new Post { Id = 1, Conteudo = "Post 1", AutorId = usuarioId, PermissaoVisualizar = Permissao.PUBLICO },
        new Post { Id = 2, Conteudo = "Post 2", AutorId = visitanteId, PermissaoVisualizar = Permissao.PUBLICO },
    };

        _amizadeRepositoryMock.Setup(r => r.VerificarAmizadeAceita(usuarioId, visitanteId)).ReturnsAsync(false);
        _postRepositoryMock.Setup(r => r.ListarPostsPublicosUsuario(usuarioId)).ReturnsAsync(postsPublicos);

        // Act
        var result = await _postService.ObterPostsDoUsuario(usuarioId, visitanteId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(postsPublicos.Count, result.Count);

        for (int i = 0; i < postsPublicos.Count; i++)
        {
            var expectedPost = postsPublicos[i];
            var actualPost = result[i];

            Assert.Equal(expectedPost.Id, actualPost.Id);
            Assert.Equal(expectedPost.Conteudo, actualPost.Conteudo);
            Assert.Equal(expectedPost.AutorId, actualPost.AutorId);
        }
    }

    [Fact]
    public async Task ObterPostsDoUsuario_CasoNaoSejaAmigo_RetornaListaVazia()
    {
        // Arrange
        int usuarioId = 1;
        int visitanteId = 2;

        var usuario = new Usuario("Carlos", "carlos@gmail.com", "Ali", new DateOnly(1999, 11, 19), "12345678", BC.HashPassword("senha123"), "perfil.jpg") { Id = usuarioId };
        var visitante = new Usuario("João", "joao@gmail.com", "João", new DateOnly(2000, 5, 10), "87654321", BC.HashPassword("senha456"), "perfil.jpg") { Id = visitanteId };

        var postsPrivados = new List<Post>
    {
        new Post { Id = 1, Conteudo = "Post 1", AutorId = usuarioId, PermissaoVisualizar = Permissao.PRIVADO },
        new Post { Id = 2, Conteudo = "Post 2", AutorId = visitanteId, PermissaoVisualizar = Permissao.PRIVADO },
    };

        _amizadeRepositoryMock.Setup(r => r.VerificarAmizadeAceita(usuarioId, visitanteId)).ReturnsAsync(false);
        _postRepositoryMock.Setup(r => r.ListarPostsPublicosUsuario(usuarioId)).ReturnsAsync(new List<Post>());
        _postRepositoryMock.Setup(r => r.ListarTodosPostsUsuario(usuarioId)).ReturnsAsync(postsPrivados);

        // Act
        var result = await _postService.ObterPostsDoUsuario(usuarioId, visitanteId);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task AlterarPermissaoPostUsuario_PostExistente_AlteraPermissao()
    {
        // Arrange
        int postId = 1;
        Permissao novaPermissao = Permissao.PRIVADO;

        var post = new Post { Id = postId, PermissaoVisualizar = Permissao.PUBLICO };

        _postRepositoryMock.Setup(r => r.ObterPostIdAsync(postId)).ReturnsAsync(post);
        _postRepositoryMock.Setup(r => r.AtualizarPost(It.IsAny<Post>())).Returns(Task.CompletedTask);

        // Act
        await _postService.AlterarPermissaoPostUsuario(postId, novaPermissao);

        // Assert
        Assert.Equal(novaPermissao, post.PermissaoVisualizar);
        _postRepositoryMock.Verify(r => r.AtualizarPost(post), Times.Once);
    }

    [Fact]
    public async Task AlterarPermissaoPostUsuario_PostNaoEncontrado_DeveRetornarNotificacao()
    {
        // Arrange
        int postId = 1;
        Permissao permissao = Permissao.PUBLICO;
        Post post = null;

        _postRepositoryMock.Setup(repo => repo.ObterPostIdAsync(postId))
            .ReturnsAsync(post);

        // Act
        var result = await _postService.AlterarPermissaoPostUsuario(postId, permissao);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result.Notifications);
        Assert.Equal("Post não encontrado.", result.Notifications.First().Message);

        _postRepositoryMock.Verify(repo => repo.AtualizarPost(It.IsAny<Post>()), Times.Never);
    }


    [Fact]
    public async Task AtualizarContadorCurtidas_PostExistente_AtualizaContadorCurtidas()
    {
        // Arrange
        int postId = 1;
        int curtidas = 1;

        var post = new Post { Id = postId };

        _postRepositoryMock.Setup(r => r.ObterPostIdAsync(postId)).ReturnsAsync(post);
        _curtidaRepositoryMock.Setup(r => r.ContarCurtidasDoPost(postId)).ReturnsAsync(curtidas);
        _postRepositoryMock.Setup(r => r.AtualizarPost(It.IsAny<Post>())).Returns(Task.CompletedTask);

        // Act
        await _postService.AtualizarContadorCurtidas(postId);

        // Assert
        Assert.Equal(curtidas, post.Curtidas);
        _postRepositoryMock.Verify(r => r.AtualizarPost(post), Times.Once);
    }

    [Fact]
    public async Task AtualizarContadorCurtidas_PostNaoEncontrado_NaoAtualizaContadorCurtidas()
    {
        // Arrange
        int postId = 1;

        _postRepositoryMock.Setup(r => r.ObterPostIdAsync(postId)).ReturnsAsync((Post)null);

        // Act
        await _postService.AtualizarContadorCurtidas(postId);

        // Assert
        _postRepositoryMock.Verify(r => r.AtualizarPost(It.IsAny<Post>()), Times.Never);
    }

}
