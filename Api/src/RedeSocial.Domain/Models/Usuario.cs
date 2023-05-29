namespace RedeSocial.Domain.Models;

public class Usuario : Base
{
    public Usuario(string nome, string email, string apelido, DateOnly dataNascimento, string cep, string imagemPerfil)
    {
        Nome = nome;
        Email = email;
        Apelido = apelido;
        DataNascimento = dataNascimento;
        Cep = cep;
        ImagemPerfil = imagemPerfil;
    }

    public Usuario(string nome, string email, string apelido, DateOnly dataNascimento, string cep, string senhaHash, string imagemPerfil)
    {
        Nome = nome;
        Email = email;
        Apelido = apelido;
        DataNascimento = dataNascimento;
        Cep = cep;
        SenhaHash = senhaHash;
        ImagemPerfil = imagemPerfil;
    }

    public string Nome { get; set; }
    public string Email { get; set; }
    public string Apelido { get; set; } = string.Empty;
    public DateOnly DataNascimento { get; set; }
    public string Cep { get; set; }
    public string SenhaHash { get; set; }
    public string ImagemPerfil { get; set; }
    public List<Amizade> Amigos { get; set; } = new();
    public List<Amizade> Convites { get; set; } = new();
    public List<Post> Posts { get; set; } = new();
}
