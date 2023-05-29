using Microsoft.AspNetCore.Routing;
using Moq;
using RedeSocial.Application.Contacts.Documents.Request;
using RedeSocial.Application.Contacts.Documents.Response;
using RedeSocial.Application.Implementations.Services;
using RedeSocial.Domain.Contracts.Repositories;
using RedeSocial.Domain.Models;
using RedeSocial.Infrastructure.Repositories;
using BC = BCrypt.Net.BCrypt;

namespace RedeSocial.TestUnit.Services;

public class UsuarioServiceTests
{
    private Mock<IUsuarioRepository> _usuarioRepositoryMock;
    private UsuarioService _usuarioService;


    public UsuarioServiceTests()
    {
        _usuarioRepositoryMock = new Mock<IUsuarioRepository>();
        _usuarioService = new UsuarioService(_usuarioRepositoryMock.Object);
    }

    [Fact]
    public async Task Cadastro_Sucesso()
    {
        var usuarioRepositoryMock = new Mock<IUsuarioRepository>();
        usuarioRepositoryMock.Setup(repo => repo.ObterUsuarioPorEmail(It.IsAny<string>()))
            .ReturnsAsync((Usuario)null);

        var usuarioService = new UsuarioService(usuarioRepositoryMock.Object);

        var request = new UsuarioRequest
        {
            Nome = "Carlos",
            Email = "carlos@gmail.com",
            Apelido = "Ali",
            DataNascimento = new DateOnly(1999, 11, 19),
            Cep = "93806020",
            SenhaHash = "123",
            ImagemPerfil = "instagram"
        };

        var response = await usuarioService.Cadastro(request);

        Assert.NotNull(response);
        Assert.IsType<UsuarioResponse>(response);
    }

    [Fact]
    public async Task Cadastro_Invalido_SemNome_ReturnaNotificacao()
    {
        // Arrange
        var usuarioRepositoryMock = new Mock<IUsuarioRepository>();
        var usuarioService = new UsuarioService(usuarioRepositoryMock.Object);

        var request = new UsuarioRequest
        {
            Email = "carlos@gmail.com",
            Apelido = "Ali",
            DataNascimento = new DateOnly(1999, 11, 19),
            Cep = "93806020",
            SenhaHash = "123",
            ImagemPerfil = "instagram"
        };

        // Act
        var response = await usuarioService.Cadastro(request);

        // Assert
        Assert.NotNull(response);
        Assert.IsType<UsuarioResponse>(response);
        Assert.NotNull(response.Notifications);
        Assert.NotEmpty(response.Notifications.ToList());
        Assert.Equal(1, response.Notifications.ToList().Count);
        Assert.Equal("O campo Nome é obrigatório.", response.Notifications.ToList()[0].Message);
    }

    [Fact]
    public async Task ValidateLogin_CredenciaisCorretas_RetornaLoginResponseAutenticado()
    {
        var loginRequest = new LoginRequest
        {
            Email = "carlos@gmail.com",
            Senha = "123"
        };

        var usuario = new Usuario("Carlos", "carlos@gmail.com", "Carlos", new DateOnly(1999, 11, 19), "12345678", BC.HashPassword("senha123"), "perfil.jpg")
        {
            Id = 1
        };

        _usuarioRepositoryMock.Setup(repo => repo.ObterCredenciaisUsuario(loginRequest.Email))
            .ReturnsAsync(usuario);

        var response = await _usuarioService.ValidateLogin(loginRequest);

        Assert.NotNull(response);
        Assert.True(response.IsAuthenticated);
        Assert.Equal(usuario.Id, response.Id);
        Assert.Equal(usuario.Email, response.Email);
        Assert.Empty(response.Notifications);
    }

}
