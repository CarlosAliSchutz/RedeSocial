import axios from "axios";
import { AMIZADE_KEY, API_URL } from "../../constants";
import AuthService from "../auth/auth";
import { useRequest } from "../_base/use-request";

export const useDesfazerAmizade = () => {
  const { handleRequest, data } = useRequest();

  function postDesfazerAmizade(amigoId) {
    const token = AuthService.getToken();
    handleRequest(
      axios.delete(`${API_URL}/${AMIZADE_KEY}/remover-amizade/${amigoId}`, {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      })
    );
  }

  return { postDesfazerAmizade, resposta: data };
};
