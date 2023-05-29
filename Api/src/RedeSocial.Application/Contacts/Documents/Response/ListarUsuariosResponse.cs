using RedeSocial.Application.Validations.Core;

namespace RedeSocial.Application.Contacts.Documents.Response;

public class ListarUsuariosResponse : Notifiable
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    public string Apelido { get; set; }
    public DateOnly DataNascimento { get; set; }
    public string Cep { get; set; }
    public string ImagemPerfil { get; set; }
}
