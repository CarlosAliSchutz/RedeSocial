import axios from "axios";
import { API_URL, POST_KEY } from "../../constants";
import AuthService from "../auth/auth";
import { useRequest } from "../_base/use-request";

export const useFeed = () => {
  const { handleRequest, data } = useRequest();

  function getPosts(pagina = 1, quantidadePorPagina = 10) {
    const token = AuthService.getToken();
    handleRequest(
      axios.get(`${API_URL}/${POST_KEY}/feed`, {
        headers: {
          Authorization: `Bearer ${token}`,
        },
        params: {
          pagina,
          quantidadePorPagina,
        },
      })
    );
  }
  return { getPosts, posts: data };
};
