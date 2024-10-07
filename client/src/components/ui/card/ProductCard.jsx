import React, { useEffect } from "react";
import { useState } from "react";
import "bootstrap/dist/css/bootstrap.min.css";
// import p from "../../../assets/p.jpg";

const Products = () => {
  const [productData, setProductData] = useState([
    { productName: "Circuit", productPrice: 500 },
    { productName: "IOT device", productPrice: 1000 },
  ]);
  const [searchQuery, setSearchQuey] = useState("");

  // useEffect(() => {
  //   try {
  //     setProductData();
  //   } catch (error) {
  //     console.log("Error fetching product data", error);
  //   }
  // }, []);

  const filterProduct = productData.filter((procudtItem) => {
    const { productName, productPrice } = procudtItem;
    const prodPrice = productPrice.toString();
    const lowerCaseQuery = searchQuery.toLowerCase();

    return (
      productName.toLowerCase().includes(lowerCaseQuery) ||
      productPrice.includes(lowerCaseQuery)
    );
  });

  return (
    <div>
      <div className="container mt-5">
        <div className="row mb-4">
          <div className="col-md-6 offset-md-3">
            <input
              type="text"
              className="form-control"
              placeholder="Search products..."
              value=""
              onChange={() => filterProduct()}
            />
          </div>
        </div>
      </div>
      <div>
        {productData.map((prData, index) => {
          return (
            <div className="col-md-4 m-4" key={index}>
              <div
                className="card shadow-sm p-2 mb-5 bg-white rounded "
                style={{ width: "250px" }}
              >
                <h5 className="card-title text-center">{prData}</h5>
                <img
                  src={p}
                  className="card-img-top mx-auto d-block"
                  alt="product_img"
                  style={{ width: "150px", height: "150px" }}
                />
                <div className="card-body text-center">
                  <h5 className="">Price: Rs: {prData}</h5>
                  <button className=" btn btn-primary">Add To Cart</button>
                </div>
              </div>
            </div>
          );
        })}
        ;
      </div>
    </div>
  );
};
export default Products;
