import { axiosInstance } from "../_base/axios-instance";
import { useRequest } from "../_base/use-request";

export const useLogout = () => {
  const { handleRequest } = useRequest();

  function postLogout() {
    handleRequest(axiosInstance.get("/logout"));
};
return { postLogout}
}
