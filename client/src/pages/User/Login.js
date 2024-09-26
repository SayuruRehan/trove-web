import React, {useState} from "react";
import {Form, Button, Alert} from "react-bootstrap";
import axios from "axios"; // Import axios
import "../../styles/login.css";
import BackgroundImage from "../../assets/background.png";
import Logo from "../../assets/logo.png";

const Login = () => {
  const [inputUsername, setInputUsername] = useState("");
  const [inputPassword, setInputPassword] = useState("");

  const [show, setShow] = useState(false);
  const [errorMeg, setErrorMeg] = useState("Incorrect username or password.");
  const [loading, setLoading] = useState(false);

  const handleSubmit = async (event) => {
    event.preventDefault();
    setLoading(true);

    try {
      const response = await axios.post(
        `${process.env.REACT_APP_WEB_API}/users/login`,
        {
          username: inputUsername,
          password: inputPassword,
        }
      );

      // Assuming the API returns success with user data
      if (response.data) {
        console.log(`Login successful for user: ${response.data.username}`);

        // Store username and role in session storage
        sessionStorage.setItem("username", response.data.username);
        sessionStorage.setItem("userId", response.data.userId);
        sessionStorage.setItem("role", response.data.role);

        // Redirect to the dashboard or another page
        window.location.href = "/";
      } else {
        setErrorMeg(response.data.message);
        setShow(true);
      }
    } catch (error) {
      // Corrected error handling for catch block
      if (
        error.response &&
        error.response.data &&
        error.response.data.message
      ) {
        setErrorMeg(error.response.data.message);
      } else {
        setErrorMeg("An unknown error occurred.");
      }
      console.error("Login error:", error);
      setShow(true);
    } finally {
      setLoading(false);
    }
  };

  const handlePassword = () => {
    // Add your "forgot password" logic here
  };

  return (
    <div
      className="sign-in__wrapper"
      style={{backgroundImage: `url(${BackgroundImage})`}}
    >
      <div className="sign-in__backdrop"></div>
      <Form className="shadow p-4 bg-white rounded" onSubmit={handleSubmit}>
        <img
          className="img-thumbnail mx-auto d-block mb-2"
          src={Logo}
          alt="logo"
        />
        <div className="h4 mb-2 text-center">Sign In</div>
        {show && (
          <Alert
            className="mb-2"
            variant="danger"
            onClose={() => setShow(false)}
            dismissible
          >
            {errorMeg}
          </Alert>
        )}
        <Form.Group className="mb-2" controlId="username">
          <Form.Label>Username</Form.Label>
          <Form.Control
            type="text"
            value={inputUsername}
            placeholder="Username"
            onChange={(e) => setInputUsername(e.target.value)}
            required
          />
        </Form.Group>
        <Form.Group className="mb-2" controlId="password">
          <Form.Label>Password</Form.Label>
          <Form.Control
            type="password"
            value={inputPassword}
            placeholder="Password"
            onChange={(e) => setInputPassword(e.target.value)}
            required
          />
        </Form.Group>

        {!loading ? (
          <Button className="w-100" variant="primary" type="submit">
            Log In
          </Button>
        ) : (
          <Button className="w-100" variant="primary" type="submit" disabled>
            Logging In...
          </Button>
        )}
        {/* <div className="d-grid justify-content-end">
          <Button
            className="text-muted px-0"
            variant="link"
            onClick={handlePassword}
          >
            Forgot password?
          </Button>
        </div> */}
      </Form>
      <div className="w-100 mb-2 position-absolute bottom-0 start-50 translate-middle-x text-white text-center">
        Made by SLIIT-SSD | &copy;2024
      </div>
    </div>
  );
};

export default Login;
