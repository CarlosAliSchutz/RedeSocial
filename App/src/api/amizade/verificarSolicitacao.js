import axios from "axios";
import { AMIZADE_KEY, API_URL } from "../../constants";
import AuthService from "../auth/auth";
import { useRequest } from "../_base/use-request";

export const useVerificarSolicitacao = () => {
  const { handleRequest, data } = useRequest();

  function getVerificaSolicitacao(usuarioId) {
    const token = AuthService.getToken();
    handleRequest(
      axios.get(`${API_URL}/${AMIZADE_KEY}/usuarios/${usuarioId}/status-amizade`, {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      })
    );
  }

  return { getVerificaSolicitacao, solicitacao: data };
};
