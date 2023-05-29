using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RedeSocial.Application.Contacts;
using RedeSocial.Application.Contacts.Documents.Request;
using RedeSocial.Application.Contacts.Documents.Response;
using RedeSocial.Application.Contacts.Documents.Response.Error;
using RedeSocial.Application.Implementations.Services;
using RedeSocial.Domain.Models;

namespace RedeSocial.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ComentarioController : ControllerBase
{
    private readonly IComentarioService _comentarioService;

    public ComentarioController(IComentarioService comentarioService)
    {
        _comentarioService = comentarioService;
    }


    [HttpPost("{postId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]

    public async Task<ActionResult<Comentario>> AdicionarComentario(int postId, ComentarioRequest comentarioRequest)
    {
        var usuarioId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "Id")?.Value);

        var comentario = await _comentarioService.Comentar(postId, comentarioRequest, usuarioId);

        if (comentario == null)
        {
            return NotFound(); 
        }

        return Ok(comentario);
    }

    [HttpGet("post/{postId}/comentarios")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ComentarioResponse>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObterComentariosDoPost(int postId)
    {
        var comentarios = await _comentarioService.ObterComentariosDoPostAsync(postId);

        if (comentarios == null)
        {
            return NotFound();
        }

        var response = comentarios.Select(comentario => new ComentarioResponse
        {
            Id = comentario.Id,
            PostId = comentario.PostId,
            AutorId = comentario.AutorId,
            Conteudo = comentario.Conteudo,
            DataPublicacao = comentario.DataPublicacao,
            AutorNome = comentario.Autor
        }).ToList();

        return Ok(response);
    }

}
