import React from "react";
import styles from "./TooltipWrapper.module.css";

const TooltipWrapper = ({ children, text, className }) => {
  return (
    <div className={`${styles["tooltip-container"]} ${className || ""}`}>
      {children}
      <span className={styles["tooltip-text"]}>{text}</span>
    </div>
  );
};

export default TooltipWrapper; // Export it as default
