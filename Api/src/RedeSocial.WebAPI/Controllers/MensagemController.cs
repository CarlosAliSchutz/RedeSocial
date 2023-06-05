using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using RedeSocial.Application.Contacts;
using RedeSocial.Application.Contacts.Documents.Request;
using RedeSocial.Application.Contacts.Documents.Response;
using RedeSocial.Application.Contacts.Documents.Response.Error;

namespace RedeSocial.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MensagemController : Controller
{
    private readonly IMensagemService _mensagemService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public MensagemController(IMensagemService mensagemService, IHttpContextAccessor httpContextAccessor)
    {
        _mensagemService = mensagemService;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpPost("enviar-mensagem")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
    public async Task<IActionResult> EnviarMensagem(MensagemRequest mensagemRequest)
    {
        var usuarioId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "Id")?.Value);

        var mensagem = await _mensagemService.EnviarMensagem(usuarioId, mensagemRequest);

        if (mensagem == null)
        {
            return NotFound();
        }

        var mensagemResponse = new MensagemResponse
        {
            Id = mensagem.Id,
            RemetenteId = mensagem.RemetenteId,
            RemetenteNome = mensagem.RemetenteNome,
            DestinatarioId = mensagem.DestinatarioId,
            DestinatarioNome = mensagem.DestinatarioNome,
            Conteudo = mensagem.Conteudo,
            DataEnvio = mensagem.DataEnvio
        };

        return Ok(mensagemResponse);
    }

    [HttpGet("{amigoId}")]
    public async Task<IActionResult> ObterConversa(int amigoId)
    {
        var usuarioId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "Id")?.Value);

        var mensagens = await _mensagemService.ObterConversa(usuarioId, amigoId);

        var mensagemResponses = new List<MensagemResponse>();
        foreach (var mensagem in mensagens)
        {
            var mensagemResponse = new MensagemResponse
            {
                Id = mensagem.Id,
                RemetenteId = mensagem.RemetenteId,
                RemetenteNome = mensagem.RemetenteNome,
                DestinatarioId = mensagem.DestinatarioId,
                DestinatarioNome = mensagem.DestinatarioNome,
                Conteudo = mensagem.Conteudo,
                DataEnvio = mensagem.DataEnvio
            };
            mensagemResponses.Add(mensagemResponse);
        }

        return Ok(mensagemResponses);
    }
}
