import axios from "axios";
import { API_URL, POST_KEY } from "../../constants";
import AuthService from "../auth/auth";
import { useRequest } from "../_base/use-request";

export const usePermissao = () => {
  const { handleRequest, data } = useRequest();

  function putAlterarPermissao(postId, permissaoVisualizar) {
    const token = AuthService.getToken();
    handleRequest(
      axios.put(
        `${API_URL}/${POST_KEY}/${postId}/permissao`,
        parseInt(permissaoVisualizar),
        {
          headers: {
            Authorization: `Bearer ${token}`,
            "Content-Type": "application/json", // Adicione este cabeçalho
          },
        }
      )
    );
  }
  return { putAlterarPermissao, novaPermissao: data };
};
