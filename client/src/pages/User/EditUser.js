import React, {useState} from "react";
import axios from "axios";
import {toast} from "react-toastify";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faEdit} from "@fortawesome/free-solid-svg-icons";

export default function EditUser({user, onUserUpdated}) {
  const [showModal, setShowModal] = useState(false);
  const [username, setUsername] = useState("");
  const [phoneNumber, setPhoneNumber] = useState("");
  const [email, setEmail] = useState("");
  const [address, setAddress] = useState("");
  const [role, setRole] = useState("");
  const [password, setPassword] = useState("");
  const [ratings, setRatings] = useState("");
  const [status, setStatus] = useState(0); // Default status
  const [errors, setErrors] = useState([]);

  // Function to show the modal and initialize fields
  const handleShow = () => {
    setUsername(user.username || "");
    setPhoneNumber(user.phoneNumber || "");
    setEmail(user.email || "");
    setAddress(user.address || "");
    setRole(user.role || "");
    setPassword(""); // Don't pre-fill password field for security reasons
    setRatings(user.ratings || "");
    setStatus(user.status || 0); // Default to Accept if undefined
    setShowModal(true);
  };

  const handleClose = () => {
    setShowModal(false);
    setErrors([]);
  };

  const handleSubmit = (e) => {
    e.preventDefault();

    // Prepare a JSON Patch array to hold the operations
    const patchData = [];

    // Check and push only changed fields to the patchData
    if (username !== user.username) {
      patchData.push({op: "replace", path: "/Username", value: username});
    }

    if (phoneNumber !== user.phoneNumber) {
      patchData.push({
        op: "replace",
        path: "/PhoneNumber",
        value: parseInt(phoneNumber),
      });
    }

    if (email !== user.email) {
      patchData.push({op: "replace", path: "/Email", value: email});
    }

    if (address !== user.address) {
      patchData.push({op: "replace", path: "/Address", value: address});
    }

    if (role !== user.role) {
      patchData.push({op: "replace", path: "/Role", value: role});
    }

    // Password should be updated only if provided (and not pre-filled for security reasons)
    if (password) {
      patchData.push({op: "replace", path: "/Password", value: password});
    }

    if (ratings !== user.ratings) {
      patchData.push({op: "replace", path: "/Ratings", value: ratings});
    }

    if (status !== user.status) {
      patchData.push({op: "replace", path: "/Status", value: parseInt(status)});
    }

    // Clear any previous errors
    setErrors([]);

    // Make the PATCH request using axios
    axios
      .patch(`${process.env.REACT_APP_WEB_API}/Users/${user.userId}`, patchData)
      .then((response) => {
        console.log("User updated successfully:", response.data);
        toast.success("User updated successfully!");
        handleClose();

        if (onUserUpdated) {
          onUserUpdated(response.data.user);
        }
      })
      .catch((error) => {
        console.error("There was an error updating the user!", error);
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
                <h5 className="modal-title">Edit User</h5>
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
                        <label className="form-label" htmlFor="username">
                          Username
                        </label>
                        <input
                          type="text"
                          id="username"
                          className="form-control"
                          placeholder="Enter username"
                          value={username}
                          onChange={(e) => setUsername(e.target.value)}
                          required
                        />
                      </div>

                      <div className="form-outline mb-4">
                        <label className="form-label" htmlFor="phoneNumber">
                          Phone Number
                        </label>
                        <input
                          type="number"
                          id="phoneNumber"
                          className="form-control"
                          placeholder="Enter phone number"
                          value={phoneNumber}
                          onChange={(e) => setPhoneNumber(e.target.value)}
                          required
                        />
                      </div>

                      <div className="form-outline mb-4">
                        <label className="form-label" htmlFor="email">
                          Email
                        </label>
                        <input
                          type="email"
                          id="email"
                          className="form-control"
                          placeholder="Enter email"
                          value={email}
                          onChange={(e) => setEmail(e.target.value)}
                          required
                        />
                      </div>
                    </div>

                    <div className="col-md-6">
                      <div className="form-outline mb-4">
                        <label className="form-label" htmlFor="address">
                          Address
                        </label>
                        <input
                          type="text"
                          id="address"
                          className="form-control"
                          placeholder="Enter address"
                          value={address}
                          onChange={(e) => setAddress(e.target.value)}
                          required
                        />
                      </div>

                      <div className="form-outline mb-4">
                        <label className="form-label" htmlFor="role">
                          Role
                        </label>
                        <select
                          id="role"
                          className="form-control"
                          value={role}
                          onChange={(e) => setRole(e.target.value)}
                          required
                        >
                          <option value={"Admin"}>Admin</option>
                          <option value={"CRS"}>CRS</option>
                          <option value={"Vendor"}>Vendor</option>
                          <option value={"Customer"}>Customer</option>
                        </select>
                      </div>

                      <div className="form-outline mb-4">
                        <label className="form-label" htmlFor="password">
                          Password
                        </label>
                        <input
                          type="password"
                          id="password"
                          className="form-control"
                          placeholder="Enter new password again for veriification"
                          value={password}
                          onChange={(e) => setPassword(e.target.value)}
                        />
                      </div>

                      <div className="form-outline mb-4">
                        <label className="form-label" htmlFor="ratings">
                          Ratings (optional)
                        </label>
                        <input
                          type="text"
                          id="ratings"
                          className="form-control"
                          placeholder="Enter ratings"
                          value={ratings}
                          onChange={(e) => setRatings(e.target.value)}
                        />
                      </div>

                      <div className="form-outline mb-4">
                        <label className="form-label" htmlFor="status">
                          Status
                        </label>
                        <select
                          id="status"
                          className="form-control"
                          value={status}
                          onChange={(e) => setStatus(parseInt(e.target.value))}
                        >
                          <option value={0}>Accept</option>
                          <option value={1}>Reject</option>
                          <option value={2}>Hold</option>
                          <option value={3}>Pending</option>
                        </select>
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
