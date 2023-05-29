using System.ComponentModel.DataAnnotations;

namespace RedeSocial.Application.Contacts.Documents.Request;

public class EditarPerfilRequest
{
    [MaxLength(50, ErrorMessage = "O campo Apelido deve ter no máximo 50 caracteres.")]
    public string Apelido { get; set; }

    [MaxLength(512, ErrorMessage = "O campo Imagem de Perfil deve ter no máximo 512 caracteres.")]
    public string ImagemPerfil { get; set; }

}
