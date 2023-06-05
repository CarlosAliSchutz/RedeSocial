using Microsoft.EntityFrameworkCore;
using RedeSocial.Domain.Contracts.Repositories;
using RedeSocial.Domain.Models;
using RedeSocial.Infrastructure.Data;

namespace RedeSocial.Infrastructure.Repositories;

public class MensagemRepository : BaseRepository, IMensagemRepository
{
    private readonly DataContext _context;

    public MensagemRepository(DataContext context) : base(context)
    {
        _context = context;
    }

    public async Task CriarMensagem(Mensagem mensagem)
    {
        await _context.Set<Mensagem>().AddAsync(mensagem);
        await _context.SaveChangesAsync();
    }

    public async Task<Mensagem> ObterMensagemId(int mensagemId)
    {
        return await _context.Set<Mensagem>().FindAsync(mensagemId);
    }

    public async Task<List<Mensagem>> GetConversaAsync(int usuarioId, int amigoId)
    {
        return await _context.Set<Mensagem>()
            .Where(m => (m.UsuarioId == usuarioId && m.AmigoId == amigoId) || (m.UsuarioId == amigoId && m.AmigoId == usuarioId))
            .OrderBy(m => m.DataEnvio)
            .ToListAsync();
    }
}
