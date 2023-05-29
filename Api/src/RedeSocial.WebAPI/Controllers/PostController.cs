using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RedeSocial.Application.Contacts;
using RedeSocial.Application.Contacts.Documents.Request;
using RedeSocial.Application.Contacts.Documents.Response;
using RedeSocial.Application.Implementations.Services;
using RedeSocial.Application.Utils;
using RedeSocial.Domain.Models.Enums;

namespace RedeSocial.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PostController : ControllerBase
{
    private readonly IPostService _postService;
    private readonly IUsuarioService _usuarioService;
    private readonly AuthenticationUtils _authenticationUtils;

    public PostController(IPostService postService, IUsuarioService usuarioService, AuthenticationUtils authenticationUtils)
    {
        _postService = postService;
        _usuarioService = usuarioService;
        _authenticationUtils = authenticationUtils;
    }

    [HttpPost("criar")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(PostResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CriarPost(CriarPostRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userIdClaim = User.Claims.FirstOrDefault(x => x.Type.Equals("Id"))?.Value;
        if (!int.TryParse(userIdClaim, out int usuarioId))
        {
            return BadRequest("UserId inválido.");
        }

        var response = await _postService.CriarPost(usuarioId, request);

        return CreatedAtAction(nameof(GetPost), new { postId = response.PostId }, response);
    }

    [HttpGet("{postId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PostResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetPost(int postId)
    {
        var post = _postService.ObterPost(postId);

        var usuario = _usuarioService.ObterUsuario(post.AutorId);


        if (post == null)
        {
            return NotFound();
        }

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

        return Ok(response);
    }

    [HttpGet("feed")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ListarPostsDoUsuarioEAmigos([FromQuery] int pagina = 1, [FromQuery] int quantidadePorPagina = 10)
    {
        var usuarioId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "Id")?.Value);

        var posts = await _postService.ListarPostsDoUsuarioEAmigos(usuarioId, pagina, quantidadePorPagina);

        return Ok(posts);
    }

    [HttpGet("visitar/{usuarioId}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> VisitarPerfil(int usuarioId)
    {
        var visitanteId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "Id")?.Value);

        var posts = await _postService.ObterPostsDoUsuario(usuarioId, visitanteId);

        if (posts == null)
        {
            return NotFound();
        }

        return Ok(posts);
    }

    [HttpPut("{postId}/permissao")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AlterarPermissaoPost(int postId, [FromBody] Permissao permissao)
    {
        var usuarioId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "Id")?.Value);

        var post = await _postService.ListarPostAsync(postId);

        if (post == null)
        {
            return NotFound();
        }

        if (post.AutorId != usuarioId)
        {
            return Forbid();
        }

        await _postService.AlterarPermissaoPostUsuario(postId, permissao);

        return Ok();
    }

}
