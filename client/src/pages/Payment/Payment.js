import React, {useEffect, useState} from "react";
import Footer from "../../components/Footer";
import Header from "../../components/Header";
import CreatePayment from "./CreatePayment";
import EditPayment from "./EditPayment";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faTrash} from "@fortawesome/free-solid-svg-icons";
import $ from "jquery";
import "bootstrap/dist/css/bootstrap.min.css";
import "datatables.net-bs4/css/dataTables.bootstrap4.min.css";
import dt from "datatables.net-bs4";
import axios from "axios";
import {ToastContainer, toast} from "react-toastify";
import ClipLoader from "react-spinners/ClipLoader";
import "react-toastify/dist/ReactToastify.css";

export default function Payment() {
  const [payments, setPayments] = useState([]);
  const [isDataLoaded, setIsDataLoaded] = useState(false);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    fetchPayments();

    return () => {
      if ($.fn.DataTable.isDataTable("#paymentTable")) {
        $("#paymentTable").DataTable().destroy(true);
      }
    };
  }, []);

  useEffect(() => {
    if (isDataLoaded) {
      $("#paymentTable").DataTable();
    }
  }, [isDataLoaded]);

  const fetchPayments = () => {
    axios
        .get(`${process.env.REACT_APP_WEB_API}/Payments`)
        .then((response) => {
          setPayments(response.data);
          setIsDataLoaded(true);
        })
        .catch((error) => {
          console.error("There was an error fetching the payments!", error);
        });
    setLoading(false);
  };

  // Function to handle delete
  const deletePayment = (paymentId) => {
    axios
        .delete(`${process.env.REACT_APP_WEB_API}/Payments/${paymentId}`)
        .then(() => {
          setPayments(payments.filter((payment) => payment.paymentId !== paymentId));
          toast.success("Payment deleted successfully!");
        })
        .catch((error) => {
          console.error("There was an error deleting the payment!", error);
          toast.error("There was an error deleting the payment.");
        });
  };

  // Function to handle new payment creation
  const handlePaymentCreated = (newPayment) => {
    setPayments([...payments, newPayment]);
  };

  // Function to handle payment update
  const handlePaymentUpdated = (updatedPayment) => {
    const updatedPayments = payments.map((payment) =>
        payment.paymentId === updatedPayment.paymentId ? updatedPayment : payment
    );
    setPayments(updatedPayments);
  };

  return (
      <>
        <Header />
        <ToastContainer />
          <div className="ml-10 mr-10 mb-10">
              <div className="d-flex justify-content-between align-items-center mb-4">
                  <h2>Payment List</h2>
                  <CreatePayment onPaymentCreated={handlePaymentCreated}/>
              </div>

              <table
                  id="paymentTable"
                  className="table table-hover table-bordered shadow-sm"
                  style={{width: "100%"}}
              >
                  <thead className="bg-gray-100">
                  <tr>
                      <th>Payment ID</th>
                      <th>Payment Reference</th>
                      <th>Amount</th>
                      <th>User ID</th>
                      <th>Actions</th>
                  </tr>
                  </thead>
                  <tbody>
                  {payments.length > 0 ? (
                      payments.map((payment) => (
                          <tr key={payment.paymentId}>
                              <td>{payment.paymentId}</td>
                              <td>{payment.paymentReference}</td>
                              <td>${payment.amount.toFixed(2)}</td>
                              <td>{payment.userId}</td>
                              <td className="d-flex justify-content-around">
                                  <EditPayment
                                      payment={payment}
                                      onPaymentUpdated={handlePaymentUpdated}
                                  />
                                  <button
                                      className="text-red-600 hover:text-red-800 transition"
                                      onClick={() => deletePayment(payment.paymentId)}
                                  >
                                      <FontAwesomeIcon icon={faTrash}/>
                                  </button>
                              </td>
                          </tr>
                      ))
                  ) : (
                      <tr>
                          <td colSpan="5" className="text-center">
                              No payments available
                          </td>
                      </tr>
                  )}
                  </tbody>
              </table>
              <div className="d-flex justify-content-center">
                  {loading && (
                      <ClipLoader
                          color="#000"
                          loading={loading}
                          size={150}
                          aria-label="Loading Spinner"
                          data-testid="loader"
                      />
                  )}
              </div>
          </div>
          <Footer/>
      </>
  );
}
