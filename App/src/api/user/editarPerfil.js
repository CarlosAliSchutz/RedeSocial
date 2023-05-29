import axios from "axios";
import { API_URL, USER_KEY } from "../../constants";
import AuthService from "../auth/auth";
import { useRequest } from "../_base/use-request";

export const useEditarPerfil = () => {
  const { handleRequest, data, error } = useRequest();

  function putEditarPerfil(apelido, imagemPerfil) {
    const token = AuthService.getToken();
    handleRequest(
      axios.put(
        `${API_URL}/${USER_KEY}/editar-perfil`,
        {
          apelido,
          imagemPerfil,
        },
        {
          headers: {
            Authorization: `Bearer ${token}`,
          },
        }
      )
    );
  }
  return { putEditarPerfil, usuario: data, error };
};
