namespace RedeSocial.Domain.Models;

public class Comentario : Base
{
    public string Conteudo { get; set; }
    public int PostId { get; set; }
    public Post Post { get; set; }
    public int AutorId { get; set; }
    public string Autor { get; set; }

    public DateTime DataPublicacao { get; set; }
}
