using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RedeSocial.Application.Contacts;
using RedeSocial.Application.Contacts.Documents.Request;
using RedeSocial.Application.Contacts.Documents.Response;
using RedeSocial.Application.Contacts.Documents.Response.Error;
using RedeSocial.WebAPI.Security.Interface;
using System.Security.Claims;

namespace RedeSocial.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuarioController : ControllerBase
{
    private readonly IUsuarioService _usuarioService;
    private readonly ITokenService _tokenService;

    public UsuarioController(IUsuarioService usuarioService, ITokenService tokenService)
    {
        _usuarioService = usuarioService;
        _tokenService = tokenService;
    }

    [HttpPost("cadastro")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UsuarioRequest))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
    public async Task<ActionResult<UsuarioResponse>> Cadastrar([FromBody] UsuarioRequest request)
    {
        var response = await _usuarioService.Cadastro(request);

        if (!response.IsValid())
            return NotFound(new ErrorResponse(response.Notifications));

        return Ok(response);
    }

    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoginResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorResponse))]
    public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
    {
        var response = await _usuarioService.ValidateLogin(request);

        if (!response.IsValid())
            return Unauthorized(new ErrorResponse(response.Notifications));

        var token = _tokenService.GenerateToken(response);

        return Ok(new
        {
            usuario = response,
            token
        });
    }

    [HttpGet("listar-usuarios")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<UsuarioResponse>))]
    public ActionResult<IEnumerable<UsuarioResponse>> List(int? id = null)
    {
        var usuarios = _usuarioService.ListarUsuarios(id);

        if (id.HasValue && !usuarios.Any())
        {
            return NotFound();
        }


        return Ok(usuarios);
    }


    [HttpGet("autenticar")]
    [Authorize]
    public async Task<IActionResult> Authenticated()
    {
        var userIdClaim = User.Claims.FirstOrDefault(x => x.Type.Equals("Id"))?.Value;
        if (!int.TryParse(userIdClaim, out int userId))
        {
            return BadRequest("UserId inválido.");
        }

        var user = await _usuarioService.ObterDadosUsuario(userId);

        if (user == null)
        {
            return NotFound("Usuário não encontrado.");
        }

        return Ok(user);
    }

    [HttpGet("pesquisar")]
    [Authorize]
    public async Task<IActionResult> ListarUsuarios([FromQuery] string busca, [FromQuery] int pagina = 1, [FromQuery] int quantidadePorPagina = 10)
    {
        var usuario = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "Id")?.Value);

        var usuarios = await _usuarioService.PesquisarUsuarios(usuario, busca, pagina, quantidadePorPagina);

        return Ok(usuarios);
    }

    [HttpGet("pesquisar-amigos")]
    [Authorize]
    public async Task<IActionResult> ListarAmigos([FromQuery] string busca, [FromQuery] int pagina = 1, [FromQuery] int quantidadePorPagina = 10)
    {
        var usuarioAutenticado = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "Id")?.Value);

        var amigos = await _usuarioService.PesquisarAmigos(usuarioAutenticado, busca, pagina, quantidadePorPagina);

        return Ok(amigos);
    }

    [HttpPut("editar-perfil")]
    [Authorize]
    public async Task<IActionResult> EditarPerfil([FromBody] EditarPerfilRequest request)
    {
        int usuarioId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "Id")?.Value);

        var usuario = await _usuarioService.ObterUsuarioAsync(usuarioId);

        if (usuario == null)
        {
            return NotFound("Usuário não encontrado.");
        }

        usuario.Apelido = request.Apelido;
        usuario.ImagemPerfil = request.ImagemPerfil;

        await _usuarioService.EditarPerfil(usuario);

        return Ok("Perfil atualizado com sucesso.");
    }


}
