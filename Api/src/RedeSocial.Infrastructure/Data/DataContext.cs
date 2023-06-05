using Microsoft.EntityFrameworkCore;
using RedeSocial.Domain.Models;

namespace RedeSocial.Infrastructure.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Amizade> Amizades { get; set; }
    public DbSet<Base> Bases { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Curtida> Curtidas { get; set; }
    public DbSet<Comentario> Comentarios { get; set; }
    public DbSet<Mensagem> Mensagens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Amizade>()
        .HasOne(a => a.Amigo)
        .WithMany()
        .HasForeignKey(a => a.AmigoId)
        .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Amizade>()
        .HasOne(a => a.Usuario) 
        .WithMany()
        .HasForeignKey(a => a.UsuarioId) 
        .OnDelete(DeleteBehavior.Restrict);

        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}