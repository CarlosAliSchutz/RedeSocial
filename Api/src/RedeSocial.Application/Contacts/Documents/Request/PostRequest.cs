using RedeSocial.Domain.Models.Enums;

namespace RedeSocial.Application.Contacts.Documents.Request;

public class PostRequest
{
    public string Conteudo { get; set; }

    public Permissao PermissaoVisualizar { get; set; }
}
