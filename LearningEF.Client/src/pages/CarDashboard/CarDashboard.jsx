import React, { useState, useEffect, useCallback } from "react";
import { getCars } from "../../api/carService"; // Import the service function
import CarList from "../../components/CarList/CarList"; // Import the UI component
import CarForm from "../../components/CarForm/CarForm";
import styles from "./CarDashboard.module.css";

const CarDashboard = () => {
  // Data storage
  const [cars, setCars] = useState([]);
  // UI/Logic indicators
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  // Editing Car
  const [editingCar, setEditingCar] = useState(null);

  // Define Fetch Logic in a reusable function (*useCallback*)
  const fetchCars = useCallback(async () => {
    setLoading(true);
    setError(null);
    try {
      // Fetch data using service function
      const data = await getCars();

      // Update state on success
      setCars(data);
    } catch (err) {
      // Update error state on failure
      setError("Failed to load cars. Please check the API connection.");
      console.error("Fetch error details:", err);
    } finally {
      // Always turn off loading indicator
      setLoading(false);
    }
  }, []);

  // useEffect hook is where side effects occur - Initial Data Load
  useEffect(() => {
    fetchCars();
  }, [fetchCars]);

  // Handler for successful form submission (OPTIMISTIC UPDATE) ---
  const handleCarAdded = (newCar) => {
    if (newCar) {
      // Check to ensure the list is initialized before trying to add to it
      setCars((prevCars) => [
        ...(prevCars || []), // Safely ensures prevCars is treated as an array
        newCar, // Add the new car object returned from the API
      ]);
      // Might want to display a success message here later.
    }
  };

  // Function to start editing car
  const startEdit = (car) => {
    setEditingCar(car);
  };

  // Function to stop editing
  const stopEdit = () => {
    setEditingCar(null);
  };

  const handleCarUpdated = (updatedCar) => {
    // Optimistically update the list by finding the old car and replacing it.
    setCars((prevCars) =>
      (prevCars || []).map(
        (car) =>
          car.carId === updatedCar.carId
            ? updatedCar // Found the match: replace it with the new data
            : car // No match: keep the old car object
      )
    );

    // Clear the editing state to close the form
    stopEdit();
  };

  return (
    <div className={styles["dashboard-container"]}>
      {/* The main content layout area */}
      <div className={styles["content-layout"]}>
        {/* Left Side: Car List */}
        <div className={styles["list-panel"]}>
          {/* Pass the state to the presentational component */}
          <CarList
            cars={cars}
            loading={loading}
            error={error}
            onEdit={startEdit}
          />
        </div>

        {/* Right Side: Add Car Form */}
        <div className={styles["form-panel"]}>
          {editingCar ? (
            <CarForm
              initialCar={editingCar}
              onSubmissionSuccess={handleCarUpdated}
              onCancel={stopEdit}
            />
          ) : (
            <CarForm initialCar={null} onSubmissionSuccess={handleCarAdded} />
          )}
        </div>
      </div>
    </div>
  );
};

export default CarDashboard;
