import { useState } from "react";
import "./App.css";
import MainLayout from "./components/layout/MainLayout";
import AdminLayout from "./components/layout/AdminLayout";
import CartContextProvider from "./components/providers/ContextProvider";
import { ToastContainer } from "react-toastify";
import { AuthProvider } from "./context/authContext";

function App() {
  const [layout, setLayout] = useState(false);

  return (
    <AuthProvider>
      <CartContextProvider>
        <MainLayout />
        <ToastContainer />
      </CartContextProvider>
    </AuthProvider>
  );
}

export default App;
