import React from "react";
import { Link } from "react-router-dom";
import Card from 'react-bootstrap/Card';
import Container from 'react-bootstrap/Container';

const csrDashboards = () => {
  return (
    <Container>
      <h5 className="d-flex justify-content-center align-items-center mt-4">CSR Dashboard</h5>
      <div className="mt-5 d-flex justify-content-center align-items-center gap-3">
        <Link to="/users">
          <Card style={{ width: '15rem' }}>
            <Card.Body className="d-flex justify-content-center align-items-center">
              <div className="d-flex flex-column justify-content-center align-items-center">
                <i class="bi bi-people-fill" style={{ fontSize: '2rem' }}></i>
                <p className="text-center">User Management</p>
              </div>
            </Card.Body>
          </Card>
        </Link>

        <Link to="/allOrders">
          <Card style={{ width: '15rem' }}>
            <Card.Body className="d-flex justify-content-center align-items-center">
              <div className="d-flex flex-column justify-content-center align-items-center">
                <i class="bi bi-box-fill" style={{ fontSize: '2rem' }}></i>
                <p className="text-center">Order Management</p>
              </div>
            </Card.Body>
          </Card>
        </Link>
      </div>
    </Container>
  )
}

export default csrDashboards
