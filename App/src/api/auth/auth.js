import axios from "axios";
import { API_URL, USER_KEY } from "../../constants";

class AuthService {
  async login(email, senha) {
    const response = await axios.post(`${API_URL}/${USER_KEY}/login`, {
      email,
      senha,
    });
    if (response.data && response.data.token) {
      localStorage.setItem("token", response.data.token);
    }
    return response.data;
  }

  logout() {
    localStorage.removeItem("token");
  }

  getToken() {
    return localStorage.getItem("token");
  }

  isAuthenticated() {
    const token = this.getToken();

    return token !== null;
  }
}

export default new AuthService();
