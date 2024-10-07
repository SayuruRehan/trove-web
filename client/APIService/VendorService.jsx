import axios from "axios";
import { BASE_URL } from "./BASEUrl";

class VendorService {
  // Vendor end points

  getAllVendors(config = {}) {
    return axios.get(`${BASE_URL}/api/vendor`, config);
  }

  getVendorById(id, config = {}) {
    return axios.get(`${BASE_URL}/api/vendor/${id}`, config);
  }

  addVendor(vendor) {
    return axios.post(`${BASE_URL}/api/vendor`, vendor);
  }

  updateVendorDetails(id, venodr) {
    return axios.put(`${BASE_URL}/api/vendor/${id}`, venodr);
  }

  // updateVendorStatus() {
  //   return axios.put(`${BASE_URL}/api/vendor/${id}`);
  // }

  deleteVendor(id) {
    return axios.delete(`${BASE_URL}/api/vendor/${id}`);
  }
}

export default new VendorService();
