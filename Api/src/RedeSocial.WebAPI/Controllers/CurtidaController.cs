using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RedeSocial.Application.Contacts;

namespace RedeSocial.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CurtidaController : ControllerBase
{
    private readonly IPostService _postService;
    private readonly ICurtidaService _curtidaService;

    public CurtidaController(IPostService postService, ICurtidaService curtidaService)
    {
        _postService = postService;
        _curtidaService = curtidaService;
    }

    [HttpPost("{postId}/curtir")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CurtirPost(int postId)
    {
        var usuarioId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "Id")?.Value);

        var post = _postService.ObterPost(postId);

        if (post == null)
        {
            return NotFound();
        }

        var curtido = await _curtidaService.CurtirPost(postId, usuarioId);

        return Ok(curtido);
    }

}
