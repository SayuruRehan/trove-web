import React, { useEffect, useState } from 'react'
import Table from 'react-bootstrap/Table';
import Container from 'react-bootstrap/Container';
import Button from 'react-bootstrap/Button';
import Dropdown from 'react-bootstrap/Dropdown';
import Badge from 'react-bootstrap/Badge';
import UserService from '../../../APIService/UserService';
import { ToastContainer, toast } from "react-toastify";
import VendorService from '../../../APIService/VendorService';

const VendorAccountManagement = () => {

    const [vendors, setVendors] = useState([])
    console.log(vendors)

    //fetch all users
    const getAllUsers = async () => {
        try {
            const response = await VendorService.getUnapprovedVendors()
            setVendors(response?.data)
        } catch (err) {
            console.error('Error fetching vendors')
        }
    }

    //change status of the user
    const handleAccountStatus = async (userId) => {
        try {
            const response = await UserService.updateUserStatus(userId)
            if (response.status == 200) {
                toast.success("Vendor account activated Successfully!", {
                    autoClose: 250,
                    position: "top-right",
                });
                getAllUsers()
            }
        } catch (err) {
            console.error('Error Updating status of user')
        }

    }

    //execute ones
    useEffect(() => {
        getAllUsers()
    }, [])

    return (
        <Container className='mt-4'>
            <div className='d-flex align-items-center justify-content-center'>
                <p></p>
                <h4>Unapproved Vendors</h4>
            </div>
            <Table striped bordered hover className='mt-2'>
                <thead>
                    <tr>
                        <th>FirstName</th>
                        <th>LastName</th>
                        <th>Email</th>
                        <th>Mobile</th>
                        <th>Status</th>
                    </tr>
                </thead>
                <tbody>
                    {
                        vendors.length > 0 && vendors.map((user, index) => (
                            <tr key={index}>
                                <td>{user.firstname}</td>
                                <td>{user.lastname}</td>
                                <td>{user.email}</td>
                                <td>{user.phone}</td>
                                <td className='d-flex align-items-center'>
                                    <Dropdown className='text-center'>
                                        <Dropdown.Toggle variant='info' style={{ backgroundColor: "white" }} className='dropdown_btn' id="dropdown-basic">
                                            <Badge bg={user.isApproved == 0 ? "danger" : "success"} className='p-1'>
                                                {user.isApproved == false ? 'Deactive' : 'Active'} <i className="fa-solid fa-angle-down"></i>
                                            </Badge>
                                        </Dropdown.Toggle>
                                        <Dropdown.Menu>
                                            <Dropdown.Item onClick={() => handleAccountStatus(user.id)}>Active</Dropdown.Item>
                                        </Dropdown.Menu>
                                    </Dropdown>
                                </td>
                            </tr>
                        ))
                    }
                </tbody>
            </Table>

        </Container>
    )
}

export default VendorAccountManagement
