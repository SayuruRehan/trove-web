// IT21167300 - Sumanasekara P. D. M.

import { axiosAPI } from "../api";

class AuthServices {
  login(credentials) {
    return axiosAPI.post(`/auth/login`, credentials);
  }

  register(userData) {
    return axiosAPI.get(`/auth/register`, userData);
  }
}

export default new AuthServices();
