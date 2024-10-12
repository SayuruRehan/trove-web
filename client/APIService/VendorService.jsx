import { axiosAPI } from "../api";

class VendorService {
  // Vendor end points

  getCustomerFeedback() {
    return axiosAPI.get(`/feedback`);
  }

  getUnapprovedVendors() {
    return axiosAPI.get(`/user/vendors/unApproved`);
  }
}

export default new VendorService();
