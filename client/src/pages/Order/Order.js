import React, {useEffect, useState} from "react";
import Footer from "../../components/Footer";
import Header from "../../components/Header";
import CreateOrder from "./CreateOrder";
import EditOrder from "./EditOrder";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faEdit, faTrash, faPlus} from "@fortawesome/free-solid-svg-icons";
import $ from "jquery";
import "bootstrap/dist/css/bootstrap.min.css";
import "datatables.net-bs4/css/dataTables.bootstrap4.min.css";
import dt from "datatables.net-bs4";
import axios from "axios";
import {ToastContainer, toast} from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import ClipLoader from "react-spinners/ClipLoader";
import LoadingSpinner from "../../components/ui/loading-chip";

export default function Order() {
  const [orders, setOrders] = useState([]);
  const [isDataLoaded, setIsDataLoaded] = useState(false);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    axios
      .get(`${process.env.REACT_APP_WEB_API}/Orders`)
      .then((response) => {
        setOrders(response.data);
        setIsDataLoaded(true);
      })
      .catch((error) => {
        console.error("Error fetching orders:", error);
        toast.error("Error fetching orders.");
      });
    setLoading(false);
    return () => {
      if ($.fn.DataTable.isDataTable("#orderTable")) {
        $("#orderTable").DataTable().destroy(true);
      }
    };
  }, []);

  useEffect(() => {
    if (isDataLoaded) {
      $("#orderTable").DataTable();
    }
  }, [isDataLoaded]);

  const deleteOrder = (orderId) => {
    axios
      .delete(`${process.env.REACT_APP_WEB_API}/Orders/${orderId}`)
      .then((response) => {
        setOrders(orders.filter((order) => order.orderId !== orderId));
        toast.success("Order deleted successfully!");
      })
      .catch((error) => {
        console.error("Error deleting order:", error);
        toast.error("Error deleting order.");
      });
  };

  const handleOrderCreated = (newOrder) => {
    setOrders([...orders, newOrder]);
  };

  const handleOrderUpdated = (updatedOrder) => {
    const updatedOrders = orders.map((order) =>
      order.orderId === updatedOrder.orderId ? updatedOrder : order
    );
    setOrders(updatedOrders);
  };

  // Function to return background color class for the status
  const getStatusBackgroundClass = (status) => {
    switch (status.toLowerCase()) {
      case "delivered":
        return "bg-success text-white rounded-pill px-2 py-1";
      case "cancelled":
        return "bg-danger text-white rounded-pill px-2 py-1";
      case "processing":
      default:
        return "bg-warning text-dark rounded-pill px-2 py-1";
    }
  };

  return (
    <>
      <Header />
      <ToastContainer />
      <div className="ml-10 mr-10 mb-10">
          <div className="d-flex justify-content-between align-items-center mb-4">
            <h2>Order List</h2>
        </div>

        <div className="d-flex justify-content-end mb-3">
          <CreateOrder onOrderCreated={handleOrderCreated} />
        </div>
        <table
          id="orderTable"
          className="table "
          style={{width: "100%"}}
        >
          <thead>
            <tr>
              <th>Order ID</th>
              <th>Order Date</th>
              <th>Description</th>
              <th>Amount</th>
              <th>Delivery Method</th>
              <th>Status</th>
              <th>Phone Number</th>
              <th>Delivery Address</th>
              <th>Actions</th>
            </tr>
          </thead>
          <tbody>
            {orders.length > 0 ? (
              orders.map((order) => (
                  <tr key={order.orderId}>
                    <td>{order.orderId}</td>
                    <td>{new Date(order.orderDate).toLocaleDateString()}</td>
                    <td>{order.orderDescription}</td>
                    <td>{order.amount}</td>
                    <td>{order.deliveryMethod}</td>
                    <td>
                    <span
                        className={`px-2 py-1 rounded-full text-sm font-semibold ${
                            order.status === "delivered" // Assuming status is a string; adjust according to your actual data type
                                ? "bg-green-200 text-green-800"
                                : order.status === "cancelled"
                                    ? "bg-red-200 text-red-800"
                                    : order.status === "processing"
                                        ? "bg-yellow-200 text-yellow-800"
                                        : "bg-gray-200 text-gray-800"
                        }`}
                    >
                      {order.status === "delivered"
                          ? "Delivered"
                          : order.status === "cancelled"
                              ? "Cancelled"
                              : order.status === "processing"
                                  ? "Processing"
                                  : "Pending"}
                    </span>
                    </td>
                    <td>{order.phoneNumber}</td>
                    <td>{order.deliveryAddress}</td>
                    <td>
                      <EditOrder
                          order={order}
                          onOrderUpdated={handleOrderUpdated}
                      />
                      <button
                          className="btn btn-danger btn-sm"
                          onClick={() => deleteOrder(order.orderId)}
                      >
                        <FontAwesomeIcon icon={faTrash}/>
                      </button>
                    </td>
                  </tr>
              ))
            ) : (
                <tr>
                  <td colSpan="9" className="text-center">
                    {loading ?null:"No data available"}
                  </td>
                </tr>
            )}
          </tbody>
        </table>
        <div className="flex justify-center">
          {loading ? <LoadingSpinner /> : null}
        </div>
      </div>
      <Footer/>
    </>
  );
}
