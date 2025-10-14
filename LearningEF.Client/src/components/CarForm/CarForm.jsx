import React, { useState, useEffect } from "react";
import { addCar, updateCar } from "../../api/carService"; // Import the service function
import TooltipWrapper from "../../utils/TooltipWrapper/TooltipWrapper";
import styles from "./CarForm.module.css";

// Define the initial empty state for a new car
const initialCarState = {
  make: "",
  model: "",
  year: "", // Keep this as a string for input purposes
  color: "",
  price: "", // Keep this as a string for input purposes
};

const CarForm = ({ initialCar, onSubmissionSuccess, onCancel }) => {
  // Determine mode: If initialCar exists, we are in Edit mode
  const isEditing = !!initialCar;

  const [formData, setFormData] = useState(initialCarState);
  // Add a simple state for feedback/UX
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [error, setError] = useState(null);

  // Use useEffect to load data when the component mounts or initialCar changes
  useEffect(() => {
    if (isEditing && initialCar) {
      // Pre-fill the form with the car data
      // Convert year and price back to strings for the input fields
      setFormData({
        make: initialCar.make,
        model: initialCar.model,
        year: initialCar.year.toString(),
        color: initialCar.color,
        price: initialCar.price.toString(),
      });
    } else {
      // Clear the form if we switch back to Add mode (initialCar is null)
      setFormData(initialCarState);
    }
  }, [initialCar, isEditing]); // Dependency array runs this effect when initialCar changes

  // Create a universal handler for input changes
  const handleInputChange = (e) => {
    const { name, value } = e.target;

    // Use the functional update to ensure correct state update
    setFormData((prevData) => ({
      ...prevData,
      [name]: value,
    }));
  };

  // Create the submission handler
  const handleSubmit = async (e) => {
    e.preventDefault(); // REQUIRED: Stops the browser's default form submission (page reload)

    // Basic data conversion/validation
    const carDataToSend = {
      // Pass the CarId only in EDIT mode
      ...(isEditing && { carId: initialCar.carId }),
      ...formData,
      year: parseInt(formData.year),
      price: parseFloat(formData.price),
    };

    // Basic validation check
    if (isNaN(carDataToSend.year) || isNaN(carDataToSend.price)) {
      setError("Please ensure Year and Price are valid numbers.");
      return; // Stop submission on validation failure
    }

    setIsSubmitting(true);
    setError(null);

    try {
      // Conditional API Call
      let submittedCar;

      if (isEditing) {
        // For PUT, we need the ID in the URL and the full data in the body
        submittedCar = await updateCar(initialCar.carId, carDataToSend);
      } else {
        submittedCar = await addCar(carDataToSend);
      }

      // Success: Notify parent and reset form (or close edit mode)
      onSubmissionSuccess(submittedCar);
      setFormData(initialCarState);
      // if (onCarAdded) {
      //   onCarAdded(newCar); // Notifies parent to refresh list
      // }
    } catch (err) {
      setError(`Submission failed: ${err.message}. Check API status.`);
      console.error("Form Submission Error:", err);
    } finally {
      setIsSubmitting(false);
    }
  };

  return (
    <form onSubmit={handleSubmit} className={styles["car-form"]}>
      <h2>{isEditing ? `Edit Car #${initialCar.carId}` : "Add New Car"}</h2>

      {error && <div className={styles["form-error"]}>{error}</div>}

      <div className={styles["form-group"]}>
        <label htmlFor="make">Make</label>
        <input
          id="make"
          type="text"
          name="make"
          value={formData.make} // CONTROLS the input value
          onChange={handleInputChange} // HANDLES changes
          required
        />
      </div>

      <div className={styles["form-group"]}>
        <label htmlFor="model">Model</label>
        <input
          id="model"
          type="text"
          name="model"
          value={formData.model}
          onChange={handleInputChange}
          required
        />
      </div>

      <div className={styles["form-group"]}>
        <label htmlFor="year">Year</label>
        <input
          id="year"
          type="number" // Use number type for better mobile keyboard/input validation
          name="year"
          value={formData.year}
          onChange={handleInputChange}
          required
        />
      </div>

      <div className={styles["form-group"]}>
        <label htmlFor="price">Price</label>
        <input
          id="price"
          type="number"
          name="price"
          value={formData.price}
          onChange={handleInputChange}
          required
        />
      </div>

      <div className={styles["form-group"]}>
        <label htmlFor="color">Color</label>
        <input
          id="color"
          type="text"
          name="color"
          value={formData.color}
          onChange={handleInputChange}
          required
        />
      </div>

      <div className={styles["button-group"]}>
        <TooltipWrapper
          text={isEditing ? "Save Changes" : "Add Car"}
          className={styles["button-item"]}
        >
          {/* Submit Button */}
          <button
            type="submit"
            disabled={isSubmitting}
            className={styles["submit-button"]}
          >
            {isSubmitting ? "Adding..." : isEditing ? "💾" : "Add Car"}
          </button>
        </TooltipWrapper>

        {isEditing && (
          <TooltipWrapper
            text="Discard Changes"
            className={styles["button-item"]}
          >
            <button
              type="button"
              onClick={onCancel}
              className={styles["cancel-button"]}
            >
              ❌
            </button>
          </TooltipWrapper>
        )}
      </div>
    </form>
  );
};

export default CarForm;
