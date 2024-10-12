import React, { useEffect, useState } from 'react'
import Container from 'react-bootstrap/Container';
import Card from 'react-bootstrap/Card';
import Button from 'react-bootstrap/Button';
import OverlayTrigger from 'react-bootstrap/OverlayTrigger';
import Tooltip from 'react-bootstrap/Tooltip';
import APIService from '../../APIService/APIService';
import RatingComponent from '../components/RatingComponent';
import UpdateOrderModal from '../components/ui/UpdateOrderModal';
import { ToastContainer, toast } from "react-toastify";
import Modal from 'react-bootstrap/Modal';

const Order = () => {

  const [orders, setOrders] = useState([])
  const [loading, setLoading] = useState(true);
  const [orderObj, setOrderObj] = useState()
  const [selectedOrder, setSelectedOrder] = useState()
  console.log(orders)
  const [show, setShow] = useState(false);
  const [showEdit, setShowEdit] = useState(false)
  const [cancel, setCancel] = useState(false)

  const handleClose = () => setShow(false); 
  const handleShow = () => setShow(true);

  const handleEditOpen = () => setShowEdit(true)
  const handleEditClose = () => setShowEdit(false);

  const handleOpen = () => setCancel(true)
  const handleCloseModal = () => setCancel(false)

  //fetch all orders user placed
  const fetchOrders = async () => {
    try {
      const response = await APIService.getAllOrders()
      setOrders(response.data)
    } catch (err) {
      console.log('error fetching orders')
    } finally {
      setLoading(false)
    }
  }

  useEffect(() => {
    fetchOrders()
  }, [])

  //formate date month year
  function formateDate(dataString) {
    const date = new Date(dataString)
    const year = date.getFullYear()
    const month = String(date.getMonth() + 1).padStart(2, '0')
    const day = String(date.getDate()).padStart(2, '0')

    return `${year}.${month}.${day}`
  }

  const editOrder = (selectedOrderObj) => {
    handleEditOpen()//open modal
    setOrderObj(selectedOrderObj)
  }

  const cancelOrder = (order) => {
    handleOpen()
    setSelectedOrder(order)
  }

  //handle true response
  const returnTrue = async () => {
    try {
      let updatedObj = { ...selectedOrder, status: 'Cancelled' }
      const response = await APIService.updateOrderDetails(updatedObj, updatedObj.id)
      if (response.status == 200) {
        toast.success("Order cancel request is sent!", {
          autoClose: 300,
          position: "top-right",
        });
        fetchOrders()
      }
    } catch (err) {
      console.error('Error cancelling order')
      toast.error("Error cancelling order!", {
        autoClose: 250,
        position: "top-right",
      });
    } finally {
      handleCloseModal()
    }
  }

  //give feedback to vendor
  const giveFeedback = async () => {
    try {
      const response = await APIService.updateVendor()
      console.log(response)
    } catch (err) {
      console.error('Error giving feedback')
    }
  }

  useEffect(() => {
    giveFeedback()
  },[])
  
  return (
    <Container>
      <div className='mt-4'>
        <Modal show={cancel} onHide={handleCloseModal}>
          <Modal.Header closeButton>
            <Modal.Title>Confirm Cancel</Modal.Title>
          </Modal.Header>
          <Modal.Body>Are sure you want to cancel the order</Modal.Body>
          <Modal.Footer>
            <Button variant="secondary" onClick={handleCloseModal}>
              Close
            </Button>
            <Button variant="primary" onClick={returnTrue}>
              Confirm
            </Button>
          </Modal.Footer>
        </Modal>
        <h4 className='d-flex align-items-center justify-content-center'>Placed Orders</h4>
        {
          loading ? (
            <div class="spinner-border m-5" style={{ width: '5rem', height: '5rem' }} role="status">
              <span class="sr-only"></span>
            </div>
          ) : (
            orders?.length > 0 && orders?.map((order, index) => (
              <Card style={{ maxWidth: '30rem', marginTop: '1rem', border: '1px solid grey' }} key={index}>
                <div style={{ display: 'flex', flexDirection: 'row' }}>
                  <div style={{ border: '1px solid grey', borderRadius: '5px', maxWidth: '200px' }}>
                    <Card.Img src={order.imageUrl} />
                  </div>
                  <div>
                    <Card.Body>

                      <div style={{ display: 'flex', gap: '15.23rem' }}>
                        <Card.Text className="mb-1 text-muted">OrderId</Card.Text>
                        <Card.Text className="mb-1 text-muted">{order.orderId}</Card.Text>
                      </div>

                      {order.orderItems?.map((item, index) => (
                        <div key={index} className='d-flex align-items-center justify-content-between mt-2 gap-5'>
                          <Card.Subtitle className="mb-1 text-muted">{item.productName}</Card.Subtitle>
                          <Card.Subtitle className="mb-1 text-muted">x&nbsp;&nbsp;{item.quantity}</Card.Subtitle>
                        </div>
                      ))}
                      <div className='d-flex align-items-baseline justify-content-between mt-3 gap-5'>
                        <Card.Subtitle className="mb-2 text-muted">Ordered Date</Card.Subtitle>
                        <Card.Subtitle className="mb-2 text-muted">{formateDate(order.createdAt)}</Card.Subtitle>
                      </div>
                      <div className='d-flex align-items-center justify-content-between gap-5 mt-2'>
                        <Card.Subtitle className="mb-2 text-muted">Total Amount</Card.Subtitle>
                        <Card.Subtitle className="mb-2 text-muted">Rs.{order.totalAmount.toFixed(2)}</Card.Subtitle>
                      </div>
                      <div className='d-flex align-items-baseline justify-content-between mt-2'>
                        <Button size='sm' variant={
                          order.status == "Pending" ? 'primary' :
                            order.status == "Delivered" ? 'success' :
                              order.status == "Cancelled" ? 'danger' :
                                order.status == "PartiallyDelivered" ? 'warning' : 'Secondary'}
                          className='mt-2 px-3' style={{ color: 'white' }}>{order.status}</Button>

                        {order.status == 'Delivered' ? <p onClick={handleShow} style={{
                          cursor: 'pointer',
                          color: 'blue',
                          transition: 'color 0.3s ease'
                        }}
                          onMouseEnter={(e) => (e.target.style.color = 'darkblue')}
                          onMouseLeave={(e) => (e.target.style.color = 'blue')}
                        >Give a feedback</p> : ''}

                        {order.status == 'Pending' ?
                          <div className='d-flex gap-3'>
                            <div onClick={() => editOrder(order)} style={{
                              backgroundColor: '#ffcc00', padding: '1.13rem', width: '22px', cursor: 'pointer', height: '22px',
                              borderRadius: '50%', display: 'flex', alignItems: 'center', justifyContent: 'center'
                            }}>
                              <OverlayTrigger placement="bottom" overlay={<Tooltip id="tooltip-disabled">Edit order</Tooltip>}>
                                <i class="bi bi-pencil" style={{ fontSize: '1.3rem' }}></i>
                              </OverlayTrigger>
                            </div>

                            <div onClick={() => cancelOrder(order)} style={{
                              backgroundColor: 'red', padding: '1.13rem', width: '22px', cursor: 'pointer', height: '22px',
                              borderRadius: '50%', display: 'flex', alignItems: 'center', justifyContent: 'center'
                            }}>
                              <OverlayTrigger placement="bottom" overlay={<Tooltip id="tooltip-disabled">Make cancel request</Tooltip>}>
                                <i class="bi bi-x-lg"></i>
                              </OverlayTrigger>
                            </div>
                          </div>
                          : ''}
                      </div>
                    </Card.Body>
                  </div>
                </div>
              </Card>
            ))
          )
        }
      </div>
      <RatingComponent show={show} handleClose={handleClose} />
      <UpdateOrderModal show={showEdit} handleClose={handleEditClose} selectedOrder={orderObj} />
    </Container>
  )
}

export default Order
