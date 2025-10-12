import React, { useState, useEffect } from "react";
import { getCars } from "../../api/carService"; // Import the service function
import CarCard from "../../components/CarCard/CarCard"; // Import the UI component
import styles from "./Carlist.module.css";

const CarList = () => {
  // Data storage
  const [cars, setCars] = useState([]);

  // UI/Logic indicators
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  // useEffect hook is where side effects (like data fetching) occur
  useEffect(() => {
    const fetchCars = async () => {
      try {
        // Fetch data using our service function
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
    };

    fetchCars();
  }, []); // Empty dependency array means this runs only once after the initial render

  // --- Conditional Rendering for UX ---

  if (loading) {
    return <div className={styles["car-list-status"]}>Loading car data...</div>;
  }

  if (error) {
    return <div className={styles["car-list-error"]}>Error: {error}</div>;
  }

  if (cars.length === 0) {
    return (
      <div className={styles["car-list-status"]}>
        No cars found in the database.
      </div>
    );
  }

  // --- Success Rendering ---
  return (
    <div className={styles["car-list-container"]}>
      <h1>Available Cars ({cars.length})</h1>

      {/* The list structure */}
      <div className={styles["car-list"]}>
        {cars.map((car) => (
          // Key is essential for React list rendering performance
          <CarCard key={car.carId} car={car} />
        ))}
      </div>
    </div>
  );
};

export default CarList;
