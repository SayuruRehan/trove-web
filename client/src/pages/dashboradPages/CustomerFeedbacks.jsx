import React, { useState, useEffect } from "react";
import VendorService from "../../../APIService/VendorService";
import { useAuth } from "../../context/authContext";

const CustomerFeedbacks = () => {
  const [feedbacks, setFeedbacks] = useState([]);
  const [rating, setRating] = useState(0);
  const [error, setError] = useState(null);

  const { user } = useAuth();

  useEffect(() => {
    if (feedbacks.length > 0) {
      const totalRating = feedbacks.reduce(
        (acc, feedback) => acc + feedback.rating,
        0
      );
      setRating(totalRating / feedbacks.length);
    } else {
      setRating(0);
    }
  }, [feedbacks]);

  const fetchVendorData = async () => {
    try {
      const response = await VendorService.getCustomerFeedback();
      setFeedbacks(response.data);
    } catch (error) {
      setError("Error fetching Vendor data...");
      console.log("Error fetching Vendor data", error);
    }
  };

  useEffect(() => {
    fetchVendorData();
  }, [user]);

  console.log(feedbacks);

  const calculateAverageRating = () => {
    //(sum += rating) / rating.count  5+7 = 12/2 = 6/2 * 100
  };

  return (
    <div className="container mt-5">
      <div className="d-flex justify-content-between mb-4">
        <div>
          <h4 className="text-center text-primary">Customer Feedbacks</h4>
        </div>
        <div className="ml-auto">
          <h4 className="text-primary">Average Rating</h4>
          <span>
            <h4>
              {rating * 10}
              <i className="bi bi-star-fill" style={{ color: "#FFD700" }}></i>
            </h4>
          </span>
        </div>
      </div>
      <table className="table table-hover text-center">
        <thead className="table-light">
          <tr>
            <th>First Name</th>
            <th>Last Name</th>
            <th>Feedback</th>
            <th>Rating</th>
          </tr>
        </thead>
        <tbody className="table-light">
          {feedbacks.length > 0 ? (
            feedbacks.map((feedback) => (
              <tr key={feedback.feedbackId}>
                <td>{feedback.firstName}</td>
                <td>{feedback.lastName}</td>
                <td>{feedback.customerFeedbackText}</td>
                <td>{feedback.rating}</td>
              </tr>
            ))
          ) : (
            <td colSpan="3">No feedback available.</td>
          )}
        </tbody>
      </table>
    </div>
  );
};

export default CustomerFeedbacks;
