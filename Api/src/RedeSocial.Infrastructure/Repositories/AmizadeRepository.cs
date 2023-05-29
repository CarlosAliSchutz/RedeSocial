using Microsoft.EntityFrameworkCore;
using RedeSocial.Application.Contacts.Documents.Response;
using RedeSocial.Domain.Contracts.Repositories;
using RedeSocial.Domain.Models;
using RedeSocial.Domain.Models.Enums;
using RedeSocial.Infrastructure.Data;

namespace RedeSocial.Infrastructure.Repositories;

public class AmizadeRepository : BaseRepository, IAmizadeRepository
{
    private readonly DataContext _context;

    public AmizadeRepository(DataContext context) : base(context)
    {
        _context = context;
    }

    public async Task<bool> VerificarPedidoAmizadeAsync(int usuarioId, int amigoId)
    {
        var pedidoAmizade = await _context.Amizades
            .FirstOrDefaultAsync(p => p.UsuarioId == usuarioId && p.AmigoId == amigoId);

        return pedidoAmizade != null;
    }

    public async Task CriarPedidoAmizadeAsync(Amizade pedidoAmizade)
    {
        _context.Amizades.Add(pedidoAmizade);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Amizade>> PedidosSolicitados(int usuarioId)
    {
        return await _context.Amizades
            .Where(a => a.AmigoId == usuarioId && a.StatusAmizade == StatusAmizade.SOLICITADO)
            .ToListAsync();
    }

    public Amizade ObterAmizadePorId(int pedidoAmizadeId)
    {
        return _context.Amizades.Find(pedidoAmizadeId);
    }

    public async Task<List<Amizade>> ObterAmizadesDoUsuario(int usuarioId, StatusAmizade statusAmizade)
    {
        return await _context.Amizades
            .Where(a => (a.UsuarioId == usuarioId || a.AmigoId == usuarioId) && a.StatusAmizade == statusAmizade)
            .Include(a => a.Usuario)
            .Include(a => a.Amigo)
            .ToListAsync();
    }

    public void AtualizarPedidoAmizade(Amizade pedidoAmizade)
    {
        _context.Amizades.Update(pedidoAmizade);
    }

    public void Salvar()
    {
        _context.SaveChanges();
    }

    public async Task<bool> VerificarAmizadeAceita(int usuarioId, int visitanteId)
    {
        var amizade = await _context.Amizades.FirstOrDefaultAsync(a => 
                (a.UsuarioId == usuarioId && a.AmigoId == visitanteId) || 
                (a.UsuarioId == visitanteId && a.AmigoId == usuarioId));
        return amizade?.StatusAmizade == StatusAmizade.ACEITO;
    }

    public async Task<Amizade> ObterAmizade(int usuarioId, int amigoId)
    {
        return await _context.Amizades.FirstOrDefaultAsync(a =>
            (a.UsuarioId == usuarioId && a.AmigoId == amigoId)
            || (a.UsuarioId == amigoId && a.AmigoId == usuarioId));
    }

    public async Task RemoverAmizade(Amizade amizade)
    {
        _context.Amizades.Remove(amizade);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> VerificarAmizadeSolicitada(int usuarioAutenticadoId, int usuarioSolicitadoId)
    {
        return await _context.Amizades.AnyAsync(a =>
            (a.UsuarioId == usuarioAutenticadoId && a.AmigoId == usuarioSolicitadoId && a.StatusAmizade == StatusAmizade.SOLICITADO) ||
            (a.AmigoId == usuarioSolicitadoId && a.UsuarioId == usuarioAutenticadoId && a.StatusAmizade == StatusAmizade.SOLICITADO));
    }
}
