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
  const userRole = sessionStorage.getItem("role");

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
        .then(() => {
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

  return (
      <>
        <Header />
        <ToastContainer />
        <div className="container mx-auto py-8">
          <div className="flex justify-between items-center mb-6">
            <h2 className="text-2xl font-bold text-gray-700">User List</h2>
            <CreateUser onOrderCreated={handleUserCreated} />
          </div>

          <div className="overflow-x-auto shadow-md rounded-lg">
            <table
                id="userTable"
                className="min-w-full bg-white border border-gray-300"
            >
              <thead className="bg-gray-100">
              <tr>
                <th className="px-4 py-2 text-left text-gray-600">User ID</th>
                <th className="px-4 py-2 text-left text-gray-600">Name</th>
                <th className="px-4 py-2 text-left text-gray-600">Email</th>
                <th className="px-4 py-2 text-left text-gray-600">Phone Number</th>
                <th className="px-4 py-2 text-left text-gray-600">Role</th>
                <th className="px-4 py-2 text-left text-gray-600">Status</th>
                <th className="px-4 py-2 text-left text-gray-600">Actions</th>
              </tr>
              </thead>
              <tbody>
              {users.length > 0 ? (
                  users.map((user) => (
                      <tr
                          key={user.userId}
                          className="border-t hover:bg-gray-50 transition"
                      >
                        <td className="px-4 py-2">{user.userId}</td>
                        <td className="px-4 py-2">{user.username}</td>
                        <td className="px-4 py-2">{user.email}</td>
                        <td className="px-4 py-2">{user.phoneNumber}</td>
                        <td className="px-4 py-2">{user.role}</td>
                        <td className="px-4 py-2">
                      <span
                          className={`px-2 py-1 rounded-full text-sm font-semibold ${
                              user.status === 0
                                  ? "bg-green-200 text-green-800"
                                  : user.status === 1
                                      ? "bg-red-200 text-red-800"
                                      : user.status === 2
                                          ? "bg-yellow-200 text-yellow-800"
                                          : "bg-gray-200 text-gray-800"
                          }`}
                      >
                        {user.status === 0
                            ? "Accept"
                            : user.status === 1
                                ? "Reject"
                                : user.status === 2
                                    ? "Hold"
                                    : "Pending"}
                      </span>
                        </td>
                        <td className="px-4 py-2 flex space-x-2">
                          {!(userRole === "CRS" && user.role === "Admin") && (
                              <>
                                <EditUser
                                    user={user}
                                    onUserUpdated={handleUserUpdated}
                                />
                                <button
                                    className="text-red-600 hover:text-red-800 transition"
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
                    <td colSpan="7" className="text-center py-4 text-gray-500">
                      No data available
                    </td>
                  </tr>
              )}
              </tbody>
            </table>
          </div>

          {loading && (
              <div className="flex justify-center mt-8">
                <ClipLoader color="#000" loading={loading} size={150} />
              </div>
          )}
        </div>
        <Footer />
      </>
  );
}
