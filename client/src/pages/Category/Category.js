import React, {useEffect, useState} from "react";
import axios from "axios";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faEdit, faTrash, faPlus} from "@fortawesome/free-solid-svg-icons";
import {Modal, Button} from "react-bootstrap";
import "bootstrap/dist/css/bootstrap.min.css";
import Header from "../../components/Header";
import Footer from "../../components/Footer";
import {toast} from "react-toastify";
import ClipLoader from "react-spinners/ClipLoader";

const Category = () => {
  const [categories, setCategories] = useState([]);
  const [selectedCategory, setSelectedCategory] = useState(null);
  const [showModal, setShowModal] = useState(false);
  const [modalType, setModalType] = useState("");
  const [newCategoryData, setNewCategoryData] = useState({
    categoryName: "",
    categoryDescription: "",
    isActive: true,
  });
  const [errors, setErrors] = useState({});
  const [loading, setLoading] = useState(true);

  // Fetch all categories
  const fetchCategories = async () => {
    try {
      const response = await axios.get(
        `${process.env.REACT_APP_WEB_API}/Category`
      );
      setCategories(response.data);
    } catch (error) {
      console.error("Error fetching categories:", error);
      alert("Failed to fetch categories.");
    }
    setLoading(false);
  };

  useEffect(() => {
    fetchCategories();
  }, []);

  // Handle modal actions
  const handleShowModal = (type, category = null) => {
    setSelectedCategory(category);
    setModalType(type);
    setErrors({});

    if (category) {
      setNewCategoryData({
        categoryName: category.categoryName,
        categoryDescription: category.categoryDescription,
        isActive: category.isActive,
      });
    } else {
      setNewCategoryData({
        categoryName: "",
        categoryDescription: "",
        isActive: true,
      });
    }
    setShowModal(true);
  };

  const handleCloseModal = () => {
    setShowModal(false);
  };

  // Validate form fields
  const validateForm = () => {
    const newErrors = {};
    if (!newCategoryData.categoryName) {
      newErrors.categoryName = "Category Name is required";
    }
    if (
      newCategoryData.isActive === undefined ||
      newCategoryData.isActive === null
    ) {
      newErrors.isActive = "Active/Inactive status is required";
    }
    setErrors(newErrors);
    return Object.keys(newErrors).length === 0; // Return true if no errors
  };

  // Handle Create or Update Category
  const handleSaveCategory = async () => {
    if (!validateForm()) {
      return;
    }

    try {
      if (modalType === "create") {
        await axios.post(
          `${process.env.REACT_APP_WEB_API}/Category`,
          newCategoryData
        );
        alert("Category created successfully.");
      } else if (modalType === "edit") {
        await axios.put(
          `${process.env.REACT_APP_WEB_API}/Category/${selectedCategory.categoryId}`,
          newCategoryData
        );
        alert("Category updated successfully.");
      }

      fetchCategories();
      handleCloseModal();
    } catch (error) {
      console.error(
        `Error ${modalType === "create" ? "creating" : "updating"} category:`,
        error
      );
      alert(
        `Failed to ${modalType === "create" ? "create" : "update"} category.`
      );
    }
  };

  // Handle Delete
  const handleDeleteCategory = async () => {
    try {
      await axios.delete(
        `${process.env.REACT_APP_WEB_API}/Category/${selectedCategory.categoryId}`
      );
      setCategories(
        categories.filter(
          (category) => category.categoryId !== selectedCategory.categoryId
        )
      );
      handleCloseModal();
      alert("Category deleted successfully.");
    } catch (error) {
      console.error("Error deleting category:", error);
      alert("Failed to delete category.");
    }
  };

  return (
    <>
      <Header />
      <div className="ml-10 mr-10 mb-10">
        <div className="flex justify-center">
          <h2>Category List</h2>
        </div>
        <Button
          variant="primary"
          className="mb-3 float-end"
          onClick={() => handleShowModal("create")}
        >
          <FontAwesomeIcon icon={faPlus} /> Create New Category
        </Button>
        <table className="table table-bordered table-striped">
          <thead>
            <tr>
              <th>#</th>
              <th>Category Name</th>
              <th style={{width: "50%"}}>Description</th>
              <th>Status</th>
              <th>Actions</th>
            </tr>
          </thead>
          <tbody>
            {categories.length > 0 ? (
              categories.map((category, index) => (
                <tr key={category.categoryId}>
                  <td>{index + 1}</td>
                  <td>{category.categoryName}</td>
                  <td>{category.categoryDescription || "No Description"}</td>
                  <td
                    style={{
                      color: category.isActive ? "green" : "red",
                      fontWeight: "bold",
                    }}
                  >
                    {category.isActive ? "Active" : "Inactive"}
                  </td>
                  <td>
                    <button
                      className="mx-1 btn btn-warning btn-sm"
                      onClick={() => handleShowModal("edit", category)}
                    >
                      <FontAwesomeIcon icon={faEdit} />
                    </button>
                    <button
                      className="mx-1 btn btn-danger btn-sm"
                      onClick={() => handleShowModal("delete", category)}
                    >
                      <FontAwesomeIcon icon={faTrash} />
                    </button>
                  </td>
                </tr>
              ))
            ) : (
              <tr>
                <td colSpan="5" className="text-center">
                  No categories available
                </td>
              </tr>
            )}
          </tbody>
        </table>
        <div className="flex justify-center">
          <ClipLoader
            color="#000"
            loading={loading}
            size={150}
            aria-label="Loading Spinner"
            data-testid="loader"
          />
        </div>
      </div>

      {/* Modal for create, edit, and delete */}
      <Modal show={showModal} onHide={handleCloseModal}>
        <Modal.Header closeButton>
          <Modal.Title>
            {modalType === "create"
              ? "Create New Category"
              : modalType === "edit"
              ? "Edit Category"
              : "Delete Category"}
          </Modal.Title>
        </Modal.Header>
        <Modal.Body>
          {modalType === "create" || modalType === "edit" ? (
            <form>
              <div className="form-group">
                <label>Category Name</label>
                <input
                  type="text"
                  className="form-control"
                  value={newCategoryData.categoryName}
                  onChange={(e) =>
                    setNewCategoryData({
                      ...newCategoryData,
                      categoryName: e.target.value,
                    })
                  }
                />
                {errors.categoryName && (
                  <small className="text-danger">{errors.categoryName}</small>
                )}
              </div>
              <div className="form-group">
                <label>Description (Optional)</label>
                <textarea
                  className="form-control"
                  value={newCategoryData.categoryDescription}
                  onChange={(e) =>
                    setNewCategoryData({
                      ...newCategoryData,
                      categoryDescription: e.target.value,
                    })
                  }
                ></textarea>
              </div>
              <div className="form-group">
                <label>Active Status</label>
                <select
                  className="form-control"
                  value={newCategoryData.isActive}
                  onChange={(e) =>
                    setNewCategoryData({
                      ...newCategoryData,
                      isActive: e.target.value === "true",
                    })
                  }
                >
                  <option value={true}>Active</option>
                  <option value={false}>Inactive</option>
                </select>
                {errors.isActive && (
                  <small className="text-danger">{errors.isActive}</small>
                )}
              </div>
            </form>
          ) : modalType === "delete" ? (
            <p>Are you sure you want to delete this category?</p>
          ) : null}
        </Modal.Body>
        <Modal.Footer>
          <Button variant="secondary" onClick={handleCloseModal}>
            Close
          </Button>
          {modalType === "create" || modalType === "edit" ? (
            <Button variant="primary" onClick={handleSaveCategory}>
              {modalType === "create" ? "Create" : "Save Changes"}
            </Button>
          ) : modalType === "delete" ? (
            <Button variant="danger" onClick={handleDeleteCategory}>
              Delete
            </Button>
          ) : null}
        </Modal.Footer>
      </Modal>

      <Footer />
    </>
  );
};

export default Category;
