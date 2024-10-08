import React from "react";
import MainNavbar from "../ui/nav/navbar";
import Router from "../../router/Router";
import { Container } from "react-bootstrap";

const MainLayout = () => {
  return (
    <div className="d-flex flex-column min-vh-100">
      <MainNavbar />

      <Container className="flex-grow-1 d-flex">
        <Router />
      </Container>
    </div>
  );
};

export default MainLayout;
