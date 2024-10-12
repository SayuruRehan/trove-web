import { axiosAPI } from "../api";

class AuthServices {
  login(credentials) {
    return axiosAPI.post(`/auth/login`, credentials);
  }

  register(userData) {
    return axiosAPI.post(`/auth/register`, userData);
  }
}

export default new AuthServices();
