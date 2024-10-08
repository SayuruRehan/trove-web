import React, { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { toast } from "react-toastify";
import "react-toastify/ReactToastify.css";
import VendorService from "../../../APIService/VendorService";

const UpdateVendor = () => {
  const [vendor, setVendor] = useState({
    vendorName: "",
    vendorEmail: "",
    vendorPhone: "",
    vendorAddress: "",
    vendorCity: "",
    isActive: false,
  });
  const [error, setError] = useState(null);

  const { id } = useParams();
  const navigate = useNavigate();

  useEffect(() => {
    const abortController = new AbortController();

    const fetchVendorData = async () => {
      try {
        const response = await VendorService.getVendorById(id, {
          signal: abortController.signal,
        });
        setVendor(response.data);
      } catch (error) {
        if (error === "AbortError") {
          console.log("Fetch aborted!");
        } else {
          setError("Error fetching Vendor data...");
          console.log("Error fetching Vendor data", error);
        }
      }
    };
    fetchVendorData();

    return () => {
      abortController.abort();
    };
  }, [id]);

  console.log(">>>", vendor);

  const handleChange = (e) => {
    const { name, value } = e.target;
    const updatedValue = name === "isActive" ? value === "active" : value;
    setVendor({ ...vendor, [name]: updatedValue });
  };

  const updateVendorDetails = async (e) => {
    e.preventDefault();
    try {
      await VendorService.updateVendorDetails(id, vendor);
      toast.success("Vendor updated successfully!");
      navigate(`/dashboard/vendors`);
    } catch (error) {
      setError("Error occured");
      console.log("Error updating Vendor details");
    }
  };

  return (
    <>
      <div className="container my-4">
        <div className="card shadow-sm p-4">
          <h3 className="mb-4">Update Vendor Details</h3>
          <form onSubmit={updateVendorDetails}>
            <div className="row">
              <div className="form-group col-md-6">
                <label htmlFor="vendorName" className="m-2">
                  Name
                </label>
                <input
                  type="text"
                  className="form-control"
                  id="vendorName"
                  name="vendorName"
                  placeholder="Enter a name"
                  value={vendor.vendorName}
                  onChange={handleChange}
                />
                <label htmlFor="vendorEmail" className="m-2">
                  Email
                </label>
                <input
                  type="text"
                  className="form-control"
                  id="vendorEmail"
                  name="vendorEmail"
                  placeholder="Enter a email"
                  value={vendor.vendorEmail}
                  onChange={handleChange}
                />
              </div>
              <div className="form-group col-md-6">
                <label htmlFor="vendorPhone" className="m-2">
                  Phone Number
                </label>
                <input
                  type="text"
                  className="form-control"
                  id="vendorPhone"
                  name="vendorPhone"
                  placeholder="Enter a phone number"
                  value={vendor.vendorPhone}
                  onChange={handleChange}
                />

                <label htmlFor="vendorAddress" className="m-2">
                  Address Location
                </label>
                <input
                  type="text"
                  className="form-control"
                  id="vendorAddress"
                  name="vendorAddress"
                  placeholder="Enter a address"
                  value={vendor.vendorAddress}
                  onChange={handleChange}
                />
              </div>
              <div className="form-group col-md-6">
                <label htmlFor="vendorCity" className="m-2">
                  City
                </label>
                <input
                  type="text"
                  className="form-control"
                  id="vendorCity"
                  name="vendorCity"
                  placeholder="Enter a city"
                  value={vendor.vendorCity}
                  onChange={handleChange}
                />
              </div>
              <div className="form-group col-md-6">
                <label
                  htmlFor="isActive"
                  className="mt-3"
                  style={{ marginLeft: "0.5rem" }}
                >
                  Active Status
                </label>
                <select
                  id="isActive"
                  name="isActive"
                  value={vendor.isActive ? "active" : "inactive"}
                  onChange={handleChange}
                  style={{
                    marginLeft: "1rem",
                    padding: "0.5rem",
                    cursor: "pointer",
                    borderRadius: "4px",
                    border: "1px solid #ccc",
                  }}
                >
                  <option value="active">Active</option>
                  <option value="inactive">In-active</option>
                </select>
              </div>
            </div>
            <div className="form-group">
              <label htmlFor="products" className="m-2">
                Products
              </label>
            </div>
            <div className="form-group">
              <label htmlFor="feedbacks" className="m-2">
                Feedbacks
              </label>
            </div>
            <div className="d-flex align-items-center justify-content-end mt-3">
              <button
                type="button"
                className="btn btn-primary m-1"
                style={{ width: "7rem", fontSize: "18px" }}
                onClick={() => navigate(`/dashboard/vendors`)}
              >
                Cancel
              </button>
              <button
                type="submit"
                className="btn btn-success m-1"
                style={{ width: "7rem", fontSize: "18px" }}
              >
                Update
              </button>
            </div>
          </form>
        </div>
      </div>
    </>
  );
};

export default UpdateVendor;
