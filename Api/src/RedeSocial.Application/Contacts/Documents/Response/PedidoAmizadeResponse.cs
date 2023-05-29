using RedeSocial.Domain.Models.Enums;

namespace RedeSocial.Application.Contacts.Documents.Response;

public class PedidoAmizadeResponse
{
    public int PedidoId { get; set; }
    public int UsuarioId { get; set; }
    public int AmigoId { get; set; }
    public string StatusAmizade { get; set; }
}

