using RedeSocial.Domain.Models.Enums;

namespace RedeSocial.Domain.Models;

public class Post : Base
{
    public int AutorId { get; set; }
    public Usuario Autor { get; set; }
    public string Conteudo { get; set; }
    public DateTime Criacao { get; set; }
    public int Curtidas { get; set; }
    public Permissao PermissaoVisualizar { get; set; }
    public List<Comentario> Comentarios { get; set; }
}
