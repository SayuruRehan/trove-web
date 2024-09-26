import React from "react";
import {Navigate} from "react-router-dom";

// Create a protected route component that takes a component and roles allowed as props
const ProtectedRoute = ({children, allowedRoles}) => {
  // Get the role from sessionStorage
  const role = sessionStorage.getItem("role");

  // If there is no role in sessionStorage, it means the user is not logged in, redirect to login
  if (!role) {
    return <Navigate to="/login" />;
  }

  // If the user's role is not in the allowed roles, redirect them to login
  if (!allowedRoles.includes(role)) {
    return <Navigate to="/not-allowed" />;
  }

  // If the user is logged in and has the correct role, render the children (the route's component)
  return children;
};

export default ProtectedRoute;
