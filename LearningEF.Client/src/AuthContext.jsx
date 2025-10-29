import React, { createContext, useState, useContext, useEffect } from "react";
import { getToken, logoutUser } from "./api/LoginService"; // Import the service helper

// 1. Create the Context object
const AuthContext = createContext(null);

// 2. Create the Provider component
export const AuthProvider = ({ children }) => {
  // Check localStorage on load for an existing token
  const [token, setToken] = useState(getToken());

  // Function to handle successful login
  const login = (newToken) => {
    setToken(newToken);
    // Token storage happens inside LoginService, but we update React state here
  };

  // Function to handle logout
  const logout = () => {
    logoutUser(); // Clear localStorage
    setToken(null); // Clear React state
    // Note: In a real app, you might invalidate the token on the backend here
  };

  // Optional: Check token validity on mount/change (basic check)
  useEffect(() => {
    if (token && !getToken()) {
      // Token was in state but vanished from storage (e.g., manually cleared)
      setToken(null);
    }
  }, [token]);

  // The value exposed to consumers
  const contextValue = {
    token,
    isLoggedIn: !!token, // True if token is non-null/non-empty string
    login,
    logout,
  };

  return (
    <AuthContext.Provider value={contextValue}>{children}</AuthContext.Provider>
  );
};

// 3. Create a custom hook for easy consumption
export const useAuth = () => {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error("useAuth must be used within an AuthProvider");
  }
  return context;
};
