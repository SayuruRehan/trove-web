import React, {useEffect, useState} from "react";
import Footer from "../../components/Footer";
import Header from "../../components/Header";
import CreateOrder from "./CreateOrder";
import EditOrder from "./EditOrder";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faEdit, faTrash} from "@fortawesome/free-solid-svg-icons";
import $ from "jquery";
import "bootstrap/dist/css/bootstrap.min.css";
import "datatables.net-bs4/css/dataTables.bootstrap4.min.css";
import dt from "datatables.net-bs4";
import axios from "axios";
import {ToastContainer, toast} from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import ClipLoader from "react-spinners/ClipLoader";

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
        return "bg-success text-white";
      case "cancelled":
        return "bg-danger text-white";
      case "processing":
      default:
        return "bg-warning text-dark";
    }
  };

  return (
    <>
      <Header />
      <ToastContainer />
      <div className="ml-10 mr-10 mb-10">
        <div className="flex justify-center">
          <h2>Order List</h2>
        </div>

        <div className="d-flex justify-content-end mb-3">
          <CreateOrder onOrderCreated={handleOrderCreated} />
        </div>
        <table
          id="orderTable"
          className="table table-striped table-bordered"
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
                  <td className={getStatusBackgroundClass(order.status)}>
                    {order.status}
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
                      <FontAwesomeIcon icon={faTrash} />
                    </button>
                  </td>
                </tr>
              ))
            ) : (
              <tr>
                <td colSpan="9" className="text-center">
                  No data available
                </td>
              </tr>
            )}
          </tbody>
        </table>
        <div className="flex justify-center">
          <ClipLoader
            color="#000"
            loading={loading}
            size={150}
            aria-label="Loading Spinner"
            data-testid="loader"
          />
        </div>
      </div>
      <Footer />
    </>
  );
}
