import { useEffect, useState } from "react";
import {
  useBuscarPost,
  useComentar,
  useComentarios,
  useCurtir,
  useListarUsuarios,
} from "../../../api";
import ComentarioEmoji from "../../../utils/img/comment.png";
import CurtidaEmoji from "../../../utils/img/curtir.png";
import FotoPadrao from "../../../utils/img/foto-padrao.png";
import { Button } from "../button/button.component";
import { Input } from "../input/input.component";
import "./index.css";

export function PostAmigo({ post }) {
  const { getComentarios, comentarios } = useComentarios();
  const { postComentar, comentar } = useComentar();
  const [comentariosAberto, setComentariosAberto] = useState(false);
  const [comentario, setComentario] = useState("");
  const { postCurtir, curtida } = useCurtir();
  const { getBuscaPost, busca } = useBuscarPost();

  const { getBuscaUsuario, buscaUsuario } = useListarUsuarios();

  useEffect(() => {
    getBuscaUsuario(post?.autorId);
  }, [post]);

  const dataCriacao = new Date(post?.criacao);
  const dataFormatada = dataCriacao.toLocaleDateString("pt-BR", {
    timeZone: "UTC",
  });

  function handleCurtir() {
    postCurtir(post?.id);
  }

  useEffect(() => {
    getBuscaPost(post?.id);
  }, [curtida]);

  useEffect(() => {
    getComentarios(post?.id);
  }, [comentar]);

  function handleComentar() {
    postComentar(post?.id, comentario);
    setComentario("");
  }

  return (
    <div className="post">
      <span>{dataFormatada}</span>
      <div className="postPerfil">
        <img
          className="imagemPerfil"
          src={
            buscaUsuario[0]?.imagemPerfil
              ? buscaUsuario[0]?.imagemPerfil
              : FotoPadrao
          }
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
            <Button disabled={comentario == ""} onClick={handleComentar}>
              Comentar
            </Button>
          </div>
        </>
      )}
    </div>
  );
}
