import axios from "axios";
import { API_URL, USER_KEY } from "../../constants";
import AuthService from "../auth/auth";
import { useRequest } from "../_base/use-request";

export const useListarUsuarios = () => {
  const { handleRequest, data } = useRequest();

  function getBuscaUsuario(id) {
    const token = AuthService.getToken();
    handleRequest(
      axios.get(`${API_URL}/${USER_KEY}/listar-usuarios`, {
        params: {
          id: id,
        },
        headers: {
          Authorization: `Bearer ${token}`,
        },
      })
    );
  }

  return { getBuscaUsuario, buscaUsuario: data };
};
