using RedeSocial.Application.Contacts;
using RedeSocial.Application.Contacts.Documents.Request;
using RedeSocial.Application.Contacts.Documents.Response;
using RedeSocial.Application.Implementations.Mappers;
using RedeSocial.Application.Validations.Core;
using RedeSocial.Domain.Contracts.Repositories;
using RedeSocial.Domain.Models;
using RedeSocial.Domain.Utils;
using BC = BCrypt.Net.BCrypt;

namespace RedeSocial.Application.Implementations.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioService(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public async Task<UsuarioResponse> Cadastro(UsuarioRequest request)
        {
            var usuario = request.ToUsuario();

            var response = new UsuarioResponse();

            if (string.IsNullOrEmpty(request.Nome))
            {
                response.AddNotification(new Notification("O campo Nome é obrigatório."));
                return response;
            }

            if (string.IsNullOrEmpty(request.Email))
            {
                response.AddNotification(new Notification("O campo Email é obrigatório."));
                return response;
            }

            if (request.DataNascimento == default)
            {
                response.AddNotification(new Notification("O campo DataNascimento é obrigatório."));
                return response;
            }

            if (string.IsNullOrEmpty(request.Cep))
            {
                response.AddNotification(new Notification("O campo Cep é obrigatório."));
                return response;
            }

            if (string.IsNullOrEmpty(request.SenhaHash))
            {
                response.AddNotification(new Notification("O campo Senha é obrigatório."));
                return response;
            }

            if (await _usuarioRepository.ObterUsuarioPorEmail(usuario.Email) != null)
            {
                response.AddNotification(new Notification("Cadastro inválido."));
                return response;
            }

            await _usuarioRepository.AddUsuario(usuario);

            response.Nome = usuario.Nome;
            response.Email = usuario.Email;

            return usuario.ToUsuarioResponse();
        }

        public async Task<LoginResponse> ValidateLogin(LoginRequest loginRequest)
        {
            var usuario = await _usuarioRepository.ObterCredenciaisUsuario(loginRequest.Email);
            var response = new LoginResponse();

            if (usuario == null || !BC.Verify(loginRequest.Senha, usuario.SenhaHash))
            {
                response.AddNotification(new Notification("Email ou senha incorretos."));
                response.IsAuthenticated = false;
                response.Email = null;
                return response;
            }

            return new LoginResponse
            {
                IsAuthenticated = true,
                Email = usuario.Email,
                Id = usuario.Id
            };
        }

        public IEnumerable<ListarUsuariosResponse> ListarUsuarios(int? id = null)
        {
            if (id.HasValue)
            {
                var usuario = _usuarioRepository.ObterId(id.Value).Result;
                return new List<ListarUsuariosResponse> { usuario?.ToUsuarios() };
            }

            var usuarios = _usuarioRepository.Listar().Result;
            return usuarios.Select(usuario => usuario.ToUsuarios());
        }

        public async Task<Usuario> ObterUsuarioAsync(int usuarioId)
        {
            return await _usuarioRepository.ObterId(usuarioId);
        }

        public Usuario ObterUsuario(int usuarioId)
        {
            return _usuarioRepository.ObterIdUsuario(usuarioId);
        }


        public async Task<UsuarioLogadoResponse> ObterDadosUsuario(int userId)
        {
            var user = await _usuarioRepository.ObterUsuarioIdAsync(userId);

            if (user == null)
            {
                return null;
            }

            var usuarioResponse = new UsuarioLogadoResponse
            {
                Id = user.Id,
                Nome = user.Nome,
                Email = user.Email,
                Apelido = user.Apelido,
                DataNascimento = user.DataNascimento,
                Cep = user.Cep,
                ImagemPerfil = user.ImagemPerfil,
            };

            return usuarioResponse;
        }

        public async Task<PaginatedList<Usuario>> PesquisarUsuarios(int usuarioAutenticado, string busca, int pagina, int quantidadePorPagina)
        {
            return await _usuarioRepository.ListarUsuarios(usuarioAutenticado, busca, pagina, quantidadePorPagina);
        }

        public async Task<PaginatedList<Usuario>> PesquisarAmigos(int usuarioAutenticado, string busca, int pagina, int quantidadePorPagina)
        {
            return await _usuarioRepository.ListarAmigos(usuarioAutenticado, busca, pagina, quantidadePorPagina);
        }

        public async Task EditarPerfil(Usuario usuario)
        {
            await _usuarioRepository.AtualizarUsuario(usuario);
        }
    }
}
