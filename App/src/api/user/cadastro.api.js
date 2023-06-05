import axios from "axios";
import { API_URL, USER_KEY } from "../../constants";
import { useRequest } from "../_base/use-request";

export const useCadastro = () => {
  const { handleRequest, data, error } = useRequest();

  function postCadastro({
    nome,
    email,
    apelido,
    dataNascimento,
    cep,
    senhaHash,
    imagemPerfil,
  }) {
    handleRequest(
      axios.post(`${API_URL}/${USER_KEY}/cadastro`, {
        nome,
        email,
        apelido,
        dataNascimento,
        cep,
        senhaHash,
        imagemPerfil,
      })
    );
  }
  return { postCadastro, usuario: data, error };
};
