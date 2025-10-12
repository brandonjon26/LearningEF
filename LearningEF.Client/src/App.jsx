import { useState } from "react";
import reactLogo from "./assets/react.svg";
import viteLogo from "/vite.svg";
import "./App.css";
import CarList from "./pages/CarList/CarList";

function App() {
  return (
    <div className="main-app-container">
      <CarList />
    </div>
  );
}

export default App;
