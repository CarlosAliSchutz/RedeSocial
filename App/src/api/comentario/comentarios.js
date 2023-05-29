import axios from "axios";
import { API_URL, COMENTARIO_KEY } from "../../constants";
import AuthService from "../auth/auth";
import { useRequest } from "../_base/use-request";

export const useComentarios = () => {
  const { handleRequest, data } = useRequest();

  function getComentarios(idPost) {
    const token = AuthService.getToken();
    handleRequest(
      axios.get(`${API_URL}/${COMENTARIO_KEY}/post/${idPost}/comentarios`, {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      })
    );
  }

  return { getComentarios, comentarios: data };
};
