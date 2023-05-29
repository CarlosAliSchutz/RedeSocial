import axios from "axios";
import { API_URL } from "../../constants";
import AuthService from "../auth/auth";

export const axiosInstance = axios.create({
  baseURL: API_URL,
  timeout: 5000,
  withCredentials: true,
});

axiosInstance.interceptors.request.use((config) => {
  const token = AuthService.getToken();
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});
