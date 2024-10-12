import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import {
  Form,
  Button,
  Container,
  Row,
  Col,
  Card,
  Alert,
} from "react-bootstrap";
import VendorService from "../../../APIService/VendorService";
import k from "../../assets/k.png";

const LoginForm = () => {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState(null);

  const navigate = useNavigate();

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      const response = await VendorService.loginVendor({
        email,
        password,
      });

      if (response.status == 200) {
        navigate(`/vendor`);
      }
    } catch (error) {
      setError(error.response.data);
    }
  };

  return (
    <Container
      fluid
      className="d-flex flex-column flex-grow-1 justify-content-center align-items-center bg-light"
    >
      <Row className="d-flex justify-content-center">
        <Col
          xs={10}
          md={6}
          lg={4}
          className="d-flex justify-content-center align-items-center"
        >
          <img
            src={k}
            alt="Vendor"
            className="img-fluid"
            style={{
              width: "100%",
              height: "auto",
            }}
          />
        </Col>
        <Col xs={12} md={6} lg={4}>
          <Card className="shadow-lg p-4 rounded">
            {error && (
              <Alert
                variant="danger"
                onClose={() => setError(null)}
                dismissible
              >
                {error}
              </Alert>
            )}
            <Card.Body>
              <h2 className="text-center text-primary mb-4">Vendor Login</h2>

              <Form onSubmit={handleSubmit}>
                <Form.Group controlId="formBasicEmail" className="mb-3">
                  <Form.Label>Email address</Form.Label>
                  <Form.Control
                    type="email"
                    placeholder="Enter email"
                    value={email}
                    onChange={(e) => setEmail(e.target.value)}
                    required
                  />
                </Form.Group>

                <Form.Group controlId="formBasicPassword" className="mb-3">
                  <Form.Label>Password</Form.Label>
                  <Form.Control
                    type="password"
                    placeholder="Password"
                    value={password}
                    onChange={(e) => setPassword(e.target.value)}
                    required
                  />
                </Form.Group>

                <Button
                  variant="primary"
                  type="submit"
                  className="w-100 py-2 mt-3"
                >
                  Log In
                </Button>
              </Form>
            </Card.Body>
          </Card>
        </Col>
      </Row>
    </Container>
  );
};

export default LoginForm;
