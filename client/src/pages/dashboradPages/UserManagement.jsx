import React, { useState } from 'react'
import Table from 'react-bootstrap/Table';
import Container from 'react-bootstrap/Container';
import Button from 'react-bootstrap/Button';
import Dropdown from 'react-bootstrap/Dropdown';
import Badge from 'react-bootstrap/Badge';


const UserManagements = () => {

  const [users, setUsers] = useState([
    {
      id:1,
      name: 'anoj',
      email: 'anoj@gmail.com',
      mobile: '0772812121',
      status: 'Active'
    },
    {
      id:2,
      name: 'harini',
      email: 'harini@gmail.com',
      mobile: '0772812121',
      status: 'Deactive'
    },
  ])

  const handleAccountStatus = (id,status) => {
    setUsers(users.map(user =>
      user.id === id ? {...user,status} : user
    ))
  }

  return (
    <Container className='mt-4'>
      <div className='d-flex align-items-center justify-content-center'>
        <p></p>
        <h4>Users</h4>
      </div>
      <Table striped bordered hover className='mt-2'>
        <thead>
          <tr>
            <th>Name</th>
            <th>Email</th>
            <th>Mobile</th>
            <th>Status</th>
          </tr>
        </thead>
        <tbody>
          {
            users.length > 0 && users.map((user, index) => (
              <tr key={index}>
                <td>{user.name}</td>
                <td>{user.email}</td>
                <td>{user.mobile}</td>
                <td className='d-flex align-items-center'>
                  <Dropdown className='text-center'>
                    <Dropdown.Toggle variant='info' style={{ backgroundColor: "white" }} className='dropdown_btn' id="dropdown-basic">
                      <Badge bg={user.status == "Deactive" ? "danger" : "success"} className='p-1'>
                        {user.status} <i className="fa-solid fa-angle-down"></i>
                      </Badge>
                    </Dropdown.Toggle>
                    <Dropdown.Menu>
                      <Dropdown.Item onClick={() => handleAccountStatus(user.id, "Active")}>Active</Dropdown.Item>
                      <Dropdown.Item onClick={() => handleAccountStatus(user.id, "Deactive")}>Deactive</Dropdown.Item>
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

export default UserManagements
