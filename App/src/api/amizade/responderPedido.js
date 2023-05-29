import axios from "axios";
import { AMIZADE_KEY, API_URL } from "../../constants";
import AuthService from "../auth/auth";
import { useRequest } from "../_base/use-request";

export const useAmizadeResponder = () => {
  const { handleRequest, data } = useRequest();

  function postResponderPedido(pedidoAmizadeId, aceitar) {
    const token = AuthService.getToken();
    handleRequest(
      axios.post(
        `${API_URL}/${AMIZADE_KEY}/responder-pedido-amizade/${pedidoAmizadeId}`,
        null,
        {
          headers: {
            Authorization: `Bearer ${token}`,
          },
          params: {
            aceitar: aceitar,
          },
        }
      )
    );
  }

  return { postResponderPedido, resposta: data };
};
