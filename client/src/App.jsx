import { useState } from "react";
import "./App.css";
import MainLayout from "./components/layout/MainLayout";
import AdminLayout from "./components/layout/AdminLayout";
import CartContextProvider from "./components/providers/ContextProvider";
import { ToastContainer } from "react-toastify";

function App() {
  const [layout, setLayout] = useState(false);

  return (
    <CartContextProvider>
      <MainLayout />
      <ToastContainer />
    </CartContextProvider>
  );
}

export default App;
