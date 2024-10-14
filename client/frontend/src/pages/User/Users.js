import React, {useEffect, useState} from "react";
import Footer from "../../components/Footer";
import Header from "../../components/Header";
import CreateUser from "./CreateUser";
import EditUser from "./EditUser";
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

export default function Users() {
  const [users, setUsers] = useState([]);
  const [isDataLoaded, setIsDataLoaded] = useState(false);
  const [loading, setLoading] = useState(true);
  const userRole = sessionStorage.getItem("role"); // Retrieve the logged-in user's role

  useEffect(() => {
    axios
      .get(`${process.env.REACT_APP_WEB_API}/Users`)
      .then((response) => {
        setUsers(response.data);
        setIsDataLoaded(true);
      })
      .catch((error) => {
        console.error("Error fetching users:", error);
        toast.error("Error fetching users.");
      });
    setLoading(false);
    return () => {
      if ($.fn.DataTable.isDataTable("#userTable")) {
        $("#userTable").DataTable().destroy(true);
      }
    };
  }, []);

  useEffect(() => {
    if (isDataLoaded) {
      $("#userTable").DataTable();
    }
  }, [isDataLoaded]);

  const deleteUser = (userId) => {
    axios
      .delete(`${process.env.REACT_APP_WEB_API}/Users/${userId}`)
      .then((response) => {
        setUsers(users.filter((user) => user.userId !== userId));
        toast.success("User deleted successfully!");
      })
      .catch((error) => {
        console.error("Error deleting user:", error);
        toast.error("Error deleting user.");
      });
  };

  const handleUserCreated = (newUser) => {
    setUsers([...users, newUser]);
  };

  const handleUserUpdated = (updatedUser) => {
    const updatedUsers = users.map((user) =>
      user.userId === updatedUser.userId ? updatedUser : user
    );
    setUsers(updatedUsers);
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
          <h2>User List</h2>
        </div>

        <div className="d-flex justify-content-end mb-3">
          <CreateUser onOrderCreated={handleUserCreated} />
        </div>
        <table
          id="userTable"
          className="table table-striped table-bordered"
          style={{width: "100%"}}
        >
          <thead>
            <tr>
              <th>User ID</th>
              <th>Name</th>
              <th>Email</th>
              <th>Phone Number</th>
              <th>Role</th>
              <th>Status</th>
              <th>Actions</th>
            </tr>
          </thead>
          <tbody>
            {users.length > 0 ? (
              users.map((user) => (
                <tr key={user.userId}>
                  <td>{user.userId}</td>
                  <td>{user.username}</td>
                  <td>{user.email}</td>
                  <td>{user.phoneNumber}</td>
                  <td>{user.role}</td>
                  <td>
                    {user.status === 0
                      ? "Accept"
                      : user.status === 1
                      ? "Reject"
                      : user.status === 2
                      ? "Hold"
                      : user.status === 3
                      ? "Pending"
                      : "Pending"}
                  </td>
                  <td>
                    {/* Conditionally hide Edit/Delete for CRS role viewing Admin */}
                    {!(userRole === "CRS" && user.role === "Admin") && (
                      <>
                        <EditUser
                          user={user}
                          onUserUpdated={handleUserUpdated}
                        />
                        <button
                          className="btn btn-danger btn-sm"
                          onClick={() => deleteUser(user.userId)}
                        >
                          <FontAwesomeIcon icon={faTrash} />
                        </button>
                      </>
                    )}
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
