namespace RedeSocial.Application.Contacts;

public interface ICurtidaService
{
    Task<bool> CurtirPost(int postId, int usuarioId);
}