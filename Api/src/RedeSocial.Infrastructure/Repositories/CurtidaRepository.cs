using Microsoft.EntityFrameworkCore;
using RedeSocial.Domain.Contracts.Repositories;
using RedeSocial.Domain.Models;
using RedeSocial.Infrastructure.Data;

namespace RedeSocial.Infrastructure.Repositories;

public class CurtidaRepository : ICurtidaRepository
{
    private readonly DataContext _context;

    public CurtidaRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<Curtida> ObterCurtida(int postId, int usuarioId)
    {
        return await _context.Curtidas
            .FirstOrDefaultAsync(c => c.PostId == postId && c.UsuarioId == usuarioId);
    }

    public async Task Criar(Curtida curtida)
    {
        await _context.Curtidas.AddAsync(curtida);
        await _context.SaveChangesAsync();
    }

    public async Task Remover(Curtida curtida)
    {
        _context.Curtidas.Remove(curtida);
        await _context.SaveChangesAsync();
    }

    public async Task<int> ContarCurtidasDoPost(int postId)
    {
        return await _context.Curtidas.CountAsync(c => c.PostId == postId);
    }
}