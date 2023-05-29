using RedeSocial.Application.Validations.Core;

namespace RedeSocial.Application.Contacts.Documents.Response;

public class UsuarioResponse : Notifiable
{
    public string Nome { get; set; }

    public string Email { get; set; }
}
