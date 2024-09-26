import React, {useState, useEffect} from "react";
import axios from "axios";
import {ProgressBar} from "react-bootstrap";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faPhone, faMapMarkerAlt, faUser} from "@fortawesome/free-solid-svg-icons";
import {faStar} from "@fortawesome/free-solid-svg-icons";
import "../../styles/Profile.css";
import Header from "../../components/Header";
import Footer from "../../components/Footer";
import ClipLoader from "react-spinners/ClipLoader";

const Profile = () => {
  const [user, setUser] = useState(null);
  const [ratingsSummary, setRatingsSummary] = useState(null);
  const [reviews, setReviews] = useState([]);
  const [isVendor, setIsVendor] = useState(false);
  const [loading, setLoading] = useState(true);

  const userId = sessionStorage.getItem("userId");

  useEffect(() => {
    const fetchUserProfile = async () => {
      try {
        const response = await axios.get(
            `${process.env.REACT_APP_WEB_API}/Users/${userId}`
        );
        setUser(response.data);

        if (response.data.role === "Vendor") {
          setIsVendor(true);
          await fetchVendorRatingSummary(response.data.userId);
          await fetchVendorReviews(response.data.userId);
        }
      } catch (error) {
        console.error("Error fetching user data:", error);
      }
      setLoading(false);
    };

    const fetchVendorRatingSummary = async (vendorId) => {
      try {
        const response = await axios.get(
            `${process.env.REACT_APP_WEB_API}/rating/vendor/${vendorId}/summary`
        );
        setRatingsSummary(response.data);
      } catch (error) {
        console.error("Error fetching vendor rating summary:", error);
      }
    };

    const fetchVendorReviews = async (vendorId) => {
      try {
        const response = await axios.get(
            `${process.env.REACT_APP_WEB_API}/rating/vendor/${vendorId}`
        );
        const reviewsWithUsernames = await Promise.all(
            response.data.map(async (review) => {
              const userResponse = await axios.get(
                  `${process.env.REACT_APP_WEB_API}/Users/${review.userId}`
              );
              return {...review, username: userResponse.data.username};
            })
        );
        setReviews(reviewsWithUsernames);
      } catch (error) {
        console.error("Error fetching vendor reviews:", error);
      }
    };

    fetchUserProfile();
  }, [userId]);

  if (!user) {
    return <div>Loading...</div>;
  }

  return (
      <>
        <Header />
        <div className="container mt-5">
          <div className="row">
            {/* Left Column */}
            <div className="col-md-6 mb-4">
              {/* Store Basic Information */}
              <div className="card shadow-lg p-4 rounded-lg mb-4">
                <h4 className="mb-3">Store Info</h4>
                <div className="card-body p-0">
                  <p className="card-text">
                    <FontAwesomeIcon icon={faUser} className="me-2" />
                    <strong>Username:</strong> {user.username}
                  </p>
                  <p className="card-text">
                    <FontAwesomeIcon icon={faPhone} className="me-2" />
                    <strong>Phone Number:</strong> {user.phoneNumber}
                  </p>
                  <p className="card-text">
                    <FontAwesomeIcon icon={faMapMarkerAlt} className="me-2" />
                    <strong>Address:</strong> {user.address}
                  </p>
                  <p className="card-text">
                    <FontAwesomeIcon icon={faUser} className="me-2" />
                    <strong>Role:</strong> {user.role}
                  </p>
                </div>
              </div>

              {/* Vendor Ratings */}
              {isVendor && ratingsSummary && (
                  <div className="card shadow-lg p-4 rounded-lg">
                    <h4 className="mb-3">Ratings</h4>
                    <div className="d-flex align-items-center mb-4">
                      <h2 className="me-3 big-rating">
                        {ratingsSummary.averageRating.toFixed(1)}
                      </h2>
                      <p className="small-text">out of 5</p>
                    </div>
                    <h5 className="text-muted">
                      {ratingsSummary.totalReviews} reviews
                    </h5>

                    <div className="star-distribution mt-3">
                      {[5, 4, 3, 2, 1].map((star) => (
                          <div className="d-flex align-items-center mb-2" key={star}>
                            <FontAwesomeIcon icon={faStar} className="text-warning me-2" />
                            <span>{star} Star</span>
                            <ProgressBar
                                now={
                                    (ratingsSummary.starDistribution[star] /
                                        ratingsSummary.totalReviews) *
                                    100
                                }
                                className="flex-grow-1 mx-3"
                                style={{ height: "8px" }}
                            />
                          </div>
                      ))}
                    </div>
                  </div>
              )}
            </div>

            {/* Right Column (Reviews Section) */}
            <div className="col-md-6">
              <div className="card shadow-lg p-4 rounded-lg">
                <h4 className="mb-3">Reviews</h4>
                <div className="reviews-scroll" style={{ maxHeight: "400px", overflowY: "auto" }}>
                  {reviews.length > 0 ? (
                      reviews.map((review) => (
                          <div className="mb-3 card p-3 shadow-sm" key={review.ratingId}>
                            <h6>
                              <FontAwesomeIcon icon={faStar} className="text-warning me-2" />
                              {review.ratingValue} / 5
                            </h6>
                            <p>{review.comment}</p>
                            <p className="text-muted">
                              Posted on {new Date(review.datePosted).toLocaleDateString()} by {review.username}
                            </p>
                          </div>
                      ))
                  ) : (
                      <p>No reviews yet.</p>
                  )}
                </div>
              </div>
            </div>
          </div>
        </div>

        {loading && (
            <div className="d-flex justify-content-center my-5">
              <ClipLoader
                  color="#000"
                  loading={loading}
                  size={150}
                  aria-label="Loading Spinner"
                  data-testid="loader"
              />
            </div>
        )}
        <Footer />
      </>
  );
};

export default Profile;
