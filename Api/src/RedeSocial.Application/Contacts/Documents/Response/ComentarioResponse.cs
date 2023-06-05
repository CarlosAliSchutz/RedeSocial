using RedeSocial.Application.Validations.Core;

namespace RedeSocial.Application.Contacts.Documents.Response;

public class ComentarioResponse : Notifiable
{
    public int Id { get; set; }
    public int PostId { get; set; }
    public string Conteudo { get; set; }
    public DateTime DataPublicacao { get; set; }
    public int AutorId { get; set; }
    public string AutorNome { get; set; }
}
