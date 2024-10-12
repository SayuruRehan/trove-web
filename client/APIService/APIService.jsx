import { axiosAPI } from "../api";

class APIService {
  // Order end points

  purchaseOrder(orderObj) {
    return axiosAPI.post(`/order`, orderObj);
  }

  getAllOrders() {
    return axiosAPI.get(`/order`);
  }

  updateOrderDetails(updateOrderObj, id) {
    return axiosAPI.put(`/order/${id}`, updateOrderObj);
  }

  deleteOrder(id) {
    return axiosAPI.delete(`/order/${id}`);
  }

  // Vendor end points

  getAllVendors(config = {}) {
    return axiosAPI.get(`/vendor`, config);
  }

  addVendor() {}

  getVendorById() {}

  updateVendor(id, vendorObj) {
    return axiosAPI.put(`/vendor/${id}`, vendorObj);
  }

  getVendorOrders(vendorId) {
    return axiosAPI.get(`/order/vendor/${vendorId}/suborders`);
  }

  updateOrderStatus(orderId, orderObj) {
    return axiosAPI.put(`/order/orderitems/${orderId}`, orderObj);
  }

  getOrderWithItems() {
    return axiosAPI.get(`/order/items`);
  }
}

export default new APIService();
