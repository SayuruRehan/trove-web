import React, { useEffect, useState } from "react";
import Card from "react-bootstrap/Card";
import Button from "react-bootstrap/Button";
import Container from "react-bootstrap/Container";
import CancelConfirmModal from "../components/CancelConfirmModal";
import APIService from "../../APIService/APIService";
import { ToastContainer, toast } from "react-toastify";
import NotificationService from "../../APIService/NotificationService"

const OrderCancelationRequest = () => {
  const [orders, setOrders] = useState([]);
  const [filterOrders, setFilterOrders] = useState([]);
  const [show, setShow] = useState(false);
  const [comment, setComment] = useState("");
  const [selectedObj, setSelectedObj] = useState();
  const [senderId, setSenderId] = useState("")
  const [title, setTitle] = useState("Your Order Deleted Successfully")

  console.log(filterOrders)

  //get the user object from local storage and covert to JSON 
  const getUserId = () => {
    const user = JSON.parse(localStorage.getItem("user"))
    setSenderId(user.userId)
  }

  //handle modal show close
  const handleClose = () => setShow(false);
  const handleShow = (orderObj) => {
    setShow(true);
    setSelectedObj(orderObj);
  };

  //notify user after cancel the order
  const sendNotification = async (notifyObj) => {
    try {
      console.log('object', notifyObj)
      const response = await NotificationService.addNotification(notifyObj)
      console.log(response)
    } catch (err) {
      console.error('Error send notification', err)
    }
  }

  //handle delete order and send a comment
  const confirmCancel = async () => {
    try {
      const response = await APIService.deleteOrder(selectedObj.id);
      if (response.status == 200) {
        toast.warning("Order deleted Successfully!", {
          autoClose: 300,
          position: "top-right",
        });
        fetchOrders();
        const notifyObj = {
          customerId: selectedObj?.userId,
          senderId,
          title,
          description: comment
        }

        await sendNotification(notifyObj)
      }
    } catch (err) {
      console.error("Error deleting order");
    } finally {
      setShow(false);
      console.log(comment);
    }
  };

  // Fetch orders
  const fetchOrders = async () => {
    try {
      const response = await APIService.getAllOrders();
      setOrders(response.data);
    } catch (err) {
      console.log("Error fetching orders");
    }
  };

  // Filter cancelled orders
  useEffect(() => {
    setFilterOrders(orders.filter((order) => order.status === "Cancelled"));
  }, [orders]);

  // Execute API calls once when component mounts
  useEffect(() => {
    fetchOrders();
    getUserId()
  }, []);

  return (
    <Container>
      <h6 className="mt-3 d-flex align-items-center justify-content-center">
        Order Cancellation Requests
      </h6>
      <div className="mt-4">
        {filterOrders.length > 0 ? (
          filterOrders.map((order, index) => (
            <Card
              style={{ width: "28rem", height: "auto", marginTop: "1rem" }}
              key={index}
            >
              <Card.Body>
                <div className="d-flex justify-content-between">
                  <Card.Subtitle>orderId :</Card.Subtitle>
                  <Card.Subtitle>{order.orderId}</Card.Subtitle>
                </div>
                <div>
                  <div className="d-flex justify-content-between mt-3">
                    <Card.Subtitle className="mb-1 text-muted">
                      Customer Name :
                    </Card.Subtitle>
                    <Card.Subtitle className="mb-1 text-muted">
                      {order.userName}
                    </Card.Subtitle>
                  </div>
                  <div className="d-flex justify-content-between mt-1">
                    <Card.Subtitle className="mb-1 text-muted">
                      TotalAmount :
                    </Card.Subtitle>
                    <Card.Subtitle className="mb-1 text-muted">
                      {order.totalAmount}
                    </Card.Subtitle>
                  </div>
                </div>
                <div className="d-flex justify-content-between mt-1">
                  <Card.Subtitle className="mb-1 text-muted">
                    Shipping address :
                  </Card.Subtitle>
                  <Card.Subtitle className="mb-1 text-muted">
                    {order.shippingAddress}
                  </Card.Subtitle>
                </div>
                <div>
                  <Card.Subtitle>
                    <Button
                      variant="danger"
                      onClick={() => handleShow(order)}
                      className="py-0 px-4 mt-4"
                    >
                      Cancel
                    </Button>
                  </Card.Subtitle>
                </div>
              </Card.Body>
            </Card>
          ))
        ) : (
          <h4>No cancel requests were found</h4>
        )}
      </div>
      <CancelConfirmModal
        show={show}
        handleClose={handleClose}
        confirmCancel={confirmCancel}
        setComment={setComment}
      />
    </Container>
  );
};

export default OrderCancelationRequest;
