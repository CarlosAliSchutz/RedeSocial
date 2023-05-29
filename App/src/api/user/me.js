import axios from "axios";
import { API_URL, USER_KEY } from "../../constants";
import { useRequest } from "../_base/use-request";
import AuthService from "../auth/auth";

export const useProfile = () => {
  const { handleRequest, data, error } = useRequest();

  function getProfile() {
    const token = AuthService.getToken();
    handleRequest(
      axios.get(`${API_URL}/${USER_KEY}/autenticar`, {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      })
    );
  }
  return { getProfile, perfil: data, error };
};
