import React, {useEffect, useState} from "react";
import Footer from "../../components/Footer";
import Header from "../../components/Header";
import CreatePayment from "./CreatePayment";
import EditPayment from "./EditPayment";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faEdit, faTrash} from "@fortawesome/free-solid-svg-icons";
import $ from "jquery";
import "bootstrap/dist/css/bootstrap.min.css";
import "datatables.net-bs4/css/dataTables.bootstrap4.min.css";
import dt from "datatables.net-bs4";
import axios from "axios";
import {ToastContainer, toast} from "react-toastify";
import ClipLoader from "react-spinners/ClipLoader";

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
      .then((response) => {
        // Remove the deleted payment from the state
        setPayments(
          payments.filter((payment) => payment.paymentId !== paymentId)
        );
        // Display success toast message
        toast.success("Payment deleted successfully!");
      })
      .catch((error) => {
        console.error("There was an error deleting the payment!", error);
        // Display error toast message
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
        <div className="flex justify-center">
          <h2>Payment List</h2>
        </div>

        <div className="d-flex justify-content-end mb-3">
          <CreatePayment onPaymentCreated={handlePaymentCreated} />
        </div>

        <table
          id="paymentTable"
          className="table table-striped table-bordered"
          style={{width: "100%"}}
        >
          <thead>
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
                  <td>{payment.amount}</td>
                  <td>{payment.userId}</td>
                  <td>
                    <EditPayment
                      payment={payment}
                      onPaymentUpdated={handlePaymentUpdated}
                    />
                    <button
                      className="btn btn-danger btn-sm"
                      onClick={() => deletePayment(payment.paymentId)}
                    >
                      <FontAwesomeIcon icon={faTrash} /> Delete
                    </button>
                  </td>
                </tr>
              ))
            ) : (
              <tr>
                <td colSpan="5" className="text-center">
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
