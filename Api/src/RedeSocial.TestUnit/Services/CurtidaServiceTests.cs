using Moq;
using RedeSocial.Application.Contacts;
using RedeSocial.Domain.Contracts.Repositories;
using RedeSocial.Domain.Models;
using RedeSocial.Application.Implementations.Services;

namespace RedeSocial.TestUnit.Services;

public class CurtidaServiceTests
{
    private readonly Mock<ICurtidaRepository> _curtidaRepositoryMock;
    private readonly Mock<IPostRepository> _postRepositoryMock;
    private readonly ICurtidaService _curtidaService;

    public CurtidaServiceTests()
    {
        _curtidaRepositoryMock = new Mock<ICurtidaRepository>();
        _postRepositoryMock = new Mock<IPostRepository>();
        _curtidaService = new CurtidaService(_curtidaRepositoryMock.Object, _postRepositoryMock.Object);
    }

    [Fact]
    public async Task CurtirPost_CurtidaExistente_DeveRemoverCurtida()
    {
        // Arrange
        int postId = 1;
        int usuarioId = 1;

        var curtidaExistente = new Curtida { PostId = postId, UsuarioId = usuarioId };
        _curtidaRepositoryMock.Setup(r => r.ObterCurtida(postId, usuarioId)).ReturnsAsync(curtidaExistente);

        // Act
        var result = await _curtidaService.CurtirPost(postId, usuarioId);

        // Assert
        Assert.False(result);
        _curtidaRepositoryMock.Verify(r => r.Remover(curtidaExistente), Times.Once);
        _postRepositoryMock.Verify(r => r.DecrementarCurtidas(postId), Times.Once);
        _curtidaRepositoryMock.Verify(r => r.Criar(It.IsAny<Curtida>()), Times.Never);
        _postRepositoryMock.Verify(r => r.IncrementarCurtidas(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task CurtirPost_CurtidaNaoExistente_DeveCriarCurtida()
    {
        // Arrange
        int postId = 1;
        int usuarioId = 1;

        _curtidaRepositoryMock.Setup(r => r.ObterCurtida(postId, usuarioId)).ReturnsAsync((Curtida)null);

        // Act
        var result = await _curtidaService.CurtirPost(postId, usuarioId);

        // Assert
        Assert.True(result);
        _curtidaRepositoryMock.Verify(r => r.Remover(It.IsAny<Curtida>()), Times.Never);
        _postRepositoryMock.Verify(r => r.DecrementarCurtidas(It.IsAny<int>()), Times.Never);
        _curtidaRepositoryMock.Verify(r => r.Criar(It.IsAny<Curtida>()), Times.Once);
        _postRepositoryMock.Verify(r => r.IncrementarCurtidas(postId), Times.Once);
    }
}