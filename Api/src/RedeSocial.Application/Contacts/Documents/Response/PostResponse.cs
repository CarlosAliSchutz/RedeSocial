using RedeSocial.Application.Validations.Core;
using RedeSocial.Domain.Models.Enums;

namespace RedeSocial.Application.Contacts.Documents.Response;

public class PostResponse : Notifiable
{
    public int PostId { get; set; }
    public int AutorId { get; set; }
    public string AutorName { get; set; }
    public string ImagemPerfil { get; set; }
    public string Conteudo { get; set; }
    public DateTime Criacao { get; set; }
    public int Curtidas { get; set; }
    public Permissao PermissaoVisualizar { get; set; }
}
