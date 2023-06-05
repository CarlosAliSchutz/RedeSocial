using Moq;
using RedeSocial.Application.Contacts.Documents.Response;
using RedeSocial.Application.Contacts.Documents.Request;
using RedeSocial.Application.Implementations.Services;
using RedeSocial.Domain.Contracts.Repositories;
using RedeSocial.Domain.Models;
using BC = BCrypt.Net.BCrypt;

namespace RedeSocial.TestUnit.Services;

public class ComentarioServiceTests
{
    private Mock<IPostRepository> _postRepositoryMock;
    private Mock<IUsuarioRepository> _usuarioRepositoryMock;
    private Mock<IComentarioRepository> _comentarioRepositoryMock;
    private ComentarioService _comentarioService;
    private UsuarioService _usuarioService;

    public ComentarioServiceTests()
    {
        _postRepositoryMock = new Mock<IPostRepository>();
        _usuarioRepositoryMock = new Mock<IUsuarioRepository>();
        _comentarioRepositoryMock = new Mock<IComentarioRepository>();
        _usuarioService = new UsuarioService(_usuarioRepositoryMock.Object);
        _comentarioService = new ComentarioService(_postRepositoryMock.Object, _usuarioService, _comentarioRepositoryMock.Object);
    }

    [Fact]
    public async Task Comentar_PostExistente_DeveRetornarComentarioResponse()
    {
        // Arrange
        int postId = 2;
        int autorId = 1;
        var comentarioRequest = new ComentarioRequest
        {
            Conteudo = "Ótimo post!"
        };

        var usuario = new Usuario("Carlos", "carlos@gmail.com", "Ali", new DateOnly(1999, 11, 19), "12345678", BC.HashPassword("senha123"), "perfil.jpg")
        {
            Id = autorId
        };

        var post = new Post { Id = postId };

        _postRepositoryMock.Setup(r => r.ObterPostIdAsync(postId)).ReturnsAsync(post);
        _usuarioRepositoryMock.Setup(u => u.ObterUsuarioIdAsync(autorId)).ReturnsAsync(usuario);

        // Act
        var result = await _comentarioService.Comentar(postId, comentarioRequest, autorId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(comentarioRequest.Conteudo, result.Conteudo);
        Assert.Equal(DateTime.Now.Date, result.DataPublicacao.Date);
        Assert.Equal(autorId, result.AutorId);
        Assert.Equal(usuario.Nome, result.AutorNome);

        _comentarioRepositoryMock.Verify(r => r.Add(It.IsAny<Comentario>()), Times.Once);
    }

    [Fact]
    public async Task Comentar_PostNaoExistente_DeveRetornarComentarioResponseComNotificacaoPostNaoEncontrado()
    {
        // Arrange
        int postId = 2;
        int autorId = 1;
        var comentarioRequest = new ComentarioRequest
        {
            Conteudo = "Ótimo post!"
        };

        var usuario = new Usuario("Carlos", "carlos@gmail.com", "Ali", new DateOnly(1999, 11, 19), "12345678", BC.HashPassword("senha123"), "perfil.jpg")
        {
            Id = autorId
        };

        _usuarioRepositoryMock.Setup(u => u.ObterUsuarioIdAsync(autorId)).ReturnsAsync(usuario);

        // Act
        var result = await _comentarioService.Comentar(postId, comentarioRequest, autorId);

        // Assert
        Assert.NotNull(result);

        _comentarioRepositoryMock.Verify(r => r.Add(It.IsAny<Comentario>()), Times.Never);
        Assert.Single(result.Notifications);
        Assert.Contains(result.Notifications, n => n.Message == "Post não encontrado.");
    }

    [Fact]
    public async Task ObterComentariosDoPostAsync_DeveRetornarListaDeComentarios()
    {
        // Arrange
        int postId = 2;
        var comentarios = new List<Comentario>
    {
        new Comentario { Id = 1, Conteudo = "Ótimo post!", PostId = postId, AutorId = 1, Autor = "Carlos", DataPublicacao = DateTime.Now },
        new Comentario { Id = 2, Conteudo = "Parabéns!", PostId = postId, AutorId = 2, Autor = "Maria", DataPublicacao = DateTime.Now },
        new Comentario { Id = 3, Conteudo = "Muito interessante.", PostId = postId, AutorId = 3, Autor = "João", DataPublicacao = DateTime.Now }
    };

        _comentarioRepositoryMock.Setup(r => r.Obter(postId)).ReturnsAsync(comentarios);

        // Act
        var result = await _comentarioService.ObterComentariosDoPostAsync(postId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(comentarios.Count, result.Count);

        Assert.Equal(comentarios[0].Id, result[0].Id);
        Assert.Equal(comentarios[0].Conteudo, result[0].Conteudo);
        Assert.Equal(comentarios[0].PostId, result[0].PostId);
        Assert.Equal(comentarios[0].AutorId, result[0].AutorId);
        Assert.Equal(comentarios[0].Autor, result[0].Autor);
        Assert.Equal(comentarios[0].DataPublicacao, result[0].DataPublicacao);

        Assert.Equal(comentarios[1].Id, result[1].Id);
        Assert.Equal(comentarios[1].Conteudo, result[1].Conteudo);
        Assert.Equal(comentarios[1].PostId, result[1].PostId);
        Assert.Equal(comentarios[1].AutorId, result[1].AutorId);
        Assert.Equal(comentarios[1].Autor, result[1].Autor);
        Assert.Equal(comentarios[1].DataPublicacao, result[1].DataPublicacao);

        Assert.Equal(comentarios[2].Id, result[2].Id);
        Assert.Equal(comentarios[2].Conteudo, result[2].Conteudo);
        Assert.Equal(comentarios[2].PostId, result[2].PostId);
        Assert.Equal(comentarios[2].AutorId, result[2].AutorId);
        Assert.Equal(comentarios[2].Autor, result[2].Autor);
        Assert.Equal(comentarios[2].DataPublicacao, result[2].DataPublicacao);

        _comentarioRepositoryMock.Verify(r => r.Obter(postId), Times.Once);
    }
}
