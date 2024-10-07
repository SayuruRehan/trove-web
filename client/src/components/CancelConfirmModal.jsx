import React from 'react'
import Button from 'react-bootstrap/Button';
import Form from 'react-bootstrap/Form';
import Modal from 'react-bootstrap/Modal';

const CancelConfirmModal = ({ show, handleClose, confirmCancel, setComment}) => {

    return (
        <div>
            <Modal show={show} onHide={handleClose}>
                <Modal.Header closeButton>
                    <Modal.Title>Confirm Order Cancellation</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <Form>
                        <Form.Group className="mb-3" controlId="exampleForm.ControlTextarea1">
                            <Form.Label>Enter comment:</Form.Label>
                            <Form.Control as="textarea" rows={3} onChange={(e) => setComment(e.target.value)} />
                        </Form.Group>
                    </Form>
                </Modal.Body>
                <Modal.Footer>
                    <Button variant="secondary" onClick={handleClose}>
                        Close
                    </Button>
                    <Button variant="primary" onClick={confirmCancel}>
                        Confirm Cancel
                    </Button>
                </Modal.Footer>
            </Modal>
        </div>
    )
}

export default CancelConfirmModal
