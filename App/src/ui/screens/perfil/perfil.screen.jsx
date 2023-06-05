import { useEffect, useState } from "react";
import { Link, useParams } from "react-router-dom";
import {
  useAmizades,
  useDesfazerAmizade,
  useFeedAmigo,
  useListarUsuarios,
  useLogout,
  useProfile,
  useSolicitarAmizade,
  useVerificarSolicitacao,
} from "../../../api";
import { TOKEN_KEY, USER_KEY } from "../../../constants";
import useGlobalUser from "../../../context/user/user.context";
import FotoPadrao from "../../../utils/img/foto-padrao.png";
import Logo from "../../../utils/img/Logo.png";
import {
  Amizades,
  Button,
  PesquisarUsuarios,
  PostAmigo,
  Solicitacoes,
} from "../../components";
import "./index.css";

export function PerfilScreen() {
  const { usuarioId } = useParams();
  const { getBuscaUsuario, buscaUsuario } = useListarUsuarios();
  const { getPostsAmigo, posts } = useFeedAmigo();
  const { getProfile, error } = useProfile();
  const { postLogout } = useLogout();
  const [, setUser] = useGlobalUser();
  const [solicitacoesAberto, setSolicitacoesAberto] = useState(false);
  const [amigosAberto, setAmigosAberto] = useState(false);
  const [pesquisaAberto, setPesquisaAberto] = useState(false);
  const { getAmizades, amigos } = useAmizades();
  const { postDesfazerAmizade, resposta } = useDesfazerAmizade();
  const { postConviteAmizade, convite } = useSolicitarAmizade();
  const { getVerificaSolicitacao, solicitacao } = useVerificarSolicitacao();

  useEffect(() => {
    getVerificaSolicitacao(usuarioId);
  }, [convite]);

  useEffect(() => {
    getAmizades();
  }, [resposta]);

  useEffect(() => {
    getBuscaUsuario(usuarioId);
  }, [usuarioId]);

  useEffect(() => {
    if (error?.message) {
      localStorage.removeItem(USER_KEY);
      localStorage.removeItem(TOKEN_KEY);
    }
  }, [error]);

  useEffect(() => {
    getProfile();
  }, []);

  const amigo = amigos.filter((obj) => obj.id == usuarioId).length > 0;

  useEffect(() => {
    getPostsAmigo(usuarioId);
  }, [usuarioId]);

  const nomeUsuario = buscaUsuario[0]?.apelido
    ? buscaUsuario[0]?.apelido
    : buscaUsuario[0]?.nome;

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

  const cepFormatado = formatCep(buscaUsuario[0]?.cep);

  function handleClickLogout() {
    postLogout();

    setUser(null);
    localStorage.removeItem(USER_KEY);
    localStorage.removeItem(TOKEN_KEY);
  }

  const dataNascimneto = new Date(buscaUsuario[0]?.dataNascimento);
  const dataFormatada = dataNascimneto.toLocaleDateString("pt-BR", {
    timeZone: "UTC",
  });

  function handleDesfazerAmizade(e) {
    e.preventDefault();
    postDesfazerAmizade(usuarioId);
  }
  function handleSolicitarAmizade(e) {
    e.preventDefault();
    postConviteAmizade(usuarioId);
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
            src={
              buscaUsuario[0]?.imagemPerfil
                ? buscaUsuario[0]?.imagemPerfil
                : FotoPadrao
            }
            alt="foto de perfil"
          />
        </div>
        <div className="profileInfo">
          <h4 className="profileInfoName">{nomeUsuario}</h4>
        </div>
        <div className="info-navegacao">
          <ul>
            <li>
              <Link to={"/feed/"}>Página Inicial</Link>
            </li>
            <li
              onClick={() => {
                setPesquisaAberto(!pesquisaAberto);
                setAmigosAberto(false);
                setSolicitacoesAberto(false);
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
              }}
            >
              Amigos
            </li>
            {amigosAberto && (
              <>
                <Amizades />
              </>
            )}
          </ul>
        </div>

        <div className="info-usuario">
          <h1>Perfil</h1>
          <h2>Nome: {buscaUsuario[0]?.nome}</h2>
          <h3>Apelido: {buscaUsuario[0]?.apelido}</h3>
          <h3>Email: {buscaUsuario[0]?.email}</h3>
          <h3>Data de Nascimento: {dataFormatada}</h3>
          <h3>CEP: {cepFormatado}</h3>
        </div>
        <div className="div-central">
          {amigo && (
            <button
              onClick={handleDesfazerAmizade}
              className="button-desfazer-amizade"
            >
              Defazer Amizade
            </button>
          )}

          {solicitacao && (
            <>
              <button
                disabled={solicitacao == true}
                onClick={handleSolicitarAmizade}
                className="button-desabilitado-amizade"
              >
                Solicitação já enviada
              </button>
            </>
          )}

          {!amigo && !solicitacao && (
            <button
              onClick={handleSolicitarAmizade}
              className="button-solicitar-amizade"
            >
              Solicitar Amizade
            </button>
          )}
          <h1 className="publi-text">Publicações</h1>
          {posts &&
            posts.map((post, index) => {
              return <PostAmigo post={post} key={index} />;
            })}
          {posts && posts.length < 1 && (
            <>
              <p className="amigo-sem-publicacoes">Sem Publicações</p>
            </>
          )}
        </div>
      </div>
    </>
  );
}
