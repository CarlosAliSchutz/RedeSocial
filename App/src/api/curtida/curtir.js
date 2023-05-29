import axios from "axios";
import { API_URL, CURTIDA_KEY } from "../../constants";
import AuthService from "../auth/auth";
import { useRequest } from "../_base/use-request";

export const useCurtir = () => {
  const { handleRequest, data } = useRequest();

  function postCurtir(postId) {
    const token = AuthService.getToken();
    handleRequest(
      axios.post(
        `${API_URL}/${CURTIDA_KEY}/${postId}/curtir`,
        {},
        {
          headers: {
            Authorization: `Bearer ${token}`,
          },
        }
      )
    );
  }
  return { postCurtir, curtida: data };
};
