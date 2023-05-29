import { RouterProvider } from "react-router-dom";
import { GlobalUserProvider } from "./context/user/user.context";
import { router } from "./router";

export default function App() {
  return (
    <>
     <GlobalUserProvider>
        <RouterProvider router={router} />
        </GlobalUserProvider>
    </>
  );
}

export { App };

