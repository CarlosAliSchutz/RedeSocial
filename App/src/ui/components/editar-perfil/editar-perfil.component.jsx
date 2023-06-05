import { useState } from "react";
import { Button } from "../button/button.component";
import { Input } from "../input/input.component";
import "./index.css";

export function EditarPerfil({ onEditarPerfil }) {
  const [apelido, setApelido] = useState("");
  const [imagemPerfil, setImagemPerfil] = useState("");

  function handleEditarPerfil(e) {
    e.preventDefault();
    onEditarPerfil(apelido, imagemPerfil);
    setApelido("");
    setImagemPerfil("");
  }

  return (
    <div className="editar-perfil">
      <Input
        value={apelido}
        name={"apelido"}
        label={"Apelido"}
        onChange={(e) => setApelido(e.target.value)}
        autoComplete="off"
      />
      <Input
        value={imagemPerfil}
        name={"imagemPerfil"}
        label={"Imagem de Perfil"}
        onChange={(e) => setImagemPerfil(e.target.value)}
        autoComplete="off"
      />
      <Button
        onClick={handleEditarPerfil}
        disabled={apelido == "" || imagemPerfil == ""}
      >
        Alterar
      </Button>
    </div>
  );
}
