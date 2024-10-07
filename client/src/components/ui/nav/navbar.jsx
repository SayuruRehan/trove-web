import React, { useState } from "react";
import {
  Container,
  Nav,
  Navbar,
  NavDropdown,
  Badge,
  Image,
} from "react-bootstrap";
import { useCartContext } from "../../providers/ContextProvider";
import CartModal from "./CartModal";
import { ShoppingBag, User } from "lucide-react";

const MainNavbar = () => {
  const { itemCount } = useCartContext();
  const [showCartModal, setShowCartModal] = useState(false);

  const handleOpenModal = () => setShowCartModal(true);
  const handleCloseModal = () => setShowCartModal(false);

  return (
    <>
      <Navbar
        expand="lg"
        bg="light"
        variant="light"
        className="shadow-sm py-2"
        sticky="top"
      >
        <Container>
          <Navbar.Brand href="#" className="font-weight-bold text-primary">
            E-com
          </Navbar.Brand>
          <Navbar.Toggle aria-controls="basic-navbar-nav" />
          <Navbar.Collapse id="basic-navbar-nav">
            <Nav className="me-auto">
              <Nav.Link href="/vendor/register" className="mx-2">
                Connect With Us
              </Nav.Link>
              <Nav.Link href="/" className="mx-2">
                Product Listing
              </Nav.Link>
              <Nav.Link href="/orders" className="mx-2">
                Orders
              </Nav.Link>
              <NavDropdown
                title="Dashboard"
                id="basic-nav-dropdown"
                className="mx-2"
              >
                <NavDropdown.Item href="/adminDashboard">
                  Admin
                </NavDropdown.Item>
                <NavDropdown.Item href="/csr">CSR</NavDropdown.Item>
                <NavDropdown.Item href="/vendor">Vendor</NavDropdown.Item>
              </NavDropdown>
            </Nav>

            <Nav className="align-items-center">
              <div className="position-relative me-3 d-flex align-items-center">
                <ShoppingBag
                  size={24}
                  onClick={handleOpenModal}
                  className="text-primary cursor-pointer"
                />
                {itemCount > 0 && (
                  <Badge
                    bg="danger"
                    pill
                    className="position-absolute top-0 start-100 translate-middle"
                  >
                    {itemCount}
                  </Badge>
                )}
              </div>

              <NavDropdown
                align="end"
                title={
                  <div className="d-inline-block">
                    <Image
                      src="https://i.pinimg.com/736x/0d/64/98/0d64989794b1a4c9d89bff571d3d5842.jpg"
                      roundedCircle
                      width="32"
                      height="32"
                      alt="User Avatar"
                      className="border border-primary"
                    />
                  </div>
                }
                id="user-nav-dropdown"
              >
                <NavDropdown.Item
                  href="/login"
                  className="d-flex align-items-center"
                >
                  <User size={18} className="me-2" /> Login
                </NavDropdown.Item>
                <NavDropdown.Item
                  href="/register"
                  className="d-flex align-items-center"
                >
                  <User size={18} className="me-2" /> Register
                </NavDropdown.Item>
              </NavDropdown>
            </Nav>
          </Navbar.Collapse>
        </Container>
      </Navbar>
      <CartModal show={showCartModal} onClose={handleCloseModal} />
    </>
  );
};

export default MainNavbar;
