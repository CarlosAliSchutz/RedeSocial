using RedeSocial.Application.Contacts;
using RedeSocial.Application.Contacts.Documents.Response;
using RedeSocial.Application.Validations.Core;
using RedeSocial.Domain.Contracts.Repositories;
using RedeSocial.Domain.Models;
using RedeSocial.Domain.Models.Enums;

namespace RedeSocial.Application.Implementations.Services;

public class AmizadeService : IAmizadeService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IAmizadeRepository _amizadeRepository;

    public AmizadeService(IUsuarioRepository usuarioRepository, IAmizadeRepository amizadeRepository)
    {
        _usuarioRepository = usuarioRepository;
        _amizadeRepository = amizadeRepository;
    }

    public async Task<bool> EnviarPedidoAmizadeAsync(int usuarioId, int amigoId)
    {
        var usuario = await _usuarioRepository.ObterUsuarioIdAsync(usuarioId);
        var amigo = await _usuarioRepository.ObterUsuarioIdAsync(amigoId);

        if (usuario == null || amigo == null)
        {
            return false; 
        }

        if (usuario == amigo)
        {
            return false;
        }

        bool pedidoExistente = await _amizadeRepository.VerificarPedidoAmizadeAsync(usuarioId, amigoId);

        if (pedidoExistente)
        {
            return false; 
        }

        var pedidoAmizade = new Amizade
        {
            UsuarioId = usuarioId,
            AmigoId = amigoId,
            StatusAmizade = StatusAmizade.SOLICITADO,
        };

        await _amizadeRepository.CriarPedidoAmizadeAsync(pedidoAmizade);

        return true; 
    }

    public async Task<List<PedidoAmizadeResponse>> ListarPedidosAmizadeSolicitados(int usuarioId)
    {
        var pedidos = await _amizadeRepository.PedidosSolicitados(usuarioId);

        var response = pedidos.Select(pedido => new PedidoAmizadeResponse
        {
            PedidoId = pedido.Id,
            UsuarioId = pedido.UsuarioId,
            AmigoId = pedido.AmigoId,
            StatusAmizade = pedido.StatusAmizade.ToString()
        }).ToList();

        return response;
    }

    public async Task<List<Usuario>> ObterAmigos(int usuarioId)
    {
        var amizades = await _amizadeRepository.ObterAmizadesDoUsuario(usuarioId, StatusAmizade.ACEITO);

        var amigosEUsuarios = amizades.SelectMany(a => new[] { a.Amigo, a.Usuario }).Distinct();
        var amigosSemUsuarioAutenticado = amigosEUsuarios.Where(u => u.Id != usuarioId);

        return amigosSemUsuarioAutenticado.ToList();
    }

    public Amizade ObterPedidoAmizadePorId(int pedidoAmizadeId)
    {
        return _amizadeRepository.ObterAmizadePorId(pedidoAmizadeId);
    }

    public PedidoAmizadeResponse ResponderPedidoAmizade(int pedidoAmizadeId, bool aceitar)
    {
        var pedidoAmizade = _amizadeRepository.ObterAmizadePorId(pedidoAmizadeId);
        var response = new PedidoAmizadeResponse();

        if (pedidoAmizade == null)
        {
            response.AddNotification(new Notification("Pedido de amizade não encontrado."));
            return response;
        }

        if (pedidoAmizade.StatusAmizade != StatusAmizade.SOLICITADO)
        {
            response.AddNotification(new Notification("Este pedido de amizade já foi respondido anteriormente."));
            return response;
        }

        pedidoAmizade.StatusAmizade = aceitar ? StatusAmizade.ACEITO : StatusAmizade.NEGADO;

        _amizadeRepository.AtualizarPedidoAmizade(pedidoAmizade);

        return response;
    }

    public async Task<PedidoAmizadeResponse> RemoverAmizade(int usuarioId, int amigoId)
    {
        var amizade = await _amizadeRepository.ObterAmizade(usuarioId, amigoId);
        var response = new PedidoAmizadeResponse();

        if (amizade == null || amizade.StatusAmizade != StatusAmizade.ACEITO)
        {
            response.AddNotification(new Notification("Não foi possível remover a amizade."));
            return response;
        }

        await _amizadeRepository.RemoverAmizade(amizade);

        return response;
    }

    public async Task<bool> VerificarAmizadeSolicitada(int usuarioAutenticadoId, int usuarioSolicitadoId)
    {
        return await _amizadeRepository.VerificarAmizadeSolicitada(usuarioAutenticadoId, usuarioSolicitadoId);
    }

}

