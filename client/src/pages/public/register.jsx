import React, { useState } from "react";
import { Form, Button, Container, Row, Col, Card } from "react-bootstrap";
import AuthService from "../../../APIService/AuthService";
import { validatePassword } from "../../utils/password.validater";
import { useNavigate } from "react-router-dom";
import l from "../../assets/l.png";

const RegisterForm = () => {
  const navigate = useNavigate();
  const [formData, setFormData] = useState({
    firstName: "",
    lastName: "",
    email: "",
    phone: "",
    role: "",
    password: "",
  });

  const [passwordErrors, setPasswordErrors] = useState([]);
  const [isLoading, setIsLoading] = useState(false);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData((prevState) => ({
      ...prevState,
      [name]: value,
    }));

    if (name === "password") {
      setPasswordErrors(validatePassword(value));
    }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setIsLoading(true);
    const errors = validatePassword(formData.password);
    if (errors.length > 0) {
      setPasswordErrors(errors);
      return;
    }
    console.log("Registration attempt with:", formData);

    const response = await AuthService.register(formData);
    if (response.status === 200) {
      navigate("/login");
    }

    setIsLoading(false);
  };

  return (
      <Container className="d-flex flex-column flex-grow-1 justify-content-center align-items-center">
        <Row className="w-100 justify-content-center">
          <Col
              xs={12}
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
          <Col xs={12} md={6} lg={6}>
            <Card className="shadow-lg p-4 rounded" style={{ borderColor: "#8BB334" }}>
              <Card.Body>
                <h2 className="text-center mb-4" style={{ color: "#8BB334" }}>Register</h2>
                <Form onSubmit={handleSubmit}>
                  <Row>
                    <Col xs={12} md={6} className="mb-3">
                      <Form.Group controlId="formFirstName" className="mb-3">
                        <Form.Label style={{ color: "#5D5D64" }}>First Name</Form.Label>
                        <Form.Control
                            type="text"
                            name="firstName"
                            placeholder="Enter first name"
                            value={formData.firstName}
                            onChange={handleChange}
                            required
                            style={{ backgroundColor: "#F4FFE3", borderColor: "#8BB334" }} // Light green background, apple green border
                        />
                      </Form.Group>
                    </Col>
                    <Col xs={12} md={6} className="mb-3">
                      <Form.Group controlId="formLastName" className="mb-3">
                        <Form.Label style={{ color: "#5D5D64" }}>Last Name</Form.Label>
                        <Form.Control
                            type="text"
                            name="lastName"
                            placeholder="Enter last name"
                            value={formData.lastName}
                            onChange={handleChange}
                            required
                            style={{ backgroundColor: "#F4FFE3", borderColor: "#8BB334" }}
                        />
                      </Form.Group>
                    </Col>
                  </Row>

                  <Row>
                    <Col xs={12} md={6} className="mb-3">
                      <Form.Group controlId="formEmail" className="mb-3">
                        <Form.Label style={{ color: "#5D5D64" }}>Email address</Form.Label>
                        <Form.Control
                            type="email"
                            name="email"
                            placeholder="Enter email"
                            value={formData.email}
                            onChange={handleChange}
                            required
                            style={{ backgroundColor: "#F4FFE3", borderColor: "#8BB334" }}
                        />
                      </Form.Group>
                    </Col>
                    <Col xs={12} md={6} className="mb-3">
                      <Form.Group controlId="formPhone" className="mb-3">
                        <Form.Label style={{ color: "#5D5D64" }}>Phone</Form.Label>
                        <Form.Control
                            type="tel"
                            name="phone"
                            placeholder="Enter phone number"
                            value={formData.phone}
                            onChange={handleChange}
                            required
                            style={{ backgroundColor: "#F4FFE3", borderColor: "#8BB334" }}
                        />
                      </Form.Group>
                    </Col>
                  </Row>

                  <Row>
                    <Col xs={12} md={6} className="mb-3">
                      <Form.Group controlId="formPassword" className="mb-3">
                        <Form.Label style={{ color: "#5D5D64" }}>Password</Form.Label>
                        <Form.Control
                            type="password"
                            name="password"
                            placeholder="Enter password"
                            value={formData.password}
                            onChange={handleChange}
                            required
                            style={{ backgroundColor: "#F4FFE3", borderColor: "#8BB334" }}
                            isInvalid={passwordErrors.length > 0}
                        />
                        <Form.Control.Feedback type="invalid">
                          {passwordErrors.map((error, index) => (
                              <div key={index}>{error.description}</div>
                          ))}
                        </Form.Control.Feedback>
                      </Form.Group>
                    </Col>

                    <Col xs={12} md={6} className="mb-3">
                      <Form.Group controlId="formRole" className="mb-3">
                        <Form.Label style={{ color: "#5D5D64" }}>Role</Form.Label>
                        <div>
                          <Form.Check
                              inline
                              type="radio"
                              label="Vendor"
                              name="role"
                              value="vendor"
                              checked={formData.role === "vendor"}
                              onChange={handleChange}
                              required
                          />
                          <Form.Check
                              inline
                              type="radio"
                              label="CSR"
                              name="role"
                              value="csr"
                              checked={formData.role === "csr"}
                              onChange={handleChange}
                              required
                          />
                        </div>
                      </Form.Group>
                    </Col>
                  </Row>

                  <Button
                      variant="primary"
                      type="submit"
                      className="w-100 py-2 mt-3"
                      disabled={passwordErrors.length > 0}
                      style={{ backgroundColor: "#92CF34", borderColor: "#8BB334" }} // Lime and apple green for the button
                  >
                    {isLoading && (
                        <l-ring
                            size="20"
                            stroke="2"
                            bg-opacity="0"
                            speed="2"
                            color="white"
                        />
                    )}
                    Register
                  </Button>
                </Form>

                <div className="text-center mt-3">
                  <p className="text-muted">
                    Already have an account?{" "}
                    <a href="/login" style={{ color: "#8BB334" }}>
                      Login
                    </a>
                  </p>
                </div>
              </Card.Body>
            </Card>
          </Col>
        </Row>
      </Container>
  );
};

export default RegisterForm;
