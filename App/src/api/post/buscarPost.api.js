import axios from "axios";
import { API_URL, POST_KEY } from "../../constants";
import AuthService from "../auth/auth";
import { useRequest } from "../_base/use-request";

export const useBuscarPost = () => {
  const { handleRequest, data } = useRequest();

  function getBuscaPost(idPost) {
    const token = AuthService.getToken();
    handleRequest(
      axios.get(`${API_URL}/${POST_KEY}/${idPost}`, {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      })
    );
  }

  return { getBuscaPost, busca: data };
};
