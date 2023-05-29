import axios from "axios";
import { AMIZADE_KEY, API_URL } from "../../constants";
import AuthService from "../auth/auth";
import { useRequest } from "../_base/use-request";

export const useDesfazerAmizade = () => {
  const { handleRequest, data } = useRequest();

  function postDesfazerAmizade(amigoId) {
    const token = AuthService.getToken();
    handleRequest(
      axios.post(`${API_URL}/${AMIZADE_KEY}/remover-amizade`, null, {
        headers: {
          Authorization: `Bearer ${token}`,
        },
        params: {
          amigoId: amigoId,
        },
      })
    );
  }

  return { postDesfazerAmizade, resposta: data };
};
