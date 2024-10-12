import { axiosAPI } from "../api";

class UserService {
  getAllUsers() {
    return axiosAPI.get(`/user`);
  }

  updateUserStatus(userId) {
    return axiosAPI.patch(`/user/approve/${userId}`);
  }
}

export default new UserService();
