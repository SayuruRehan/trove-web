import React, { useEffect, useState } from 'react'
import Table from 'react-bootstrap/Table';
import Container from 'react-bootstrap/Container';
import { Link } from "react-router-dom";
import Dropdown from 'react-bootstrap/Dropdown';
import Badge from 'react-bootstrap/Badge';
import OverlayTrigger from 'react-bootstrap/OverlayTrigger';
import Tooltip from 'react-bootstrap/Tooltip';
import APIService from '../../APIService/APIService';
import { ToastContainer, toast } from "react-toastify";

const AllOrder = () => {

    const [orders, setOrders] = useState([])
    const [filterOrders, setFilterOrders] = useState([])
    const [loading, setLoading] = useState(false)
    console.log(orders)
    //get cancellation orders count
    useEffect(() => {
        setFilterOrders(orders.filter(order => order.status === 'Cancelled'));
    }, [orders]);

    //fetch all orders  
    const fetchOrders = async () => {
        try {
            const response = await APIService.getOrderWithItems()
            setOrders(response.data)
        } catch (err) {
            console.error('Error fetching orders')
        } finally {
            setLoading(false)
        }
    }

    //fetch all orders once
    useEffect(() => {
        fetchOrders()
    }, [])

    //change status when toggling
    const handleStatusChanging = async (orderObj, status) => {
        try {
            const updatedObj = { ...orderObj, status }
            const response = await APIService.updateOrderDetails(updatedObj, updatedObj.id)
            console.log(response)
            if (response.status == 200) {
                toast.success("Order Status Update Success!", {
                    autoClose: 300,
                    position: "top-right",
                  });
                setOrders(orders.map(order =>
                    order.id === orderObj.id ? { ...order, status } : order
                ))
            }
        } catch (err) {
            console.log('error updating order status')
        }

    }

    //format date & time
    function formateDate(dataString) {
        const date = new Date(dataString)
        const year = date.getFullYear()
        const month = String(date.getMonth() + 1).padStart(2, '0')
        const day = String(date.getDate()).padStart(2, '0')

        return `${year}.${month}.${day}`
    }

    //adding thousand seperate
    function formatNumberWithCommas(number) {
        // Convert number to string and split it into parts
        const parts = number.toString().split('.');
        // Use a regular expression to add commas
        parts[0] = parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, ",");
        // Join the parts back together
        return parts.join('.');
    }

    return (
        <Container className='mt-4'>
            <div className='d-flex align-items-baseline justify-content-between'>
                <p></p>
                <h4>All Orders</h4>
                <OverlayTrigger overlay={<Tooltip id="tooltip-disabled">Order cancellation requests</Tooltip>}>
                    <Link to="/cancelRequest"><i class="bi bi-bell-fill" style={{ fontSize: '1.92rem' }}></i>
                        <Badge bg="secondary" className="position-absolute top-4">{filterOrders.length}</Badge>
                    </Link>
                </OverlayTrigger>
            </div>
            <Table striped bordered hover className='mt-2'>
                <thead>
                    <tr>
                        <th>Order Id</th>
                        <th>Total Amount</th>
                        <th>Ordered Date</th>
                        <th>Shipping Address</th>
                        <th>Vendor Name(s)</th>
                        <th>Order Status</th>
                    </tr>
                </thead>
                <tbody>
                    {
                        orders.length > 0 ? orders.map((order, index) => (
                            <tr key={index}>
                                <td>{order.orderId}</td>
                                <td>{formatNumberWithCommas(order.totalAmount)}</td>
                                <td>{formateDate(order.createdAt)}</td>
                                <td>{order.shippingAddress}</td>
                                <td>
                                    {
                                        [...new Set(order.orderItems?.map(item => item.vendorName))].join(', ')
                                    }
                                </td>
                                <td className='d-flex align-items-center'>
                                    <Dropdown className='text-center'>
                                        <Dropdown.Toggle variant='info' style={{ backgroundColor: "white" }} className='dropdown_btn' id="dropdown-basic">
                                            <Badge bg={
                                                order.status === "Pending" ? "info" :
                                                    order.status === "Delivered" ? "success" :
                                                        order.status === "PartiallyDelivered" ? "warning" :
                                                            order.status === "Cancelled" ? "danger" : "secondary"
                                            } className='p-1'>
                                                {order.status} <i className="fa-solid fa-angle-down"></i>
                                            </Badge>
                                        </Dropdown.Toggle>
                                        <Dropdown.Menu>
                                            <Dropdown.Item onClick={() => handleStatusChanging(order, "Pending")}>Pending</Dropdown.Item>
                                            <Dropdown.Item onClick={() => handleStatusChanging(order, "Delivered")}>Delivered</Dropdown.Item>
                                            <Dropdown.Item onClick={() => handleStatusChanging(order, "PartiallyDelivered")}>PartiallyDelivered</Dropdown.Item>
                                        </Dropdown.Menu>
                                    </Dropdown>
                                </td>
                            </tr>
                        )) : <tr><td>No orders were found</td></tr>
                    }
                </tbody>
            </Table>
        </Container>
    )
}

export default AllOrder
