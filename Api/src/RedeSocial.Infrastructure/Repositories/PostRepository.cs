using Microsoft.EntityFrameworkCore;
using RedeSocial.Domain.Contracts.Repositories;
using RedeSocial.Domain.Models;
using RedeSocial.Domain.Models.Enums;
using RedeSocial.Infrastructure.Data;

namespace RedeSocial.Infrastructure.Repositories;
public class PostRepository : BaseRepository, IPostRepository
{
    private readonly DataContext _context;

    public PostRepository(DataContext context) : base(context)
    {
        _context = context;
    }

    public async Task Criar(Post post)
    {
        await _context.Posts.AddAsync(post);
        await _context.SaveChangesAsync();
    }

    public Post ObterId(int postId)
    {
        return _context.Posts.Find(postId);
    }

    public async Task<Post> ObterPostIdAsync(int postId)
    {
        return await _context.Posts.FindAsync(postId);
    }


    public async Task<List<Post>> ListarPosts(int usuarioId, int pagina, int quantidadePorPagina)
    {
        var amigos = await _context.Amizades
            .Where(a => (a.UsuarioId == usuarioId || a.AmigoId == usuarioId) && a.StatusAmizade == StatusAmizade.ACEITO)
            .ToListAsync();

        var amigosIds = amigos.Select(a => a.UsuarioId == usuarioId ? a.AmigoId : a.UsuarioId).Distinct().ToList();

        amigosIds.Add(usuarioId);

        var posts = await _context.Posts
            .Where(p => amigosIds.Contains(p.AutorId))
            .OrderByDescending(p => p.Criacao)
            .Skip((pagina - 1) * quantidadePorPagina)
            .Take(quantidadePorPagina)
            .ToListAsync();

        return posts;
    }

    public async Task<List<Post>> ListarTodosPostsUsuario(int userId)
    {
        return await _context.Posts
            .Where(p => p.AutorId == userId)
            .OrderByDescending(p => p.Criacao)
            .ToListAsync();
    }

    public async Task<List<Post>> ListarPostsPublicosUsuario(int userId)
    {
        return await _context.Posts
            .Where(p => p.AutorId == userId && p.PermissaoVisualizar == Permissao.PUBLICO)
            .OrderByDescending(p => p.Criacao)
            .ToListAsync();
    }

    public async Task AtualizarPost(Post post)
    {
        _context.Posts.Update(post);
        await _context.SaveChangesAsync();
    }

    public async Task DecrementarCurtidas(int postId)
    {
        var post = await _context.Posts.FindAsync(postId);
        if (post != null)
        {
            post.Curtidas--;
            await _context.SaveChangesAsync();
        }
    }

    public async Task IncrementarCurtidas(int postId)
    {
        var post = await _context.Posts.FindAsync(postId);
        if (post != null)
        {
            post.Curtidas++;
            await _context.SaveChangesAsync();
        }
    }

}


