import React from "react";
import { Card, Button } from "react-bootstrap";
import PropTypes from "prop-types";

const ProductCard = ({ product, onAddToCart }) => {
  return (
    <Card className="h-100 shadow-sm border w-100">
      {/* Added w-100 here */}
      <div
        className="card-img-wrapper"
        style={{ height: "200px", overflow: "hidden" }}
      >
        <Card.Img
          variant="top"
          src={
            product.imageUrl ||
            "https://curie.pnnl.gov/sites/default/files/default_images/default-image_0.jpeg"
          }
          alt={product.productName}
          className="w-100 h-100"
          style={{ objectFit: "fill", objectPosition: "center" }} // objectFit and objectPosition remain as inline styles for now
        />
      </div>
      <Card.Body className="d-flex flex-column">
        <Card.Title className="h5 mb-2">{product.productName}</Card.Title>
        <Card.Text className="mb-1">
          <span className="fw-bold">Vendor:</span> {product.vendorName}
        </Card.Text>
        <Card.Text className="mb-3">
          <span className="fw-bold">Price:</span> Rs{" "}
          {product.productPrice.toFixed(2)}
        </Card.Text>
        <Button
          variant="primary"
          onClick={() => onAddToCart(product)}
          className="mt-auto w-100"
        >
          View
        </Button>
      </Card.Body>
    </Card>
  );
};

ProductCard.propTypes = {
  product: PropTypes.shape({
    id: PropTypes.string.isRequired,
    productName: PropTypes.string.isRequired,
    description: PropTypes.string,
    productPrice: PropTypes.number.isRequired,
    imageUrl: PropTypes.string,
    vendorId: PropTypes.string.isRequired,
    vendorName: PropTypes.string.isRequired,
    stock: PropTypes.number,
  }).isRequired,
  onAddToCart: PropTypes.func.isRequired,
};

export default ProductCard;
