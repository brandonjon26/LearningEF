import React from "react";
import CarCard from "../CarCard/CarCard";
import styles from "./CarList.module.css";

const CarList = ({ cars, loading, error }) => {
  // --- Conditional Rendering
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

      <div className={styles["car-list"]}>
        {cars.map((car) => (
          <CarCard key={car.carId} car={car} />
        ))}
      </div>
    </div>
  );
};

export default CarList;
