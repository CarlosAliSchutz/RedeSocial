using RedeSocial.Application.Validations.Core;

namespace RedeSocial.Application.Contacts.Documents.Response;

public class MensagemResponse : Notifiable
{
    public int Id { get; set; }
    public int RemetenteId { get; set; }
    public string RemetenteNome { get; set; }
    public int DestinatarioId { get; set; }
    public string DestinatarioNome { get; set; }
    public string Conteudo { get; set; }
    public DateTime DataEnvio { get; set; }
}
