import { useState, useEffect } from 'react';
import Button from 'react-bootstrap/Button';
import Form from 'react-bootstrap/Form';
import Modal from 'react-bootstrap/Modal';
import APIService from '../../../APIService/APIService';
import { ToastContainer, toast } from "react-toastify";

const UpdateOrderModal = ({ show, handleClose, selectedOrder }) => {
    const [userName, setName] = useState("");
    const [shippingAddress, setShippingAddress] = useState("");
    const [mobileNumber, setMobile] = useState("");
    const [btnLoading,setBtnLoading] = useState(false)

    //if selectedOrder exits update fields accordingly
    useEffect(() => {
        if (selectedOrder) {
            setName(selectedOrder.userName || "");
            setShippingAddress(selectedOrder.shippingAddress || "");
            setMobile(selectedOrder.mobileNumber || "");
        }
    }, [selectedOrder]);

    //update fields
    const handleSaveChanges = async () => {

        setBtnLoading(true)

        let updatedOrderObj = { //create updated object
            ...selectedOrder,
            userName,
            shippingAddress,
            mobileNumber,
        }
        try {
            const response = await APIService.updateOrderDetails(updatedOrderObj, updatedOrderObj.id)
            console.log(response)
            if (response.status == 200) {
                toast.success("Order details Updated Successfully!", {
                    autoClose: 250,
                    position: "top-right",
                });
            }
        } catch (err) {
            toast.error("Error updating Order details!", {
                autoClose: 250,
                position: "top-right",
            });
            console.error('Error updating order')
        } finally{
            setBtnLoading(false)
            handleClose();
        }
    };

    return (
        <div>
            <Modal show={show} onHide={handleClose}>
                <Modal.Header closeButton>
                    <Modal.Title>Update order details</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <Form>
                        <Form.Group className="mb-3" controlId="exampleForm.ControlInput1">
                            <Form.Label>Your Name:</Form.Label>
                            <Form.Control
                                required
                                type="text"
                                value={userName}
                                onChange={(e) => setName(e.target.value)}
                                placeholder="Enter name"
                                autoFocus
                            />
                        </Form.Group>
                        <Form.Group className="mb-3" controlId="exampleForm.ControlInput2">
                            <Form.Label>Enter mobile:</Form.Label>
                            <Form.Control
                                required
                                type="text"
                                maxLength={10}
                                value={mobileNumber}
                                onChange={(e) => setMobile(e.target.value)}
                                placeholder="Enter mobile number"
                            />
                        </Form.Group>
                        <Form.Group className="mb-3" controlId="exampleForm.ControlInput3">
                            <Form.Label>Shipping address:</Form.Label>
                            <Form.Control
                                required
                                type="text"
                                value={shippingAddress}
                                onChange={(e) => setShippingAddress(e.target.value)}
                                placeholder="Enter shipping address"
                            />
                        </Form.Group>
                    </Form>
                </Modal.Body>
                <Modal.Footer>
                    <Button variant="secondary" onClick={handleClose}>
                        Close
                    </Button>
                    <button class="btn btn-primary" type="button" disabled={btnLoading} onClick={handleSaveChanges}>
                       <span class="sr-only">Save Changes</span>
                      {btnLoading  ? <span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span>  : ''}
                    </button>
                </Modal.Footer>
            </Modal>
        </div>
    );
};

export default UpdateOrderModal;
