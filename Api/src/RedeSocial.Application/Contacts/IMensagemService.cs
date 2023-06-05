using RedeSocial.Application.Contacts.Documents.Request;
using RedeSocial.Application.Contacts.Documents.Response;

namespace RedeSocial.Application.Contacts;

public interface IMensagemService
{
    Task<MensagemResponse> EnviarMensagem(int usuarioId, MensagemRequest mensagemRequest);
    Task<List<MensagemResponse>> ObterConversa(int usuarioId, int amigoId);
}
