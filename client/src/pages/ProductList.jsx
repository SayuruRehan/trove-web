import React, { useEffect, useState } from "react";
import { useCartContext } from "../components/providers/ContextProvider";
import ProductCard from "../components/ProductCard";
import ProductService from "../../APIService/ProductService";

const ProductList = () => {
  const { addToCart } = useCartContext();
  const [searchQuery, setSearchQuery] = useState("");
  const [filteredProducts, setFilteredProducts] = useState([]);
  const [Products, setProducts] = useState([]);

  const handleSearchInputChange = (e) => {
    const query = e.target.value.toLowerCase();
    setSearchQuery(query);

    const regex = new RegExp(query, "i");

    const filtered = Products?.filter((product) =>
      regex.test(product.productName)
    );
    setFilteredProducts(filtered);
  };

  const handleItemCart = (product) => {
    addToCart(product);
  };

  const fetchAllProducts = async () => {
    const response = await ProductService.getAllProducts();
    setProducts(response.data);
  };

  useEffect(() => {
    fetchAllProducts();
  }, []);

  useEffect(() => {
    setFilteredProducts(Products);
  }, [Products]);

  return (
    <div className="container-fluid ">
      <div className="container-fluid mt-5">
        {/* container-fluid for full width */}
        <div className="row mb-4">
          <div className="col-md-12">
            <input
              type="text"
              className="form-control"
              placeholder="Search products by name..."
              value={searchQuery}
              onChange={handleSearchInputChange}
            />
          </div>
        </div>
      </div>

      <div className="container-fluid">
        {/* container-fluid to ensure full width */}
        <div
          className="row justify-content-start w-100"
          style={{ minHeight: "50vh" }} // Set minimum height to prevent shrinking
        >
          {filteredProducts.length > 0 ? (
            filteredProducts.map((product) => (
              <div
                className="col-lg-3 col-md-4 col-sm-6 col-12 d-flex justify-content-center mb-4"
                key={product.id}
              >
                <ProductCard product={product} onAddToCart={handleItemCart} />
              </div>
            ))
          ) : Products.length === 0 ? (
            <div className="text-center w-100">Loading products...</div>
          ) : (
            <div className="text-center w-100">No products found.</div>
          )}
        </div>
      </div>
    </div>
  );
};

export default ProductList;
