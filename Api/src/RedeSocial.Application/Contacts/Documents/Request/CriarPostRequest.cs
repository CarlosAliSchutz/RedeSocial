using RedeSocial.Domain.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace RedeSocial.Application.Contacts.Documents.Request;

public class CriarPostRequest
{
    [Required]
    public string Conteudo { get; set; }

    public Permissao PermissaoVisualizar { get; set; }
}
