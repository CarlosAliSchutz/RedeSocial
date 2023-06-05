//Usuario
export { useCadastro } from "./user/cadastro.api";
export { useEditarPerfil } from "./user/editarPerfil.api";
export { useListarUsuarios } from "./user/listarUsuarios.api";
export { useLogout } from "./user/logout.api";
export { useProfile } from "./user/meuPerfil.api";
export { usePesquisarAmigos } from "./user/pesquisarAmigos.api";
export { usePesquisarUsuarios } from "./user/pesquisarUsuario.api";

//Post
export { useBuscarPost } from "./post/buscarPost.api";
export { useFeed } from "./post/feed.api";
export { usePermissao } from "./post/permissao.api";
export { usePost } from "./post/post.api";
export { useFeedAmigo } from "./post/visitarPerfil.api";

//Curtida
export { useCurtir } from "./curtida/curtir.api";

//Comentario
export { useComentar } from "./comentario/comentar.api";
export { useComentarios } from "./comentario/comentarios.api";

//Amizade
export { useAmizades } from "./amizade/amizades.api";
export { useDesfazerAmizade } from "./amizade/defazerAmizade.api";
export { useAmizade } from "./amizade/pedidosAmizade.api";
export { useAmizadeResponder } from "./amizade/responderPedido.api";
export { useSolicitarAmizade } from "./amizade/solicitacaoAmizade.api";
export { useVerificarSolicitacao } from "./amizade/verificarSolicitacao.api";

//Mensagem
export { useConversa } from "./mensagem/conversa";
export { useMensagem } from "./mensagem/enviarMensagem";
