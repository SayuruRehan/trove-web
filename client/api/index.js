import axios from "axios";

const axiosAPI = axios.create({
  baseURL: import.meta.env.VITE_API_URL,
  //   timeout: 5000, // Request timeout in milliseconds
  //   headers: {
  //     "Content-Type": "application/json",
  //     Accept: "application/json",
  //   },
  // withCredentials: true,
});

// Request interceptor
axiosAPI.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem("authToken");
    if (token) {
      config.headers["Authorization"] = `Bearer ${token}`;
    }
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

// Response interceptor
axiosAPI.interceptors.response.use(
  (response) => {
    return response;
  },
  (error) => {
    if (error.response) {
      console.error("Response error:", error.response.data);
      console.error("Status:", error.response.status);
    } else if (error.request) {
      console.error("Request error:", error.request);
    } else {
      console.error("Error:", error.message);
    }
    return Promise.reject(error);
  }
);

export { axiosAPI };
