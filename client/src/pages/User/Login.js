import React, {useState} from "react";
import {Form, Button, Alert} from "react-bootstrap";
import axios from "axios"; // Import axios
import "../../styles/login.css";
import BackgroundImage from "../../assets/background.jpg";
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
      <div className="login-container">
        <div className="hero-card">
          <h1>TROVE</h1>
          <p>
          Your Only E-Commerce Application!
          </p>
        </div>
        <div className="login-box">
          <main className="login">
            <Form className="shadow p-4 rounded" onSubmit={handleSubmit}>
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
                  <Button className="w-100 mt-5" variant="secondary" type="submit">
                    Log In
                  </Button>
              ) : (
                  <Button className="w-100 mt-5" variant="primary" type="submit" disabled>
                    Logging In...
                  </Button>
              )}
            </Form>
          </main>
          <footer className="signup-footer">
            <p><a href="#">Don't have an account?</a></p>
          </footer>
        </div>
      </div>
  );
};

export default Login;


// <div
//     className="sign-in__wrapper"
//     style={{backgroundImage: `url(${BackgroundImage})`}}
// >
//   <div className="sign-in__backdrop"></div>
//   <Form className="shadow p-4 bg-white rounded" onSubmit={handleSubmit}>
//     <img
//         className="img-thumbnail mx-auto d-block mb-2"
//         src={Logo}
//         alt="logo"
//     />
//     <div className="h4 mb-2 text-center">Sign In</div>
//     {show && (
//         <Alert
//             className="mb-2"
//             variant="danger"
//             onClose={() => setShow(false)}
//             dismissible
//         >
//           {errorMeg}
//         </Alert>
//     )}
//     <Form.Group className="mb-2" controlId="username">
//       <Form.Label>Username</Form.Label>
//       <Form.Control
//           type="text"
//           value={inputUsername}
//           placeholder="Username"
//           onChange={(e) => setInputUsername(e.target.value)}
//           required
//       />
//     </Form.Group>
//     <Form.Group className="mb-2" controlId="password">
//       <Form.Label>Password</Form.Label>
//       <Form.Control
//           type="password"
//           value={inputPassword}
//           placeholder="Password"
//           onChange={(e) => setInputPassword(e.target.value)}
//           required
//       />
//     </Form.Group>
//
//     {!loading ? (
//         <Button className="w-100 mt-5" variant="secondary" type="submit">
//         Log In
//         </Button>
//     ) : (
//         <Button className="w-100 mt-5" variant="primary" type="submit" disabled>
//           Logging In...
//         </Button>
//     )}
//   </Form>
// </div>