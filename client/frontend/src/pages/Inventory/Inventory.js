import React, {useEffect, useState} from "react";
import $ from "jquery";
import "bootstrap/dist/css/bootstrap.min.css";
import "datatables.net-bs4/css/dataTables.bootstrap4.min.css";
import dt from "datatables.net-bs4";
import {Modal, Button} from "react-bootstrap";
import axios from "axios";
import Header from "../../components/Header";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {
  faEdit,
  faTrash,
  faEye,
  faPlus,
} from "@fortawesome/free-solid-svg-icons";
import {Link} from "react-router-dom";
import Footer from "../../components/Footer";
import ClipLoader from "react-spinners/ClipLoader";

const Inventory = () => {
  const [orders, setOrders] = useState([]);
  const [products, setProducts] = useState([]);
  const [categories, setCategories] = useState({});
  const [selectedProduct, setSelectedProduct] = useState(null);
  const [showModal, setShowModal] = useState(false);
  const [modalType, setModalType] = useState("");
  const [imageBase64, setImageBase64] = useState("");
  const [allCategories, setAllCategories] = useState([]);
  const [loading, setLoading] = useState(true);

  // Fetch all products
  const fetchProducts = async () => {
    try {
      const response = await axios.get(
        `${process.env.REACT_APP_WEB_API}/Products`
      );
      const productsData = response.data;

      // Fetch categories for each product
      const categoriesData = {};
      await Promise.all(
        productsData.map(async (product) => {
          if (!categoriesData[product.categoryId]) {
            const categoryResponse = await axios.get(
              `${process.env.REACT_APP_WEB_API}/Category/${product.categoryId}`
            );
            categoriesData[product.categoryId] =
              categoryResponse.data.categoryName;
          }
        })
      );

      setCategories(categoriesData);
      setProducts(productsData);
      setLoading(false);
    } catch (error) {
      console.error("Error fetching products:", error);
    }
  };

  const fetchAllCategories = async () => {
    try {
      const response = await axios.get(
        `${process.env.REACT_APP_WEB_API}/Category`
      );
      setAllCategories(response.data);
    } catch (error) {
      console.error("Error fetching categories:", error);
    }
  };

  useEffect(() => {
    fetchProducts();
    fetchAllCategories();

    return () => {
      if ($.fn.DataTable.isDataTable("#productsTable")) {
        $("#productsTable").DataTable().destroy(true); // Destroy previous instance before reinitializing
      }
    };
  }, []);

  useEffect(() => {
    if (products.length > 0) {
      $("#productsTable").DataTable(); // Initialize DataTables with Bootstrap styling
    }
  }, [products]); // Only initialize DataTables after products are loaded

  const handleInventoryCreated = (newOrder) => {
    setOrders([...orders, newOrder]);
  };

  const handleShowModal = (type, product) => {
    setSelectedProduct(product);
    setModalType(type);
    setImageBase64(""); // Clear the image base64 when opening the modal
    setShowModal(true);
  };

  const handleCloseModal = () => {
    setShowModal(false);
  };

  const handleImageUpload = (e) => {
    const file = e.target.files[0];
    if (file) {
      const reader = new FileReader();
      reader.onloadend = () => {
        setImageBase64(reader.result);
      };
      reader.readAsDataURL(file);
    }
  };

  const handleEdit = async () => {
    const updatedProduct = {
      ...selectedProduct,
      productName: document.querySelector('input[name="productName"]').value,
      productDescription: document.querySelector(
        'textarea[name="productDescription"]'
      ).value,
      price: document.querySelector('input[name="price"]').value,
      stock: document.querySelector('input[name="stock"]').value,
      categoryId: document.querySelector('select[name="category"]').value,
      image: imageBase64 || selectedProduct.image,
    };

    try {
      await axios.put(
        `${process.env.REACT_APP_WEB_API}/Products/${selectedProduct.productId}`,
        updatedProduct
      );
      setProducts(
        products.map((product) =>
          product.productId === selectedProduct.productId
            ? updatedProduct
            : product
        )
      );
      handleCloseModal();
      alert("Product updated successfully.");
    } catch (error) {
      console.error("Error updating product:", error);
      alert("An error occurred while updating the product.");
    }
  };

  const handleDelete = async () => {
    if (!selectedProduct) return;

    try {
      await axios.delete(
        `${process.env.REACT_APP_WEB_API}/Products/${selectedProduct.productId}`
      );
      setProducts(
        products.filter(
          (product) => product.productId !== selectedProduct.productId
        )
      );
      handleCloseModal();
      alert("Product deleted successfully.");
    } catch (error) {
      console.error("Error deleting product:", error);
      alert("An error occurred while deleting the product.");
    }
  };

  return (
    <>
      <Header />
      <div className="ml-10 mr-10 mb-10">
        <div className="flex justify-center">
          <h2>Inventory</h2>
        </div>
        <Link
          to="/create-inventory"
          className="d-flex justify-content-end mb-3"
        >
          <button className="btn btn-primary">
            <FontAwesomeIcon className="mr-2" icon={faPlus} />
            Add Item
          </button>
        </Link>

        <table
          id="productsTable"
          className="table table-striped table-bordered"
          style={{width: "100%"}}
        >
          <thead>
            <tr>
              <th>#</th>
              <th>Product Name</th>
              <th>Image</th>
              <th style={{width: "20%"}}>Description</th>
              <th>Price</th>
              <th>Stock</th>
              <th>Category</th>
              <th>Actions</th>
            </tr>
          </thead>
          <tbody>
            {products.length > 0 ? (
              products.map((product, index) => (
                <tr key={product.productId}>
                  <td>{index + 1}</td>
                  <td>{product.productName}</td>
                  <td>
                    <img
                      src={product.image}
                      alt={product.productName}
                      style={{width: "50px"}}
                    />
                  </td>
                  <td>
                    {product.productDescription.length > 50
                      ? product.productDescription.substring(0, 50) + "..."
                      : product.productDescription}
                  </td>
                  <td>{product.price}</td>
                  <td>{product.stock}</td>
                  <td>{categories[product.categoryId] || "Loading..."}</td>
                  <td>
                    <button
                      className="mx-1 btn btn-info btn-sm"
                      onClick={() => handleShowModal("view", product)}
                    >
                      <FontAwesomeIcon icon={faEye} />
                    </button>
                    <button
                      className="mx-1 btn btn-warning btn-sm"
                      onClick={() => handleShowModal("edit", product)}
                    >
                      <FontAwesomeIcon icon={faEdit} />
                    </button>
                    <button
                      className="mx-1 btn btn-danger btn-sm"
                      onClick={() => handleShowModal("delete", product)}
                    >
                      <FontAwesomeIcon icon={faTrash} />
                    </button>
                  </td>
                </tr>
              ))
            ) : (
              <tr>
                <td colSpan="7" className="text-center">
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

      {/* Modal */}
      <Modal show={showModal} onHide={handleCloseModal}>
        <Modal.Header closeButton>
          <Modal.Title>
            {modalType === "view"
              ? "View Product"
              : modalType === "edit"
              ? "Edit Product"
              : "Delete Product"}
          </Modal.Title>
        </Modal.Header>
        <Modal.Body>
          {modalType === "view" && selectedProduct && (
            <>
              <img
                src={selectedProduct.image}
                alt={selectedProduct.productName}
                style={{
                  width: "50%",
                  height: "50%",
                  marginBottom: "30px",
                  display: "block",
                  margin: "0 auto",
                }}
              />
              <p>
                <strong>Name:</strong> {selectedProduct.productName}
              </p>
              <p>
                <strong>Description:</strong>{" "}
                {selectedProduct.productDescription}
              </p>
              <p>
                <strong>Price:</strong> {selectedProduct.price}
              </p>
              <p>
                <strong>Stock:</strong> {selectedProduct.stock}
              </p>
              <p>
                <strong>Category:</strong>{" "}
                {categories[selectedProduct.categoryId]}
              </p>
            </>
          )}
          {modalType === "edit" && selectedProduct && (
            <form>
              <img
                src={imageBase64 || selectedProduct.image}
                alt={selectedProduct.productName}
                style={{
                  width: "50%",
                  height: "50%",
                  marginBottom: "20px",
                  display: "block",
                  margin: "0 auto",
                }}
              />
              <div className="form-group">
                <label>Product Name</label>
                <input
                  type="text"
                  name="productName"
                  className="form-control"
                  defaultValue={selectedProduct.productName}
                />
              </div>
              <div className="form-group">
                <label>Description</label>
                <textarea
                  name="productDescription"
                  className="form-control"
                  defaultValue={selectedProduct.productDescription}
                ></textarea>
              </div>
              <div className="form-group">
                <label>Price</label>
                <input
                  type="number"
                  name="price"
                  className="form-control"
                  defaultValue={selectedProduct.price}
                />
              </div>
              <div className="form-group">
                <label>Stock</label>
                <input
                  type="number"
                  name="stock"
                  className="form-control"
                  defaultValue={selectedProduct.stock}
                />
              </div>
              <div className="form-group">
                <label>Category</label>
                <select
                  name="category"
                  className="form-control"
                  defaultValue={selectedProduct.categoryId}
                >
                  {allCategories.map((category) => (
                    <option
                      key={category.categoryId}
                      value={category.categoryId}
                    >
                      {category.categoryName}
                    </option>
                  ))}
                </select>
              </div>
              <div className="form-group">
                <label>Change Image</label>
                <input
                  type="file"
                  accept="image/*"
                  className="form-control"
                  onChange={handleImageUpload}
                />
              </div>
              <Button variant="primary" onClick={handleEdit}>
                Save Changes
              </Button>
            </form>
          )}

          {modalType === "delete" && (
            <p>Are you sure you want to delete this product?</p>
          )}
        </Modal.Body>
        <Modal.Footer>
          <Button variant="secondary" onClick={handleCloseModal}>
            Close
          </Button>
          {modalType === "delete" && (
            <Button variant="danger" onClick={handleDelete}>
              Delete
            </Button>
          )}
        </Modal.Footer>
      </Modal>

      <Footer />
    </>
  );
};

export default Inventory;
