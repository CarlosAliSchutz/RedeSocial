import axios from "axios";
import { API_URL, MENSAGEM_KEY } from "../../constants";
import AuthService from "../auth/auth";
import { useRequest } from "../_base/use-request";

export const useMensagem = () => {
  const { handleRequest, data } = useRequest();
  function postEnviarMensagem(amigoId, conteudo) {
    const token = AuthService.getToken();
    handleRequest(
      axios.post(
        `${API_URL}/${MENSAGEM_KEY}/enviar-mensagem`,
        {
          amigoId,
          conteudo,
        },
        {
          headers: {
            Authorization: `Bearer ${token}`,
          },
        }
      )
    )
  }


  return { postEnviarMensagem, data };
};
