using RedeSocial.Application.Contacts;
using RedeSocial.Application.Contacts.Documents.Request;
using RedeSocial.Application.Contacts.Documents.Response;
using RedeSocial.Application.Validations.Core;
using RedeSocial.Domain.Contracts.Repositories;
using RedeSocial.Domain.Models;

namespace RedeSocial.Application.Implementations.Services;

public class MensagemService : IMensagemService
{
    private readonly IMensagemRepository _mensagemRepository;
    private readonly IUsuarioService _usuarioService;

    public MensagemService(IMensagemRepository mensagemRepository, IUsuarioService usuarioService)
    {
        _mensagemRepository = mensagemRepository;
        _usuarioService = usuarioService;
    }

    public async Task<MensagemResponse> EnviarMensagem(int usuarioId, MensagemRequest mensagemRequest)
    {
        var response = new MensagemResponse();

        if (usuarioId == mensagemRequest.AmigoId)
        {
            response.AddNotification(new Notification("Não é possível enviar mensagem para o mesmo usuário."));
            return response;
        }

        var mensagem = new Mensagem
        {
            UsuarioId = usuarioId,
            Usuario = _usuarioService.ObterUsuario(usuarioId),
            AmigoId = mensagemRequest.AmigoId,
            Amigo = _usuarioService.ObterUsuario(mensagemRequest.AmigoId),
            Conteudo = mensagemRequest.Conteudo,
            DataEnvio = DateTime.UtcNow
        };

        await _mensagemRepository.CriarMensagem(mensagem);

        return new MensagemResponse
        {
            Id = mensagem.Id,
            RemetenteId = mensagem.UsuarioId,
            RemetenteNome = mensagem.Usuario.Nome,
            DestinatarioId = mensagem.AmigoId,
            DestinatarioNome = mensagem.Amigo.Nome,
            Conteudo = mensagem.Conteudo,
            DataEnvio = mensagem.DataEnvio
        };
    }


    public async Task<List<MensagemResponse>> ObterConversa(int usuarioId, int amigoId)
    {
        var mensagens = await _mensagemRepository.GetConversaAsync(usuarioId, amigoId);
        var conversaResponse = new List<MensagemResponse>();

        foreach (var mensagem in mensagens)
        {
            var usuario = await _usuarioService.ObterUsuarioAsync(mensagem.UsuarioId);
            var amigo = await _usuarioService.ObterUsuarioAsync(mensagem.AmigoId);
            var mensagemResponse = new MensagemResponse
            {
                Id = mensagem.Id,
                RemetenteId = mensagem.UsuarioId,
                RemetenteNome = usuario.Nome,
                DestinatarioId = mensagem.AmigoId,
                DestinatarioNome = amigo.Nome,
                Conteudo = mensagem.Conteudo,
                DataEnvio = mensagem.DataEnvio
            };

            conversaResponse.Add(mensagemResponse);
        }

        return conversaResponse;
    }
}