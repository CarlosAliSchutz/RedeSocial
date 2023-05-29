import { axiosInstance } from "../_base/axios-instance";

export const useLogin = () => {
  async function postLogin({ username, password }) {
    const response = await axiosInstance.post(
      "/login",
      {},
      {
        auth: {
          username,
          password,
        },
      }
    );
    return response.data;
  }
  return { postLogin };
};
