using RedeSocial.Application.Contacts;
using RedeSocial.Application.Contacts.Documents.Response;
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
        var amigos = await _amizadeRepository.ObterAmizadesDoUsuario(usuarioId, StatusAmizade.ACEITO);

        var amigosEUsuarios = amigos.SelectMany(a => new[] { a.Amigo, a.Usuario }).Distinct();
        var amigosSemUsuarioAutenticado = amigosEUsuarios.Where(u => u.Id != usuarioId);

        return amigosSemUsuarioAutenticado.ToList();
    }

    public Amizade ObterPedidoAmizadePorId(int pedidoAmizadeId)
    {
        return _amizadeRepository.ObterAmizadePorId(pedidoAmizadeId);
    }

    public void ResponderPedidoAmizade(int pedidoAmizadeId, bool aceitar)
    {
        var pedidoAmizade = _amizadeRepository.ObterAmizadePorId(pedidoAmizadeId);

        if (pedidoAmizade == null)
        {
            throw new Exception("Pedido de amizade não encontrado.");
        }

        if (pedidoAmizade.StatusAmizade != StatusAmizade.SOLICITADO)
        {
            throw new Exception("Este pedido de amizade já foi respondido anteriormente.");
        }

        pedidoAmizade.StatusAmizade = aceitar ? StatusAmizade.ACEITO : StatusAmizade.NEGADO;

        _amizadeRepository.AtualizarPedidoAmizade(pedidoAmizade);
        _amizadeRepository.Salvar();
    }

    public async Task RemoverAmizade(int usuarioId, int amigoId)
    {
        var amizade = await _amizadeRepository.ObterAmizade(usuarioId, amigoId);

        if (amizade == null || amizade.StatusAmizade != StatusAmizade.ACEITO)
        {
            throw new Exception("Não foi possível remover a amizade.");
        }

        await _amizadeRepository.RemoverAmizade(amizade);
    }

    public async Task<bool> VerificarAmizadeSolicitada(int usuarioAutenticadoId, int usuarioSolicitadoId)
    {
        return await _amizadeRepository.VerificarAmizadeSolicitada(usuarioAutenticadoId, usuarioSolicitadoId);
    }

}

