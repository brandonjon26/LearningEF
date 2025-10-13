import React, { useState, useEffect, useCallback } from "react";
import { getCars } from "../../api/carService"; // Import the service function
import CarList from "../../components/CarList/CarList"; // Import the UI component
import AddCarForm from "../../components/AddCarForm/AddCarForm";
import styles from "./CarDashboard.module.css";

const CarDashboard = () => {
  // Data storage
  const [cars, setCars] = useState([]);
  // UI/Logic indicators
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

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

  return (
    <div className={styles["dashboard-container"]}>
      {/* The main content layout area */}
      <div className={styles["content-layout"]}>
        {/* Left Side: Car List */}
        <div className={styles["list-panel"]}>
          {/* Pass the state to the presentational component */}
          <CarList cars={cars} loading={loading} error={error} />
        </div>

        {/* Right Side: Add Car Form */}
        <div className={styles["form-panel"]}>
          {/* Placeholder for the form component */}
          <AddCarForm onCarAdded={handleCarAdded} />
        </div>
      </div>
    </div>
  );
};

export default CarDashboard;
