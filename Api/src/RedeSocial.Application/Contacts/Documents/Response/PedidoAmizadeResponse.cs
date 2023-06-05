using RedeSocial.Application.Validations.Core;

namespace RedeSocial.Application.Contacts.Documents.Response;

public class PedidoAmizadeResponse : Notifiable
{
    public int PedidoId { get; set; }
    public int UsuarioId { get; set; }
    public int AmigoId { get; set; }
    public string StatusAmizade { get; set; }
}

