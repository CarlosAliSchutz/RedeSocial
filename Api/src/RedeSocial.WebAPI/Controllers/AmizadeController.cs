using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RedeSocial.Application.Contacts;
using RedeSocial.Application.Contacts.Documents.Response;
using RedeSocial.Application.Implementations.Services;
using RedeSocial.Domain.Models;

namespace RedeSocial.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AmizadeController : ControllerBase
{
    private readonly IAmizadeService _amizadeService;

    public AmizadeController(IAmizadeService amizadeService)
    {
        _amizadeService = amizadeService;
    }

    [HttpPost("pedido")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> EnviarPedidoAmizade(int amigoId)
    {
        var usuarioId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "Id")?.Value);

        var pedidoEnviado = await _amizadeService.EnviarPedidoAmizadeAsync(usuarioId, amigoId);

        if (pedidoEnviado)
        {
            return Ok("Pedido de amizade enviado com sucesso.");
        }
        else
        {
            return BadRequest("Não foi possível enviar o pedido de amizade.");
        }
    }

    [HttpGet("pedidos-amizade")]
    public async Task<ActionResult<IEnumerable<PedidoAmizadeResponse>>> ListarPedidosAmizadeSolicitados()
    {
        var usuarioId = int.Parse(User.Claims.FirstOrDefault(x => x.Type.Equals("Id"))?.Value);
        var pedidos = await _amizadeService.ListarPedidosAmizadeSolicitados(usuarioId);

        return Ok(pedidos);
    }

    [HttpPost("responder-pedido-amizade/{pedidoAmizadeId}")]
    public IActionResult ResponderPedidoAmizade(int pedidoAmizadeId, bool aceitar)
    {
        try
        {
            _amizadeService.ResponderPedidoAmizade(pedidoAmizadeId, aceitar);
            return Ok("Resposta ao pedido de amizade registrada com sucesso.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro ao responder ao pedido de amizade: {ex.Message}");
        }
    }

    [HttpGet("amigos")]
    public async Task<ActionResult<List<Usuario>>> ObterAmigos()
    {
        var usuarioId = int.Parse(User.Claims.FirstOrDefault(x => x.Type.Equals("Id"))?.Value);

        var amigos = await _amizadeService.ObterAmigos(usuarioId);

        return Ok(amigos);
    }

    [HttpPost("remover-amizade")]
    public async Task<IActionResult> RemoverAmizade(int amigoId)
    {
        var usuarioAutenticado = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "Id")?.Value);

        await _amizadeService.RemoverAmizade(usuarioAutenticado, amigoId);

        return Ok("Amizade removida com sucesso.");
    }

    [HttpGet("usuarios/{usuarioId}/status-amizade")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObterStatusAmizade(int usuarioId)
    {
        var usuarioAutenticadoId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "Id")?.Value);

        if (usuarioAutenticadoId == usuarioId)
        {
            return Unauthorized();
        }

        var amizadeSolicitada = await _amizadeService.VerificarAmizadeSolicitada(usuarioAutenticadoId, usuarioId);

        if (amizadeSolicitada)
        {
            return Ok(true);
        }
        else
        {
            return Ok(false);
        }
    }


}
