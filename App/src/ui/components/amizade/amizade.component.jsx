import { useEffect, useState } from "react";
import { useDesfazerAmizade, usePesquisarAmigos } from "../../../api";
import FotoPadrao from "../../../utils/img/foto-padrao.png";
import { Button, DesfazerAmizades, Input } from "../index";
import "./index.css";

export function Amizades() {
  const { getPesquisaAmigos, amigos } = usePesquisarAmigos();
  const [pagina, setPagina] = useState(1);
  const [pesquisa, setpesquisa] = useState("@");
  const { postDesfazerAmizade, resposta } = useDesfazerAmizade();

  useEffect(() => {
    getPesquisaAmigos(pesquisa, pagina);
  }, [resposta, pagina]);

  function handlePassaPagina() {
    setPagina((prevPagina) => prevPagina + 1);
  }

  function handleVoltarPagina() {
    setPagina((prevPagina) => prevPagina - 1);
  }

  function handleDesfazerAmizade(amigoId) {
    postDesfazerAmizade(amigoId);
  }

  return (
    <div className="pesquisar-amigos">
      <Input
        type="text"
        name="busca"
        onChange={(e) => setpesquisa(e.target.value)}
        placeholder="Pesquisar..."
      />
      <div className="amigos-buscados">
        {amigos &&
          amigos?.itens?.map((amigo, index) => {
            return (
              <div key={index}>
                <img
                  className="imagemPerfil"
                  src={amigo?.imagemPerfil ? amigo.imagemPerfil : FotoPadrao}
                />
                <p>{amigo.nome}</p>
                <DesfazerAmizades
                  amigoId={amigo?.id}
                  onDesfazerAmizade={handleDesfazerAmizade}
                />
              </div>
            );
          })}
        {pagina > 1 && (
          <Button onClick={handleVoltarPagina}>Voltar Página</Button>
        )}
        {amigos?.itens && amigos?.itens.length >= 10 && (
          <Button onClick={handlePassaPagina}>Proxima página</Button>
        )}
      </div>
    </div>
  );
}
