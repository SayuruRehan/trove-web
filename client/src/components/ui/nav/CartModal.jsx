import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import { Button, Modal } from "react-bootstrap";
import { useCartContext } from "../../providers/ContextProvider";

const CartModal = ({ show, onClose }) => {
  const { cartData } = useCartContext();

  const navigate = useNavigate();

  const proceedToCart = () => {
    navigate(`/cart`);
    onClose()
  };

  return (
    <div>
      <Modal show={show && cartData.length > 0} onHide={onClose}>
        <Modal.Header closeButton>
          <Modal.Title>Your Items</Modal.Title>
        </Modal.Header>
        <Modal.Body>
          {cartData.length > 0 ? (
            <ul className="list-group">
              {cartData.map((item, index) => {
                return (
                  <li key={index} className="list-group-item">
                    <div>
                      <h5>{item.productName}</h5>
                    </div>
                    <span>Price: Rs. {item.productPrice}</span>
                  </li>
                );
              })}
            </ul>
          ) : (
            <p>Your cart is empty.</p>
          )}
        </Modal.Body>
        <Modal.Footer>
          <Button variant="secondary" onClick={onClose}>
            Close
          </Button>
          <Button variant="primary" onClick={proceedToCart}>
            Checkout
          </Button>
        </Modal.Footer>
      </Modal>
    </div>
  );
};

export default CartModal;
