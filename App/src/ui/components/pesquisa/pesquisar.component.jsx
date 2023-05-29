import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import { usePesquisarUsuarios } from "../../../api/user/pesquisarUsuario";
import FotoPadrao from "../../../assets/foto-padrao.png";
import { Button } from "../button/button.component";
import { Input } from "../input/input.component";
import "./index.css";

export function PesquisarUsuarios() {
  const { getPesquisa, usuarios } = usePesquisarUsuarios();
  const [pagina, setPagina] = useState(1);
  const [pesquisa, setpesquisa] = useState("@");

  useEffect(() => {
    getPesquisa(pesquisa, pagina);
  }, [pesquisa, pagina]);

  function handlePassaPagina() {
    setPagina((prevPagina) => prevPagina + 1);
  }

  function handleVoltarPagina() {
    setPagina((prevPagina) => prevPagina - 1);
  }

  return (
    <div className="pesquisar-usuarios">
      <Input
        type="text"
        name="busca"
        onChange={(e) => setpesquisa(e.target.value)}
        placeholder="Pesquisar..."
      />
      <div className="usuarios-buscados">
        {usuarios &&
          usuarios?.itens?.map((usuario, index) => {
            return (
              <Link to={`/feed/${usuario.id}`}  key={index}>
                <div>
                  <img
                    className="imagemPerfil"
                    src={
                      usuario?.imagemPerfil ? usuario?.imagemPerfil : FotoPadrao
                    }
                  />
                  <p>{usuario?.nome}</p>
                </div>
              </Link>
            );
          })}
        {pagina > 1 && (
          <Button onClick={handleVoltarPagina}>Voltar Página</Button>
        )}
        {usuarios?.itens && usuarios?.itens.length >= 10 && (
          <Button onClick={handlePassaPagina}>Proxima página</Button>
        )}
      </div>
    </div>
  );
}
