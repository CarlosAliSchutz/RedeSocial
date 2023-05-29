import { useEffect, useState } from "react";
import { useComentar } from "../../../api/comentario/comentar";
import { useComentarios } from "../../../api/comentario/comentarios";
import { useCurtir } from "../../../api/curtida/curtir";
import { useBuscarPost } from "../../../api/post/buscarPost";
import { usePermissao } from "../../../api/post/permissao";
import { useProfile } from "../../../api/user/me";
import FotoPadrao from "../../../assets/foto-padrao.png";
import ComentarioEmoji from "../../../assets/comment.png";
import CurtidaEmoji from "../../../assets/curtir.png";
import { Button } from "../button/button.component";
import { Input } from "../input/input.component";
import "./index.css";

export function Post({ post }) {
  const { getComentarios, comentarios } = useComentarios();
  const { postComentar, comentar } = useComentar();
  const [comentariosAberto, setComentariosAberto] = useState(false);
  const [comentario, setComentario] = useState("");
  const { postCurtir, curtida } = useCurtir();
  const { getBuscaPost, busca } = useBuscarPost();
  const { getProfile, perfil } = useProfile();
  const [alterarPrivacidade, setAlterarPrivacidade] = useState(false);
  const [permissaoPost, setPermissaoPost] = useState();
  const { putAlterarPermissao } = usePermissao();

  const dataCriacao = new Date(post?.criacao);
  const dataFormatada = dataCriacao.toLocaleDateString("pt-BR", {
    timeZone: "UTC",
  });

  useEffect(() => {
    getProfile();
  }, []);

  function handleCurtir() {
    postCurtir(post?.postId);
  }

  useEffect(() => {
    getBuscaPost(post?.postId);
  }, [curtida]);

  useEffect(() => {
    getComentarios(post?.postId);
  }, [comentar]);

  function handleComentar() {
    postComentar(post?.postId, comentario);
    setComentario("");
  }

  useEffect(() => {
    if (post?.autorId == perfil?.id) {
      setAlterarPrivacidade(true);
    }
  }, [post, perfil]);

  function handlePermissaoChange(event) {
    const permissao = parseInt(event.target.value);
    setPermissaoPost(permissao);

    putAlterarPermissao(post?.postId, permissao);
  }
  return (
    <div className="post">
      <span>{dataFormatada}</span>
      <div className="postPerfil">
        <img
          className="imagemPerfil"
          src={post?.imagemPerfil ? post?.imagemPerfil : FotoPadrao}
          alt="Foto do perfil"
        />
        <h1>{post.autorName}</h1>
      </div>

      <h1>{post.conteudo}</h1>
      <div className="post-emoji">
        <img onClick={handleCurtir} src={CurtidaEmoji} />
        <p>{busca?.curtidas}</p>
        <div
          className="comentarios"
          onClick={() => setComentariosAberto(!comentariosAberto)}
        >
          <img src={ComentarioEmoji} />
          <p>Comentar</p>
        </div>
      </div>
      {comentariosAberto && (
        <>
          {comentarios &&
            comentarios.map((comentario) => {
              return (
                <div key={comentario.id}>
                  <h1>{comentario.autorNome}</h1>
                  <p>{comentario.conteudo}</p>
                </div>
              );
            })}
          <div className="comentar-post">
            <Input
              type="text"
              name="comentario"
              value={comentario}
              onChange={(e) => setComentario(e.target.value)}
            />
            <Button disabled={comentario == ""} onClick={handleComentar}>Comentar</Button>
          </div>
        </>
      )}
      {alterarPrivacidade && (
        <div className="post-permissoes">
          <select
            value={permissaoPost}
            onChange={handlePermissaoChange}
            defaultValue={post.permissaoVisualizar}
          >
            <option value={0}>PUBLICO</option>
            <option value={1}>PRIVADO</option>
          </select>
        </div>
      )}
    </div>
  );
}
