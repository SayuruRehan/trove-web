import React, { useState } from "react";
import { Form, Button, Container, Row, Col, Card } from "react-bootstrap";
import AuthService from "../../../APIService/AuthService";

const LoginForm = () => {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");

  const handleSubmit = async (e) => {
    e.preventDefault();

    const response = await AuthService.login({ email, password });

    if (response.status === 200) {
      localStorage.setItem("authToken", response.data.token);
    }
  };

  return (
    <Container
      fluid
      className="d-flex flex-column flex-grow-1 justify-content-center align-items-center bg-light"
    >
      <Row className="w-100 justify-content-center">
        <Col xs={12} md={6} lg={4}>
          <Card className="shadow-lg p-4 rounded">
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
                >
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
