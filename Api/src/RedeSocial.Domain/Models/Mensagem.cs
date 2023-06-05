namespace RedeSocial.Domain.Models;

public class Mensagem : Base
{
    public int UsuarioId { get; set; }
    public Usuario Usuario { get; set; }
    public int AmigoId { get; set; }
    public Usuario Amigo { get; set; }
    public string Conteudo { get; set; }
    public DateTime DataEnvio { get; set;}
}
