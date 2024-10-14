import React, {useState, useEffect} from "react";
import axios from "axios";
import {ProgressBar} from "react-bootstrap";
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
  const userRole = sessionStorage.getItem("role");

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
      <div className="container">
        <div className="row">
          {/* Left Column */}
          <div className="col-md-6">
            {/* Store Basic Information */}
            <div className="mb-4 store-info-section">
              <h4>Store Info</h4>
              <div className="card">
                <div className="card-body">
                  <p className="card-text">
                    <strong>Username:</strong> {user.username}
                  </p>
                  <p className="card-text">
                    <strong>Phone Number:</strong> {user.phoneNumber}
                  </p>
                  <p className="card-text">
                    <strong>Address:</strong> {user.address}
                  </p>
                  <p className="card-text">
                    <strong>Role:</strong> {user.role}
                  </p>
                </div>
              </div>
            </div>

            {/* Vendor Ratings */}
            {isVendor && ratingsSummary && (
              <div className="ratings-section">
                <h4>Ratings</h4>
                <div className="rating-summary">
                  <div className="average-rating">
                    <h2>
                      <span className="big-rating">
                        {ratingsSummary.averageRating.toFixed(1)}
                      </span>
                      <span className="small-text"> out of 5</span>
                    </h2>
                    <h5 className="no-of-ratings">
                      {ratingsSummary.totalReviews} reviews
                    </h5>
                  </div>

                  <div className="star-distribution">
                    {[5, 4, 3, 2, 1].map((star) => (
                      <div className="star-rating" key={star}>
                        <span>{star} Star</span>
                        <div className="progress-bar-container">
                          <ProgressBar
                            now={
                              (ratingsSummary.starDistribution[star] /
                                ratingsSummary.totalReviews) *
                              100
                            }
                            //   label={`${ratingsSummary.starDistribution[star]} reviews`}
                          />
                        </div>
                      </div>
                    ))}
                  </div>
                </div>
              </div>
            )}
          </div>

          {/* Right Column (Reviews Section) */}
          <div className="col-md-6 reviews-section">
            <h4>Reviews</h4>
            <div className="reviews-scroll">
              {reviews.length > 0 ? (
                reviews.map((review) => (
                  <div className="mb-3 card" key={review.ratingId}>
                    <div className="card-body">
                      <h6>Rating: {review.ratingValue} / 5</h6>
                      <p>{review.comment}</p>
                      <p className="text-muted">
                        Posted on{" "}
                        {new Date(review.datePosted).toLocaleDateString()} by{" "}
                        {review.username}
                      </p>
                    </div>
                  </div>
                ))
              ) : (
                <p>No reviews yet.</p>
              )}
            </div>
          </div>
        </div>
      </div>
      <div className="flex justify-center">
        <ClipLoader
          color="#000"
          loading={loading}
          size={150}
          aria-label="Loading Spinner"
          data-testid="loader"
        />
      </div>
      <Footer />
    </>
  );
};

export default Profile;
