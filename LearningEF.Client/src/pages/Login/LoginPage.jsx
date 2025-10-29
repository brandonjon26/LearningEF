import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import { loginUser } from "../../api/LoginService";
import styles from "./Login.module.css";

const LoginPage = ({ onLoginSuccess }) => {
  const [username, setUsername] = useState("testuser");
  const [password, setPassword] = useState("password");
  const [error, setError] = useState("");
  const [isLoading, setIsLoading] = useState(false);
  const navigate = useNavigate();

  const handleLogin = async (e) => {
    e.preventDefault();
    setError("");
    setIsLoading(true);

    try {
      // Call the service layer
      const token = await loginUser(username, password);

      // Success handler
      if (onLoginSuccess) onLoginSuccess(token);
      navigate("/dashboard");
    } catch (err) {
      // Catch error thrown by the service (e.g., "Invalid username or password")
      setError(err.message);
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div className={styles["login-container"]}>
      <h2>Car Inventory Login</h2>
      <form onSubmit={handleLogin} className={styles["login-form"]}>
        <div className={styles["form-group"]}>
          <label htmlFor="username">Username:</label>
          <input
            id="username"
            type="text"
            value={username}
            onChange={(e) => setUsername(e.target.value)}
            required
          />
        </div>
        <div className={styles["form-group"]}>
          <label htmlFor="password">Password:</label>
          <input
            id="password"
            type="password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            required
          />
        </div>
        <button
          type="submit"
          disabled={isLoading}
          className={styles["login-button"]}
        >
          {isLoading ? "Logging in..." : "Log In"}
        </button>
        {error && <p className={styles["login-error"]}>{error}</p>}
      </form>
    </div>
  );
};

export default LoginPage;
