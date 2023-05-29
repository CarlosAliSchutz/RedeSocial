using RedeSocial.Application.Contacts.Documents.Request;
using RedeSocial.Application.Contacts.Documents.Response;
using RedeSocial.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedeSocial.Application.Contacts;

public interface IComentarioService
{
    Task<ComentarioResponse> Comentar(int postId, ComentarioRequest comentarioRequest, int autorId);

    Task<List<Comentario>> ObterComentariosDoPostAsync(int postId);
}
