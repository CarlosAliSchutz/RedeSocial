import { useEffect, useState } from "react";
import {
  useEditarPerfil,
  useFeed,
  useLogout,
  usePost,
  useProfile,
} from "../../../api";
import { useConversa } from "../../../api/mensagem/conversa";
import { useMensagem } from "../../../api/mensagem/enviarMensagem";
import { TOKEN_KEY, USER_KEY } from "../../../constants";
import useGlobalUser from "../../../context/user/user.context";
import FotoPadrao from "../../../utils/img/foto-padrao.png";
import Logo from "../../../utils/img/Logo.png";
import {
  Amizades,
  Button,
  EditarPerfil,
  Input,
  PesquisarUsuarios,
  Post,
  Solicitacoes,
} from "../../components";
import "./index.css";

export function FeedScreen() {
  const { getProfile, perfil, error } = useProfile();
  const { postCriarMeuPost, novoPost } = usePost();
  const { postLogout } = useLogout();
  const { getPosts, posts } = useFeed();
  const [, setUser] = useGlobalUser();
  const [postNovo, setPostNovo] = useState("");
  const [permissaoPost, setPermissaoPost] = useState(0);
  const [pagina, setPagina] = useState(1);
  const [solicitacoesAberto, setSolicitacoesAberto] = useState(false);
  const [amigosAberto, setAmigosAberto] = useState(false);
  const [pesquisaAberto, setPesquisaAberto] = useState(false);
  const [editarPerfil, setEditarPerfil] = useState(false);
  const { putEditarPerfil, usuario } = useEditarPerfil();

  const { getConversa, mensagens } = useConversa();
  const { postEnviarMensagem, mensagem } = useMensagem();

  useEffect(() => {
    getProfile();
  }, [usuario]);

  useEffect(() => {
    getPosts(pagina);
  }, [novoPost, pagina, usuario]);

  function handlePassaPagina() {
    setPagina((prevPagina) => prevPagina + 1);
  }

  function handleVoltarPagina() {
    setPagina((prevPagina) => prevPagina - 1);
  }

  const nomeUsuario = perfil.apelido ? perfil.apelido : perfil.nome;

  const formatCep = (value) => {
    if (typeof value === "string" && value.length === 8) {
      const numericValue = value.replace(/\D/g, "");

      if (numericValue.length === 8) {
        const formattedCep = `${numericValue.substring(
          0,
          2
        )}.${numericValue.substring(2, 5)}-${numericValue.substring(5)}`;
        return formattedCep;
      }
    }

    return value;
  };

  const cepFormatado = formatCep(perfil?.cep);

  function handleNovoPost() {
    postCriarMeuPost(postNovo, permissaoPost);
    setPostNovo("");
  }

  useEffect(() => {
    if (error?.message) {
      localStorage.removeItem(USER_KEY);
      localStorage.removeItem(TOKEN_KEY);
    }
  }, [error]);

  function handleClickLogout() {
    postLogout();

    setUser(null);
    localStorage.removeItem(USER_KEY);
    localStorage.removeItem(TOKEN_KEY);
  }

  const dataNascimneto = new Date(perfil?.dataNascimento);
  const dataFormatada = dataNascimneto.toLocaleDateString("pt-BR", {
    timeZone: "UTC",
  });

  function handleEditarPerfil(apelido, imagemPerfil) {
    putEditarPerfil(apelido, imagemPerfil);
  }

  function teste() {
    getConversa(2);
  }

  function teste2() {
    postEnviarMensagem(2, "react");
  }

  return (
    <>
      <div className="profile">
        <div className="profileCover">
          <img className="logo" src={Logo} />
          <Button onClick={handleClickLogout} className="logout">
            LOGOUT
          </Button>
          <img
            className="profileUserImg"
            src={perfil?.imagemPerfil ? perfil?.imagemPerfil : FotoPadrao}
            alt="foto de perfil"
          />
        </div>
        <div className="profileInfo">
          <h4 className="profileInfoName">{nomeUsuario}</h4>
        </div>
        <div className="info-navegacao">
          <ul>
            <li
              onClick={() => {
                setPesquisaAberto(!pesquisaAberto);
                setAmigosAberto(false);
                setSolicitacoesAberto(false);
                setEditarPerfil(false);
              }}
            >
              Pesquisar
            </li>
            {pesquisaAberto && (
              <>
                <PesquisarUsuarios />
              </>
            )}
            <li
              onClick={() => {
                setSolicitacoesAberto(!solicitacoesAberto);
                setAmigosAberto(false);
                setPesquisaAberto(false);
                setEditarPerfil(false);
              }}
            >
              Pedidos de amizade
            </li>
            {solicitacoesAberto && (
              <>
                <Solicitacoes />
              </>
            )}
            <li
              onClick={() => {
                setAmigosAberto(!amigosAberto);
                setSolicitacoesAberto(false);
                setPesquisaAberto(false);
                setEditarPerfil(false);
              }}
            >
              Amigos
            </li>
            {amigosAberto && (
              <>
                <Amizades />
              </>
            )}
            <li
              onClick={() => {
                setEditarPerfil(!editarPerfil);
                setAmigosAberto(false);
                setSolicitacoesAberto(false);
                setPesquisaAberto(false);
              }}
            >
              Editar Perfil
            </li>
            {editarPerfil && (
              <>
                <EditarPerfil onEditarPerfil={handleEditarPerfil} />
              </>
            )}
          </ul>
        </div>

        <div className="info-usuario">
          <h1>Perfil</h1>
          <h2>Nome: {perfil?.nome}</h2>
          <h3>Apelido: {perfil?.apelido}</h3>
          <h3>Email: {perfil?.email}</h3>
          <h3>Data de Nascimento: {dataFormatada}</h3>
          <h3>CEP: {cepFormatado}</h3>
        </div>
        <div className="profilePublicacoes">
          <div className="publicacoes">
            <img
              className="imagemPerfil"
              src={perfil?.imagemPerfil ? perfil?.imagemPerfil : FotoPadrao}
              alt="foto de perfil"
            />

            <Input
              type="text"
              className="profileButton"
              onChange={(e) => setPostNovo(e.target.value)}
              name="conteudo"
              placeholder="Compartilhe aqui"
              value={postNovo}
            />

            <select
              onChange={(e) => setPermissaoPost(parseInt(e.target.value))}
            >
              <option value={0}>PUBLICO</option>
              <option value={1}>PRIVADO</option>
            </select>

            <Button disabled={postNovo == ""} onClick={handleNovoPost}>
              Postar
            </Button>
          </div>
        </div>
        <div>
          <h1 className="publi-text">Publicações</h1>
          {posts &&
            posts.map((post, index) => {
              return <Post post={post} key={index} />;
            })}
        </div>
        {pagina > 1 && (
          <Button onClick={handleVoltarPagina}>Voltar Página</Button>
        )}
        {posts && posts.length >= 10 && (
          <Button onClick={handlePassaPagina}>Proxima página</Button>
        )}
      </div>
    </>
  );
}
