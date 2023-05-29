using System.ComponentModel.DataAnnotations;

namespace RedeSocial.Application.Contacts.Documents.Request;

public class LoginRequest
{
    [Required]
    public string Email { get; set; }

    [Required]
    public string Senha { get; set; }
}