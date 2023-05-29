import { useEffect } from "react";
import { useAmizadeResponder } from "../../../api/amizade/responderPedido";
import { useListarUsuarios } from "../../../api/user/listarUsuarios";
import FotoPadrao from "../../../assets/foto-padrao.png";
import "./index.css";

export function AmigoBox({ amigo, pedido, onAceitarAmizade }) {
  const { getBuscaUsuario, buscaUsuario } = useListarUsuarios();

  function handleAceitarPedido(e) {
    e.preventDefault();
    onAceitarAmizade(pedido, true);
  }

  function handleNegarPedido(e) {
    e.preventDefault();
    onAceitarAmizade(pedido, false);
  }

  useEffect(() => {
    getBuscaUsuario(amigo);
  }, []);

  return (
    <div className="amigos-solicitacao">
      <img
        className="imagemPerfil"
        src={
          buscaUsuario[0]?.imagemPerfil
            ? buscaUsuario[0]?.imagemPerfil
            : FotoPadrao
        }
      />
      <p>{buscaUsuario[0]?.nome}</p>
      <button className="aceitar-amigo" onClick={handleAceitarPedido}>
        Aceitar
      </button>
      <button className="negar-amigo" onClick={handleNegarPedido}>
        Negar
      </button>
    </div>
  );
}
