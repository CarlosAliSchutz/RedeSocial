import axios from "axios";
import { API_URL, POST_KEY } from "../../constants";
import AuthService from "../auth/auth";
import { useRequest } from "../_base/use-request";

export const usePost = () => {
  const { handleRequest, data } = useRequest();

  function postCriarMeuPost( conteudo, permissaoVisualizar ) {
    const token = AuthService.getToken();
    handleRequest(
      axios.post(
        `${API_URL}/${POST_KEY}/criar`,
        {
          conteudo,
          permissaoVisualizar,
        },
        {
          headers: {
            Authorization: `Bearer ${token}`,
          },
        }
      )
    );
  }
  return { postCriarMeuPost, novoPost: data };
};
