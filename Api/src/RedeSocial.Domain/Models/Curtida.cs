namespace RedeSocial.Domain.Models;

public class Curtida : Base
{
    public int PostId { get; set; }
    public Post Post { get; set; }
    public int UsuarioId { get; set; }
    public Usuario Usuario { get; set; }
}
