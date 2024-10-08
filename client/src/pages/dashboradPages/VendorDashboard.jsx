import React from "react";
import { Link } from "react-router-dom";
import Card from "react-bootstrap/Card";
import Container from "react-bootstrap/Container";

const vendorDashboard = () => {
  return (
    <Container>
      <h5 className="d-flex justify-content-center align-items-center mt-4">
        Vendor Dashboard
      </h5>
      <div className="mt-5 d-flex gap-3 justify-content-center align-items-center ">
        <Link to="/manage-products">
          <Card style={{ width: "15rem" }}>
            <Card.Body className="d-flex justify-content-center align-items-center">
              <div className="d-flex flex-column justify-content-center align-items-center">
                <i
                  class="bi bi-file-earmark-text"
                  style={{ fontSize: "2rem" }}
                ></i>
                <p className="text-center">Product Listing Management</p>
              </div>
            </Card.Body>
          </Card>
        </Link>

        <Link to="/vendorOrder">
          <Card style={{ width: "15rem" }}>
            <Card.Body className="d-flex justify-content-center align-items-center">
              <div className="d-flex flex-column justify-content-center align-items-center">
                <i class="bi bi-box-seam-fill" style={{ fontSize: "2rem" }}></i>
                <p className="text-center">Vendor Orders</p>
              </div>
            </Card.Body>
          </Card>
        </Link>
      </div>
    </Container>
  );
};

export default vendorDashboard;
