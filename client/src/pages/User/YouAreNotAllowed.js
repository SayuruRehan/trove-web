import React from "react";
import {useNavigate} from "react-router-dom";
import "bootstrap/dist/css/bootstrap.min.css";
import Header from "../../components/Header";
import Footer from "../../components/Footer";

export default function YouAreNotAllowed() {
  const navigate = useNavigate();

  // Function to go back to the previous page
  const handleGoBack = () => {
    navigate(-1); // Go back to the previous page
  };

  return (
    <>
      <Header />
      <div
        className="d-flex justify-content-center align-items-center"
        style={{marginTop: "100px", marginBottom: "100px"}}
      >
        <div className="text-center p-5 bg-white shadow rounded-lg">
          <h1 className="mb-4">YOU ARE NOT ALLOWED</h1>
          <p className="text-muted mb-4">
            Sorry, you don't have permission to access this page.
          </p>
          <button className="btn btn-primary" onClick={handleGoBack}>
            Go to Previous Page
          </button>
        </div>
      </div>
      <Footer />
    </>
  );
}
