using Moq;
using RedeSocial.Application.Contacts;
using RedeSocial.Application.Implementations.Services;
using RedeSocial.Domain.Contracts.Repositories;
using RedeSocial.Domain.Models.Enums;
using RedeSocial.Domain.Models;
using BC = BCrypt.Net.BCrypt;
using System;

namespace RedeSocial.TestUnit.Services;

public class AmizadeServiceTests
{
    private Mock<IAmizadeRepository> _amizadeRepositoryMock;
    private Mock<IUsuarioRepository> _usuarioRepositoryMock;
    private AmizadeService _amizadeService;
    private UsuarioService _usuarioService;

    public AmizadeServiceTests()
    {
        _amizadeRepositoryMock = new Mock<IAmizadeRepository>();
        _usuarioRepositoryMock = new Mock<IUsuarioRepository>();
        _amizadeService = new AmizadeService(_usuarioRepositoryMock.Object, _amizadeRepositoryMock.Object);
        _usuarioService = new UsuarioService(_usuarioRepositoryMock.Object);
    }

    [Fact]
    public async Task EnviarPedidoAmizadeAsync_DeveRetornarTrueQuandoPedidoEnviadoComSucesso()
    {
        // Arrange
        int usuarioId = 1;
        int amigoId = 2;

        var usuario = new Usuario("Carlos", "carlos@gmail.com", "Ali", new DateOnly(1999, 11, 19), "12345678", BC.HashPassword("senha123"), "perfil.jpg") { Id = usuarioId };
        var amigo = new Usuario("Maria", "maria@gmail.com", "Maria", new DateOnly(1995, 3, 15), "98765432", BC.HashPassword("senha789"), "perfil.jpg") { Id = amigoId };

        _usuarioRepositoryMock.Setup(repo => repo.ObterUsuarioIdAsync(usuarioId))
            .ReturnsAsync(usuario);
        _usuarioRepositoryMock.Setup(repo => repo.ObterUsuarioIdAsync(amigoId))
            .ReturnsAsync(amigo);
        _amizadeRepositoryMock.Setup(repo => repo.VerificarPedidoAmizadeAsync(usuarioId, amigoId))
            .ReturnsAsync(false);
        _amizadeRepositoryMock.Setup(repo => repo.CriarPedidoAmizadeAsync(It.IsAny<Amizade>()))
            .Returns(Task.CompletedTask);

        // Act
        bool resultado = await _amizadeService.EnviarPedidoAmizadeAsync(usuarioId, amigoId);

        // Assert
        Assert.True(resultado);
        _usuarioRepositoryMock.Verify(repo => repo.ObterUsuarioIdAsync(usuarioId), Times.Once);
        _usuarioRepositoryMock.Verify(repo => repo.ObterUsuarioIdAsync(amigoId), Times.Once);
        _amizadeRepositoryMock.Verify(repo => repo.VerificarPedidoAmizadeAsync(usuarioId, amigoId), Times.Once);
        _amizadeRepositoryMock.Verify(repo => repo.CriarPedidoAmizadeAsync(It.Is<Amizade>(a =>
            a.UsuarioId == usuarioId && a.AmigoId == amigoId && a.StatusAmizade == StatusAmizade.SOLICITADO)), Times.Once);
    }

    [Fact]
    public async Task ListarPedidosAmizadeSolicitados_DeveRetornarPedidosCorretos()
    {
        // Arrange
        int usuarioId = 1;

        var pedidos = new List<Amizade>
            {
                new Amizade { Id = 1, UsuarioId = 1, AmigoId = 2, StatusAmizade = StatusAmizade.SOLICITADO },
                new Amizade { Id = 2, UsuarioId = 1, AmigoId = 3, StatusAmizade = StatusAmizade.SOLICITADO },
            };

        _amizadeRepositoryMock.Setup(repo => repo.PedidosSolicitados(usuarioId))
            .ReturnsAsync(pedidos);

        // Act
        var result = await _amizadeService.ListarPedidosAmizadeSolicitados(usuarioId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);

        Assert.Equal(1, result[0].PedidoId);
        Assert.Equal(1, result[0].UsuarioId);
        Assert.Equal(2, result[0].AmigoId);
        Assert.Equal(StatusAmizade.SOLICITADO.ToString(), result[0].StatusAmizade);

        Assert.Equal(2, result[1].PedidoId);
        Assert.Equal(1, result[1].UsuarioId);
        Assert.Equal(3, result[1].AmigoId);
        Assert.Equal(StatusAmizade.SOLICITADO.ToString(), result[1].StatusAmizade);
    }

    [Fact]
    public async Task ObterAmigos_DeveRetornarAmigosCorretos()
    {
        // Arrange
        int usuarioId = 1;

        var usuario = new Usuario("Carlos", "carlos@gmail.com", "Ali", new DateOnly(1999, 11, 19), "12345678", BC.HashPassword("senha123"), "perfil.jpg") { Id = usuarioId };
        var amigo = new Usuario("João", "joao@gmail.com", "João", new DateOnly(2000, 5, 10), "87654321", BC.HashPassword("senha456"), "perfil.jpg") { Id = 2 };
        var amiga = new Usuario("Maria", "maria@gmail.com", "Maria", new DateOnly(1995, 3, 15), "98765432", BC.HashPassword("senha789"), "perfil.jpg") { Id = 3 };

        var amizades = new List<Amizade>
    {
        new Amizade { Id = 1, UsuarioId = 1, Usuario = usuario, AmigoId = 2, Amigo = amigo, StatusAmizade = StatusAmizade.ACEITO },
        new Amizade { Id = 3, UsuarioId = 3, Usuario = amiga, AmigoId = 1, Amigo = usuario, StatusAmizade = StatusAmizade.ACEITO },
    };

        _amizadeRepositoryMock.Setup(repo => repo.ObterAmizadesDoUsuario(usuarioId, StatusAmizade.ACEITO))
            .ReturnsAsync(amizades);

        // Act
        var result = await _amizadeService.ObterAmigos(usuarioId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);

        Assert.Contains(result, u => u?.Id == 2);
        Assert.Contains(result, u => u?.Id == 3);
    }

    [Fact]
    public void ObterPedidoAmizadePorId_DeveRetornarPedidoAmizadeCorreto()
    {
        // Arrange
        int pedidoAmizadeId = 1;

        var usuario = new Usuario("Carlos", "carlos@gmail.com", "Ali", new DateOnly(1999, 11, 19), "12345678", BC.HashPassword("senha123"), "perfil.jpg") { Id = 1 };
        var amigo = new Usuario("João", "joao@gmail.com", "João", new DateOnly(2000, 5, 10), "87654321", BC.HashPassword("senha456"), "perfil.jpg") { Id = 2 };

        var pedidoAmizade = new Amizade { Id = pedidoAmizadeId, UsuarioId = 1, Usuario = usuario, AmigoId = 2, Amigo = amigo, StatusAmizade = StatusAmizade.ACEITO };
;
        _amizadeRepositoryMock.Setup(repo => repo.ObterAmizadePorId(pedidoAmizadeId))
            .Returns(pedidoAmizade);

        // Act
        var result = _amizadeService.ObterPedidoAmizadePorId(pedidoAmizadeId);

        // Assert
        Assert.Equal(pedidoAmizade, result);
        _amizadeRepositoryMock.Verify(repo => repo.ObterAmizadePorId(pedidoAmizadeId), Times.Once);
    }

    [Fact]
    public void AceitarPedidoAmizade_DeveAtualizarStatusAmizadeCorretamente()
    {
        // Arrange
        int pedidoAmizadeId = 1;
        bool aceitar = true;

        var usuario = new Usuario("Carlos", "carlos@gmail.com", "Ali", new DateOnly(1999, 11, 19), "12345678", BC.HashPassword("senha123"), "perfil.jpg") { Id = 1 };
        var amigo = new Usuario("João", "joao@gmail.com", "João", new DateOnly(2000, 5, 10), "87654321", BC.HashPassword("senha456"), "perfil.jpg") { Id = 2 };

        var pedidoAmizade = new Amizade { Id = pedidoAmizadeId, UsuarioId = 1, Usuario = usuario, AmigoId = 2, Amigo = amigo, StatusAmizade = StatusAmizade.SOLICITADO };

        _amizadeRepositoryMock.Setup(repo => repo.ObterAmizadePorId(pedidoAmizadeId))
            .Returns(pedidoAmizade);

        // Act
        _amizadeService.ResponderPedidoAmizade(pedidoAmizadeId, aceitar);

        // Assert
        Assert.Equal(aceitar ? StatusAmizade.ACEITO : StatusAmizade.NEGADO, pedidoAmizade.StatusAmizade);
        _amizadeRepositoryMock.Verify(repo => repo.AtualizarPedidoAmizade(pedidoAmizade), Times.Once);
    }

    [Fact]
    public void NegarPedidoAmizade_DeveAtualizarStatusAmizadeCorretamente()
    {
        // Arrange
        int pedidoAmizadeId = 1;
        bool aceitar = false;

        var usuario = new Usuario("Carlos", "carlos@gmail.com", "Ali", new DateOnly(1999, 11, 19), "12345678", BC.HashPassword("senha123"), "perfil.jpg") { Id = 1 };
        var amigo = new Usuario("João", "joao@gmail.com", "João", new DateOnly(2000, 5, 10), "87654321", BC.HashPassword("senha456"), "perfil.jpg") { Id = 2 };

        var pedidoAmizade = new Amizade { Id = pedidoAmizadeId, UsuarioId = 1, Usuario = usuario, AmigoId = 2, Amigo = amigo, StatusAmizade = StatusAmizade.SOLICITADO };

        _amizadeRepositoryMock.Setup(repo => repo.ObterAmizadePorId(pedidoAmizadeId))
            .Returns(pedidoAmizade);

        // Act
        _amizadeService.ResponderPedidoAmizade(pedidoAmizadeId, aceitar);

        // Assert
        Assert.Equal(aceitar ? StatusAmizade.ACEITO : StatusAmizade.NEGADO, pedidoAmizade.StatusAmizade);
        _amizadeRepositoryMock.Verify(repo => repo.AtualizarPedidoAmizade(pedidoAmizade), Times.Once);
    }

    [Fact]
    public void ResponderPedidoAmizade_DeveAtualizarStatusAmizadeCorretamente()
    {
        // Arrange
        int pedidoAmizadeId = 1;
        bool aceitar = false;

        var usuario = new Usuario("Carlos", "carlos@gmail.com", "Ali", new DateOnly(1999, 11, 19), "12345678", BC.HashPassword("senha123"), "perfil.jpg") { Id = 1 };
        var amigo = new Usuario("João", "joao@gmail.com", "João", new DateOnly(2000, 5, 10), "87654321", BC.HashPassword("senha456"), "perfil.jpg") { Id = 2 };

        var pedidoAmizade = new Amizade { Id = pedidoAmizadeId, UsuarioId = 1, Usuario = usuario, AmigoId = 2, Amigo = amigo, StatusAmizade = StatusAmizade.SOLICITADO };

        _amizadeRepositoryMock.Setup(repo => repo.ObterAmizadePorId(pedidoAmizadeId))
            .Returns(pedidoAmizade);

        // Act
        _amizadeService.ResponderPedidoAmizade(pedidoAmizadeId, aceitar);

        // Assert
        Assert.Equal(aceitar ? StatusAmizade.ACEITO : StatusAmizade.NEGADO, pedidoAmizade.StatusAmizade);
        _amizadeRepositoryMock.Verify(repo => repo.AtualizarPedidoAmizade(pedidoAmizade), Times.Once);
    }

    [Fact]
    public void ResponderPedidoAmizade_StatusJaAceito_DeveLancarException()
    {
        // Arrange
        int pedidoAmizadeId = 1;
        bool aceitar = true;

        var usuario = new Usuario("Carlos", "carlos@gmail.com", "Ali", new DateOnly(1999, 11, 19), "12345678", BC.HashPassword("senha123"), "perfil.jpg") { Id = 1 };
        var amigo = new Usuario("João", "joao@gmail.com", "João", new DateOnly(2000, 5, 10), "87654321", BC.HashPassword("senha456"), "perfil.jpg") { Id = 2 };

        var pedidoAmizade = new Amizade { Id = pedidoAmizadeId, UsuarioId = 1, Usuario = usuario, AmigoId = 2, Amigo = amigo, StatusAmizade = StatusAmizade.ACEITO };

        _amizadeRepositoryMock.Setup(repo => repo.ObterAmizadePorId(pedidoAmizadeId))
            .Returns(pedidoAmizade);

        // Act
        var response = _amizadeService.ResponderPedidoAmizade(pedidoAmizadeId, aceitar);

        // Assert
        Assert.NotNull(response);
        Assert.NotEmpty(response.Notifications);
        Assert.Contains(response.Notifications, n => n.Message == "Este pedido de amizade já foi respondido anteriormente.");
        _amizadeRepositoryMock.Verify(repo => repo.AtualizarPedidoAmizade(It.IsAny<Amizade>()), Times.Never);
    }

    [Fact]
    public void ResponderPedidoAmizade_PedidoNulo_DeveRetornarPedidoNaoEncontrado()
    {
        // Arrange
        int pedidoAmizadeId = 1;
        bool aceitar = true;
        Amizade pedidoAmizade = null;

        _amizadeRepositoryMock.Setup(repo => repo.ObterAmizadePorId(pedidoAmizadeId))
            .Returns(pedidoAmizade);

        // Act
        var response = _amizadeService.ResponderPedidoAmizade(pedidoAmizadeId, aceitar);

        // Assert
        Assert.NotNull(response);
        Assert.NotEmpty(response.Notifications);
        Assert.Contains(response.Notifications, n => n.Message == "Pedido de amizade não encontrado.");
        _amizadeRepositoryMock.Verify(repo => repo.AtualizarPedidoAmizade(It.IsAny<Amizade>()), Times.Never);
    }



    [Fact]
    public async Task RemoverAmizade_AmizadeExistente_DeveRemoverAmizade()
    {
        // Arrange
        int usuarioId = 1;
        int amigoId = 2;
        var amizade = new Amizade
        {
            UsuarioId = usuarioId,
            AmigoId = amigoId,
            StatusAmizade = StatusAmizade.ACEITO
        };

        _amizadeRepositoryMock.Setup(repo => repo.ObterAmizade(usuarioId, amigoId))
            .ReturnsAsync(amizade);

        // Act
        await _amizadeService.RemoverAmizade(usuarioId, amigoId);

        // Assert
        _amizadeRepositoryMock.Verify(repo => repo.RemoverAmizade(amizade), Times.Once);
    }

    [Fact]
    public async Task RemoverAmizade_AmizadeSolicitada_DeveRetornarNotificacao()
    {
        // Arrange
        int usuarioId = 1;
        int amigoId = 2;
        Amizade amizade = new Amizade { StatusAmizade = StatusAmizade.SOLICITADO };

        _amizadeRepositoryMock.Setup(repo => repo.ObterAmizade(usuarioId, amigoId))
            .ReturnsAsync(amizade);

        // Act
        var response = await _amizadeService.RemoverAmizade(usuarioId, amigoId);

        // Assert
        Assert.Single(response.Notifications);
        Assert.Equal("Não foi possível remover a amizade.", response.Notifications.First().Message);
        _amizadeRepositoryMock.Verify(repo => repo.RemoverAmizade(It.IsAny<Amizade>()), Times.Never);
    }


    [Fact]
    public async Task VerificarAmizadeSolicitada_DeveRetornarTrueSeExisteSolicitacao()
    {
        // Arrange
        int usuarioAutenticadoId = 1;
        int usuarioSolicitadoId = 2;

        _amizadeRepositoryMock.Setup(repo => repo.VerificarAmizadeSolicitada(usuarioAutenticadoId, usuarioSolicitadoId))
            .ReturnsAsync(true);

        // Act
        var result = await _amizadeService.VerificarAmizadeSolicitada(usuarioAutenticadoId, usuarioSolicitadoId);

        // Assert
        Assert.True(result);
    }

}

