import axios from "axios";
import { AMIZADE_KEY, API_URL } from "../../constants";
import AuthService from "../auth/auth";
import { useRequest } from "../_base/use-request";

export const useAmizades = () => {
  const { handleRequest, data } = useRequest();

  function getAmizades() {
    const token = AuthService.getToken();
    handleRequest(
      axios.get(`${API_URL}/${AMIZADE_KEY}/amigos`, {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      })
    );
  }

  return { getAmizades, amigos: data };
};
