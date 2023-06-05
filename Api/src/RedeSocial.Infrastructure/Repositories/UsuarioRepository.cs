using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using RedeSocial.Domain.Contracts.Repositories;
using RedeSocial.Domain.Models;
using RedeSocial.Domain.Models.Enums;
using RedeSocial.Domain.Utils;
using RedeSocial.Infrastructure.Data;
using System.Security.Claims;
using BC = BCrypt.Net.BCrypt;

namespace RedeSocial.Infrastructure.Repositories;

public class UsuarioRepository : BaseRepository, IUsuarioRepository
{
    private readonly DataContext _context;

    public UsuarioRepository(DataContext context) : base(context)
    {
        _context = context;
    }
    private static string Hash(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            throw new ArgumentNullException(nameof(input));
        }

        return BC.HashPassword(input);
    }

    public static string BuscarUsuarioAutenticado(HttpContext context)
    {
        var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return userId;
    }

    public async Task<Usuario> AddUsuario(Usuario usuario)
    {
        if (usuario == null)
        {
            throw new ArgumentNullException(nameof(usuario));
        }

        if (string.IsNullOrEmpty(usuario.SenhaHash))
        {
            throw new ArgumentNullException(nameof(usuario.SenhaHash));
        }

        usuario.SenhaHash = Hash(usuario.SenhaHash);
        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();

        return usuario;
    }

    public async Task<Usuario?> ObterCredenciaisUsuario(string username)
    {
        return await _context.Usuarios.Where(u => u.Email.Equals(username)).FirstOrDefaultAsync();
    }

    public async Task<Usuario> ObterId(int id)
    {
        return await _context.Usuarios.FindAsync(id);
    }

    public Usuario ObterIdUsuario(int id)
    {
        return _context.Usuarios.FirstOrDefault(u => u.Id == id);
    }



    public async Task<Usuario> ObterUsuarioPorEmail(string email)
    {
        return await _context.Usuarios.Where(u => u.Email.Contains(email)).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Usuario>> Listar()
        => await _context.Usuarios.ToListAsync();

    public async Task<Usuario> ObterUsuarioIdAsync(int userId)
    {
        return await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == userId);

    }

    public async Task<PaginatedList<Usuario>> ListarUsuarios(int usuarioAutenticado, string busca, int pagina, int pageSize)
    {
        var buscaQuery = _context.Usuarios.AsQueryable();

        if (!string.IsNullOrEmpty(busca))
        {
            buscaQuery = buscaQuery
                .Where(u => u.Nome.Contains(busca) || u.Email.Contains(busca));
        }

        buscaQuery = buscaQuery.Where(u => u.Id != usuarioAutenticado);

        var usuarios = await buscaQuery.CountAsync();

        var totalPaginas = (int)Math.Ceiling(usuarios / (double)pageSize);

        pagina = Math.Max(1, Math.Min(pagina, totalPaginas));

        var indiceInicial = (pagina - 1) * pageSize;
        var quantidadeUsuarios = await buscaQuery
            .Skip(indiceInicial)
            .Take(pageSize)
            .ToListAsync();

        return new PaginatedList<Usuario>(quantidadeUsuarios, pagina, totalPaginas, usuarios);
    }

    public async Task<PaginatedList<Usuario>> ListarAmigos(int usuarioAutenticado, string busca, int pagina, int quantidadePorPagina)
    {
        var buscaQuery = _context.Amizades
            .Where(a => (a.UsuarioId == usuarioAutenticado || a.AmigoId == usuarioAutenticado) && a.StatusAmizade == StatusAmizade.ACEITO)
            .Select(a => a.UsuarioId == usuarioAutenticado ? a.Amigo : a.Usuario)
            .AsEnumerable(); 

        if (!string.IsNullOrEmpty(busca))
        {
            buscaQuery = buscaQuery.Where(u => u.Nome.Contains(busca) || u.Email.Contains(busca));
        }

        var totalUsuarios = buscaQuery.Count();

        var totalPaginas = (int)Math.Ceiling(totalUsuarios / (double)quantidadePorPagina);

        pagina = Math.Max(1, Math.Min(pagina, totalPaginas));

        var indiceInicial = (pagina - 1) * quantidadePorPagina;
        var usuariosPaginados = buscaQuery
            .Skip(indiceInicial)
            .Take(quantidadePorPagina)
            .ToList();

        return new PaginatedList<Usuario>(usuariosPaginados, pagina, totalPaginas, totalUsuarios);
    }

    public async Task AtualizarUsuario(Usuario usuario)
    {
        _context.Usuarios.Update(usuario);
        await _context.SaveChangesAsync();
    }

}
