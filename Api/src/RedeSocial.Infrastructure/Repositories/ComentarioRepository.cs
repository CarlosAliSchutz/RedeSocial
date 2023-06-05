using Microsoft.EntityFrameworkCore;
using RedeSocial.Domain.Contracts.Repositories;
using RedeSocial.Domain.Models;
using RedeSocial.Infrastructure.Data;

namespace RedeSocial.Infrastructure.Repositories;

public class ComentarioRepository : IComentarioRepository
{
    private readonly DataContext _context;

    public ComentarioRepository(DataContext context)
    {
        _context = context;
    }

    public async Task Add(Comentario comentario)
    {
        await _context.Comentarios.AddAsync(comentario);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Comentario>> Obter(int postId)
    {
        return await _context.Comentarios
            .Where(c => c.PostId == postId)
            .ToListAsync();
    }
}
