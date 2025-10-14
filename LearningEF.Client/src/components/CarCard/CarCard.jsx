import React from "react";
import { useState } from "react";
import { useEffect } from "react";
import styles from "./CarCard.module.css";
import TooltipWrapper from "../../utils/TooltipWrapper/TooltipWrapper";

// Destructure the car object directly from props for clean access
const CarCard = ({ car, onEdit, onDelete }) => {
  // Defensive Coding: Check if a car object was actually passed
  if (!car) {
    return (
      <div className={styles["car-card-missing"]}>No car data available.</div>
    );
  }

  return (
    <div className={styles["car-card"]}>
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

      <div className={styles["car-actions"]}>
        <TooltipWrapper text="Edit">
          <button className={styles["btn-edit"]} onClick={() => onEdit(car)}>
            📝
          </button>
        </TooltipWrapper>
        <TooltipWrapper text="Delete">
          <button
            className={styles["btn-delete"]}
            onClick={() => onDelete(car.carId)}
          >
            🗑️
          </button>
        </TooltipWrapper>
      </div>
    </div>
  );
};

export default CarCard;
