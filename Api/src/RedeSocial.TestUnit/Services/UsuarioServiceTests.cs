using Microsoft.AspNetCore.Routing;
using Moq;
using RedeSocial.Application.Contacts.Documents.Request;
using RedeSocial.Application.Contacts.Documents.Response;
using RedeSocial.Application.Implementations.Services;
using RedeSocial.Domain.Contracts.Repositories;
using RedeSocial.Domain.Models;
using RedeSocial.Domain.Utils;
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
    public async Task Cadastro_Invalido_SemEmail_ReturnaNotificacao()
    {
        // Arrange
        var usuarioRepositoryMock = new Mock<IUsuarioRepository>();
        var usuarioService = new UsuarioService(usuarioRepositoryMock.Object);

        var request = new UsuarioRequest
        {
            Nome = "Carlos",
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
        Assert.Equal("O campo Email é obrigatório.", response.Notifications.ToList()[0].Message);
    }

    [Fact]
    public async Task Cadastro_Invalido_DataNascimento_ReturnaNotificacao()
    {
        // Arrange
        var usuarioRepositoryMock = new Mock<IUsuarioRepository>();
        var usuarioService = new UsuarioService(usuarioRepositoryMock.Object);

        var request = new UsuarioRequest
        {
            Nome = "Carlos",
            Email = "carlos@gmail.com",
            Apelido = "Ali",
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
        Assert.Equal("O campo DataNascimento é obrigatório.", response.Notifications.ToList()[0].Message);
    }

    [Fact]
    public async Task Cadastro_Invalido_Cep_ReturnaNotificacao()
    {
        // Arrange
        var usuarioRepositoryMock = new Mock<IUsuarioRepository>();
        var usuarioService = new UsuarioService(usuarioRepositoryMock.Object);

        var request = new UsuarioRequest
        {
            Nome = "Carlos",
            Email = "carlos@gmail.com",
            Apelido = "Ali",
            DataNascimento = new DateOnly(1999, 11, 19),
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
        Assert.Equal("O campo Cep é obrigatório.", response.Notifications.ToList()[0].Message);
    }

    [Fact]
    public async Task Cadastro_Invalido_Senha_ReturnaNotificacao()
    {
        // Arrange
        var usuarioRepositoryMock = new Mock<IUsuarioRepository>();
        var usuarioService = new UsuarioService(usuarioRepositoryMock.Object);

        var request = new UsuarioRequest
        {
            Nome = "Carlos",
            Email = "carlos@gmail.com",
            Apelido = "Ali",
            DataNascimento = new DateOnly(1999, 11, 19),
            Cep = "93806020",
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
        Assert.Equal("O campo Senha é obrigatório.", response.Notifications.ToList()[0].Message);
    }

    [Fact]
    public async Task Cadastro_Invalido_EmailJaExiste_RetornaNotificacao()
    {
        // Arrange
        var usuarioRepositoryMock = new Mock<IUsuarioRepository>();
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

        var usuarioExistente = new Usuario("Fulano", "carlos@gmail.com", "Fulano", new DateOnly(1990, 1, 1), "123456", BC.HashPassword("senha123"), "perfil.jpg");
        usuarioRepositoryMock.Setup(repo => repo.ObterUsuarioPorEmail(request.Email))
            .ReturnsAsync(usuarioExistente);

        // Act
        var response = await usuarioService.Cadastro(request);

        // Assert
        Assert.NotNull(response);
        Assert.IsType<UsuarioResponse>(response);
        Assert.NotNull(response.Notifications);
        Assert.NotEmpty(response.Notifications.ToList());
        Assert.Equal(1, response.Notifications.ToList().Count);
        Assert.Equal("Cadastro inválido.", response.Notifications.ToList()[0].Message);

        usuarioRepositoryMock.Verify(repo => repo.ObterUsuarioPorEmail(request.Email), Times.Once);
    }


    [Fact]
    public async Task ValidateLogin_CredenciaisCorretas_RetornaLoginResponseAutenticado()
    {
        // Arrange
        var usuario = new Usuario("Carlos", "carlos@gmail.com", "Ali", new DateOnly(1999, 11, 19), "12345678", BC.HashPassword("senha123"), "perfil.jpg")
        {
            Id = 1
        };

        var loginRequest = new LoginRequest
        {
            Email = "carlos@gmail.com",
            Senha = "senha123"
        };

        var usuarioRepositoryMock = new Mock<IUsuarioRepository>();
        usuarioRepositoryMock.Setup(repo => repo.ObterCredenciaisUsuario(loginRequest.Email))
            .ReturnsAsync(usuario);

        var usuarioService = new UsuarioService(usuarioRepositoryMock.Object);

        // Act
        var response = await usuarioService.ValidateLogin(loginRequest);

        // Assert
        Assert.NotNull(response);
        Assert.True(response.IsAuthenticated);
        Assert.Equal(usuario.Id, response.Id);
        Assert.Equal(usuario.Email, response.Email);
        Assert.Empty(response.Notifications);

        usuarioRepositoryMock.Verify(repo => repo.ObterCredenciaisUsuario(loginRequest.Email), Times.Once);
    }

    [Fact]
    public async Task ValidateLogin_CredenciaisIncorretas_RetornaNotificacaoErro()
    {
        // Arrange
        var usuario = new Usuario("Carlos", "carlos@gmail.com", "Carlos", new DateOnly(1999, 11, 19), "12345678", BC.HashPassword("senha123"), "perfil.jpg")
        {
            Id = 1
        };

        var loginRequest = new LoginRequest
        {
            Email = "carlos@gmail.com",
            Senha = "123"
        };

        var usuarioRepositoryMock = new Mock<IUsuarioRepository>();
        usuarioRepositoryMock.Setup(repo => repo.ObterCredenciaisUsuario(loginRequest.Email))
            .ReturnsAsync(usuario);

        var usuarioService = new UsuarioService(usuarioRepositoryMock.Object);

        // Act
        var response = await usuarioService.ValidateLogin(loginRequest);

        // Assert
        Assert.NotNull(response);
        Assert.False(response.IsAuthenticated);
        Assert.Equal(0, response.Id);
        Assert.Null(response.Email);
        Assert.NotEmpty(response.Notifications);
        Assert.Equal("Email ou senha incorretos.", response.Notifications.ElementAt(0).Message);

        usuarioRepositoryMock.Verify(repo => repo.ObterCredenciaisUsuario(loginRequest.Email), Times.Once);
    }

    [Fact]
    public void ListarUsuarios_DeveRetornarListaUsuarios()
    {
        // Arrange
        var usuarios = new List<Usuario>
    {
        new Usuario("Carlos", "carlos@gmail.com", "Carlos", new DateOnly(1999, 11, 19), "12345678", BC.HashPassword("senha123"), "perfil.jpg") { Id = 1 },
        new Usuario("João", "joao@gmail.com", "João", new DateOnly(2000, 5, 10), "87654321", BC.HashPassword("senha456"), "perfil.jpg") { Id = 2 },
        new Usuario("Maria", "maria@gmail.com", "Maria", new DateOnly(1995, 3, 15), "98765432", BC.HashPassword("senha789"), "perfil.jpg") { Id = 3 }
    };

        var usuarioRepositoryMock = new Mock<IUsuarioRepository>();
        usuarioRepositoryMock.Setup(repo => repo.Listar())
            .ReturnsAsync(usuarios);

        var usuarioService = new UsuarioService(usuarioRepositoryMock.Object);

        // Act
        var response = usuarioService.ListarUsuarios();

        // Assert
        Assert.NotNull(response);

        var listarUsuariosResponseList = response.ToList();
        Assert.Equal(usuarios.Count, listarUsuariosResponseList.Count);

        for (int i = 0; i < usuarios.Count; i++)
        {
            var expectedUsuario = usuarios[i];
            var actualUsuario = listarUsuariosResponseList[i];

            Assert.Equal(expectedUsuario.Id, actualUsuario.Id);
            Assert.Equal(expectedUsuario.Nome, actualUsuario.Nome);
            Assert.Equal(expectedUsuario.Email, actualUsuario.Email);
            Assert.Equal(expectedUsuario.Apelido, actualUsuario.Apelido);
            Assert.Equal(expectedUsuario.DataNascimento, actualUsuario.DataNascimento);
            Assert.Equal(expectedUsuario.Cep, actualUsuario.Cep);
            Assert.Equal(expectedUsuario.ImagemPerfil, actualUsuario.ImagemPerfil);
        }

        usuarioRepositoryMock.Verify(repo => repo.Listar(), Times.Once);
    }

    [Fact]
    public async Task ObterUsuarioAsync_DeveRetornarUsuario()
    {
        // Arrange
        var usuarioId = 1;
        var usuario = new Usuario("Carlos", "carlos@gmail.com", "Carlos", new DateOnly(1999, 11, 19), "12345678", BC.HashPassword("senha123"), "perfil.jpg")
        {
            Id = usuarioId
        };

        var usuarioRepositoryMock = new Mock<IUsuarioRepository>();
        usuarioRepositoryMock.Setup(repo => repo.ObterId(usuarioId))
            .ReturnsAsync(usuario);

        var usuarioService = new UsuarioService(usuarioRepositoryMock.Object);

        // Act
        var result = await usuarioService.ObterUsuarioAsync(usuarioId);

        // Assert
        Assert.Equal(usuario, result);

        usuarioRepositoryMock.Verify(repo => repo.ObterId(usuarioId), Times.Once);
    }

    [Fact]
    public void ObterUsuario_DeveRetornarUsuario()
    {
        // Arrange
        var usuarioId = 1;
        var usuario = new Usuario("Carlos", "carlos@gmail.com", "Carlos", new DateOnly(1999, 11, 19), "12345678", BC.HashPassword("senha123"), "perfil.jpg")
        {
            Id = usuarioId
        };

        var usuarioRepositoryMock = new Mock<IUsuarioRepository>();
        usuarioRepositoryMock.Setup(repo => repo.ObterIdUsuario(usuarioId))
            .Returns(usuario);

        var usuarioService = new UsuarioService(usuarioRepositoryMock.Object);

        // Act
        var result = usuarioService.ObterUsuario(usuarioId);

        // Assert
        Assert.Equal(usuario, result);

        usuarioRepositoryMock.Verify(repo => repo.ObterIdUsuario(usuarioId), Times.Once);
    }

    [Fact]
    public async Task ObterDadosUsuario_DeveRetornarUsuarioLogadoResponse()
    {
        // Arrange
        var userId = 1;
        var user = new Usuario("Carlos", "carlos@gmail.com", "Carlos", new DateOnly(1999, 11, 19), "12345678", BC.HashPassword("senha123"), "perfil.jpg")
        {
            Id = userId
        };

        var usuarioRepositoryMock = new Mock<IUsuarioRepository>();
        usuarioRepositoryMock.Setup(repo => repo.ObterUsuarioIdAsync(userId))
            .ReturnsAsync(user);

        var usuarioService = new UsuarioService(usuarioRepositoryMock.Object);

        // Act
        var result = await usuarioService.ObterDadosUsuario(userId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(user.Id, result.Id);
        Assert.Equal(user.Nome, result.Nome);
        Assert.Equal(user.Email, result.Email);
        Assert.Equal(user.Apelido, result.Apelido);
        Assert.Equal(user.DataNascimento, result.DataNascimento);
        Assert.Equal(user.Cep, result.Cep);
        Assert.Equal(user.ImagemPerfil, result.ImagemPerfil);

        usuarioRepositoryMock.Verify(repo => repo.ObterUsuarioIdAsync(userId), Times.Once);
    }

    [Fact]
    public async Task PesquisarUsuarios_DeveRetornarPaginatedListDeUsuarios()
    {
        // Arrange
        var usuarioAutenticado = 1;
        var busca = "carlos";
        var pagina = 1;
        var quantidadePorPagina = 10;

        var usuarios = new List<Usuario>
        {
            new Usuario("Carlos", "carlos@gmail.com", "Carlos", new DateOnly(1999, 11, 19), "12345678", BC.HashPassword("senha123"), "perfil.jpg") { Id = 1 },
            new Usuario("João", "joao@gmail.com", "João", new DateOnly(2000, 5, 10), "87654321", BC.HashPassword("senha456"), "perfil.jpg") { Id = 2 },
            new Usuario("Maria", "maria@gmail.com", "Maria", new DateOnly(1995, 3, 15), "98765432", BC.HashPassword("senha789"), "perfil.jpg") { Id = 3 }

        };

        var usuarioRepositoryMock = new Mock<IUsuarioRepository>();
        usuarioRepositoryMock.Setup(repo => repo.ListarUsuarios(usuarioAutenticado, busca, pagina, quantidadePorPagina))
            .ReturnsAsync(new PaginatedList<Usuario>(usuarios, pagina, 1, usuarios.Count));

        var usuarioService = new UsuarioService(usuarioRepositoryMock.Object);

        // Act
        var result = await usuarioService.PesquisarUsuarios(usuarioAutenticado, busca, pagina, quantidadePorPagina);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(usuarios.Count, result.Itens.Count);
        Assert.Equal(pagina, result.Pagina);
        Assert.Equal(1, result.TotalPaginas);
        Assert.Equal(usuarios.Count, result.TotalItens);

        usuarioRepositoryMock.Verify(repo => repo.ListarUsuarios(usuarioAutenticado, busca, pagina, quantidadePorPagina), Times.Once);
    }

    [Fact]
    public async Task PesquisarAmigos_DeveRetornarPaginatedListDeUsuarios()
    {
        // Arrange
        var usuarioAutenticado = 1;
        var busca = "carlos";
        var pagina = 1;
        var quantidadePorPagina = 10;

        var amigos = new List<Usuario>
    {
        new Usuario("Carlos", "carlos@gmail.com", "Carlos", new DateOnly(1999, 11, 19), "12345678", BC.HashPassword("senha123"), "perfil.jpg") { Id = 1 },
        new Usuario("João", "joao@gmail.com", "João", new DateOnly(2000, 5, 10), "87654321", BC.HashPassword("senha456"), "perfil.jpg") { Id = 2 },
        new Usuario("Maria", "maria@gmail.com", "Maria", new DateOnly(1995, 3, 15), "98765432", BC.HashPassword("senha789"), "perfil.jpg") { Id = 3 }
    };

        var usuarioRepositoryMock = new Mock<IUsuarioRepository>();
        usuarioRepositoryMock.Setup(repo => repo.ListarAmigos(usuarioAutenticado, busca, pagina, quantidadePorPagina))
            .ReturnsAsync(new PaginatedList<Usuario>(amigos, pagina, 1, amigos.Count));

        var usuarioService = new UsuarioService(usuarioRepositoryMock.Object);

        // Act
        var result = await usuarioService.PesquisarAmigos(usuarioAutenticado, busca, pagina, quantidadePorPagina);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(amigos.Count, result.Itens.Count);
        Assert.Equal(pagina, result.Pagina);
        Assert.Equal(1, result.TotalPaginas);
        Assert.Equal(amigos.Count, result.TotalItens);

        usuarioRepositoryMock.Verify(repo => repo.ListarAmigos(usuarioAutenticado, busca, pagina, quantidadePorPagina), Times.Once);
    }

    [Fact]
    public async Task EditarPerfil_DeveAtualizarUsuarioDoRepositorio()
    {
        // Arrange
        var usuario = new Usuario("Carlos", "carlos@gmail.com", "Ali", new DateOnly(1999, 11, 19), "12345678", BC.HashPassword("senha123"), "perfil.jpg");

        usuario.Apelido = "Fulano";

        var usuarioRepositoryMock = new Mock<IUsuarioRepository>();
        usuarioRepositoryMock.Setup(repo => repo.AtualizarUsuario(usuario))
            .Returns(Task.CompletedTask);

        var usuarioService = new UsuarioService(usuarioRepositoryMock.Object);

        // Act
        await usuarioService.EditarPerfil(usuario);

        // Assert
        usuarioRepositoryMock.Verify(repo => repo.AtualizarUsuario(usuario), Times.Once);
    }
}
