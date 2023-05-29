using System.ComponentModel.DataAnnotations;

namespace RedeSocial.Application.Contacts.Documents.Request;

public class UsuarioRequest
{
    [Required(ErrorMessage = "O campo Nome é obrigatório.")]
    [MaxLength(255, ErrorMessage = "O campo Nome deve ter no máximo 255 caracteres.")]
    public string Nome { get; set; }

    [Required(ErrorMessage = "O campo Email é obrigatório.")]
    [MaxLength(255, ErrorMessage = "O campo Email deve ter no máximo 255 caracteres.")]
    public string Email { get; set; }

    [MaxLength(50, ErrorMessage = "O campo Apelido deve ter no máximo 50 caracteres.")]
    public string Apelido { get; set; }

    [Required(ErrorMessage = "O campo Data de Nascimento é obrigatório.")]
    public DateOnly DataNascimento { get; set; }

    [Required(ErrorMessage = "O campo CEP é obrigatório.")]
    [MaxLength(8, ErrorMessage = "O campo CEP deve ter no máximo 8 caracteres.")]
    public string Cep { get; set; }

    [Required(ErrorMessage = "O campo Senha é obrigatório.")]
    [MaxLength(128, ErrorMessage = "O campo Senha deve ter no máximo 128 caracteres.")]
    public string SenhaHash { get; set; }

    [MaxLength(512, ErrorMessage = "O campo Imagem de Perfil deve ter no máximo 512 caracteres.")]
    public string ImagemPerfil { get; set; }
}