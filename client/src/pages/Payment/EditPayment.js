import React, {useState, useEffect} from "react";
import axios from "axios";
import {toast} from "react-toastify";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faEdit} from "@fortawesome/free-solid-svg-icons";

export default function EditPayment({payment, onPaymentUpdated}) {
  const [showModal, setShowModal] = useState(false);
  const [paymentReference, setPaymentReference] = useState("");
  const [amount, setAmount] = useState("");
  const [userId, setUserId] = useState("");
  const [errors, setErrors] = useState([]);

  const handleShow = () => {
    setPaymentReference(payment.paymentReference || "");
    setAmount(payment.amount);
    setUserId(payment.userId);
    setShowModal(true);
  };

  const handleClose = () => {
    setShowModal(false);
    setErrors([]);
  };

  const handleSubmit = (e) => {
    e.preventDefault();

    const updatedPaymentData = {
      paymentId: payment.paymentId,
      paymentReference,
      amount: parseFloat(amount),
      userId,
    };

    setErrors([]);

    axios
      .put(
        `${process.env.REACT_APP_WEB_API}/Payments/${payment.paymentId}`,
        updatedPaymentData
      )
      .then((response) => {
        console.log("Payment updated successfully:", response.data);
        toast.success("Payment updated successfully!");
        handleClose();
        if (onPaymentUpdated) {
          onPaymentUpdated(response.data.payment);
        }
      })
      .catch((error) => {
        console.error("There was an error updating the payment!", error);
        if (error.response && error.response.data) {
          const responseData = error.response.data;
          if (responseData.errors) {
            const errorMessages = [];
            for (const key in responseData.errors) {
              errorMessages.push(...responseData.errors[key]);
            }
            setErrors(errorMessages);
            errorMessages.forEach((err) => toast.error(err));
          } else if (responseData.message) {
            setErrors([responseData.message]);
            toast.error(responseData.message);
          } else {
            setErrors(["An unexpected error occurred."]);
            toast.error("An unexpected error occurred.");
          }
        } else {
          setErrors(["An unexpected error occurred."]);
          toast.error("An unexpected error occurred.");
        }
      });
  };

  return (
    <>
      <button
        type="button"
        className="btn btn-primary btn-sm me-2"
        onClick={handleShow}
      >
        <FontAwesomeIcon icon={faEdit} className="me-1" /> Edit
      </button>

      {showModal && (
        <div
          className="modal fade show"
          tabIndex="-1"
          role="dialog"
          style={{display: "block", backgroundColor: "rgba(0, 0, 0, 0.5)"}}
        >
          <div className="modal-dialog modal-lg d-flex justify-content-center">
            <div className="modal-content">
              <div className="modal-header">
                <h5 className="modal-title">Edit Payment</h5>
                <button
                  type="button"
                  className="btn-close"
                  onClick={handleClose}
                  aria-label="Close"
                ></button>
              </div>
              <div className="modal-body p-4">
                {errors.length > 0 && (
                  <div className="alert alert-danger">
                    <ul>
                      {errors.map((error, index) => (
                        <li key={index}>{error}</li>
                      ))}
                    </ul>
                  </div>
                )}
                <form onSubmit={handleSubmit}>
                  <div className="row">
                    <div className="col-md-6">
                      <div className="form-outline mb-4">
                        <label
                          className="form-label"
                          htmlFor="paymentReference"
                        >
                          Payment Reference
                        </label>
                        <input
                          type="text"
                          id="paymentReference"
                          className="form-control"
                          placeholder="Enter payment reference"
                          value={paymentReference}
                          onChange={(e) => setPaymentReference(e.target.value)}
                        />
                      </div>
                      <div className="form-outline mb-4">
                        <label className="form-label" htmlFor="amount">
                          Amount
                        </label>
                        <input
                          type="number"
                          step="0.01"
                          id="amount"
                          className="form-control"
                          placeholder="Enter amount"
                          value={amount}
                          onChange={(e) => setAmount(e.target.value)}
                        />
                      </div>
                    </div>
                    <div className="col-md-6">
                      <div className="form-outline mb-4">
                        <label className="form-label" htmlFor="userId">
                          User ID
                        </label>
                        <input
                          type="text"
                          id="userId"
                          className="form-control"
                          placeholder="Enter user ID"
                          value={userId}
                          onChange={(e) => setUserId(e.target.value)}
                        />
                      </div>
                    </div>
                  </div>
                  <button type="submit" className="btn btn-primary btn-block">
                    Update
                  </button>
                  <button
                    type="button"
                    className="btn btn-secondary btn-block mt-3"
                    onClick={handleClose}
                  >
                    Close
                  </button>
                </form>
              </div>
            </div>
          </div>
        </div>
      )}
    </>
  );
}
