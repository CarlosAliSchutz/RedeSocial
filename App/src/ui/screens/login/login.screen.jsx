import { useState } from "react";
import { useNavigate } from "react-router-dom";
import "react-toastify/dist/ReactToastify.css";
import AuthService from "../../../api/auth/auth";
import { TOKEN_KEY } from "../../../constants";
import useGlobalUser from "../../../context/user/user.context";
import { Button, Input } from "../../components";
import "./index.css";

export function LoginScreen() {
  const [email, setEmail] = useState("");
  const [senha, setSenha] = useState("");
  const [, setUser] = useGlobalUser();
  const navigate = useNavigate();

  const handleLogin = async () => {
    try {
      const response = await AuthService.login(email, senha);
      localStorage.setItem(TOKEN_KEY, response.token);
      window.location.href = "/feed";
      setUser(response);
    } catch (error) {
      console.log("Erro ao efetuar o login:", error);
    }
  };
  function handleCadastro() {
    navigate("/cadastro");
  }

  return (
    <div className="login">
      <div className="login__div">
        <h1>Acessar</h1>
        <Input
          label="E-mail:"
          name="email"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
          autoComplete="off"
        />
        <Input
          label="Senha:"
          name="password"
          value={senha}
          onChange={(e) => setSenha(e.target.value)}
          type="password"
        />
        <div className="button-div">
          <Button className="button-login" onClick={handleLogin}>
            Login
          </Button>
        </div>
      </div>
      <div>
        <Button className="button-cadastro" onClick={handleCadastro}>
          Cadastro
        </Button>
      </div>
    </div>
  );
}
