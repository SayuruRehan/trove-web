import React, { useEffect, useState } from "react";
import Container from "react-bootstrap/Container";
import Card from "react-bootstrap/Card";
import Button from "react-bootstrap/Button";
import Dropdown from "react-bootstrap/Dropdown";
import Badge from "react-bootstrap/Badge";
import APIService from "../../../APIService/APIService";
import { toast } from "react-toastify";
import { useAuth } from "../../context/authContext";

const VendorListings = () => {
  const [orders, setOrders] = useState([]);
  const [loading, setLoading] = useState(false);

  const { user } = useAuth();

  // Get vendor-specific orders
  const fetchVendorOrder = async () => {
    try {
      setLoading(true);
      const response = await APIService.getVendorOrders(user.userId);
      setOrders(response.data);
    } catch (err) {
      console.error("Error fetching vendor orders", err);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchVendorOrder();
  }, [user]);

  function formatDate(dataString) {
    const date = new Date(dataString);
    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, "0");
    const day = String(date.getDate()).padStart(2, "0");

    return `${year}.${month}.${day}`;
  }

  const handleStatusChanging = async (orderObj, fulfillmentStatus) => {
    try {
      const updatedObj = { ...orderObj, fulfillmentStatus };
      console.log(updatedObj);
      const response = await APIService.updateOrderStatus(
        updatedObj.id,
        updatedObj
      );

      if (response.status === 200) {
        toast.success("Order Status Update Success!", {
          autoClose: 300,
          position: "top-right",
        });

        // Update order status locally after successful update
        setOrders((prevOrders) =>
          prevOrders.map((order) =>
            order.id === orderObj.id ? { ...order, fulfillmentStatus } : order
          )
        );
      }
    } catch (err) {
      console.error("Error updating order status", err);
      toast.error("Error Updating Order Status!", {
        autoClose: 300,
        position: "top-right",
      });
    }
  };

  return (
    <Container>
      <div className="mt-4">
        <h4 className="d-flex align-items-center justify-content-center">
          Your Orders
        </h4>
        {loading ? (
          <p>Loading...</p>
        ) : orders.length > 0 ? (
          orders.map((order, index) => (
            <Card
              style={{
                maxWidth: "29rem",
                marginTop: "1rem",
                border: "1px solid grey",
              }}
              key={index}
            >
              <div style={{ display: "flex", flexDirection: "row" }}>
                {/* <div style={{ border: '1px solid grey', borderRadius: '5px', maxWidth: '200px' }}>
                  <Card.Img src="https://consumer.huawei.com/content/dam/huawei-cbg-site/common/mkt/plp/phones-20230509/nova-series/nova12-i.png" />
                </div> */}
                <div>
                  <Card.Body>
                    <Card.Title>{order.productName}</Card.Title>
                    <div style={{ display: "flex", gap: "18.25rem" }}>
                      <Card.Text className="mb-1 text-muted">
                        Order Id:
                      </Card.Text>
                      <Card.Text className="mb-1 text-muted">
                        {order.id.slice(0, 6)}
                      </Card.Text>
                    </div>

                    <div
                      key={index}
                      className="d-flex align-items-center justify-content-between mt-2 gap-5"
                    >
                      <Card.Subtitle className="mb-1 text-muted">
                        {order.productName}
                      </Card.Subtitle>
                      <Card.Subtitle className="mb-1 text-muted">
                        x&nbsp;&nbsp;{order.quantity}
                      </Card.Subtitle>
                    </div>

                    <div className="d-flex align-items-baseline justify-content-between mt-1 gap-5">
                      <Card.Subtitle className="mb-2 text-muted">
                        Ordered Date
                      </Card.Subtitle>
                      <Card.Subtitle className="mb-2 text-muted">
                        {formatDate(order.createdAt)}
                      </Card.Subtitle>
                    </div>

                    <div className="d-flex align-items-baseline justify-content-between  gap-5">
                      <Card.Subtitle className="mb-2 text-muted">
                        Vendor Name
                      </Card.Subtitle>
                      <Card.Subtitle className="mb-1 text-muted">
                        {order.vendorName}
                      </Card.Subtitle>
                    </div>

                    <div className="d-flex align-items-center justify-content-between gap-5 mt-0">
                      <Card.Subtitle className="mb-2 text-muted">
                        Shipping address
                      </Card.Subtitle>
                      <Card.Subtitle className="mb-2 text-muted">
                        {order.shippingAddress}
                      </Card.Subtitle>
                    </div>

                    <div className="d-flex align-items-center justify-content-between gap-5 mt-2">
                      <Card.Subtitle className="mb-2 text-muted">
                        Total Amount
                      </Card.Subtitle>
                      <Card.Subtitle className="mb-2 text-muted">
                        Rs.{order.productPrice}.00
                      </Card.Subtitle>
                    </div>

                    <div className="d-flex align-items-baseline mt-2 gap-3">
                      <Card.Subtitle className="mb-2 text-black">
                        Edit Order Status:{" "}
                      </Card.Subtitle>
                      <Dropdown className="text-center">
                        <Dropdown.Toggle
                          variant="info"
                          style={{ backgroundColor: "white" }}
                          className="dropdown_btn"
                          id="dropdown-basic"
                        >
                          <Badge
                            bg={
                              order.fulfillmentStatus === "Pending"
                                ? "info"
                                : order.fulfillmentStatus === "Delivered"
                                ? "success"
                                : order.fulfillmentStatus ===
                                  "PartiallyDelivered"
                                ? "warning"
                                : ""
                            }
                            className="p-1"
                          >
                            {order.fulfillmentStatus}{" "}
                            <i className="fa-solid fa-angle-down"></i>
                          </Badge>
                        </Dropdown.Toggle>
                        <Dropdown.Menu>
                          <Dropdown.Item
                            onClick={() =>
                              handleStatusChanging(order, "Delivered")
                            }
                          >
                            mark as Delivered
                          </Dropdown.Item>
                          <Dropdown.Item
                            onClick={() =>
                              handleStatusChanging(order, "PartiallyDelivered")
                            }
                          >
                            mark as PartiallyDelivered
                          </Dropdown.Item>
                        </Dropdown.Menu>
                      </Dropdown>
                    </div>
                  </Card.Body>
                </div>
              </div>
            </Card>
          ))
        ) : (
          <p>No orders</p>
        )}
      </div>
    </Container>
  );
};

export default VendorListings;
