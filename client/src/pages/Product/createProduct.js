import React, { useState, useEffect } from "react";
import { Form, Button, Col, Row } from "react-bootstrap";
import axios from "axios";
import "../../styles/AddProductPage.css"; // Import custom styles
import Header from "../../components/Header";
import Footer from "../../components/Footer";
import { useNavigate } from "react-router-dom";

const AddProductPage = () => {
  const navigate = useNavigate();
  const [imageBase64, setImageBase64] = useState(""); // Store Base64 image
  const [formData, setFormData] = useState({
    productName: "",
    productDescription: "",
    price: "",
    stock: "",
    categoryId: "",
    vendorId: "",
  });
  const [categories, setCategories] = useState([]);
  const [vendors, setVendors] = useState([]);

  useEffect(() => {
    // Fetch categories when component mounts
    const fetchCategories = async () => {
      try {
        const response = await axios.get(
          `${process.env.REACT_APP_WEB_API}/Category`
        );
        setCategories(response.data);
      } catch (error) {
        console.error("Error fetching categories:", error);
      }
    };

    // Fetch vendors (filter users with the role 'Vendor')
    const fetchVendors = async () => {
      try {
        const response = await axios.get(
          `${process.env.REACT_APP_WEB_API}/Users`
        );
        const vendorsData = response.data.filter(
          (user) => user.role === "Vendor"
        );
        setVendors(vendorsData);
      } catch (error) {
        console.error("Error fetching vendors:", error);
      }
    };

    fetchCategories();
    fetchVendors();
  }, []);

  const handleImageUpload = (e) => {
    const file = e.target.files[0];
    if (file) {
      const reader = new FileReader();
      reader.onloadend = () => {
        setImageBase64(reader.result); // Store Base64 image
      };
      reader.readAsDataURL(file); // Convert image to Base64
    }
  };

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData({ ...formData, [name]: value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    // Prepare the form data with the Base64 image
    const productData = {
      ...formData,
      image: imageBase64, // Base64 image string
    };

    try {
      const response = await axios.post(
        `${process.env.REACT_APP_WEB_API}/Products`,
        productData,
        {
          headers: {
            "Content-Type": "application/json",
          },
        }
      );

      alert("Product created successfully!");
      console.log(response.data);
      navigate("/products");
    } catch (error) {
      console.error("Error creating product:", error);
      alert("Failed to create product.");
    }
  };

  return (
    <>
      <Header />
      <div className="m-20 add-product-container">
        <h2 className="mb-4">Add New Product</h2>
        <Form onSubmit={handleSubmit}>
          <Row>
            {/* Left Side: Thumbnail and Categories */}
            <Col md={4}>
              {/* Thumbnail Section */}
              <div className="thumbnail-section">
                <h5>Thumbnail</h5>
                <div className="thumbnail-upload">
                  {imageBase64 ? (
                    <img
                      src={imageBase64}
                      alt="Thumbnail Preview"
                      className="thumbnail-preview"
                    />
                  ) : (
                    <label
                      htmlFor="thumbnailInput"
                      className="thumbnail-placeholder"
                    >
                      Click to upload an image
                    </label>
                  )}
                  <input
                    type="file"
                    id="thumbnailInput"
                    className="d-none"
                    accept="image/*"
                    onChange={handleImageUpload}
                  />
                </div>
              </div>

              {/* Categories Section */}
              <div className="mt-4 categories-section">
                <h5>Categories</h5>
                <Form.Group controlId="categorySelect">
                  <Form.Label>Add product to a category</Form.Label>
                  <Form.Control
                    as="select"
                    name="categoryId" // Use 'categoryId' instead of 'category'
                    onChange={handleChange}
                    required
                  >
                    <option value="">Select a category</option>
                    {categories.map((cat) => (
                      <option key={cat.categoryId} value={cat.categoryId}>
                        {cat.categoryName}
                      </option>
                    ))}
                  </Form.Control>
                </Form.Group>
              </div>
            </Col>

            {/* Right Side: General, Pricing, and Vendor */}
            <Col md={8}>
              {/* General Section */}
              <div className="general-section">
                <h5>General</h5>
                <Form.Group controlId="productName">
                  <Form.Label>Product Name</Form.Label>
                  <Form.Control
                    type="text"
                    name="productName"
                    placeholder="Enter product name"
                    value={formData.productName}
                    onChange={handleChange}
                    required
                  />
                </Form.Group>

                <Form.Group controlId="description">
                  <Form.Label>Description</Form.Label>
                  <Form.Control
                    as="textarea"
                    name="productDescription"
                    rows={4}
                    placeholder="Enter product description"
                    value={formData.productDescription}
                    onChange={handleChange}
                  />
                </Form.Group>
              </div>

              {/* Pricing Section */}
              <div className="mt-4 pricing-section">
                <h5>Pricing</h5>
                <Form.Group controlId="price">
                  <Form.Label>Price</Form.Label>
                  <Form.Control
                    type="number"
                    name="price"
                    placeholder="Enter product price"
                    value={formData.price}
                    onChange={handleChange}
                    required
                  />
                </Form.Group>

                <Form.Group controlId="stock">
                  <Form.Label>Stock</Form.Label>
                  <Form.Control
                    type="number"
                    name="stock"
                    placeholder="Enter available stock"
                    value={formData.stock}
                    onChange={handleChange}
                    required
                  />
                </Form.Group>
              </div>

              {/* Vendor Section */}
              <div className="mt-4 vendor-section">
                <h5>Vendor Details</h5>
                <Form.Group controlId="vendorSelect">
                  <Form.Label>Select Vendor</Form.Label>
                  <Form.Control
                    as="select"
                    name="vendorId"
                    onChange={handleChange}
                    required
                  >
                    <option value="">Select a vendor</option>
                    {vendors.map((vendor) => (
                      <option key={vendor.userId} value={vendor.userId}>
                        {vendor.username}
                      </option>
                    ))}
                  </Form.Control>
                </Form.Group>
              </div>

              <Button type="submit" variant="success" className="mt-4">
                Save Product
              </Button>
            </Col>
          </Row>
        </Form>
      </div>
      <Footer />
    </>
  );
};

export default AddProductPage;
