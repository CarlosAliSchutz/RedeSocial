import axios from "axios";
import { API_URL, MENSAGEM_KEY } from "../../constants";
import AuthService from "../auth/auth";
import { useRequest } from "../_base/use-request";

export const useConversa = () => {
  const { handleRequest, data, error } = useRequest();

  async function getConversa(amigoId) {
    try {
      const token = AuthService.getToken();
      const response = await axios.get(
        `${API_URL}/${MENSAGEM_KEY}/${amigoId}`,
        {
          headers: {
            Authorization: `Bearer ${token}`,
          },
        }
      );
      handleRequest(response);
      
    } catch (error) {
      console.error("Erro na solicitação de conversa:", error);
    }
  }

  return { getConversa, mensagens: data, error };
};
