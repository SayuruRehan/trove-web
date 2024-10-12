import React, { useState } from "react";
import {
  Form,
  Button,
  Container,
  Row,
  Col,
  Card,
  Alert,
} from "react-bootstrap";
import AuthService from "../../../APIService/AuthService";
import { useAuth } from "../../context/authContext";
import { useNavigate } from "react-router-dom";
import k from "../../assets/k.png";

const LoginForm = () => {
  const navigate = useNavigate();
  const { setAuthData } = useAuth();
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState(null);
  const [isLoading, setIsLoading] = useState(false);

  const handleSubmit = async (e) => {
    e.preventDefault();
    setIsLoading(true);
    setError(null);

    try {
      const response = await AuthService.login({ email, password });

      if (response.data.isSuccess) {
        setAuthData(response.data);
        navigate("/");
      } else {
        setError(response.data.message);
      }
    } catch (error) {
      if (error.response) {
        switch (error.response.status) {
          case 403:
            setError(
              "Your account is pending approval. Please try again later or contact support."
            );
            break;
          case 401:
            setError("Invalid email or password. Please try again.");
            break;
          default:
            setError("An error occurred. Please try again later.");
        }
      } else {
        setError(
          "Unable to connect to the server. Please check your internet connection."
        );
      }
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <Container
      fluid
      className="d-flex flex-column flex-grow-1 justify-content-center align-items-center bg-light"
    >
      <Row className="w-100 justify-content-center">
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
              <h2 className="text-center text-primary mb-4">Login</h2>

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
                  disabled={isLoading}
                  style={{
                    display: "flex",
                    alignItems: "center",
                    gap: "20px",
                    justifyContent: "center",
                  }}
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
                  Log In
                </Button>
              </Form>

              <div className="text-center mt-3">
                <p className="text-muted">
                  Don't have an account?{" "}
                  <a href="/register" className="text-primary">
                    Sign up
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

export default LoginForm;
