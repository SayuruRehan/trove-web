import React, { useEffect, useState } from "react";
import { useCartContext } from "../components/providers/ContextProvider";
import ProductCard from "../components/ProductCard";
import p from "../assets/p.jpg";
import ProductService from "../../APIService/ProductService";

const ProductList = () => {
  const { addToCart } = useCartContext();
  const [searchQuery, setSearchQuery] = useState("");
  const [filteredProducts, setFilteredProducts] = useState([]);
  const [Products, setProducts] = useState([]);
  console.log(Products)
  const handleSearchInputChange = (e) => {
    const query = e.target.value.toLowerCase();
    setSearchQuery(query);

    const filtered = Products?.filter((product) =>
      product.productName.toLowerCase().includes(query)
    );
    setFilteredProducts(filtered);
  };

  const handleItemCart = (product) => {
    addToCart(product);
  };

  const fetchAllProducts = async () => {
    const response = await ProductService.getAllProducts();
    const productsWithDefaultImage = response.data.map((product) => ({
      ...product,
    }));
    setProducts(productsWithDefaultImage);
  };

  useEffect(() => {
    fetchAllProducts();
  }, []);

  useEffect(() => {
    setFilteredProducts(Products);
  }, [Products]);

  return (
    <div>
      <div className="container mt-5">
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
      <div className="container">
        <div className="row justify-content-center">
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
            <div className="text-center">Loading products...</div>
          ) : (
            <div className="text-center">No products found.</div>
          )}
        </div>
      </div>
    </div>
  );
};

export default ProductList;
