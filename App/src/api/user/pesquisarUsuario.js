import axios from "axios";
import { API_URL, USER_KEY } from "../../constants";
import AuthService from "../auth/auth";
import { useRequest } from "../_base/use-request";

export const usePesquisarUsuarios = () => {
  const { handleRequest, data } = useRequest();

  function getPesquisa(busca, pagina, quantidadePorPagina) {
    const token = AuthService.getToken();
    handleRequest(
      axios.get(`${API_URL}/${USER_KEY}/pesquisar`, {
        params: {
          busca: busca,
          pagina: pagina,
          quantidadePorPagina: quantidadePorPagina,
        },
        headers: {
          Authorization: `Bearer ${token}`,
        },
      })
    );
  }

  return { getPesquisa, usuarios: data };
};
