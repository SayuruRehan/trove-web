import React, { useState, useEffect } from "react";
import {
  Container,
  Row,
  Col,
  Card,
  Button,
  Form,
  ListGroup,
  Modal,
  Image,
  Badge,
} from "react-bootstrap";
import { ring } from "ldrs";
import { CirclePlus, Pencil, Trash2 } from "lucide-react";

ring.register();

import ProductService from "../../../APIService/ProductService";

const VendorDashboard = () => {
  const [isLoading, setIsLoading] = useState(false);
  const [products, setProducts] = useState([
    {
      id: 1,
      productName: "Product 1",
      productPrice: 19.99,
      description: "Description for Product 1",
      imageUrl:
        "https://www.apple.com/newsroom/images/product/iphone/standard/Apple_iPhone-13-Pro_Colors_09142021_big.jpg.large.jpg",
    },
  ]);
  const [showModal, setShowModal] = useState(false);
  const [currentProduct, setCurrentProduct] = useState({
    id: null,
    productName: "",
    productPrice: "",
    description: "",
    imageUrl: null,
    stock: 0,
  });
  const [previewImage, setPreviewImage] = useState("");

  const getProductList = async () => {
    const response = await ProductService.getVenderProducts(
      "65074c59a3e8fa0c65432109"
    );
    if (response.data) setProducts(response.data);
  };

  useEffect(() => {
    getProductList();
  }, []);

  useEffect(() => {
    return () => {
      if (previewImage && previewImage.startsWith("blob:")) {
        URL.revokeObjectURL(previewImage);
      }
    };
  }, [previewImage]);

  const handleShowModal = (
    product = {
      id: null,
      productName: "",
      productPrice: "",
      description: "",
      imageUrl: null,
    }
  ) => {
    setCurrentProduct(product);
    setPreviewImage(product.imageUrl || "");
    setShowModal(true);
  };

  const handleCloseModal = () => {
    setShowModal(false);
    setCurrentProduct({
      id: null,
      productName: "",
      productPrice: "",
      description: "",
      image: null,
    });
    setPreviewImage("");
  };

  const handleInputChange = (e) => {
    const { name, value } = e.target;
    setCurrentProduct({ ...currentProduct, [name]: value });
  };

  const handleImageChange = (e) => {
    const file = e.target.files[0];
    if (file) {
      setCurrentProduct({ ...currentProduct, image: file });

      // Create a preview using URL.createObjectURL
      const objectUrl = URL.createObjectURL(file);
      setPreviewImage(objectUrl);
    }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    setIsLoading(true);

    const formData = new FormData();
    formData.append("productName", currentProduct.productName);
    formData.append("productPrice", currentProduct.productPrice);
    formData.append("description", currentProduct.description);
    if (currentProduct.image instanceof File) {
      formData.append("image", currentProduct.image);
    }

    if (currentProduct.id) {
      await ProductService.updateVenderProduct(formData, currentProduct.id);
    } else {
      const response = await ProductService.addProduct(formData);
      if (response) {
        alert("product add successful");
      }
    }
    getProductList();
    handleCloseModal();
    setIsLoading(false);
  };

  const handleDelete = async (id) => {
    console.log("id", id);
    if (id) {
      await ProductService.deleteVenderProduct(id);
    }
    getProductList();
  };

  return (
    <Container fluid className="py-4 px-3 px-md-4">
      <Card className="shadow-sm mb-4">
        <Card.Body>
          <h1 className="h3 mb-0">Vendor Dashboard</h1>
        </Card.Body>
      </Card>

      <Row>
        <Col lg={8} className="mb-4 mb-lg-0">
          <Card className="shadow-sm h-100">
            <Card.Header className="bg-primary text-white">
              <h5 className="mb-0">Product Listings</h5>
            </Card.Header>
            <ListGroup variant="flush">
              {products.map((product) => (
                <ListGroup.Item
                  key={product.id}
                  className="d-flex flex-column flex-md-row justify-content-between align-items-start align-items-md-center py-3"
                >
                  <div className="d-flex align-items-center mb-2 mb-md-0">
                    <Image
                      src={product.imageUrl}
                      rounded
                      className="me-3"
                      width={60}
                      height={60}
                      style={{ objectFit: "cover" }}
                    />
                    <div>
                      <h6 className="mb-0">{product.productName}</h6>
                      <Badge bg="success" className="mt-1">
                        ${product.productPrice}
                      </Badge>
                    </div>
                  </div>
                  <div className="d-flex">
                    <Button
                      variant="outline-primary"
                      size="sm"
                      className="me-2"
                      onClick={() => handleShowModal(product)}
                    >
                      <Pencil size={16} className="me-1" /> Edit
                    </Button>
                    <Button
                      variant="outline-danger"
                      size="sm"
                      onClick={() => handleDelete(product.id)}
                    >
                      <Trash2 size={16} className="me-1" /> Delete
                    </Button>
                  </div>
                </ListGroup.Item>
              ))}
            </ListGroup>
          </Card>
        </Col>
        <Col lg={4}>
          <Card className="shadow-sm">
            <Card.Header className="bg-success text-white">
              <h5 className="mb-0">Quick Actions</h5>
            </Card.Header>
            <Card.Body>
              <Button
                variant="primary"
                className="w-100 d-flex align-items-center justify-content-center"
                onClick={() => handleShowModal()}
              >
                <CirclePlus size={20} className="me-2" />
                Add New Product
              </Button>
            </Card.Body>
          </Card>
        </Col>
      </Row>

      <Modal show={showModal} onHide={handleCloseModal} size="lg">
        <Modal.Header closeButton className="bg-light">
          <Modal.Title>
            {currentProduct.id ? "Edit Product" : "Add New Product"}
          </Modal.Title>
        </Modal.Header>
        <Modal.Body>
          <Form onSubmit={handleSubmit}>
            <Row>
              <Col md={6}>
                <Form.Group className="mb-3">
                  <Form.Label>Product Name</Form.Label>
                  <Form.Control
                    type="text"
                    name="productName"
                    value={currentProduct.productName}
                    onChange={handleInputChange}
                    required
                  />
                </Form.Group>
              </Col>
              <Col md={6}>
                <Form.Group className="mb-3">
                  <Form.Label>Price</Form.Label>
                  <Form.Control
                    type="number"
                    name="productPrice"
                    value={currentProduct.productPrice}
                    onChange={handleInputChange}
                    required
                  />
                </Form.Group>
              </Col>
            </Row>
            <Form.Group className="mb-3">
              <Form.Label>Description</Form.Label>
              <Form.Control
                as="textarea"
                rows={3}
                name="description"
                value={currentProduct.description}
                onChange={handleInputChange}
                required
              />
            </Form.Group>
            <Form.Group className="mb-3">
              <Form.Label>Image</Form.Label>
              <Form.Control
                type="file"
                name="image"
                onChange={handleImageChange}
                accept="image/*"
              />
              {previewImage && (
                <div className="mt-2">
                  <Image
                    src={previewImage}
                    rounded
                    className="img-thumbnail"
                    style={{
                      maxWidth: "200px",
                      maxHeight: "200px",
                      objectFit: "cover",
                    }}
                    alt="Product preview"
                  />
                </div>
              )}
            </Form.Group>
            <Button
              variant="primary"
              type="submit"
              disabled={isLoading}
              className="w-100"
            >
              <span className="d-flex align-items-center justify-content-center">
                {isLoading && (
                  <div className="me-2">
                    <l-ring
                      size="20"
                      stroke="2"
                      bg-opacity="0"
                      speed="2"
                      color="white"
                    />
                  </div>
                )}
                {currentProduct.id ? "Update Product" : "Add Product"}
              </span>
            </Button>
          </Form>
        </Modal.Body>
      </Modal>
    </Container>
  );
};

export default VendorDashboard;
