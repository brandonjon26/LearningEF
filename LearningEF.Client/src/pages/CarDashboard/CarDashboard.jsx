import React, { useState, useEffect, useCallback } from "react";
import { flushSync } from "react-dom";
import { useAuth } from "../../AuthContext";
import { getCars, deleteCar } from "../../api/CarService"; // Import the service function
import CarList from "../../components/CarList/CarList"; // Import the UI component
import CarForm from "../../components/CarForm/CarForm";
import styles from "./CarDashboard.module.css";

const CarDashboard = () => {
  const { logout, isLoggedIn } = useAuth();
  const [cars, setCars] = useState([]); // Data storage
  const [loading, setLoading] = useState(true); // UI/Logic indicators
  const [error, setError] = useState(null);
  const [editingCar, setEditingCar] = useState(null); // Editing Car

  // --- New Logout Handler ---
  const handleLogout = () => {
    logout();
    // The application will automatically redirect to /login via the PrivateRoute wrapper
    // Clean up local state, though component will unmount/re-render soon
    setCars([]);
    setEditingCar(null);
  };

  // Define Fetch Logic in a reusable function (*useCallback*)
  const fetchCars = useCallback(async () => {
    // Only attempt to fetch if we are actually logged in
    if (!isLoggedIn) {
      setLoading(false);
      return;
    }

    setLoading(true);
    setError(null);
    try {
      // Fetch data using service function
      const data = await getCars();

      // Update state on success
      setCars(data);
    } catch (err) {
      const errorMessage = err.message || "";
      if (
        errorMessage.includes("unauthorized") ||
        errorMessage.includes("expired")
      ) {
        // If the service throws an auth error, clear the session state.
        setError("Session expired. Logging out...");
        setTimeout(logout, 1500); // Wait briefly before logging out
      } else {
        // Update error state on failure
        setError("Failed to load cars. Please check the API connection.");
      }
      console.error("Fetch error details:", err);
    } finally {
      // Always turn off loading indicator
      setLoading(false);
    }
  }, [isLoggedIn, logout]); // Dependency on isLoggedIn and logout

  // useEffect hook is where side effects occur - Initial Data Load
  useEffect(() => {
    // Only run fetchCars if isLoggedIn state is true
    if (isLoggedIn) {
      fetchCars();
    }
  }, [fetchCars, isLoggedIn]);

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

  const handleDelete = async (carId) => {
    // Optimistically update: Instantly remove the car from the list
    setCars((prevCars) =>
      (prevCars || []).filter((car) => car.carId !== carId)
    );

    try {
      await deleteCar(carId);
      console.log(`Successfully deleted car ID: ${carId}`); // Wtite success message
    } catch (error) {
      // Write error message
      console.error(`Failed to delete car ${carId}.`, error);

      // Display message to the user
      flushSync(() => {
        setError("Error deleting car. Reverting list.");
      });

      // Clear the error message and Revert the list
      setTimeout(async () => {
        await fetchCars();
        setError(null);
      }, 2500);
    }
  };

  return (
    <div className={styles["dashboard-container"]}>
      {/* Logout Button Area */}
      <div className={styles["logout-area"]}>
        <button
          onClick={handleLogout}
          disabled={!isLoggedIn}
          className={styles["logout-button"]}
        >
          Logout
        </button>
      </div>
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
            onDelete={handleDelete}
          />
        </div>

        {/* Right Side: Add Car Form */}
        <div className={styles["form-panel"]}>
          {editingCar ? (
            <CarForm
              initialCar={editingCar}
              onSubmissionSuccess={handleCarUpdated}
              onApiError={fetchCars}
              onCancel={stopEdit}
            />
          ) : (
            <CarForm
              initialCar={null}
              onSubmissionSuccess={handleCarAdded}
              onApiError={fetchCars}
            />
          )}
        </div>
      </div>
    </div>
  );
};

export default CarDashboard;
