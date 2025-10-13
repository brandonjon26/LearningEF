import { useState } from "react";
import reactLogo from "./assets/react.svg";
import viteLogo from "/vite.svg";
import "./App.css";
import CarDashboard from "./pages/CarDashboard/CarDashboard";

function App() {
  return (
    <div className="main-app-container">
      <CarDashboard />
    </div>
  );
}

export default App;
