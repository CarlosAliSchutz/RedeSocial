import { useEffect } from "react";
import { useAmizade, useAmizadeResponder } from "../../../api";
import { AmigoBox } from "./amigo.component";

export function Solicitacoes() {
  const { getSolicitacoesAmizade, pedidos } = useAmizade();
  const { postResponderPedido, resposta } = useAmizadeResponder();

  useEffect(() => {
    getSolicitacoesAmizade();
  }, [resposta]);

  function handleAceitarPedido(pedido, aceita) {
    postResponderPedido(pedido, aceita);
  }

  return (
    <>
      {pedidos &&
        pedidos.map((pedido, index) => {
          return (
            <AmigoBox
              amigo={pedido?.usuarioId}
              pedido={pedido?.pedidoId}
              key={index}
              onAceitarAmizade={handleAceitarPedido}
            />
          );
        })}
    </>
  );
}
