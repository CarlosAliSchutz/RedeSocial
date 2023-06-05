using Moq;
using RedeSocial.Application.Contacts;
using RedeSocial.Application.Contacts.Documents.Request;
using RedeSocial.Application.Contacts.Documents.Response;
using RedeSocial.Application.Implementations.Services;
using RedeSocial.Application.Validations.Core;
using RedeSocial.Domain.Contracts.Repositories;
using RedeSocial.Domain.Models;
using BC = BCrypt.Net.BCrypt;

namespace RedeSocial.TestUnit.Services;

public class MensagemServiceTests
{
    private readonly Mock<IMensagemRepository> _mensagemRepositoryMock;
    private readonly Mock<IUsuarioService> _usuarioServiceMock;
    private readonly IMensagemService _mensagemService;

    public MensagemServiceTests()
    {
        _mensagemRepositoryMock = new Mock<IMensagemRepository>();
        _usuarioServiceMock = new Mock<IUsuarioService>();
        _mensagemService = new MensagemService(_mensagemRepositoryMock.Object, _usuarioServiceMock.Object);
    }

    [Fact]
    public async Task EnviarMensagem_MesmoUsuarioIdEAmigoId_DeveRetornarNotificacao()
    {
        // Arrange
        int usuarioId = 1;
        var mensagemRequest = new MensagemRequest { AmigoId = usuarioId };
        var expectedNotification = new Notification("Não é possível enviar mensagem para o mesmo usuário.");

        // Act
        var result = await _mensagemService.EnviarMensagem(usuarioId, mensagemRequest);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result.Notifications);
        Assert.Contains(result.Notifications, n => n.Message == expectedNotification.Message);
        _mensagemRepositoryMock.Verify(r => r.CriarMensagem(It.IsAny<Mensagem>()), Times.Never);
    }

    [Fact]
    public async Task EnviarMensagem_Sucesso_DeveCriarMensagemERetornarMensagemResponse()
    {
        // Arrange
        int usuarioId = 1;
        int amigoId = 2;
        var mensagemRequest = new MensagemRequest { AmigoId = amigoId, Conteudo = "Olá, amigo!" };
        var usuario = new Usuario("Carlos", "carlos@gmail.com", "Ali", new DateOnly(1999, 11, 19), "12345678", BC.HashPassword("senha123"), "perfil.jpg") { Id = usuarioId };
        var amigo = new Usuario("Maria", "maria@gmail.com", "Maria", new DateOnly(1995, 3, 15), "98765432", BC.HashPassword("senha789"), "perfil.jpg") { Id = amigoId };
        var mensagem = new Mensagem
        {
            UsuarioId = usuarioId,
            Usuario = usuario,
            AmigoId = amigoId,
            Amigo = amigo,
            Conteudo = mensagemRequest.Conteudo,
            DataEnvio = DateTime.UtcNow
        };
        var expectedResponse = new MensagemResponse
        {
            Id = mensagem.Id,
            RemetenteId = mensagem.UsuarioId,
            RemetenteNome = mensagem.Usuario.Nome,
            DestinatarioId = mensagem.AmigoId,
            DestinatarioNome = mensagem.Amigo.Nome,
            Conteudo = mensagem.Conteudo,
            DataEnvio = mensagem.DataEnvio
        };

        _usuarioServiceMock.Setup(s => s.ObterUsuario(usuarioId)).Returns(usuario);
        _usuarioServiceMock.Setup(s => s.ObterUsuario(amigoId)).Returns(amigo);
        _mensagemRepositoryMock.Setup(r => r.CriarMensagem(It.IsAny<Mensagem>())).Returns(Task.CompletedTask);

        // Act
        var result = await _mensagemService.EnviarMensagem(usuarioId, mensagemRequest);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedResponse.Id, result.Id);
        Assert.Equal(expectedResponse.RemetenteId, result.RemetenteId);
        Assert.Equal(expectedResponse.RemetenteNome, result.RemetenteNome);
        Assert.Equal(expectedResponse.DestinatarioId, result.DestinatarioId);
        Assert.Equal(expectedResponse.DestinatarioNome, result.DestinatarioNome);
        Assert.Equal(expectedResponse.Conteudo, result.Conteudo);
    }

    [Fact]
    public async Task ObterConversa_DeveRetornarConversaCorreta()
    {
        // Arrange
        int usuarioId = 1;
        int amigoId = 2;

        var mensagem1 = new Mensagem
        {
            Id = 1,
            UsuarioId = usuarioId,
            AmigoId = amigoId,
            Conteudo = "Olá",
            DataEnvio = DateTime.UtcNow
        };

        var mensagem2 = new Mensagem
        {
            Id = 2,
            UsuarioId = amigoId,
            AmigoId = usuarioId,
            Conteudo = "Olá, como você está?",
            DataEnvio = DateTime.UtcNow.AddMinutes(1)
        };

        var usuario = new Usuario("Carlos", "carlos@gmail.com", "Ali", new DateOnly(1999, 11, 19), "12345678", BC.HashPassword("senha123"), "perfil.jpg") { Id = usuarioId };
        var amigo = new Usuario("Maria", "maria@gmail.com", "Maria", new DateOnly(1995, 3, 15), "98765432", BC.HashPassword("senha789"), "perfil.jpg") { Id = amigoId };

        var mensagens = new List<Mensagem> { mensagem1, mensagem2 };

        _mensagemRepositoryMock.Setup(r => r.GetConversaAsync(usuarioId, amigoId)).ReturnsAsync(mensagens);
        _usuarioServiceMock.Setup(s => s.ObterUsuarioAsync(usuarioId)).ReturnsAsync(usuario);
        _usuarioServiceMock.Setup(s => s.ObterUsuarioAsync(amigoId)).ReturnsAsync(amigo);

        var expectedResponse = new List<MensagemResponse>
    {
        new MensagemResponse
        {
            Id = mensagem1.Id,
            RemetenteId = mensagem1.UsuarioId,
            RemetenteNome = usuario.Nome,
            DestinatarioId = mensagem1.AmigoId,
            DestinatarioNome = amigo.Nome,
            Conteudo = mensagem1.Conteudo,
            DataEnvio = mensagem1.DataEnvio
        },
        new MensagemResponse
        {
            Id = mensagem2.Id,
            RemetenteId = mensagem2.UsuarioId,
            RemetenteNome = amigo.Nome,
            DestinatarioId = mensagem2.AmigoId,
            DestinatarioNome = usuario.Nome,
            Conteudo = mensagem2.Conteudo,
            DataEnvio = mensagem2.DataEnvio
        }
    };

        // Act
        var result = await _mensagemService.ObterConversa(usuarioId, amigoId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedResponse.Count, result.Count);

        for (int i = 0; i < expectedResponse.Count; i++)
        {
            Assert.Equal(expectedResponse[i].Id, result[i].Id);
            Assert.Equal(expectedResponse[i].RemetenteId, result[i].RemetenteId);
            Assert.Equal(expectedResponse[i].RemetenteNome, result[i].RemetenteNome);
            Assert.Equal(expectedResponse[i].DestinatarioId, result[i].DestinatarioId);
            Assert.Equal(expectedResponse[i].DestinatarioNome, result[i].DestinatarioNome);
            Assert.Equal(expectedResponse[i].Conteudo, result[i].Conteudo);
            Assert.True(DateTimeOffset.Equals(expectedResponse[i].DataEnvio, result[i].DataEnvio));
        }
    }
}
