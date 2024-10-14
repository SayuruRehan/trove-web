// NotificationPanel.js
import React, {useState, useEffect} from "react";
import axios from "axios";
import "./NotificationPanel.css";
import ClipLoader from "react-spinners/ClipLoader";

const NotificationPanel = () => {
  const [isOpen, setIsOpen] = useState(false);
  const [notifications, setNotifications] = useState([]);
  const [loading, setLoading] = useState(true);

  // Assuming role and userId are available in sessionStorage
  const userId = sessionStorage.getItem("userId");
  const role = sessionStorage.getItem("role");

  const togglePanel = () => {
    setIsOpen(!isOpen);
  };

  useEffect(() => {
    if (isOpen) {
      fetchNotifications();
    }
  }, [isOpen]);

  const fetchNotifications = async () => {
    try {
      setLoading(true);
      const response = await axios.get(
        `${process.env.REACT_APP_WEB_API}/notification`
      );
      const allNotifications = response.data;

      // Filter notifications based on role and userId
      const filteredNotifications = allNotifications.filter((notification) => {
        // Show notifications for the matching role or specific user if userId is present
        return (
          notification.role === role &&
          (!notification.userId || notification.userId === userId)
        );
      });

      setNotifications(filteredNotifications.reverse());
    } catch (error) {
      console.error("Error fetching notifications:", error);
    } finally {
      setLoading(false);
    }
  };

  const handleDelete = async (notificationId) => {
    try {
      await axios.delete(
        `${process.env.REACT_APP_WEB_API}/notification/${notificationId}`
      );
      // Remove the deleted notification from the state
      setNotifications((prevNotifications) =>
        prevNotifications.filter(
          (notification) => notification.notificationId !== notificationId
        )
      );
    } catch (error) {
      console.error("Error deleting notification:", error);
    }
  };

  return (
    <div>
      <div className="pr-3" onClick={togglePanel}>
        <ion-icon name="notifications-outline" size="large"></ion-icon>
      </div>
      {isOpen && (
        <div className="notification-panel">
          <h3>Notifications</h3>
          {loading ? (
            <p>Loading notifications...</p>
          ) : (
            <ul>
              {notifications.length > 0 ? (
                notifications.map((notification) => (
                  <li
                    key={notification.notificationId}
                    className="notification-item"
                  >
                    <div className="notification-content">
                      <strong>{notification.title}</strong>
                      <p>{notification.message}</p>
                      <small>
                        {new Date(notification.dateTime).toLocaleString()}
                      </small>
                    </div>
                    <div
                      className="delete-icon"
                      onClick={() => handleDelete(notification.notificationId)}
                    >
                      <ion-icon name="trash-outline" size="small"></ion-icon>
                    </div>
                  </li>
                ))
              ) : (
                <p>No notifications available.</p>
              )}
            </ul>
          )}
          <ClipLoader
            color="#000"
            loading={loading}
            size={50}
            aria-label="Loading Spinner"
            data-testid="loader"
          />
        </div>
      )}
    </div>
  );
};

export default NotificationPanel;
