import axios from "axios";
import { API_URL, POST_KEY } from "../../constants";
import AuthService from "../auth/auth";
import { useRequest } from "../_base/use-request";

export const useFeedAmigo = () => {
  const { handleRequest, data } = useRequest();

  function getPostsAmigo(usuarioId) {
    const token = AuthService.getToken();
    handleRequest(
      axios.get(`${API_URL}/${POST_KEY}/visitar/${usuarioId}`, {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      })
    );
  }
  return { getPostsAmigo, posts: data };
};
