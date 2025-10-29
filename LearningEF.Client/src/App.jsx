import {
  BrowserRouter as Router,
  Routes,
  Route,
  Navigate,
} from "react-router-dom";
import { AuthProvider, useAuth } from "./AuthContext";
import { useState } from "react";
import reactLogo from "./assets/react.svg";
import viteLogo from "/vite.svg";
import "./App.css";
import CarDashboard from "./pages/CarDashboard/CarDashboard";
import LoginPage from "./pages/Login/LoginPage";

// --- 1. Define the Private Route Wrapper ---
// This component checks if the user is logged in before rendering the protected component.
const PrivateRoute = ({ element: Element }) => {
  // 🔑 Use the custom hook to access global authentication state
  const { isLoggedIn } = useAuth();

  // If logged in, render the element; otherwise, redirect to the login page
  return isLoggedIn ? <Element /> : <Navigate to="/login" replace />;
};
// ------------------------------------------

// --- 2. Define the Routes Component ---
// This contains all the application's paths.
function AppRoutes() {
  const { login } = useAuth();

  return (
    <Routes>
      {/* Public Route: The login page */}
      {/* Note: The LoginPage receives 'onLoginSuccess' from AuthContext (via context, not props) */}
      <Route path="/login" element={<LoginPage onLoginSuccess={login} />} />

      {/* Protected Route: The Car Dashboard */}
      {/* Only users with a valid token stored in AuthContext can access this */}
      <Route
        path="/dashboard"
        element={<PrivateRoute element={CarDashboard} />}
      />

      {/* Default/Catch-all route: Redirects all unknown paths to the dashboard */}
      <Route path="*" element={<Navigate to="/dashboard" replace />} />
    </Routes>
  );
}
// ------------------------------------------

function App() {
  return (
    <Router>
      <AuthProvider>
        <div className="main-app-container">
          <AppRoutes />
        </div>
      </AuthProvider>
    </Router>
  );
}

export default App;
