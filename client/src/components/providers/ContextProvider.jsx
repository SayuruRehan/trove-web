import { createContext, useContext, useEffect, useState } from "react";
import { ToastContainer, toast } from "react-toastify";

const CartContext = createContext();

export const useCartContext = () => {
  const context = useContext(CartContext);
  if (!context) throw new Error("createContext is missing!");
  return context;
};

const CartContextProvider = ({ children }) => {
  const [cartData, setCartData] = useState([]);
  const [itemCount, setItemCount] = useState(0);

  let localCartData = [];

  const addToCart = (product) => {
    if (localStorage.getItem("localCartData") !== null) {
      localCartData = JSON.parse(localStorage.getItem("localCartData"));

      const checkAlreadyAdded = localCartData.some(
        (item) => item.id === product.id
      );

      if (checkAlreadyAdded) {
        toast.error("Item already added!", {
          autoClose: 150,
          position: "top-right",
        });
        return;
      }
    }

    localCartData.push({ ...product, quantity: 1 });
    setCartData(localCartData);
    localStorage.setItem("localCartData", JSON.stringify(localCartData));

    toast.success("Item added!", {
      autoClose: 150,
      position: "top-right",
    });

    setItemCount(localCartData.length);
  };

  useEffect(() => {
    const storeData = JSON.parse(localStorage.getItem("localCartData"));
    if (storeData && storeData.length > 0) {
      setCartData(JSON.parse(localStorage.getItem("localCartData")));
    }
  }, []);

  return (
    <CartContext.Provider
      value={{ addToCart, itemCount, cartData, setCartData }}
    >
      {children}
    </CartContext.Provider>
  );
};

export default CartContextProvider;
