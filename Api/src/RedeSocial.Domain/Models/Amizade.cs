using RedeSocial.Domain.Models.Enums;

namespace RedeSocial.Domain.Models;

public class Amizade : Base
{
    public int UsuarioId { get; set; }
    public Usuario Usuario { get; set; }

    public int AmigoId { get; set; }
    public Usuario Amigo { get; set; }

    public StatusAmizade StatusAmizade { get; set; }
}
