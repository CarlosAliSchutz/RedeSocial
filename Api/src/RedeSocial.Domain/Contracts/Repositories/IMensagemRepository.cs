using RedeSocial.Domain.Models;

namespace RedeSocial.Domain.Contracts.Repositories;

public interface IMensagemRepository
{
    Task CriarMensagem(Mensagem mensagem);
    Task<Mensagem> ObterMensagemId(int mensagemId);
    Task<List<Mensagem>> GetConversaAsync(int usuarioId, int amigoId);
}
