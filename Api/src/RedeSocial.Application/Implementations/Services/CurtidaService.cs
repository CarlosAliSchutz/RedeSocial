using RedeSocial.Application.Contacts;
using RedeSocial.Domain.Contracts.Repositories;
using RedeSocial.Domain.Models;

namespace RedeSocial.Application.Implementations.Services;

public class CurtidaService : ICurtidaService
{
    private readonly ICurtidaRepository _curtidaRepository;
    private readonly IPostRepository _postRepository;

    public CurtidaService(ICurtidaRepository curtidaRepository, IPostRepository postRepository)
    {
        _curtidaRepository = curtidaRepository;
        _postRepository = postRepository;
    }

    public async Task<bool> CurtirPost(int postId, int usuarioId)
    {
        var curtida = await _curtidaRepository.ObterCurtida(postId, usuarioId);

        if (curtida != null)
        {
            await _curtidaRepository.Remover(curtida);
            await _postRepository.DecrementarCurtidas(postId);
            return false;
        }
        else
        {
            var novaCurtida = new Curtida
            {
                PostId = postId,
                UsuarioId = usuarioId
            };

            await _curtidaRepository.Criar(novaCurtida);
            await _postRepository.IncrementarCurtidas(postId);
            return true;
        }
    }
}