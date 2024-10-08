import React, { useState } from "react";
import { Form, Button, Container, Row, Col } from "react-bootstrap";
import { toast } from "react-toastify";
import "react-toastify/ReactToastify.css";
import VendorService from "../../../APIService/VendorService";
import l from "../../assets/l.png";

const VendorRegistration = () => {
  const [formData, setFormData] = useState({
    vendorName: "",
    vendorEmail: "",
    vendorPhone: "",
    vendorAddress: "",
    vendorCity: "",
  });
  const [error, setError] = useState(null);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData((prevData) => ({
      ...prevData,
      [name]: value,
    }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    try {
      const response = await VendorService.addVendor(formData);
      if (response.status === 200 || response.status === 204) {
        toast.success("Vendor updated successfully!");
      }
      setFormData({
        vendorName: "",
        vendorEmail: "",
        vendorPhone: "",
        vendorAddress: "",
        vendorCity: "",
      });
    } catch (error) {
      setError("Error registering...");
      console.log("Error occured when registration", error);
    }
  };

  return (
    <div>
      <Container>
        <Row className="d-flex justify-content-center mt-5">
          <Col xs={12} className="text-center mb-4"></Col>
        </Row>

        <Row className="d-flex justify-content-center">
          <Col
            xs={10}
            md={6}
            lg={5}
            className="d-flex justify-content-center align-items-center"
          >
            <img
              src={l}
              alt="Vendor"
              className="img-fluid"
              style={{
                width: "100%",
                height: "auto",
              }}
            />
          </Col>

          <Col xs={12} md={12} lg={6}>
            <h3 className="text-center mb-3">Vendor Registeration</h3>
            <Form onSubmit={handleSubmit}>
              <Form.Group className="mb-3" controlId="formVendorName">
                <Form.Label>First Name</Form.Label>
                <Form.Control
                  type="text"
                  name="vendorName"
                  placeholder="Enter name"
                  value={formData.vendorName}
                  onChange={handleChange}
                  required
                />
              </Form.Group>

              <Form.Group className="mb-3" controlId="formVendorEmail">
                <Form.Label>Email address</Form.Label>
                <Form.Control
                  type="email"
                  name="vendorEmail"
                  placeholder="Enter email"
                  value={formData.vendorEmail}
                  onChange={handleChange}
                  required
                />
              </Form.Group>

              <Form.Group className="mb-3" controlId="formVendorPhone">
                <Form.Label>Phone</Form.Label>
                <Form.Control
                  type="tel"
                  name="vendorPhone"
                  placeholder="Enter phone number"
                  value={formData.vendorPhone}
                  onChange={handleChange}
                  required
                />
              </Form.Group>

              <Form.Group className="mb-3" controlId="formVendorAddress">
                <Form.Label>Address</Form.Label>
                <Form.Control
                  type="text"
                  name="vendorAddress"
                  placeholder="Enter address"
                  value={formData.vendorAddress}
                  onChange={handleChange}
                  required
                />
              </Form.Group>

              <Form.Group className="mb-3" controlId="formVendorCity">
                <Form.Label>City</Form.Label>
                <Form.Control
                  type="text"
                  name="vendorCity"
                  placeholder="Enter city"
                  value={formData.vendorCity}
                  onChange={handleChange}
                  required
                />
              </Form.Group>

              <Button variant="primary" type="submit" className="w-100">
                Register
              </Button>
            </Form>

            <Row className="d-flex justify-content-center mt-1">
              <Col xs={12} className="text-center">
                <span
                  className="bi bi-info-circle"
                  style={{
                    color: "#007bff",
                    fontSize: "1.5rem",
                    marginRight: "8px",
                  }}
                ></span>
                <span style={{ fontSize: "1rem", color: "#007bff" }}>
                  After admin approval, you will receive an email with access
                  details.
                </span>
              </Col>
            </Row>
          </Col>
        </Row>
      </Container>
    </div>
  );
};

export default VendorRegistration;
