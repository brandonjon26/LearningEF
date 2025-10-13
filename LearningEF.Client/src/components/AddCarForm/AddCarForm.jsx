import React, { useState } from "react";
import { addCar } from "../../api/carService"; // Import the service function
import styles from "./AddCarForm.module.css";

// Define the initial empty state for a new car
const initialCarState = {
  make: "",
  model: "",
  year: "", // We'll keep this as a string for input purposes
  color: "",
  price: "", // We'll keep this as a string for input purposes
};

const AddCarForm = ({ onCarAdded }) => {
  const [formData, setFormDatra] = useState(initialCarState);

  // Add a simple state for feedback/UX
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [error, setError] = useState(null);

  // Create a universal handler for input changes
  const handleInputChange = (e) => {
    const { name, value } = e.target;

    // Use the functional update to ensure correct state update
    setFormDatra((prevData) => ({
      ...prevData,
      [name]: value,
    }));
  };

  // Create the submission handler
  const handleSubmit = async (e) => {
    e.preventDefault(); // REQUIRED: Stops the browser's default form submission (page reload)

    // Basic data conversion/validation
    const carDataToSend = {
      ...formData,
      year: parseInt(formData.year),
      price: parseFloat(formData.price),
    };

    // Basic validation check
    if (isNaN(carDataToSend.year) || isNaN(carDataToSend.price)) {
      setError("Please ensure Year and Price are valid numbers.");
    }

    setIsSubmitting(true);
    setError(null);

    try {
      const newCar = await addCar(carDataToSend);

      // Reset the form and notify the parent component (CarDashboard)
      setFormDatra(initialCarState);
      if (onCarAdded) {
        onCarAdded(newCar); // Notifies parent to refresh list
      }
    } catch (err) {
      setError(`Submission failed: ${err.message}. Check API status.`);
      console.error("Add Car Error:", err);
    } finally {
      setIsSubmitting(false);
    }
  };

  return (
    <form onSubmit={handleSubmit} className={styles["add-car-form"]}>
      <h2>Add New Car</h2>

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

      {/* Submit Button */}
      <button
        type="submit"
        disabled={isSubmitting}
        className={styles["submit-button"]}
      >
        {isSubmitting ? "Adding..." : "Add Car"}
      </button>
    </form>
  );
};

export default AddCarForm;
