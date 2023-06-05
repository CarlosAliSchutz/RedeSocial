import axios from "axios";
import { AMIZADE_KEY, API_URL } from "../../constants";
import AuthService from "../auth/auth";
import { useRequest } from "../_base/use-request";

export const useAmizade = () => {
  const { handleRequest, data } = useRequest();

  function getSolicitacoesAmizade() {
    const token = AuthService.getToken();
    handleRequest(
      axios.get(`${API_URL}/${AMIZADE_KEY}/pedidos-amizade`, {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      })
    );
  }

  return { getSolicitacoesAmizade, pedidos: data };
};
