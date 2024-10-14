import React, { useState } from "react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faCirclePlus } from "@fortawesome/free-solid-svg-icons";
import axios from "axios";
import { toast } from 'react-toastify';

export default function CreatePayment({ onPaymentCreated }) {
  // State to manage the modal visibility
  const [showModal, setShowModal] = useState(false);

  // Form fields
  const [paymentReference, setPaymentReference] = useState('');
  const [amount, setAmount] = useState('');
  const [userId, setUserId] = useState('');
  const [errors, setErrors] = useState([]);

  // Functions to open and close the modal
  const handleShow = () => setShowModal(true);
  const handleClose = () => {
    setShowModal(false);
    setErrors([]);
    setPaymentReference('');
    setAmount('');
    setUserId('');
  };

  const handleSubmit = (e) => {
    e.preventDefault();

    const paymentData = {
      paymentId: "",
      paymentReference,
      amount: parseFloat(amount),
      userId,
    };

    // Reset errors
    setErrors([]);

    // Post the data to the API
    axios
      .post(`${process.env.REACT_APP_WEB_API}/Payments`, paymentData)
      .then((response) => {
        // Handle success
        console.log('Payment created successfully:', response.data);
        // Close the modal and reset the form
        toast.success('Payment created successfully!');
        
        handleClose();
        // Notify parent component to refresh the payment list
        if (onPaymentCreated) {
          onPaymentCreated(response.data.payment);
        }
      })
      .catch((error) => {
        // Handle error
        console.error('There was an error creating the payment!', error);
        if (error.response && error.response.data) {
          const responseData = error.response.data;
          console.log('Error response data:', responseData); // Log detailed error
          if (responseData.errors) {
            const errorMessages = [];
            for (const key in responseData.errors) {
              errorMessages.push(...responseData.errors[key]);
            }
            setErrors(errorMessages);
            // Display error toast messages
            errorMessages.forEach((err) => toast.error(err));
          } else if (responseData.message) {
            setErrors([responseData.message]);
            toast.error(responseData.message);
          } else {
            setErrors(['An unexpected error occurred.']);
            toast.error('An unexpected error occurred.');
          }
        } else {
          setErrors(['An unexpected error occurred.']);
          toast.error('An unexpected error occurred.');
        }
      });
  };

  return (
    <>
      {/* Button to trigger the modal */}
      <button
        type="button"
        className="btn btn-success d-flex align-items-center"
        onClick={handleShow}
      >
        <FontAwesomeIcon icon={faCirclePlus} className="me-2" /> Create Payment
      </button>

      {/* Modal */}
      {showModal && (
        <div
          className="modal fade show"
          tabIndex="-1"
          role="dialog"
          style={{ display: "block", backgroundColor: "rgba(0, 0, 0, 0.5)" }}
        >
          <div className="modal-dialog modal-lg d-flex justify-content-center">
            <div className="modal-content">
              <div className="modal-header">
                <h5 className="modal-title">Create New Payment</h5>
                <button
                  type="button"
                  className="btn-close"
                  onClick={handleClose}
                  aria-label="Close"
                ></button>
              </div>

              {/* Full modal body without scrolling */}
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
                    {/* Left Column */}
                    <div className="col-md-6">
                      {/* Payment Reference input */}
                      <div className="form-outline mb-4">
                        <label className="form-label" htmlFor="paymentReference">
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

                      {/* Amount input */}
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

                    {/* Right Column */}
                    <div className="col-md-6">
                      {/* User ID input */}
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

                  {/* Submit button */}
                  <button type="submit" className="btn btn-primary btn-block">
                    Submit
                  </button>

                  {/* Close button inside the modal body */}
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
