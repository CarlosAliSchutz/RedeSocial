import axios from "axios";
import { AMIZADE_KEY, API_URL } from "../../constants";
import AuthService from "../auth/auth";
import { useRequest } from "../_base/use-request";

export const useSolicitarAmizade = () => {
  const { handleRequest, data } = useRequest();

  function postConviteAmizade(amigoId) {
    const token = AuthService.getToken();
    handleRequest(
      axios.post(`${API_URL}/${AMIZADE_KEY}/pedido`, null, {
          headers: {
            Authorization: `Bearer ${token}`,
          },
          params: {
            amigoId: amigoId,
          },
        }
      )
    );
  }

  return { postConviteAmizade, convite: data };
};
