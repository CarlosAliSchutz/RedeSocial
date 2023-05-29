using RedeSocial.Application.Validations.Core;

namespace RedeSocial.Application.Contacts.Documents.Response;

public class LoginResponse : Notifiable
{
    public bool IsAuthenticated { get; set; } = false;

    public int Id { get; set; }
    public string Email { get; set; }
}