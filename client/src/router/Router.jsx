import React from "react";
import { Route, Routes, Navigate } from "react-router-dom";
import { useAuth } from "../context/authContext";

// Import all your components here
import Order from "../pages/Order";
import ProductList from "../pages/ProductList";
import ProductCart from "../pages/ProductCart";
import UserManagement from "../pages/dashboradPages/UserManagement";
import ProductManagement from "../pages/dashboradPages/ProdcutManagement";
import ProdcutAdd from "../pages/dashboradPages/ProductAdd";
import ProductUpdate from "../pages/dashboradPages/ProductUpdate";
import AdminDashboard from "../pages/AdminDashboard";
import VendorManagement from "../pages/dashboradPages/VendorManagement";
import UpdateVendor from "../pages/dashboradPages/UpdateVendor";
import AllOrder from "../pages/AllOrder";
import OrderCancelationRequest from "../pages/OrderCancelationRequest";
import CSRDashboard from "../pages/dashboradPages/csrDashboard";
import VendorDashboard from "../pages/dashboradPages/vendorDashboard";
import VendorOrders from "../pages/dashboradPages/VendorOrders";
import ProductListingDashboard from "../pages/dashboradPages/ProductListingDashboard";
import VendorRegistration from "../pages/dashboradPages/VendorRegistration";
import VendorLogin from "../pages/dashboradPages/VendorLogin";
import LoginForm from "../pages/public/login";
import RegisterForm from "../pages/public/register";
import CustomerFeedbacks from "../pages/dashboradPages/CustomerFeedbacks";
import VendorAccountManagements from "../pages/dashboradPages/VendorAccountManagements";

const Router = () => {
  const { user, isLoading } = useAuth();

  const ProtectedRoute = ({ children }) => {
    if (isLoading) {
      // You can return a loading spinner or null here
      return <div>Loading...</div>;
    }
    if (!user) {
      return <Navigate to="/login" replace />;
    }
    return children;
  };

  return (
    <Routes>
      {/* Public routes */}
      <Route path="/login" element={<LoginForm />} />
      <Route path="/register" element={<RegisterForm />} />
      <Route path="/vendor/register" element={<VendorRegistration />} />
      <Route path="/vendor/login" element={<VendorLogin />} />

      {/* Protected routes */}
      <Route
        path="/"
        element={
          <ProtectedRoute>
            <ProductList />
          </ProtectedRoute>
        }
      />
      <Route
        path="/cart"
        element={
          <ProtectedRoute>
            <ProductCart />
          </ProtectedRoute>
        }
      />
      <Route
        path="/orders"
        element={
          <ProtectedRoute>
            <Order />
          </ProtectedRoute>
        }
      />
      <Route
        path="/users"
        element={
          <ProtectedRoute>
            <UserManagement />
          </ProtectedRoute>
        }
      />

      {/* Dashboard routes */}
      <Route
        path="dashboard/allproducts"
        element={
          <ProtectedRoute>
            <ProductManagement />
          </ProtectedRoute>
        }
      />
      <Route
        path="dashboard/addproduct"
        element={
          <ProtectedRoute>
            <ProdcutAdd />
          </ProtectedRoute>
        }
      />
      <Route
        path="dashboard/updateproduct/:id"
        element={
          <ProtectedRoute>
            <ProductUpdate />
          </ProtectedRoute>
        }
      />
      <Route
        path="adminDashboard"
        element={
          <ProtectedRoute>
            <AdminDashboard />
          </ProtectedRoute>
        }
      />
      <Route
        path="dashboard/vendors"
        element={
          <ProtectedRoute>
            <VendorManagement />
          </ProtectedRoute>
        }
      />
      <Route
        path="dashboard/updatevendor/:id"
        element={
          <ProtectedRoute>
            <UpdateVendor />
          </ProtectedRoute>
        }
      />
      <Route
        path="allOrders"
        element={
          <ProtectedRoute>
            <AllOrder />
          </ProtectedRoute>
        }
      />
      <Route
        path="cancelRequest"
        element={
          <ProtectedRoute>
            <OrderCancelationRequest />
          </ProtectedRoute>
        }
      />
      <Route
        path="csr"
        element={
          <ProtectedRoute>
            <CSRDashboard />
          </ProtectedRoute>
        }
      />
      <Route
        path="vendor"
        element={
          <ProtectedRoute>
            <VendorDashboard />
          </ProtectedRoute>
        }
      />
      <Route
        path="vendorOrder"
        element={
          <ProtectedRoute>
            <VendorOrders />
          </ProtectedRoute>
        }
      />
      <Route
        path="products"
        element={
          <ProtectedRoute>
            <ProductManagement />
          </ProtectedRoute>
        }
      />
      <Route
        path="manage-products"
        element={
          <ProtectedRoute>
            <ProductListingDashboard />
          </ProtectedRoute>
        }
      />
      <Route
        path="/feedbacks"
        element={
          <ProtectedRoute>
            <CustomerFeedbacks />
          </ProtectedRoute>
        }
      />

      <Route
        path="approveVendor"
        element={
          <ProtectedRoute>
            <VendorAccountManagements />
          </ProtectedRoute>
        }
      />
    </Routes>
  );
};

export default Router;
