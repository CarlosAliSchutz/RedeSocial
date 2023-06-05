import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { toast, ToastContainer } from "react-toastify";
import { useCadastro } from "../../../api";
import { Button, Input } from "../../components";
import "./index.css";

export function CadastroScreen() {
  const [formInput, setFormInput] = useState({
    nome: "",
    email: "",
    apelido: "",
    dataNascimento: "",
    cep: "",
    senhaHash: "",
    imagemPerfil: "",
  });
  const { postCadastro, usuario, error } = useCadastro();
  const navigate = useNavigate();

  function handleChange(event) {
    const { name, value } = event.target;

    setFormInput((oldFormInput) => ({ ...oldFormInput, [name]: value }));
  }

  function handleVoltar() {
    navigate("/");
  }

  function handleSubmit(event) {
    event.preventDefault();
    postCadastro({
      nome: formInput.nome,
      email: formInput.email,
      apelido: formInput.apelido,
      dataNascimento: formInput.dataNascimento,
      cep: formInput.cep,
      senhaHash: formInput.senhaHash,
      imagemPerfil: formInput.imagemPerfil,
    });
  }

  useEffect(() => {
    if (error) {
      toast.error("Cadastro inválido!", {
        position: "top-right",
        autoClose: 5000,
        hideProgressBar: true,
        closeOnClick: true,
        pauseOnHover: true,
        draggable: true,
        progress: undefined,
        theme: "colored",
      });
    }
  }, [error]);

  useEffect(() => {
    if (usuario?.nome) {
      toast.success("Cadastro com sucesso!", {
        position: "top-right",
        autoClose: 5000,
        hideProgressBar: true,
        closeOnClick: true,
        pauseOnHover: true,
        draggable: true,
        progress: undefined,
        theme: "colored",
      });
    }
  }, [usuario?.nome]);

  return (
    <>
      <div className="cadastro">
        <form className="cadastro__form" onSubmit={handleSubmit}>
          <h1>Cadastro</h1>
          <Input
            label="Nome:"
            name="nome"
            value={formInput.nome}
            type="text"
            onChange={handleChange}
          />

          <Input
            label="E-mail:"
            name="email"
            type="text"
            value={formInput.email}
            onChange={handleChange}
          />

          <Input
            label="Apelido:"
            name="apelido"
            type="text"
            value={formInput.apelido}
            onChange={handleChange}
          />

          <Input
            label="Data Nascimento:"
            name="dataNascimento"
            type="date"
            value={formInput.dataNascimento}
            onChange={handleChange}
          />

          <Input
            label="CEP:"
            name="cep"
            type="text"
            value={formInput.cep}
            onChange={handleChange}
          />

          <Input
            label="Senha:"
            name="senhaHash"
            value={formInput.senhaHash}
            onChange={handleChange}
            type="password"
          />

          <Input
            label="Foto Perfil"
            name="imagemPerfil"
            value={formInput.imagemPerfil}
            onChange={handleChange}
            type="text"
          />

          <ToastContainer />
          <div className="button-div">
            <Button type="submit">Cadastrar</Button>
          </div>
        </form>
        <Button onClick={handleVoltar}>Voltar</Button>
      </div>
    </>
  );
}
