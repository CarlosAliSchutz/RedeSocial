import { createBrowserRouter } from "react-router-dom";
import {
  CadastroScreen,
  FeedScreen,
  LoginScreen,
  PerfilScreen,
} from "../ui/screens";
import { PrivateRoute } from "./private-route.component";

export const router = createBrowserRouter([
  {
    path: "/",
    element: <LoginScreen />,
  },
  {
    path: "/cadastro",
    element: <CadastroScreen />,
  },
  {
    path: "/feed",
    element: <PrivateRoute Screen={FeedScreen} />,
  },
  {
    path: "/feed/:usuarioId",
    element: <PrivateRoute Screen={PerfilScreen} />,
  },
]);
