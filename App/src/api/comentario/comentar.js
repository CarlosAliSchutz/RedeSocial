import axios from "axios";
import { API_URL, COMENTARIO_KEY } from "../../constants";
import AuthService from "../auth/auth";
import { useRequest } from "../_base/use-request";

export const useComentar = () => {
  const { handleRequest, data } = useRequest();

  function postComentar(postId, conteudo) {
    const token = AuthService.getToken();
    handleRequest(
      axios.post(
        `${API_URL}/${COMENTARIO_KEY}/${postId}`,
        {
          conteudo,
        },
        {
          headers: {
            Authorization: `Bearer ${token}`,
          },
        }
      )
    );
  }
  return { postComentar, comentar: data };
};
