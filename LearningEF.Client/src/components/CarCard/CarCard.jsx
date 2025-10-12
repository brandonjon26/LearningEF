import React from "react";
import { useState } from "react";
import { useEffect } from "react";
import styles from "./CarCard.module.css";

const TooltipWrapper = ({ children, text }) => (
  <div className={styles["tooltip-container"]}>
    {children}
    <span className={styles["tooltip-text"]}>{text}</span>
  </div>
);

// Destructure the car object directly from props for clean access
const CarCard = ({ car }) => {
  // Defensive Coding: Check if a car object was actually passed
  if (!car) {
    return (
      <div className={styles["car-card-missing"]}>No car data available.</div>
    );
  }

  return (
    // Use a descriptive className for CSS targeting
    <div className={styles["car-card"]}>
      {/* Display the ID (useful for debugging and key in lists) */}
      <div className={styles["car-id"]}>ID: {car.carId}</div>

      {/* Main Title: Make and Model */}
      <h3 className={styles["car-title"]}>
        {car.make} {car.model}
      </h3>

      {/* Main details block */}
      <div className={styles["car-details"]}>
        <p>
          <strong>Year:</strong> {car.year}
        </p>
        <p>
          {/* Format the price to a standard US currency string */}
          <strong>Price:</strong>{" "}
          {new Intl.NumberFormat("en-US", {
            style: "currency",
            currency: "USD",
            minimumFractionDigits: 0,
          }).format(car.price)}
        </p>
        <p>
          <strong>Color:</strong> {car.color}
        </p>
      </div>

      {/* Action buttons (for future CRUD functionality: Edit/Delete) */}
      <div className={styles["car-actions"]}>
        <TooltipWrapper text="Edit">
          <button className={styles["btn-edit"]}>📝</button>
        </TooltipWrapper>
        <TooltipWrapper text="Delete">
          <button className={styles["btn-delete"]}>🗑️</button>
        </TooltipWrapper>
      </div>
    </div>
  );
};

export default CarCard;
